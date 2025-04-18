using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using OpenQA.Selenium;
using ParaBank_CSharp.Utilities;

namespace ParaBank_CSharp.Pages
{
    public class AccountOverview
    {

        private readonly IWebDriver driver;
        private readonly WebUtils utils;

        public AccountOverview(IWebDriver driver, WebUtils utils)
        {
            this.driver = driver;
            this.utils = utils;

        }

        private readonly By accountDetails = By.XPath("//h1[contains(text(),'Account Details')]");
        private readonly By accountIdTxt = By.Id("accountId");
        private readonly By accountTypeTxt = By.Id("accountType");
        private readonly By balanceTxt = By.Id("balance");
        private readonly By availableBalanceTxt = By.Id("availableBalance");
        private readonly By transactionDetailTitle = By.XPath("//h1[contains(text(), 'Transaction Details')]");
        private readonly By transactionDetails = By.XPath("//h1[contains(text(), 'Transaction Details')]/following-sibling::table");

        private By AccountLink(string accountNumber) => By.XPath($"//table[@id='accountTable']/descendant::a[text()='{accountNumber}']");
        private By AccountOverviewBalance(string accountNumber) => By.XPath($"//td/a[text()='{accountNumber}']/../following-sibling::td[1]");
        private By AccountOverviewAvailableBalance(string accountNumber) => By.XPath($"//td/a[text()='{accountNumber}']/../following-sibling::td[2]");

        private By TransactionLink(string amount, string transactionType)
        {
            string formattedAmount = TestDataGenerator.ConvertToFloat2Dp(amount);
            return transactionType switch
            {
                "Credit" => By.XPath($"//td[text()='${formattedAmount}']/preceding-sibling::td/a[contains(text(), 'Received')]"),
                "Debit" => By.XPath($"//td[text()='${formattedAmount}']/preceding-sibling::td/a[contains(text(), 'Sent')]"),
                "Down Payment" => By.XPath($"//td[text()='${formattedAmount}']/preceding-sibling::td/a[contains(text(), 'Down Payment')]"),
                _ => throw new ArgumentException("Invalid transaction type")
            };
        }

        private By TransactionDetailField(string field) => By.XPath($"//b[contains(text(), '{field}')]/../following-sibling::td");

        [AllureStep("Verify account details for {accountNumber}")]
        public bool VerifyAccountDetails(string accountNumber, string accountType, double balance, double availableBalance)
        {
            Thread.Sleep(3000);
            if (utils.IsElementVisible(accountDetails))
            {
                string actualAccountNumber = utils.GetTextFromElement(accountIdTxt);
                string actualAccountType = utils.GetTextFromElement(accountTypeTxt);
                string actualBalance = utils.GetTextFromElement(balanceTxt);
                string actualAvailable = utils.GetTextFromElement(availableBalanceTxt);

                return actualAccountNumber == accountNumber &&
                       actualAccountType == accountType &&
                       TestDataGenerator.CompareCurrencyWithNumber(actualBalance, Convert.ToString(balance)) &&
                       TestDataGenerator.CompareCurrencyWithNumber(actualAvailable, Convert.ToString(availableBalance));
            }

            //AttachScreenshot("Account Details Not Visible");
            return false;
        }

        [AllureStep("Verify and click on account number: {accountNumber}")]
        public bool VerifyAndClickAccountNumber(string accountNumber)
        {
            Thread.Sleep(3000);
            if (utils.IsElementVisible(AccountLink(accountNumber)))
            {
                string balance = utils.GetTextFromElement(AccountOverviewBalance(accountNumber));
                string available = utils.GetTextFromElement(AccountOverviewAvailableBalance(accountNumber));

                if (balance == available)
                {
                    utils.ClickOnElement(AccountLink(accountNumber));
                    return true;
                }
            }
            return false;
        }

        [AllureStep("Click transaction for {transactionType} amount: {amount}")]
        public void ClickTransaction(string amount, string transactionType)
        {
            utils.ClickOnElement(TransactionLink(amount, transactionType));
        }

        [AllureStep("Verify transaction: {transaction}, type: {transactionType}, amount: {amount}")]
        public bool VerifyTransactionDetails(string transaction, string transactionType, string amount)
        {
            if (utils.IsElementVisible(transactionDetailTitle))
            {
                string details = utils.GetTextFromElement(transactionDetails);
                string desc = utils.GetTextFromElement(TransactionDetailField("Description"));
                string type = utils.GetTextFromElement(TransactionDetailField("Type"));
                string amt = utils.GetTextFromElement(TransactionDetailField("Amount"));

                //AllureLifecycle.Instance.AddAttachment("Transaction Details", "text/plain", Encoding.UTF8.GetBytes(details));

                return desc.Contains(transaction) &&
                       type == transactionType &&
                       TestDataGenerator.CompareCurrencyWithNumber(amt, amount);
            }

            //AttachScreenshot("Transaction Details Not Visible");
            return false;
        }

        [AllureStep("Get transaction ID")]
        public string GetTransactionId() 
        {
            return utils.GetTextFromElement(TransactionDetailField("Transaction ID"));
        }

        [AllureStep("Get transaction date")]
        public string GetTransactionDate() 
        {
            return utils.GetTextFromElement(TransactionDetailField("Date"));
        }
    }
}
