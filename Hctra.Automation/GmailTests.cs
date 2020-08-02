using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hctra.Automation
{
    [TestFixture]
    public class GmailTests : BaseTest
    {
        public GmailTests()
        {
            _browserType = BrowserType.Firefox;
        }

        [Test, Category("Gmail"), Category("Regression")]
        public void GmailTestCase()
        {
            driver.Navigate().GoToUrl("https://accounts.google.com");
        }
    }
}
