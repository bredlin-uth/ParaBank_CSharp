using Allure.NUnit.Attributes;
using ParaBank_CSharp.Pages;
using ParaBank_CSharp.Utilities;

namespace ParaBank_CSharp.Tests
{
    [TestFixture]
    public class Tests_ParaBank : BaseTest 
    {


        [Test, Order(4)]
        public void BillPayTest()
        {
            AccountServices accountServices = new AccountServices(Driver, Utils);
            accountServices.NavigateToAccountService("Bill Pay");

            BillPay billPayPage = new BillPay(Driver, Utils);
            Dictionary<string, string> payee = TestDataGenerator.GenerateRandomBillPayData("2");

            Assert.IsTrue(billPayPage.VerifyBillPayPage());
            string fromAccount = billPayPage.PayBill(payee["name"], payee["address"], payee["city"], payee["state"], payee["zipcode"], payee["phone"], payee["account"], payee["amount"]);
            Assert.IsTrue(billPayPage.VerifyTransferComplete(payee["name"], payee["amount"], fromAccount));
        }

        [Test, Order(5)]
        public void TransferFundsTest()
        {
            AccountServices accountServicesPage = new AccountServices(Driver, Utils);
            accountServicesPage.NavigateToAccountService("Transfer Funds");

            TransferFunds transferPage = new TransferFunds(Driver, Utils);
            Assert.IsTrue(transferPage.VerifyTransferFundsPage());
            var (fromAccount, toAccount) = transferPage.FundTransfer("3");
            Assert.IsTrue(transferPage.VerifyTransferComplete("3", fromAccount, toAccount));

            AccountOverview accountOverview = new AccountOverview(Driver, Utils);
            accountServicesPage.NavigateToAccountService("Accounts Overview");
            Assert.IsTrue(accountOverview.VerifyAndClickAccountNumber(fromAccount));
            accountOverview.ClickTransaction("3", "Debit");
            accountOverview.VerifyTransactionDetails("Funds Transfer Sent", "Debit", "3");
            transactionId = accountOverview.GetTransactionId();

            accountServicesPage.NavigateToAccountService("Accounts Overview");
            Assert.IsTrue(accountOverview.VerifyAndClickAccountNumber(toAccount));
            accountOverview.ClickTransaction("3", "Credit");
            accountOverview.VerifyTransactionDetails("Funds Transfer Received", "Credit", "3");
        }

        [Test, Order(6)]
        public void RequestLoanTest()
        {
            AccountServices accountServicesPage = new AccountServices(Driver, Utils);
            accountServicesPage.NavigateToAccountService("Request Loan");

            RequestLoan requestLoanPage = new RequestLoan(Driver, Utils);
            Assert.IsTrue(requestLoanPage.VerifyRequestLoanPage());
            string fromAccount = requestLoanPage.LoanRequest("10", "2");
            Assert.IsTrue(requestLoanPage.VerifyLoanRequestComplete());
            string newAccount = requestLoanPage.GetNewAccountNumber();

            requestLoanPage.ClickOnNewAccount();
            AccountOverview accountOverviewPage = new AccountOverview(Driver, Utils);
            accountOverviewPage.VerifyAccountDetails(newAccount, "LOAN", 10, 10);

            accountServicesPage.NavigateToAccountService("Accounts Overview");
            Assert.IsTrue(accountOverviewPage.VerifyAndClickAccountNumber(fromAccount));
            accountOverviewPage.ClickTransaction("2", "Down Payment");
            accountOverviewPage.VerifyTransactionDetails("Down Payment for Loan", "Debit", "2");
        }

    }
}