using Allure.NUnit.Attributes;
using ParaBank_CSharp.Pages;
using ParaBank_CSharp.Utilities;

namespace ParaBank_CSharp.Tests
{
    [TestFixture]
    public class Tests_ParaBank : BaseTest 
    {
        Dictionary<string, string>? userData;

        [Test, Order(1)]
        [AllureStep("Register To Application")]
        public void TestRegister()
        {
            userData = TestDataGenerator.GenerateRandomUserForRegistration();
            Register registerPage = new Register(Driver, Utils);
            registerPage.RegisterToApplication(
                userData["fname"],
                userData["lname"],
                userData["address"],
                userData["city"],
                userData["state"],
                userData["zipcode"],
                userData["phone"],
                userData["ssn"],
                userData["uname"],
                userData["pwd"],
                userData["pwd1"]
            );

            // Verification: Verify registration success
            AccountServices accountServicesPage = new AccountServices(Driver, Utils);
            Assert.IsTrue(accountServicesPage.VerifyAccountIsRegistered(userData["uname"]));

            // Log out
            accountServicesPage.LogOutFromApplication();
        }

        [Test, Order(2)]
        [AllureStep("Login To Application")]
        public void LoginToTheParaBankApplication()
        {
            Login loginpage = new Login(Driver, Utils);
            loginpage.LoginToApplication(userData["uname"], userData["pwd"]);
            AccountServices accountServicesPage = new AccountServices(Driver, Utils);
            Assert.IsTrue(accountServicesPage.VerifyAccountIsLoggedIn(userData["fname"]));
        }

        [Test, Order(4)]
        [AllureStep("Bill Pay")]
        public void BillPayTest()
        {
            AccountServices accountServicesPage = new AccountServices(Driver, Utils);
            accountServicesPage.NavigateToAccountService("Bill Pay");

            BillPay billPayPage = new BillPay(Driver, Utils);
            Dictionary<string, string> payee = TestDataGenerator.GenerateRandomDetailsForPayBill("2");

            Assert.IsTrue(billPayPage.VerifyBillPayPage());
            string fromAccount = billPayPage.PayBill(payee["name"], payee["address"], payee["city"], payee["state"], payee["zipcode"], payee["phone"], payee["account"], payee["amount"]);
            Assert.IsTrue(billPayPage.VerifyTransferComplete(payee["name"], payee["amount"], fromAccount));
        }

        [Test, Order(5)]
        [AllureStep("Transfer Funds")]
        public void TransferFundsTest()
        {
            AccountServices accountServicesPage = new AccountServices(Driver, Utils);
            accountServicesPage.NavigateToAccountService("Transfer Funds");

            TransferFunds transferPage = new TransferFunds(Driver, Utils);
            Assert.IsTrue(transferPage.VerifyTransferFundsPage());
            var (fromAccount, toAccount) = transferPage.FundTransfer("3");
            Assert.IsTrue(transferPage.VerifyTransferComplete("3", fromAccount, toAccount));

            AccountOverview accountOverviewPage = new AccountOverview(Driver, Utils);
            accountServicesPage.NavigateToAccountService("Accounts Overview");
            Assert.IsTrue(accountOverviewPage.VerifyAndClickAccountNumber(fromAccount));
            accountOverviewPage.ClickTransaction("3", "Debit");
            accountOverviewPage.VerifyTransactionDetails("Funds Transfer Sent", "Debit", "3");
            string transactionId = accountOverviewPage.GetTransactionId();

            accountServicesPage.NavigateToAccountService("Accounts Overview");
            Assert.IsTrue(accountOverviewPage.VerifyAndClickAccountNumber(toAccount));
            accountOverviewPage.ClickTransaction("3", "Credit");
            accountOverviewPage.VerifyTransactionDetails("Funds Transfer Received", "Credit", "3");
        }

        [Test, Order(6)]
        [AllureStep("Request Loan")]
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