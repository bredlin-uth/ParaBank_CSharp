using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using OpenQA.Selenium;
using ParaBank_CSharp.Utilities;

namespace ParaBank_CSharp.Pages
{
    public class AccountServices
    {
        private readonly IWebDriver driver;
        private readonly WebUtils utils;

        public AccountServices(IWebDriver driver, WebUtils utils)
        {
            this.driver = driver;
            this.utils = utils;

        }

        private readonly By logOutLnk = By.LinkText("Log Out");
        private readonly By registerSuccessMsg = By.XPath("//div[@id='rightPanel']");
        private readonly By registerUsernameMsg = By.XPath("//div[@id='rightPanel']/h1[@class='title']");
        private readonly By loginSuccessMsg = By.XPath("//p[@class='smallText']");
        private readonly By accountOpenedSuccessMsg = By.XPath("//div[@id='openAccountResult']");

        private By AccountService(string service)
        {
            return By.XPath($"//a[contains(text(),'{service}')]");
        }

        [AllureStep("Navigate to the account service: {service}")]
        public void NavigateToAccountService(string service)
        {
            utils.ClickOnElement(AccountService(service));
        }

        [AllureStep("Verify account is registered with username: {name}")]
        public bool VerifyAccountIsRegistered(string uname)
        {
            Thread.Sleep(3000);
            if (utils.IsElementVisible(registerSuccessMsg))
            {
                string statusOfRegister = utils.GetTextFromElement(registerSuccessMsg);
                string usernameResult = utils.GetTextFromElement(registerUsernameMsg);
                TestDataGenerator.AttachText("Verify account is registered", $"Verify account is registered: {statusOfRegister}");

                return usernameResult.Contains(uname);
            }
            return false;
        }

        [AllureStep("Log out from application")]
        public void LogOutFromApplication()
        {
            utils.ClickOnElement(logOutLnk);
        }

        [AllureStep("Verify account is logged in with first name: {fname}")]
        public bool VerifyAccountIsLoggedIn(string fname)
        {
            Thread.Sleep(3000);
            if (utils.IsElementVisible(loginSuccessMsg))
            {
                string loginMessage = utils.GetTextFromElement(loginSuccessMsg);
                TestDataGenerator.AttachText("Verify account Log in", $"Verify account is logged in: {loginMessage}");

                return loginMessage.Contains(fname);
            }
            return false;
        }

        [AllureStep("Verify new {typeOfAccount} account is opened")]
        public bool VerifyNewAccountOpening(string typeOfAccount)
        {
            Thread.Sleep(3000);
            if (utils.IsElementVisible(accountOpenedSuccessMsg))
            {
                string openingStatus = utils.GetTextFromElement(accountOpenedSuccessMsg);

                TestDataGenerator.AttachText("open account status", $"Verify new {typeOfAccount} account is opened: {openingStatus}");

                return true;
            }
            return false;
        }
    }
}
