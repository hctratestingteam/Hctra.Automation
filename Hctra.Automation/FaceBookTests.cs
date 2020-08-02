using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Hctra.Automation
{
    public class FaceBookTests : BaseTest
    {
        public FaceBookTests()
        {
            _browserType = BrowserType.Chrome;
        }

        IWebElement userName => driver.FindElement(By.Id("email"));
        IWebElement pass => driver.FindElement(By.Id("pass"));
        IWebElement button => driver.FindElement(By.Id("loginbutton"));

        [Test, Category("FB"), Category("Regression")]
        public void FacebookTestCase()
        {
            driver.Navigate().GoToUrl("https://www.facebook.com");
            userName.SendKeys("email@gmail.com");
            pass.SendKeys("password");
            button.Click();
        }
       



    }
}
