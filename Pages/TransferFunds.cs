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
    public class TransferFunds
    {
        private readonly IWebDriver driver;
        private readonly WebUtils utils;

        public TransferFunds(IWebDriver driver, WebUtils utils)
        {
            this.driver = driver;
            this.utils = utils;

        }

        private readonly By transferFundsTxt = By.XPath("//h1[contains(text(),'Transfer Funds')]");
        private readonly By accountEmptyMsg = By.XPath("//p[@id ='amount.errors' and contains(text(),'The amount cannot be empty.')]");
        private readonly By enterValidAmountMsg = By.XPath("//p[@id ='amount.errors' and contains(text(),'Please enter a valid amount.')]");
        private readonly By amountTb = By.Id("amount");
        private readonly By fromAccountDd = By.Id("fromAccountId");
        private readonly By toAccountDd = By.Id("toAccountId");
        private readonly By transferBtn = By.XPath("//input[@value='Transfer']");
        private readonly By transferCompleteTxt = By.XPath("//h1[contains(text(),'Transfer Complete!')]");
        private readonly By transferredSuccessMsg = By.XPath("//h1[contains(text(),'Transfer Complete!')]/following-sibling::p");
        private readonly By amountResultTxt = By.Id("amountResult");
        private readonly By fromAccountIdResultTxt = By.Id("fromAccountIdResult");
        private readonly By toAccountIdResultTxt = By.Id("toAccountIdResult");

        [AllureStep("Verify the Transfer Funds page is visible")]
        public bool VerifyTransferFundsPage()
        {
            bool status = utils.IsElementVisible(transferFundsTxt);
            return status;
        }

        private (string fromAccount, string toAccount) SelectFromAndToAccounts()
        {
            var fromOptions =  utils.GetValuesFromDropdown(fromAccountDd);
            utils.SelectByValue(fromAccountDd, fromOptions[0]);
            var toOptions = utils.GetValuesFromDropdown(toAccountDd);
            utils.SelectByValue(toAccountDd, toOptions[0]);

            return (fromOptions[0], toOptions[0]);
        }

        [AllureStep("Fill and submit transfer funds form")]
        public (string fromAccount, string toAccount) FundTransfer(string amount)
        {
            utils.EnterTextInField(amountTb, amount);
            var (fromAccount, toAccount) = SelectFromAndToAccounts();

            TestDataGenerator.AttachScreenshot(driver, "Form Filled - Transfer Funds");
            utils.ClickOnElement(transferBtn);

            return (fromAccount, toAccount);
        }

        [AllureStep("Verify fund transfer completion")]
        public bool VerifyTransferComplete(string amount, string fromAccount, string toAccount)
        {
            Thread.Sleep(3000);

            if (utils.IsElementVisible(transferCompleteTxt))
            {
                string resultAmount = utils.GetTextFromElement(amountResultTxt);
                string fromResult = utils.GetTextFromElement(fromAccountIdResultTxt);
                string toResult = utils.GetTextFromElement(toAccountIdResultTxt);
                string successMessage = utils.GetTextFromElement(transferredSuccessMsg);
                
                TestDataGenerator.AttachText("Transfer Success Message", successMessage);

                return resultAmount.Contains(amount) && fromResult == fromAccount && toResult == toAccount;
            }
            return false;
        }
    }
}
