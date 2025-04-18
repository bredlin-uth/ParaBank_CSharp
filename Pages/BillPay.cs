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
    public class BillPay
    {
        private readonly IWebDriver driver;
        private readonly WebUtils utils;

        public BillPay(IWebDriver driver, WebUtils utils)
        {
            this.driver = driver;
            this.utils = utils;

        }

        private readonly By billPaymentServiceTxt = By.XPath("//h1[contains(text(),'Bill Payment Service')]");
        private readonly By payeeNameTb = By.Name("payee.name");
        private readonly By addressTb = By.Name("payee.address.street");
        private readonly By cityTb = By.Name("payee.address.city");
        private readonly By stateTb = By.Name("payee.address.state");
        private readonly By zipcodeTb = By.Name("payee.address.zipCode");
        private readonly By phoneTb = By.Name("payee.phoneNumber");
        private readonly By accountTb = By.Name("payee.accountNumber");
        private readonly By verifyAccountTb = By.Name("verifyAccount");
        private readonly By amountTb = By.Name("amount");
        private readonly By fromAccountDd = By.Name("fromAccountId");
        private readonly By sendPaymentBtn = By.XPath("//input[@value='Send Payment']");
        private readonly By billPaymentCompleteTxt = By.XPath("//h1[contains(text(),'Bill Payment Complete')]");
        private readonly By billPaymentSuccessMsg = By.XPath("//h1[contains(text(),'Bill Payment Complete')]/following-sibling::p");
        private readonly By payeeNameTxt = By.Id("payeeName");
        private readonly By amountTxt = By.Id("amount");
        private readonly By fromAccountIdTxt = By.Id("fromAccountId");

        [AllureStep("Verify the Bill Pay page is visible")]
        public bool VerifyBillPayPage()
        {
            bool status = utils.IsElementVisible(billPaymentServiceTxt);
            return status;
        }

        private string SelectFromAccount()
        {
            var options = utils.GetValuesFromDropdown(fromAccountDd);
            utils.SelectByValue(fromAccountDd, options[0]);
            return options[0];
        }

        [AllureStep("Fill and submit bill payment form")]
        public string PayBill(string name, string address, string city, string state, string zipcode, string phone, string account, string amount)
        {
            utils.EnterTextInField(payeeNameTb, name);
            utils.EnterTextInField(addressTb, address);
            utils.EnterTextInField(cityTb, city);
            utils.EnterTextInField(stateTb, state);
            utils.EnterTextInField(zipcodeTb, zipcode);
            utils.EnterTextInField(phoneTb, phone);
            utils.EnterTextInField(accountTb, account);
            utils.EnterTextInField(verifyAccountTb, account);
            utils.EnterTextInField(amountTb, amount);

            string fromAccount = SelectFromAccount();
            utils.SelectByVisibleText(fromAccountDd, fromAccount);

            TestDataGenerator.AttachScreenshot(driver, "Form Filled - Bill Pay");
            utils.ClickOnElement(sendPaymentBtn);

            return fromAccount;
        }

        [AllureStep("Verify transfer completion")]
        public bool VerifyTransferComplete(string name, string amount, string fromAccount)
        {
            Thread.Sleep(3000); 
            if (utils.IsElementVisible(billPaymentCompleteTxt))
            {
                string nameResult = utils.GetTextFromElement(payeeNameTxt);
                string amountResult = utils.GetTextFromElement(amountTxt);
                string fromAccountResult = utils.GetTextFromElement(fromAccountIdTxt);
                string successMessage = utils.GetTextFromElement(billPaymentSuccessMsg);

                TestDataGenerator.AttachText("Success Message", successMessage);

                return nameResult == name && fromAccountResult == fromAccount && amountResult.Contains(amount) && TestDataGenerator.CompareCurrencyWithNumber(amountResult, amount);
            }
            return false;
        }
    }
}
