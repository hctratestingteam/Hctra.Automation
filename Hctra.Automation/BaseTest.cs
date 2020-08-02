using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Hctra.Automation
{
    public enum BrowserType
    {
        Chrome,
        Firefox

    }

    [TestFixture]
    public class BaseTest
    {
       // public IWebDriver driver;
        public BrowserType _browserType;
        public RemoteWebDriver driver { get; set; }

        public string testName = string.Empty;
        public ExtentTest test = null;
        SetUp setUp = new SetUp();


        [SetUp]
        public virtual void Setup()
        {
            testName = TestContext.CurrentContext.Test.Name.ToString().Replace("(", "").Replace(")", "");
            test = setUp.Extent.CreateTest($"<div>{testName}</div>");

            //var driverPath = AppDomain.CurrentDomain.BaseDirectory;
            //ChromeDriverService service = ChromeDriverService.CreateDefaultService(driverPath);
            //ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--disable-extensions");
            //options.AddArguments("--disable-notifications"); // to disable notification
            //options.AddArguments("--disable-application-cache"); // to disable cache
            //options.AddArguments("test-type");
            //options.AddArguments("no-sandbox");
            //options.AddArguments("--disable-plugins");
            //options.AddArguments("--enable-precise-memory-info");
            //options.AddArguments("--disable-popup-blocking");
            //options.AddArguments("test-type=browser");
            //options.AddAdditionalCapability("useAutomationExtension", false);
            //options.AddUserProfilePreference("credentials_enable_service", false);
            //options.AddUserProfilePreference("profile.password_manager_enabled", false);
            //options.AddExcludedArguments(new List<string>() { "enable-automation" });
            //options.AddExcludedArgument("--ignore-certifcate-errors");
            //driver = new ChromeDriver(service, options);            
            //driver.Manage().Window.Maximize();
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(8000);

            ChooseWebDriver(_browserType);
        }

        public void ChooseWebDriver(BrowserType browserType)
        {
            if (browserType == BrowserType.Chrome)
            {
                var options = new ChromeOptions();
                driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), options);
            }
            else if (browserType == BrowserType.Firefox)
            {
                var options = new FirefoxOptions();
                driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), options);
            }
        }

        [TearDown]
        public virtual void Clean()
        {
            TestStatus status = TestContext.CurrentContext.Result.Outcome.Status;
            Status logstatus;
            switch (status)
            {
                case TestStatus.Failed:
                    logstatus = Status.Fail;
                    test.Log(logstatus, $"<div> <span> Failure Reason: {TestContext.CurrentContext.Result.Message.ToString()}</span> <div style='text-align:center;font-family:courier;'> <span><a style='text-decoration:underline;' href='{""}' target='_blank'>Screen Shot</a></span> </div></div>" + " ");
                    break;
                case TestStatus.Inconclusive:
                    logstatus = Status.Info;
                    test.Log(logstatus);
                    break;
                case TestStatus.Skipped:
                    logstatus = Status.Skip;
                    test.Log(logstatus);
                    break;
                case TestStatus.Passed:
                    logstatus = Status.Pass;
                    break;
            }
            Thread.Sleep(10000);
            if (driver != null)
                driver.Quit();
        }

    }

    [SetUpFixture]
    public class SetUp
    {
        private static ExtentReports _extent;
        public ExtentReports Extent { get { return _extent; } }
        string htmlFile;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var HtmlBaseDirectory = Path.Combine(Directory.GetParent((new FileInfo(AppDomain.CurrentDomain.BaseDirectory).Directory.Parent).Parent.FullName).FullName, "Reports");

            if (!Directory.Exists(HtmlBaseDirectory))
                Directory.CreateDirectory(HtmlBaseDirectory);
            htmlFile = Path.Combine(HtmlBaseDirectory, "index.html");

            var htmlReporter = new ExtentHtmlReporter(htmlFile);
            htmlReporter.Config.Theme = Theme.Dark;
            htmlReporter.Config.DocumentTitle = "Automation Test Result Report";
            htmlReporter.Config.ReportName = "Automation Test Result Report";

            htmlReporter.Config.JS = "$('.brand-logo').text('').append('<img src=D:\\Users\\jloyzaga\\Documents\\FrameworkForJoe\\FrameworkForJoe\\Capgemini_logo_high_res-smaller-2.jpg>')";
            _extent = new ExtentReports();
            _extent.AddSystemInfo("Host Name", Environment.MachineName);
            _extent.AddSystemInfo("User Name", Environment.UserName);
            _extent.AttachReporter(htmlReporter);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            if (_extent != null)
            {
                _extent.Flush();
                //if (!Environment.UserName.ToLower().Contains("tfs_rpt"))
                //    using (Process myProcess = Process.Start("Chrome.exe", string.Format("{0}", htmlFile))) { }
            }
        }
    }
}