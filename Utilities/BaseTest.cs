using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Allure.Net.Commons;
using Allure.NUnit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ParaBank_CSharp.Utilities
{
    [TestFixture]
    [AllureNUnit]
    public class BaseTest
    {
        protected IWebDriver Driver { get; private set; }
        public WebUtils Utils { get; private set; }

        [OneTimeSetUp]
        public void Setup()
        {
            AllureLifecycle.Instance.CleanupResultDirectory();

            Driver = new ChromeDriver();
            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            Driver.Navigate().GoToUrl("https://parabank.parasoft.com/parabank/index.htm");
            Driver.Manage().Window.Maximize();
            Utils = new WebUtils(Driver);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Driver?.Quit();
            Driver?.Dispose();
        }
    }
}
