using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Assets.Scripts.Helpers;
using Excel;
using Mono.Data.Sqlite;
using ThreadState = System.Threading.ThreadState;
using UnityEngine;
using Assets.Scripts.ConsoleController.Console;

namespace Assets.Scripts {
    public class DataStoreExporter {

        public class DataStoreExporterState {
            public List<string> sheets;
            public List<string> excelFileDirectories;
            public string dbPath;
            public string templatePath;

            public List<string> excelFilePaths;
            public List<string> processPaths;
            public List<string> donePaths;
            public Dictionary<string, List<string>> doneErrors;
            public Dictionary<string, List<string>> doneWarnnings;
            public Dictionary<string, List<string>> doneLogs;
            public Dictionary<string, int> doneRecords;
            public IDbConnection dbConn;

            public Stopwatch stopwatch;

            public int verbosity;

            public int index;
            public int warnnings;
            public int errors;

            private List<Thread> threads;

            public DataStoreExporterState(List<string> sheets, List<string> excelFileDirectories, string dbPath) {
                this.sheets = sheets;
                this.excelFileDirectories = excelFileDirectories;
                this.dbPath = dbPath;

                excelFilePaths = new List<string>();
                processPaths = new List<string>();
                donePaths = new List<string>();
                doneErrors = new Dictionary<string, List<string>>();
                doneWarnnings = new Dictionary<string, List<string>>();
                doneLogs = new Dictionary<string, List<string>>();
                doneRecords = new Dictionary<string, int>();
                dbConn = null;

                stopwatch = new Stopwatch();

                index = 0;
                warnnings = 0;
                errors = 0;

                threads = new List<Thread>();
            }

            public bool StartWorking() {

                if (threads.Count == 0) {

                    for (var i = 0; i < excelFilePaths.Count; ++i) {
                        threads.Add(new Thread(Worker));
                    }

                    foreach (var thread in threads) {
                        thread.Start();
                    }
                }

                foreach (var thread in threads) {
                    //thread.Join();
                    if (thread.ThreadState == ThreadState.Running) {
                        return false;
                    }
                }

                return true;
            }

            public void Worker() {
                var idx = -1;
                lock (this) {
                    if (processPaths.Count != excelFilePaths.Count) {
                        idx = processPaths.Count;
                        processPaths.Add(excelFilePaths[idx]);
                    }
                }

                if (idx == -1) {
                    return;
                }

                var dataStoreExporterState = this;
                ExportDataOfFile(ref dataStoreExporterState, idx);

                lock (this) {
                    donePaths.Add(excelFilePaths[idx]);
                }
            }

            public void LogFormat(string tableName, string format, params object[] args) {

                DebugFormat(ref doneLogs, tableName, format, args);

                DebugHelper.LogFormat(this, "DataStoreExporter", "{0}:{1}", tableName, string.Format(format, args));
            }

            public void WarnFormat(string tableName, string format, params object[] args) {

                ++warnnings;

                DebugFormat(ref doneWarnnings, tableName, format, args);

                DebugHelper.WarnFormat(this, "DataStoreExporter", "{0}:{1}", tableName, string.Format(format, args));
            }

            public void ErrorFormat(string tableName, string format, params object[] args) {

                ++errors;

                DebugFormat(ref doneErrors, tableName, format, args);

                DebugHelper.ErrorFormat(this, "DataStoreExporter", "{0}:{1}", tableName, string.Format(format, args));
            }

            private void DebugFormat(ref Dictionary<string, List<string>> logs
                , string tableName, string format, params object[] args) {

                if (tableName.Length == 0) {
                    return;
                }

                var filePath = "";
                lock (excelFilePaths) {
                    foreach (var excelFilePath in excelFilePaths) {
                        if (excelFilePath.IndexOf(tableName) != -1) {
                            filePath = excelFilePath;
                            break;
                        }
                    }
                }

                if (filePath.Length != 0) {
                    if (!logs.ContainsKey(filePath)) {
                        logs.Add(filePath, new List<string>());
                    }
                    logs[filePath].Add(string.Format(format, args));
                }
            }
        }

