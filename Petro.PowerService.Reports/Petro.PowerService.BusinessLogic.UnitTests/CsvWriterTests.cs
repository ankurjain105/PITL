using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Petro.PowerService.BusinessLogic.ReportWriter;
using Shouldly;

namespace Petro.PowerService.BusinessLogic.UnitTests
{

    internal class CsvWriterTests
    {
        [Test]
        public async Task Write_EmptyDataTable()
        {
            //Arrange
            var dt = new DataTable("Test");
            string filePath = @".\EmptyDataTable.csv";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            //Act
            var writer = new CsvWriter();
            await writer.WriteAsync(filePath, dt);

            //Assert
            var fileContent = File.ReadAllText(filePath);
            fileContent.Trim().ShouldBe(string.Empty);
        }

        [Test]
        public async Task Write_NoRows_WritesHeadersOnly()
        {
            //Arrange
            var dt = new DataTable("Test");
            dt.Columns.Add("Column1", typeof(int));
            dt.Columns.Add("Column2", typeof(string));
            string filePath = @".\OnlyColumnsTest.csv";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            //Act
            var writer = new CsvWriter();
            await writer.WriteAsync(filePath, dt);

            //Assert
            var fileContent = File.ReadAllText(filePath);
            fileContent.Trim().ShouldBe("Column1,Column2");
        }

        [Test]
        public async Task Write_WithData_WritesData()
        {
            //Arrange
            var dt = new DataTable("Test");
            dt.Columns.Add("Column1", typeof(int));
            dt.Columns.Add("Column2", typeof(string));
            dt.Columns.Add("Column3", typeof(string));
            string filePath = @".\WithDataTest.csv";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            dt.Rows.Add(1, "Data11", "Data,12");
            dt.Rows.Add(2, "Data21", "Data,22");

            //Act
            var writer = new CsvWriter();
            await writer.WriteAsync(filePath, dt);

            //Assert
            var fileContent = File.ReadAllText(filePath);
            fileContent.Trim().ShouldBe("Column1,Column2,Column3\r\n1,Data11,\"Data,12\"\r\n2,Data21,\"Data,22\"");
        }
    }
}