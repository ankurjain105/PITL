using System;
using System.IO;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Petro.PowerService.BusinessLogic;
using Petro.PowerService.BusinessLogic.Configuration;
using Petro.PowerService.BusinessLogic.Core;
using Petro.PowerService.Host;

namespace Petro.PowerService.Reports.IntegrationTests
{
    public class Tests
    {

        [SetUp]
        public void Setup()
        {
            ServiceLocator.Configure();
            var config = ServiceLocator.Resolve<IServiceConfiguration>();
            if (Directory.Exists(config.ReportPath))
            {
                Directory.Delete(config.ReportPath, recursive: true);
            }

            Directory.CreateDirectory(config.ReportPath);
        }

        [TearDown]
        public void TearDown()
        {
            var config = ServiceLocator.Resolve<IServiceConfiguration>();
            Directory.Delete(config.ReportPath, recursive: true);
        }

        [Test]
        public async Task Test_Run()
        {
            var config = ServiceLocator.Resolve<IServiceConfiguration>();

            //Mock Clock
            var clock = new Mock<IClock>();
            clock.Setup(x => x.Now).Returns(new DateTime(2019, 09, 19, 12, 30, 28));
            ServiceLocator.Replace<IClock>(clock.Object);
            var before = Directory.GetFiles(config.ReportPath, "PowerPosition_20190919_1230.csv");
            Assert.AreEqual(0, before.Length);

            //Act
            var service = ServiceLocator.Resolve<IReportGenerator>();
            await service.GenerateAsync();

            //Assert
           
            var files = Directory.GetFiles(config.ReportPath, "PowerPosition_20190919_1230.csv");
            Assert.AreEqual(1, files.Length);

            var fileContent = File.ReadAllLines(Path.Combine(config.ReportPath, "PowerPosition_20190919_1230.csv"));
            Assert.AreEqual(25, fileContent.Length);
        }
    }
}