        private static string BuildCreateTable(ref DataStoreExporterState state
            , DataColumnCollection dcc
            , string tableName) {

            var sb = new StringBuilder();

            sb.AppendFormat("CREATE TABLE IF NOT EXISTS '{0}' (", tableName);

            //(id INTEGER PRIMARY KEY, str_field TEXT, blob_field BLOB)

            for (var i = 0; i < dcc.Count; ++i) {

                var column = dcc[i].Caption.Trim();
                if (column.Contains("Column")) {
                    state.WarnFormat(tableName, "{0} Unsupport data column name: {1}, type: {2}. Skip all after."
                        , tableName, column, dcc[i].DataType.ToString());
                    break;
                }

                if (i == 0 && dcc[i].DataType.ToString() == "System.Double") {
                    sb.AppendFormat("'{0}' INTEGER PRIMARY KEY,", dcc[i].Caption.Trim());
                }
                else if (dcc[i].DataType.ToString() == "System.Double") {
                    sb.AppendFormat("'{0}' INTEGER,", dcc[i].Caption.Trim());
                }
                else if (dcc[i].DataType.ToString() == "System.String") {
                    sb.AppendFormat("'{0}' TEXT,", dcc[i].Caption.Trim());
                }
                else if (dcc[i].DataType.ToString() == "System.DateTime") {
                    sb.AppendFormat("'{0}' TEXT,", dcc[i].Caption.Trim());
                }
                else if (dcc[i].DataType.ToString() == "System.Object") {
                    sb.AppendFormat("'{0}' TEXT,", dcc[i].Caption.Trim());
                }
                else {
                    state.ErrorFormat(tableName, "{0} UnSupport data type: {1}", tableName, dcc[i].DataType.ToString());
                }
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append(");");

            return sb.ToString();
        }

        private static DbType GetDbTypeBySystemType(Type type) {
            switch (type.ToString()) {
                case "System.Double":
                    return DbType.Double;
                case "System.string":
                    return DbType.String;
                default:
                    return DbType.String;
            }
        }

        private static string BuildInsertCommandText(DataColumnCollection dcc, string tableName) {

            var sb = new StringBuilder();

            sb.AppendFormat("INSERT INTO '{0}' (", tableName);

            for (var i = 0; i < dcc.Count; ++i) {
                var column = dcc[i].Caption.Trim();
                if (column.Contains("Column")) {
                    break;
                }
                sb.AppendFormat("'{0}',", column);
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(") VALUES (");

            for (var i = 0; i < dcc.Count; ++i) {
                var column = dcc[i].Caption.Trim();
                if (column.Contains("Column")) {
                    break;
                }
                sb.AppendFormat("@{0},", column);
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(");");

            return sb.ToString();
        }

        private static string Join(string separator, IList<object> value) {
            var sb = new StringBuilder();
            for (var i = 0; i < value.Count; ++i) {
                sb.Append(value[i]);
                if (i != value.Count - 1) {
                    sb.Append(separator);
                }
            }
            return sb.ToString();
        }

        private static IDbCommand BuildInsertCommand(ref DataStoreExporterState state
            , IDbConnection dbConn
            , DataColumnCollection dataColumns
            , DataRow dataRow
            , string tableName) {

            var dbcmd = dbConn.CreateCommand();

            dbcmd.CommandText = BuildInsertCommandText(dataColumns, tableName);

            if (state.verbosity > 0) {
                state.LogFormat(tableName, "{0} INSERT ROW: {1}", tableName, Join(" ", dataRow.ItemArray));
            }

            for (var i = 0; i < dataColumns.Count; ++i) {

                var column = dataColumns[i].Caption.Trim();
                if (column.Contains("Column")) {
                    break;
                }

                dbcmd.Parameters.Add(new SqliteParameter("@" + column,
                    GetDbTypeBySystemType(dataColumns[i].DataType)) {
                        Value = dataRow[dataColumns[i].Caption],
                        Size = dataRow[dataColumns[i].Caption].ToString().Length
                    });

                if (state.verbosity > 0) {
                    state.LogFormat(tableName, "INSERT ROW DETAILS: placeholder:{0}, type:{1}, data:{2}, length:{3}"
                        , "@" + dataColumns[i].Caption
                        , GetDbTypeBySystemType(dataColumns[i].DataType)
                        , dataRow[dataColumns[i].Caption]
                        , dataRow[dataColumns[i].Caption].ToString().Length);
                }
            }

            return dbcmd;
        }

        public static string[] GetFiles(string path) {
            var result = new List<string>();
            var files = Directory.GetFiles(path, "*.xlsm", SearchOption.AllDirectories);

            foreach (var file in files) {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                if (fileNameWithoutExtension != null && fileNameWithoutExtension.StartsWith("~$")) {
                    continue;
                }
                result.Add(file);
            }

            return result.ToArray();
        }

        public static DataStoreExporterState Prepare(List<string> sheets, List<string> excelFilePaths
            , string dbPath, bool remove, string templatePath = "") {

            var state = new DataStoreExporterState(sheets, excelFilePaths, dbPath);

            state.stopwatch.Start();

            state.LogFormat("", "Export DataStore Application.");

            if (excelFilePaths.Count == 0) {
                state.ErrorFormat("", "No data path specified. Use current directory. {0}"
                    , Environment.CurrentDirectory);
                return state;
            }

            if (dbPath.Length == 0) {
                state.ErrorFormat("", "No SQLite database path specified. Use default path. {0}", dbPath);
                return state;
            }

            if (remove) {
                try {
                    if (File.Exists(dbPath)) {
                        File.Delete(dbPath);
                    }
                }
                catch (Exception ex) {
                    state.ErrorFormat("", "Remove SQLite db file {0} failed: {1}",
                        dbPath, ex.Message);
                }
            }


            try {
                var conn = string.Format("URI=file:{0}", dbPath);

                using (IDbConnection dbconn = new SqliteConnection(conn)) {
                    // Open connection to the database.
                    dbconn.Open();
                    dbconn.Close();
                }

                //SQLiteConnection.CreateFile(sqliteDb);
            }
            catch (Exception ex) {
                state.ErrorFormat("", "Create SQLite db file {0} failed: {1}",
                    dbPath, ex.Message);
            }

            if (sheets.Count == 0) {
                state.ErrorFormat("", "No sheet tags defined for export.");
                return state;
            }

            if (state.errors != 0) {
                return state;
            }


            // files to export.
            foreach (var excelFilePath in excelFilePaths) {

                var files = GetFiles(excelFilePath);
                state.excelFilePaths.AddRange(files);
            }


            // Path to database.
            var connSQLite = "URI=file:" + dbPath;
            state.dbConn = new SqliteConnection(connSQLite);

            // Open connection to the database.
            state.dbConn.Open();

            state.LogFormat("", "Database created! filename: " + dbPath);

            state.templatePath = templatePath;

            return state;
        }

        public static void Done(ref DataStoreExporterState state) {

            state.stopwatch.Stop();

            var msg = string.Format("All Done! {3} Files, {4} Records in : {2:0.00}s, Warnnings: {0}, Errors: {1}"
                , state.warnnings, state.errors, (state.stopwatch.ElapsedTicks / (decimal)Stopwatch.Frequency)
                , state.excelFilePaths.Count, state.doneRecords.Values.Sum());

            if (state.errors == 0) {
                state.LogFormat("", msg);
            }
            else {
                state.ErrorFormat("", msg);
            }

            if (state.dbConn != null) {
                state.dbConn.Close();
                state.dbConn.Dispose();
                state.dbConn = null;
            }
        }

        public static bool ExportDataOfFile(ref DataStoreExporterState state, int index) {

            if (index > state.excelFilePaths.Count) {
                return false;
            }

            if (state.dbConn == null) {
                return false;
            }

            //读取模板表文件，对比excel中字段名是否与代码中一致
            var reader = new StreamReader(state.templatePath, Encoding.Default);
            string content = reader.ReadToEnd();
            reader.Close();
            content = content.Replace("\r", "*");
            content = content.Replace("\n", "*");

            var file = state.excelFilePaths[index];
            state.LogFormat(file, "Processing excel file {0}.", file);

            try {
                //using ( IDbConnection dbconn = new SqliteConnection(state.dbConn.ConnectionString) ) {
                {
                    var dbconn = (SqliteConnection)state.dbConn;
                    // Open connection to the database.
                    //dbconn.Open();

                    var stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    excelReader.IsFirstRowAsColumnNames = true;
                    var dataset = excelReader.AsDataSet();
                    excelReader.Close();

                    foreach (DataTable table in dataset.Tables) {
                        if (state.sheets.IndexOf(table.TableName) == -1) {
                            continue;
                        }

                        var tableName = Path.GetFileNameWithoutExtension(file);

                        content = content.Replace(tableName, "~");
                        string[] strs = content.Split('~');
                        string resultStr = "";
                        if (strs.Length > 1) {
                            content = strs[1];
                            strs = content.Split('{');
                            if (strs.Length > 1) {
                                content = strs[1];
                                strs = content.Split('}');
                                if (strs.Length > 0) {
                                    content = strs[0];
                                    content = content.Replace(",", "");
                                    strs = content.Split('*');
                                    for (int sub = 0; sub < strs.Length; ++sub) {
                                        if (!string.IsNullOrEmpty(strs[sub])) {
                                            strs[sub] = strs[sub].Replace("//", "#");
                                            string[] subStrs = strs[sub].Split('#');
                                            subStrs[0] = subStrs[0].Trim();
                                            resultStr += subStrs[0] + ",";
                                        }
                                    }
                                }
                            }
                        }
                        else {
                            state.ErrorFormat(tableName, "{0}未找到该模板表的定义！！！", tableName);
                        }

                        int count = table.Columns.Count;
                        for (int i = 0; i < count; ++i) {
                            string columnName = table.Columns[i].ColumnName;
                            if (!resultStr.Contains(columnName)) {
                                state.ErrorFormat(tableName, "{0}字段名与程序定义不一致！！！", columnName);
                            }
                            if (columnName.Length == 0) {
                                state.ErrorFormat(tableName, "字段名不能为空！！！");
                            }
                            if (!char.IsUpper(columnName[0])) {
                                state.ErrorFormat(tableName, "{0}字段名首字母必须大写！！！", columnName);
                            }
                            if (columnName.IndexOf("_") != -1) {
                                state.ErrorFormat(tableName, "{0}字段名中不能有下划线！！！", columnName);
                            }
                            if (columnName.IndexOf("ID") != -1) {
                                state.ErrorFormat(tableName, "{0}字段名中“ID”必须写为“Id”！！！", columnName);
                            }
                        }

                        //if ( colume.Length == 0 || !char.IsUpper(colume[0]) || colume.IndexOf("_") != -1 || colume.IndexOf("ID") != -1 )
                        //
                        // delete table if exists
                        //
                        var queryDelete = string.Format("DROP TABLE IF EXISTS '{0}';", tableName);
                        state.LogFormat(tableName, "Drop table by query: {0}, {1}", tableName, queryDelete);
                        using (var dbcmd = dbconn.CreateCommand()) {
                            dbcmd.CommandText = queryDelete;
                            dbcmd.ExecuteNonQuery();
                        }
                        //var rowEffected = dbcmd.ExecuteNonQuery();
                        //dbcmd.Dispose();
                        //( typeof(ExportDataStore) ).LogFormat("DataStoreExporter", "Table deleted: {0} {1}.", rowEffected, tableName);
                        //if ( rowEffected > 0 ) {
                        //    ( typeof(ExportDataStore) ).ErrorFormat("DataStoreExporter", "Table deleted. Duplicate data file exists.{0} {1}.",
                        //        rowEffected, tableName);
                        //    ++errors;
                        //}

                        //dbcmd = dbconn.CreateCommand();
                        //dbcmd.CommandText = string.Format("SELECT name FROM sqlite_master WHERE type = 'table';");
                        //IDataReader reader = dbcmd.ExecuteReader();
                        //while(reader.Read()) {
                        //    ( typeof(ExportDataStore) ).LogFormat("DataStoreExporter","DROP TABLE result: {0}", reader.GetString(0));
                        //}

                        //
                        // create table
                        //
                        var queryCreateTable = BuildCreateTable(ref state, table.Columns, tableName);
                        state.LogFormat(tableName, "Table create by query: {0}, {1}.", tableName, queryCreateTable);
                        using (var dbcmd = dbconn.CreateCommand()) {
                            dbcmd.CommandText = queryCreateTable;
                            dbcmd.ExecuteNonQuery();
                            state.LogFormat(tableName, "Table created. {0}", tableName);
                        }

                        //
                        // insert string and blob
                        //

                        var dbcmds = new List<IDbCommand>();
                        try {
                            dbcmds.Add(new SqliteCommand("begin", dbconn));

                            state.doneRecords[file] = table.Rows.Count;

                            for (var i = 0; i < table.Rows.Count; ++i) {
                                dbcmds.Add(BuildInsertCommand(ref state, dbconn, table.Columns, table.Rows[i], tableName));
                            }
                            dbcmds.Add(new SqliteCommand("end", dbconn));

                            lock (state.dbConn) {
                                foreach (var dbCommand in dbcmds) {
                                    try {
                                        dbCommand.ExecuteNonQuery();
                                    }
                                    catch (Exception ex) {
                                        var sb = new StringBuilder();
                                        foreach (SqliteParameter parameter in dbCommand.Parameters) {
                                            sb.AppendFormat("{0},", parameter.Value);
                                        }
                                        var msg = string.Format("Insert data ExecuteNonQuery failed: index:{0}, {1}\n{2}\n{3}"
                                            , dbcmds.IndexOf(dbCommand) + 1, sb, ex.Message, ex.StackTrace);

                                        if (ex.Message.IndexOf("constraint failed") != -1) {
                                            state.WarnFormat(tableName, msg);
                                        }
                                        else {
                                            state.ErrorFormat(tableName, msg);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex) {

                            state.ErrorFormat(tableName,
                                "Insert data failed. table: {0}, Exception: {1}", tableName,
                                ex.ToString());
                        }
                        finally {
                            foreach (var dbCommand in dbcmds) {
                                dbCommand.Dispose();
                            }
                        }

                        state.LogFormat(tableName, "Insert data to table {0} done!", tableName);
                    }

                    //dbconn.Close();
                }
            }
            catch (Exception ex) {

                state.ErrorFormat(file,
                    "Process excel file {0} failed.\n{1}", file, ex.ToString());
            }

            return true;
        }

        public static void AddVersionNum(string versionPath) {
            var reader = new StreamReader(versionPath, Encoding.Default);
            string content = reader.ReadToEnd();
            reader.Close();

            string[] strs = content.Split('=');
            if (strs.Length > 1) {
                int version = int.Parse(strs[1].Trim());
                version++;
                content = strs[0] + "= " + version;
            }

            var writer = new StreamWriter(versionPath, false, Encoding.Default);
            writer.Write(content);
            writer.Close();
        }

        public static bool GenerateTemplateFile(ref DataStoreExporterState state, string templatePath) {

            if (state.dbConn == null) {
                return false;
            }

            state.LogFormat("", "Generate template file: {0}", templatePath);

            if (File.Exists(templatePath)) {
                state.LogFormat("", "Delete file: {0}", templatePath);
                File.Delete(templatePath);
            }

            var dbcmd = state.dbConn.CreateCommand();
            dbcmd.CommandText = "SELECT name FROM sqlite_master WHERE type = \'table\';";
            var reader = dbcmd.ExecuteReader();
            dbcmd.Dispose();

            var tables = new List<string>();
            while (reader.Read()) {

                var tableName = reader.GetString(0);
                state.LogFormat(tableName, "Add table {0} for generating template.", tableName);

                tables.Add(tableName);
            }

            reader.Dispose();

            var sb = new StringBuilder();
            sb.AppendLine("namespace Assets.Scripts.DataStore {");
            sb.AppendLine("");
            sb.AppendLine("    public enum Common {");
            sb.AppendLine("        AttId");
            sb.AppendLine("    }");

            foreach (var table in tables) {
                dbcmd = state.dbConn.CreateCommand();
                dbcmd.CommandText = string.Format("SELECT * FROM '{0}' LIMIT 0;", table);
                reader = dbcmd.ExecuteReader();
                dbcmd.Dispose();

                state.LogFormat(table, "TABLE result: {0}", reader.FieldCount);

                sb.AppendLine("");
                sb.AppendFormat("    public enum {0} {{{1}", table, Environment.NewLine);

                for (var i = 0; i < reader.FieldCount; ++i) {
                    state.LogFormat(table, "TABLE {0} colume: {1}", table, reader.GetName(i));

                    var colume = reader.GetName(i);
                    if (colume.Length == 0 || !char.IsUpper(colume[0]) || colume.IndexOf("_") != -1 || colume.IndexOf("ID") != -1) {
                        state.ErrorFormat(table, "Table colume should begin with upper char and following the UpperCamelCase naming conventions. table: {0}, colume: {1}", table, reader.GetName(i));

                        colume = colume.Insert(1, char.ToUpper(colume[0]).ToString()).Remove(0, 1).Replace("_", "").Replace("ID", "Id");
                    }

                    sb.AppendFormat((i + 1) != reader.FieldCount ? "        {0},{1}" : "        {0}{1}", colume,
                        Environment.NewLine);
                }

                reader.Dispose();

                sb.AppendLine("    }");
            }

            sb.AppendLine("}");

            File.WriteAllText(templatePath, sb.ToString());

            return true;
        }
    }
}
