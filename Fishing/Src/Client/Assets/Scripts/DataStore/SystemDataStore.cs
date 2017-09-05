using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Assets.Scripts.Utilities;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Assets.Scripts.DataStore
{
    public class SystemDataStore : SingletonBase<SystemDataStore>, IDisposable
    {
        private IDbConnection dbconn;

        public class DataRecord : IDisposable
        {
            public IDataReader reader;

            public DataRecord(IDataReader inReader)
            {
                reader = inReader;
            }

            // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
            ~DataRecord()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(false);
            }

            public bool Read()
            {
                if (reader == null)
                {
                    return false;
                }

                return reader.Read();
            }

            public int FieldCount
            {
                get
                {
                    return reader == null ? 0 : reader.FieldCount;
                }
            }

            public DateTime GetDateTime(object c)
            {
                int idx = reader.GetOrdinal(c.ToString());

                if (idx == -1)
                {
                    throw new Exception(string.Format("Can't find field {0} in record: ", c));
                }

                string type = reader.GetDataTypeName(idx);
                if (type != "TEXT")
                {
                    throw new Exception(string.Format("Wrong type of field request {0} is not in record: ", c));
                }

                DateTime result;
                if (!DateTime.TryParse(reader.GetString(idx), out result))
                {
                    throw new Exception(string.Format("Wrong type of datetime field request {0} is not in record: ", c));
                }

                return result;
            }

            public int GetInt(object c)
            {
                int idx = reader.GetOrdinal(c.ToString());

                if (idx == -1)
                {
                    throw new Exception(string.Format("Can't find field {0} in record: ", c));
                }

                string type = reader.GetDataTypeName(idx);
                if (type != "INTEGER")
                {
                    throw new Exception(string.Format("Wrong type of field request {0} is not in record: ", c));
                }

                return reader.GetInt32(idx);
            }

            public string GetString(object c)
            {
                int idx = reader.GetOrdinal(c.ToString());

                if (idx == -1)
                {
                    throw new Exception(string.Format("Can't find field {0} in record: ", c));
                }

                string type = reader.GetDataTypeName(idx);
                if (type != "TEXT")
                {
                    throw new Exception(string.Format("Wrong type of field request {0} is not in record: ", c));
                }

                return reader.GetString(idx);
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("DataRecord: ");
                for (int i = 0; i < reader.FieldCount; ++i)
                {
                    sb.AppendFormat("{0}:{1}.{2}:{3}, ", i, reader.GetDataTypeName(i), reader.GetName(i), reader.GetValue(i));
                }

                sb.Remove(sb.Length - 2, 2);

                return sb.ToString();
            }

            #region IDisposable Support
            private bool disposedValue; // To detect redundant calls

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: dispose managed state (managed objects).
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                    // TODO: set large fields to null.
                    if (reader != null)
                    {
                        reader.Close();
                        reader = null;
                    }

                    disposedValue = true;
                }
            }

            // This code added to correctly implement the disposable pattern.
            public void Dispose()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
                // TODO: uncomment the following line if the finalizer is overridden above.
                // GC.SuppressFinalize(this);
            }
            #endregion
        }

        private static IEnumerator ProcessDownload(WWW www)
        {
            yield return www;
        }

        private static void Download(WWW www)
        {
            ProcessDownload(www);
            while (!www.isDone)
            {
            }
        }

        public void Disconnect()
        {
            if (dbconn != null)
            {
                dbconn.Close();
                dbconn.Dispose();
                dbconn = null;
            }
        }

        public void Disconnect(string dbFilename)
        {
            if (dbconn != null)
            {
                dbconn.Close();
                dbconn.Dispose();
                dbconn = null;
            }

            string filename = Application.persistentDataPath + "/" + dbFilename;
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        public void Connect(string dbFilename)
        {

            if (dbconn != null)
            {
                return;
            }

            // a product persistent database path.
            string filename = Application.persistentDataPath + "/" + dbFilename;
            // check if database already exists.
            if (CheckNeedCopyDBFile(dbFilename, "DBConfig.ini"))
            {
                Debug.Log("数据库版本不一致，开始拷贝数据库");
                TryCopyFile(dbFilename);
                TryCopyFile("DBConfig.ini");
            }

            //
            // initialize database
            //
            string conn = "URI=file:" + filename; //Path to database.
            try
            {
                dbconn = new SqliteConnection(conn);
                // Open connection to the database.
                dbconn.Open();

                //dbconn.Close();
                //if(File.Exists(filename)) {
                //    File.Delete(filename);
                //}
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("Database open failed! filename: {0}, {1}", filename, ex.Message);
            }
        }

        public bool CheckNeedCopyDBFile(string dbFilename, string verFileName)
        {
            string filename = Application.persistentDataPath + "/" + dbFilename;
            if (!File.Exists(filename))
            {
                return true;
            }

            string versionName = Application.persistentDataPath + "/" + verFileName;
            if (!File.Exists(versionName))
            {
                return true;
            }

            string sourceCotent = "";
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            string vertionPath = "file://" + Application.streamingAssetsPath + "/" + verFileName;
            WWW www = new WWW(vertionPath);
            Download(www);
            sourceCotent = www.text;
#elif UNITY_WEBPLAYER
            string vertionPath = "StreamingAssets/" + verFileName;
            WWW www = new WWW(vertionPath);
            Download(www);
            sourceCotent = www.text;
#elif UNITY_IPHONE
            string vertionPath = Application.dataPath + "/Raw/" + verFileName;
            try{     
                using ( StreamReader reader = new StreamReader(vertionPath, Encoding.Default) ){
                    sourceCotent = reader.ReadToEnd();
                    reader.Close();
                }                 
            } catch (Exception e){
            //log +=       "\nTest Fail with Exception " + e.ToString();
            //log +=       "\n";
            }
#elif UNITY_ANDROID
            string vertionPath = Application.streamingAssetsPath + "/" + verFileName;
            WWW www = new WWW(vertionPath);
            Download(www);
            sourceCotent = www.text;
#endif

            string targetContent = "";
            try
            {
                // copy database to real file into cache folder
                using (StreamReader reader = new StreamReader(versionName, Encoding.Default))
                {
                    targetContent = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                Debug.Log("\nTest Fail with Exception " + e);
                Debug.Log("\n\n Did you copy test.db into StreamingAssets ?\n");
            }

            if (GetVersionNum(sourceCotent) > GetVersionNum(targetContent))
            {
                return true;
            }

            return false;
        }

        private int GetVersionNum(string verContent)
        {
            int version = 0;
            string[] strs = verContent.Split('=');
            if (strs.Length > 1)
            {
                version = int.Parse(strs[1].Trim());
            }
            return version;
        }

        private void TryCopyFile(string dbFilename)
        {
            string filename = Application.persistentDataPath + "/" + dbFilename;
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            // ok , this is first time application start!
            // so lets copy prebuild database from StreamingAssets and load store to persistancePath with Test2
            byte[] bytes = null;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            string dbpath = "file://" + Application.streamingAssetsPath + "/" + dbFilename;
            WWW www = new WWW(dbpath);
            Download(www);
            bytes = www.bytes;
#elif UNITY_WEBPLAYER
            string dbpath = "StreamingAssets/" + dbFilename;
            WWW www = new WWW(dbpath);
            Download(www);
            bytes = www.bytes;
#elif UNITY_IPHONE
            string dbpath = Application.dataPath + "/Raw/" + dbFilename;
            try{     
                using ( FileStream fs = new FileStream(dbpath, FileMode.Open, FileAccess.Read, FileShare.Read) ){
                    bytes = new byte[fs.Length];
                    fs.Read(bytes,0,(int)fs.Length);
                    fs.Close();
                }                 
            } catch (Exception e){
            //log +=       "\nTest Fail with Exception " + e.ToString();
            //log +=       "\n";
            }
#elif UNITY_ANDROID
            string dbpath = Application.streamingAssetsPath + "/" + dbFilename;
            WWW www = new WWW(dbpath);
            Download(www);
            bytes = www.bytes;
#endif

            if (bytes != null)
            {
                try
                {
                    // copy database to real file into cache folder
                    using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Close();
                        Debug.Log("Copy database from streaminAssets to persistentDataPath: " + filename);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("\nTest Fail with Exception " + e);
                    Debug.Log("\n\n Did you copy test.db into StreamingAssets ?\n");
                }
            }
        }

        public List<string> QueryTableNames()
        {

            var dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = "SELECT name FROM sqlite_master WHERE type=\'table\';";
            IDataReader reader = dbcmd.ExecuteReader();
            List<string> tables = new List<string>();
            while (reader.Read())
            {
                //string c = reader.GetName(0);
                //int idx = reader.GetOrdinal(c);
                tables.Add(reader.GetString(0));
            }
            reader.Close();
            dbcmd.Dispose();

            return tables;
        }

        public DataRecord FetchRecord(string table)
        {

            var dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = string.Format("SELECT * FROM '{0}';", table);
            IDataReader reader = dbcmd.ExecuteReader();
            dbcmd.Dispose();

            //var record = new DataRecord(reader);

            return new DataRecord(reader);
        }

        public DataRecord FetchRecord(string table, int id)
        {
            using (var dbcmd = dbconn.CreateCommand())
            {
                dbcmd.CommandText = string.Format("SELECT * FROM '{0}' WHERE attID = {1};", table, id);
                IDataReader reader = dbcmd.ExecuteReader();
                return new DataRecord(reader);
            }
            //dbcmd.Dispose();
            //dbcmd = null;
        }

        public DataRecord FetchRecord<TTable>(int id)
        {
            return FetchRecord(typeof(TTable).Name, id);
        }

        public DataRecord FetchRecord(string table, int id, params object[] args)
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("SELECT ");

            foreach (var arg in args)
            {
                sb.AppendFormat("\"{0}\",", arg);
            }

            sb.Remove(sb.Length - 1, 1);
            sb.AppendFormat(" FROM '{0}' WHERE attID = {1};", table, id);

            var dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = sb.ToString();
            IDataReader reader = dbcmd.ExecuteReader();
            dbcmd.Dispose();

            return new DataRecord(reader);
        }

        public DataRecord FetchRecord<TTable>(int id, params object[] args)
        {
            return FetchRecord(typeof(TTable).Name, id, args);
        }

        public DataRecord FetchRecord(string table, params object[] args)
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("SELECT ");

            if (args.Length > 0)
            {
                foreach (var arg in args)
                {
                    sb.AppendFormat("\"{0}\",", arg);
                }
                sb.Remove(sb.Length - 1, 1);
            }
            else {
                sb.Append("*");
            }

            sb.AppendFormat(" FROM '{0}';", table);

            var dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = sb.ToString();
            IDataReader reader = dbcmd.ExecuteReader();
            dbcmd.Dispose();

            return new DataRecord(reader);
        }

        public DataRecord FetchRecord<TTable>(params object[] args)
        {
            return FetchRecord(typeof(TTable).Name, args);
        }

        #region IDisposable Support
        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                Disconnect();

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~SystemDataStore()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}