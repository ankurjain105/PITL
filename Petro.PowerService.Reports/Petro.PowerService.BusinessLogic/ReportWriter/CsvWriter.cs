using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Petro.PowerService.BusinessLogic.ReportWriter
{
    public class CsvWriter : IReportWriter
    {
        public async Task WriteAsync(string filePath, DataTable data)
        {
            using StreamWriter sw = new StreamWriter(filePath, false);
            //headers    
            for (int i = 0; i < data.Columns.Count; i++)
            {
                await sw.WriteAsync(data.Columns[i].ColumnName);
                if (i < data.Columns.Count - 1)
                {
                    await sw.WriteAsync(",");
                }
            }
            await sw.WriteAsync(sw.NewLine);
            foreach (DataRow dr in data.Rows)
            {
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value?.Contains(",") == true)
                        {
                            value = $"\"{value}\"";
                            await sw.WriteAsync(value);
                        }
                        else
                        {
                            await sw.WriteAsync(dr[i].ToString());
                        }
                    }
                    if (i < data.Columns.Count - 1)
                    {
                        await sw.WriteAsync(",");
                    }
                }
                await sw.WriteAsync(sw.NewLine);
            }
            sw.Close();
        }
    }
}