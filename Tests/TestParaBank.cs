using Allure.NUnit.Attributes;
using ParaBank_CSharp.Pages;
using ParaBank_CSharp.Utilities;


namespace ParaBank_CSharp.Tests
{
    [TestFixture]
    public class Tests_ParaBank : BaseTest 
    {
        Dictionary<string, string>? userData;
        Dictionary<string, string>? updateuserData;
        private string transactionId;
        private string transactionAmount;

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

            AccountServices accountServicesPage = new AccountServices(Driver, Utils);
            Assert.IsTrue(accountServicesPage.VerifyAccountIsRegistered(userData["uname"]));

            accountServicesPage.LogOutFromApplication();
        }

        [Test, Order(2)]
        [AllureStep("Login To Application")]
        public void TestLoginToTheParaBankApplication()
        {
            Login loginpage = new Login(Driver, Utils);
            loginpage.LoginToApplication(userData["uname"], userData["pwd"]);
            AccountServices accountServicesPage = new AccountServices(Driver, Utils);
            Assert.IsTrue(accountServicesPage.VerifyAccountIsLoggedIn(userData["fname"]));
        }

        [Test, Order(3)]
        [AllureStep("Open New Account")]
        public void TestOpenNewAccount()
        {
            OpenNewAccount accountDetailsPage = new OpenNewAccount(Driver, Utils);
            AccountOverview accountOverviewPage = new AccountOverview(Driver, Utils);
            AccountServices accountServicesPage = new AccountServices(Driver, Utils);

            foreach (var accountType in new[] { "CHECKING", "SAVINGS" })
            {
                accountServicesPage.NavigateToAccountService("Open New Account");
                accountDetailsPage.OpenAccount(accountType);
                Assert.IsTrue(accountServicesPage.VerifyNewAccountOpening(accountType));

                var newAccountNumber = accountDetailsPage.GetNewlyCreatedAccountNumber();
                accountServicesPage.NavigateToAccountService("Accounts Overview");
                Assert.IsTrue(accountOverviewPage.VerifyAndClickAccountNumber(newAccountNumber));

                accountOverviewPage.VerifyAccountDetails(newAccountNumber, accountType, 100, 100);
            }

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
        // Transaction Tests
        [Test, Order(7)]
        [AllureStep("Find Transaction Date")]
        public void TestFindTransactionDate()
        {
            FindTransactions findTransactions = new FindTransactions(Driver, Utils);
            string transactionDate = DateTime.Now.ToString("MM-dd-yyyy");
            var transactionDetails = findTransactions.FindTransactionInMultipleWays(
                "find_transaction_date", transactionDate);

            transactionId = transactionDetails.Item1;
            transactionAmount = transactionDetails.Item2;
        }

        [Test, Order(8)]
        [AllureStep("Find Transaction ID")]
        public void TestFindTransactionId()
        {
            FindTransactions findTransactions = new FindTransactions(Driver, Utils);
            if (!string.IsNullOrEmpty(transactionId))
            {
                findTransactions.FindTransactionInMultipleWays("find_transaction_id", transactionId);
            }
            else
            {
                throw new ArgumentException("Give the Transaction ID");
            }
        }

        [Test, Order(9)]
        [AllureStep("Find Transaction Date Range")]
        public void TestFindTransactionDateRange()
        {
            FindTransactions findTransactions = new FindTransactions(Driver, Utils);
            string transactionDate = DateTime.Now.ToString("MM-dd-yyyy");
            var transactionDateRange = transactionDate + "," + transactionDate;

            findTransactions.FindTransactionInMultipleWays("find_transaction_daterange", transactionDateRange);
        }

        [Test, Order(10)]
        [AllureStep("Find Transaction Amount")]
        public void TestFindTransactionAmount()
        {
            FindTransactions findTransactions = new FindTransactions(Driver, Utils);
            if (!string.IsNullOrEmpty(transactionAmount))
            {
                findTransactions.FindTransactionInMultipleWays("find_transaction_amount", transactionAmount);
            }
            else
            {
                throw new ArgumentException("Give the Transaction Amount");
            }
        }

        [Test, Order(11)]
        [AllureStep("Login To Application")]
        public void TestUpdateContactInfo()
        {
            updateuserData = TestDataGenerator.GenerateRandomUserForRegistration();
            foreach (KeyValuePair<string, string> kvp in updateuserData)
            {
                Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
            }

            UpdateContactInfo updateContactInfo = new UpdateContactInfo(Driver, Utils);
            updateContactInfo.UpdateContactInformation(
                updateuserData["fname"],
                updateuserData["lname"],
                updateuserData["address"],
                updateuserData["city"],
                updateuserData["state"],
                updateuserData["zipcode"],
                updateuserData["phone"]);


        }

    }
 
}