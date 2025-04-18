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
    public class OpenNewAccount
    {
        private readonly IWebDriver driver;
        private readonly WebUtils utils;

        public OpenNewAccount(IWebDriver driver, WebUtils utils)
        {
            this.driver = driver;
            this.utils = utils;

        }
        private readonly By type_of_account_dd = By.XPath("//select[@id='type']");
        private readonly By from_account_dd = By.XPath("//select[@id='fromAccountId']");
        private readonly By button_open_newaccount = By.XPath("//input[@value='Open New Account']");
        private readonly By new_account_id_after_opening_account = By.XPath("//a[@id='newAccountId']");

        [AllureStep("Open New Account in the application")]
        public void OpenAccount(string typeOfAccount)
        {
            try
            {
                utils.SelectByVisibleText(type_of_account_dd, typeOfAccount);
                utils.SelectByIndex(from_account_dd, 0);
                TestDataGenerator.AttachScreenshot(driver, "Opened New Account");
                utils.ClickOnElement(button_open_newaccount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while opening account: {ex.Message}");
                throw;
            }
        }

        public string GetNewlyCreatedAccountNumber()
        {
            return utils.GetTextFromElement(new_account_id_after_opening_account);
        }
    }
}
