using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace HomeW1.Method
{
    public class CommonMethod
    {
        public static MemoryStream ExportExcelFromDataTable(DataTable dt,string sheetname)
        {
            XLWorkbook workbook = new XLWorkbook();
            MemoryStream memoryStream = new MemoryStream();
            workbook.AddWorksheet(dt, sheetname);
            workbook.SaveAs(memoryStream);
            workbook.Dispose();
            return memoryStream;
        }

        public static DataTable CreateDataTable<T>(IEnumerable<T> entities)
        {
            var dt = new DataTable();

            //creating columns
            foreach (var prop in typeof(T).GetProperties())
            {
                dt.Columns.Add(prop.Name, prop.PropertyType);
            }

            //creating rows
            foreach (var entity in entities)
            {
                var values = GetObjectValues(entity);
                dt.Rows.Add(values);
            }


            return dt;
        }

        public static object[] GetObjectValues<T>(T entity)
        {
            var values = new List<object>();
            foreach (var prop in typeof(T).GetProperties())
            {
                values.Add(prop.GetValue(entity));
            }

            return values.ToArray();
        }
    }
}