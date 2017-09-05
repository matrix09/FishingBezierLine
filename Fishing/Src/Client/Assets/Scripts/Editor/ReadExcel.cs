using System.Data;
using System.Data.Odbc;
using System.Deployment.Internal;
using System.IO;
using System.Text;
using Assets.Scripts.Helpers;
using Excel;
using UnityEngine;
using Assets.Scripts.ConsoleController.Console;

namespace Assets.Scripts {
    public class ReadExcel : MonoBehaviour {

        // Use this for initialization
        public void Start() {
            //readXLS(Application.dataPath + "/Book1.xls");
            //ReadExcelFile(Application.dataPath + "/Book2007.xlsx");
            ReadExcelFile(@"D:\sandbox\VSProjects\ExportData\DataFiles\战斗公式参数\BattleTemplate.xlsm");
        }

        private void ReadExcelFile( string filePath ) {
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

            ////1. Reading from a binary Excel file ('97-2003 format; *.xls)
            //IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);

            //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            ////3. DataSet - The result of each spreadsheet will be created in the result.Tables
            //DataSet result = excelReader.AsDataSet();

            ////4. DataSet - Create column names from first row
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();

            ////5. Data Reader methods
            //do {
            //    this.LogFormat("", "excelReader.ExceptionMessage: {0}", excelReader.ExceptionMessage);
            //    this.LogFormat("", "excelReader.IsFirstRowAsColumnNames: {0}", excelReader.IsFirstRowAsColumnNames);
            //    this.LogFormat("", "excelReader.IsValid: {0}", excelReader.IsValid);
            //    this.LogFormat("", "excelReader.Name: {0}", excelReader.Name);
            //    this.LogFormat("", "excelReader.ResultsCount: {0}", excelReader.ResultsCount);
            //    this.LogFormat("", "excelReader.FieldCount: {0}", excelReader.FieldCount);
            //    this.LogFormat("", "excelReader.Depth: {0}", excelReader.Depth);
            //    this.LogFormat("", "excelReader.IsClosed: {0}", excelReader.IsClosed);

            //    //while ( excelReader.Read() ) {

            //    //    this.LogFormat("", "excelReader.FieldCount: {0}, Name: {1}", excelReader.FieldCount, excelReader.Name);

            //    //    this.LogFormat("", "excelReader.ExceptionMessage: {0}", excelReader.ExceptionMessage);
            //    //    this.LogFormat("", "excelReader.IsFirstRowAsColumnNames: {0}", excelReader.IsFirstRowAsColumnNames);
            //    //    this.LogFormat("", "excelReader.IsValid: {0}", excelReader.IsValid);
            //    //    this.LogFormat("", "excelReader.Name: {0}", excelReader.Name);
            //    //    this.LogFormat("", "excelReader.ResultsCount: {0}", excelReader.ResultsCount);
            //    //    this.LogFormat("", "excelReader.FieldCount: {0}", excelReader.FieldCount);
            //    //    this.LogFormat("", "excelReader.Depth: {0}", excelReader.Depth);
            //    //    this.LogFormat("", "excelReader.IsClosed: {0}", excelReader.IsClosed);
            //    //    //this.LogFormat("", "excelReader.ExceptionMessage: {0}", excelReader.RecordsAffected);

            //    //    for ( int i = 0; i < excelReader.FieldCount; ++i ) {
            //    //        this.LogFormat("", "GetName: {0}, {1}", i, excelReader.GetString(i));
            //    //    }
            //    //}
            //} while ( excelReader.NextResult() );




            //6. Free resources (IExcelDataReader is IDisposable)
            excelReader.Close();

            this.LogFormat("", "tables count: {0}", result.Tables.Count);

            var sb = new StringBuilder();

            foreach ( DataTable table in result.Tables ) {

                sb.Remove(0, sb.Length);
                foreach ( DataColumn column in table.Columns ) {

                    sb.AppendFormat("{0}{1}, ", column.Caption, column.DataType);
                }

                this.LogFormat("", "columns in table: {0}, {1}", table.TableName, sb.ToString());


                foreach ( DataRow row in table.Rows ) {
                    sb.Remove(0, sb.Length);
                    for ( int i = 0; i < row.ItemArray.Length; ++i ) {
                        sb.AppendFormat("{0}, ", row[i].ToString());
                    }
                    this.LogFormat("", "    row in colume: {0}", sb.ToString());
                }
            }
        }

        void readXLS( string filetoread ) {
            // Must be saved as excel 2003 workbook, not 2007, mono issue really
            string con = "Driver={Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)}; DriverId=1046; Dbq=" + filetoread + ";";
            Debug.Log(con);
            string yourQuery = "SELECT * FROM [Sheet1$]";
            // our odbc connector 
            OdbcConnection oCon = new OdbcConnection(con);
            // our command object 
            OdbcCommand oCmd = new OdbcCommand(yourQuery, oCon);
            // table to hold the data 
            DataTable dtYourData = new DataTable("YourData");
            // open the connection 
            oCon.Open();
            // lets use a datareader to fill that table! 
            OdbcDataReader rData = oCmd.ExecuteReader();
            // now lets blast that into the table by sheer man power! 
            dtYourData.Load(rData);
            // close that reader! 
            rData.Close();
            // close your connection to the spreadsheet! 
            oCon.Close();
            // wow look at us go now! we are on a roll!!!!! 
            // lets now see if our table has the spreadsheet data in it, shall we? 

            if ( dtYourData.Rows.Count > 0 ) {
                // do something with the data here 
                // but how do I do this you ask??? good question! 
                for ( int i = 0; i < dtYourData.Rows.Count; i++ ) {
                    // for giggles, lets see the column name then the data for that column! 
                    Debug.Log(dtYourData.Columns[0].ColumnName + " : " + dtYourData.Rows[i][dtYourData.Columns[0].ColumnName].ToString() +
                              "  |  " + dtYourData.Columns[1].ColumnName + " : " + dtYourData.Rows[i][dtYourData.Columns[1].ColumnName].ToString() +
                              "  |  " + dtYourData.Columns[2].ColumnName + " : " + dtYourData.Rows[i][dtYourData.Columns[2].ColumnName].ToString());
                }
            }
        }
    }
}