using System;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using ParaBank_CSharp.Utilities;
using Allure.NUnit.Attributes;

namespace ParaBank_CSharp.Pages
{
    public class FindTransactions
    {
        private readonly IWebDriver driver;
        private readonly WebUtils utils;
        private string transactionIdText;
        private string transactionAmountText;

        private readonly By findTransactionLinkText = By.LinkText("Find Transactions");
        private readonly By findTransactionById = By.Id("transactionId");
        private readonly By findTransactionByIdBtn = By.Id("findById");
        private readonly By findTransactionDateId = By.Id("transactionDate");
        private readonly By findTransactionDateIdBtn = By.Id("findByDate");
        private readonly By findTransactionDateRangeBetweenId = By.Id("fromDate");
        private readonly By findTransactionDateRangeToId = By.Id("toDate");
        private readonly By findTransactionDateRangeIdBtn = By.Id("findByDateRange");
        private readonly By findTransactionAmountId = By.Id("amount");
        private readonly By findTransactionAmountIdBtn = By.Id("findByAmount");
        private readonly By findTransactionTableId = By.Id("transactionTable");
        private readonly By findTransactionErrorXPath = By.XPath("//p[@class='error']");
        private readonly By fundsTransfer = By.XPath("//a[contains(text(),'Funds Transfer Sent') or contains(text(),'Funds Transfer Received')]");
        private readonly By transactionIdTextXPath = By.XPath("//b[normalize-space()='Transaction ID:']/parent::td/following-sibling::td");
        private readonly By transactionAmountTextXPath = By.XPath("//b[normalize-space()= 'Amount:']/parent::td/following-sibling::td");

        public FindTransactions(IWebDriver driver, WebUtils utils)
        {
            this.driver = driver;
            this.utils = utils;
        }

        public (string, string) FindTransactionInMultipleWays(string findTransactionValue, string transactionValues)
        {
            try
            {
                var findTransactionDict = new System.Collections.Generic.Dictionary<string, Action<string>>
                {
                    { "find_transaction_id", FindTransactionUsingId },
                    { "find_transaction_date", FindTransactionUsingDate },
                    { "find_transaction_daterange", FindTransactionUsingDateRange },
                    { "find_transaction_amount", FindTransactionUsingAmount }
                };

                utils.ClickOnElement(findTransactionLinkText);

                if (findTransactionDict.TryGetValue(findTransactionValue.ToLower(), out var func))
                {
                    func(transactionValues);
                    VerifyTransactionHistoryPresent();
                    return (transactionIdText, transactionAmountText);
                }
                else
                {
                    throw new ArgumentException($"Invalid transaction type: {findTransactionValue}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while finding transaction: ", ex);
            }
            finally
            {
                utils.ClickOnElement(findTransactionLinkText);
            }
        }

        private void ClickOnFindTransaction()
        {
            utils.ClickOnElement(findTransactionLinkText);
        }

        #region Transaction ID Methods
        public void EnterTransactionId(string transactionId)
        {
            utils.EnterTextInField(findTransactionById, transactionId);
        }

        public void ClickOnFindTransactionIdBtn()
        {
            utils.ClickOnElement(findTransactionByIdBtn);
        }

        public void FindTransactionUsingId(string transactionId)
        {
            EnterTransactionId(transactionId);
            ClickOnFindTransactionIdBtn();
        }
        #endregion

        #region Transaction Date Methods
        public void EnterTransactionDate(string transactionDate)
        {
            utils.EnterTextInField(findTransactionDateId, transactionDate);
        }

        public void ClickOnFindTransactionDateBtn()
        {
            utils.ClickOnElement(findTransactionDateIdBtn);
        }

        public void FindTransactionUsingDate(string transactionDate)
        {
            EnterTransactionDate(transactionDate);
            ClickOnFindTransactionDateBtn();
        }
        #endregion

        #region Transaction Date Range Methods
        public void EnterTransactionDateRangeBetween(string transactionDateRangeBetween)
        {
            utils.EnterTextInField(findTransactionDateRangeBetweenId, transactionDateRangeBetween);
        }

        public void EnterTransactionDateRangeTo(string transactionDateRangeTo)
        {
            utils.EnterTextInField(findTransactionDateRangeToId, transactionDateRangeTo);
        }

        public void ClickOnFindTransactionDateRangeBtn()
        {
            utils.ClickOnElement(findTransactionDateRangeIdBtn);
        }

        public void FindTransactionUsingDateRange(string transactionDateRangeValues)
        {
            var dateRange = transactionDateRangeValues.Split(',');
            EnterTransactionDateRangeBetween(dateRange[0]);
            EnterTransactionDateRangeTo(dateRange[1]);
            ClickOnFindTransactionDateRangeBtn();
        }
        #endregion

        #region Transaction Amount Methods
        public void EnterTransactionAmount(string transactionAmount)
        {
            utils.EnterTextInField(findTransactionAmountId, transactionAmount);
        }

        public void ClickOnFindTransactionAmountBtn()
        {
            utils.ClickOnElement(findTransactionAmountIdBtn);
        }

        public void FindTransactionUsingAmount(string transactionAmount)
        {
            EnterTransactionAmount(transactionAmount);
            ClickOnFindTransactionAmountBtn();
        }
        #endregion

        public void VerifyTransactionHistoryPresent()
        {
            transactionIdText = null;
            transactionAmountText = null;

            if (utils.IsElementVisible(findTransactionTableId))
            {
                if (utils.IsElementVisible(fundsTransfer))
                {
                    utils.ClickOnElement(fundsTransfer);
                    transactionIdText = Regex.Replace(utils.GetTextFromElement(transactionIdTextXPath), @"[^\d.]", "");
                    transactionAmountText = Regex.Replace(utils.GetTextFromElement(transactionAmountTextXPath), @"[^\d.]", "");
                }
            }
            else if (utils.IsElementVisible(findTransactionErrorXPath))
            {
                var errorText = utils.GetTextFromElement(findTransactionErrorXPath);
                throw new Exception($"Error: {errorText}");
            }
        }
    }
}
