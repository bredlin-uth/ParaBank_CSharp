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
    public class RequestLoan
    {
        private readonly IWebDriver driver;
        private readonly WebUtils utils;

        public RequestLoan(IWebDriver driver, WebUtils utils)
        {
            this.driver = driver;
            this.utils = utils;

        }

        private readonly By applyForALoanTxt = By.XPath("//h1[contains(text(),'Apply for a Loan')]");
        private readonly By loanAmountTb = By.Id("amount");
        private readonly By downPaymentTb = By.Id("downPayment");
        private readonly By fromAccountIdDd = By.Id("fromAccountId");
        private readonly By applyNowBtn = By.XPath("//input[@value='Apply Now']");
        private readonly By loanRequestProcessedTxt = By.XPath("//h1[contains(text(),'Loan Request Processed')]");
        private readonly By loanRequestApprovedTxt = By.Id("loanRequestApproved");
        private readonly By loanRequestDeniedTxt = By.Id("loanRequestDenied");
        private readonly By newAccountIdTxt = By.Id("newAccountId");
        private readonly By requestLoanErrorTxt = By.Id("requestLoanError");
        private readonly By loanProviderInfoTxt = By.XPath("//td[@id='loanProviderName']/parent::tr");
        private readonly By loanDateInfoTxt = By.XPath("//td[@id='responseDate']/parent::tr");
        private readonly By loanStatusInfoTxt = By.XPath("//td[@id='loanStatus']/parent::tr");
        private readonly By newAccountInfoTxt = By.XPath("//a[@id='newAccountId']/parent::p");

        [AllureStep("Verify the Request Loan page is visible")]
        public bool VerifyRequestLoanPage()
        {
            bool status = utils.IsElementVisible(applyForALoanTxt);
            return status;
        }

        private string SelectFromAccount()
        {
            var fromOptions = utils.GetValuesFromDropdown(fromAccountIdDd);
            utils.SelectByValue(fromAccountIdDd, fromOptions[0]);
            return fromOptions[0];
        }

        [AllureStep("Fill and submit loan request form")]
        public string LoanRequest(string amount, string downPayment)
        {
            utils.EnterTextInField(loanAmountTb, amount);
            utils.EnterTextInField(downPaymentTb, downPayment);
            string fromAccount = SelectFromAccount();
            TestDataGenerator.AttachScreenshot(driver, "Form Filled - Request Loan");
            utils.ClickOnElement(applyNowBtn);
            return fromAccount;
        }

        [AllureStep("Verify loan request completion")]
        public bool VerifyLoanRequestComplete()
        {
            Thread.Sleep(5000);
            if (utils.IsElementVisible(loanRequestProcessedTxt))
            {
                string providerInfo = utils.GetTextFromElement(loanProviderInfoTxt);
                string dateInfo = utils.GetTextFromElement(loanDateInfoTxt);
                string statusInfo = utils.GetTextFromElement(loanStatusInfoTxt);

                if (statusInfo.Contains("Denied")) 
                {
                    TestDataGenerator.AttachText("Loan Request Verification", $"Loan Request Verification: {statusInfo}");
                    return false;
                }
                else
                {
                    string transferredSuccess = utils.GetTextFromElement(loanRequestApprovedTxt);
                    string combinedInfo = string.Join("\n", providerInfo, dateInfo, statusInfo, transferredSuccess);

                    TestDataGenerator.AttachText("Loan Request Verification", combinedInfo);

                    return true;
                 }
                
            }

            return false;
        }

        [AllureStep("Get the newly created account number")]
        public string GetNewAccountNumber()
        {
            return utils.GetTextFromElement(newAccountIdTxt);
        }

        [AllureStep("Click on the new account link")]
        public void ClickOnNewAccount()
        {
            utils.ClickOnElement(newAccountIdTxt);
        }
    }
}
