//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
////using AventStack.ExtentReports;
////using AventStack.ExtentReports.Reporter;


//namespace ParaBank_CSharp.Utilities
//{
//    public class ExtentReportManager
//    {
//        private static AventStack.ExtentReports.ExtentReports extent;  // Fixed here
//        private static ExtentHtmlReporter htmlReporter;
//        private static string reportPath = AppDomain.CurrentDomain.BaseDirectory + @"\Reports\AutomationReport.html";

//        public static AventStack.ExtentReports.ExtentReports GetInstance()  // Fixed here
//        {
//            if (extent == null)
//            {
//                htmlReporter = new ExtentHtmlReporter(reportPath);
//                extent = new AventStack.ExtentReports.ExtentReports();  // Fixed here
//                extent.AttachReporter(htmlReporter);

//                extent.AddSystemInfo("Host Name", "LocalHost");
//                extent.AddSystemInfo("Environment", "QA");
//                extent.AddSystemInfo("User Name", "Test Engineer");

//                htmlReporter.Config.DocumentTitle = "Automation Test Report";
//                htmlReporter.Config.ReportName = "Functional Testing";
//                htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;
//            }
//            return extent;
//        }
//    }
//}

//using System;
//using NUnit.Framework;
//using ParaBank_CSharp.Pages;
//using ParaBank_CSharp.Utilities;
//using Allure.NUnit.Attributes;

//namespace ParaBank_CSharp.Tests
//{
//    [TestFixture]
//    public class Tests_ParaBank : BaseTest
//    {
//private string transactionId;
//private string transactionAmount;
//        private Dictionary<string, string> userData;

//        // Registration Test
//        [Test, Order(1)]
//        [AllureStep("Register To Application")]
//        public void TestRegister()
//        {
//            // Generate random user data
//            userData = TestDataGenerator.GenerateRandomUserForRegistration();
//            foreach (KeyValuePair<string, string> kvp in userData)
//            {
//                Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
//            }

//            // Register to the ParaBank application
//            Register registerPage = new Register(Driver, Utils);
//            registerPage.RegisterToApplication(
//                userData["fname"],
//                userData["lname"],
//                userData["address"],
//                userData["city"],
//                userData["state"],
//                userData["zipcode"],
//                userData["phone"],
//                userData["ssn"],
//                userData["uname"],
//                userData["pwd"],
//                userData["pwd1"]
//            );

//            // Verification: Verify registration success
//            AccountServices accountServicesPage = new AccountServices(Driver, Utils);
//            Assert.IsTrue(accountServicesPage.VerifyAccountIsRegistered(userData["uname"]));

//            // Log out
//            accountServicesPage.LogOutFromApplication();
//        }

//        // Login Test
//        [Test, Order(2)]
//        [AllureStep("Login To Application")]
//        public void LoginToTheParaBankApplication()
//        {
//            Login loginPage = new Login(Driver, Utils);
//            loginPage.LoginToApplication(userData["uname"], userData["pwd"]);
//            AccountServices accountServicesPage = new AccountServices(Driver, Utils);
//            Assert.IsTrue(accountServicesPage.VerifyAccountIsLoggedIn(userData["fname"]));
//        }

//        // Transaction Tests
//        [Test, Order(3)]
//        [AllureStep("Find Transaction Date")]
//        public void TestFindTransactionDate()
//        {
//            FindTransactions findTransactions = new FindTransactions(Driver, Utils);
//            string transactionDate = DateTime.Now.ToString("MM-dd-yyyy");
//            var transactionDetails = findTransactions.FindTransactionInMultipleWays(
//                "find_transaction_date", transactionDate);

//            transactionId = transactionDetails.Item1;
//            transactionAmount = transactionDetails.Item2;
//        }

//        [Test, Order(4)]
//        [AllureStep("Find Transaction ID")]
//        public void TestFindTransactionId()
//        {
//            FindTransactions findTransactions = new FindTransactions(Driver, Utils);
//            if (!string.IsNullOrEmpty(transactionId))
//            {
//                findTransactions.FindTransactionInMultipleWays("find_transaction_id", transactionId);
//            }
//            else
//            {
//                throw new ArgumentException("Give the Transaction ID");
//            }
//        }

//        [Test, Order(5)]
//        [AllureStep("Find Transaction Date Range")]
//        public void TestFindTransactionDateRange()
//        {
//            FindTransactions findTransactions = new FindTransactions(Driver, Utils);
//            string transactionDate = DateTime.Now.ToString("MM-dd-yyyy");
//            var transactionDateRange = transactionDate + "," + transactionDate;

//            findTransactions.FindTransactionInMultipleWays("find_transaction_daterange", transactionDateRange);
//        }

//        [Test, Order(6)]
//        [AllureStep("Find Transaction Amount")]
//        public void TestFindTransactionAmount()
//        {
//            FindTransactions findTransactions = new FindTransactions(Driver, Utils);
//            if (!string.IsNullOrEmpty(transactionAmount))
//            {
//                findTransactions.FindTransactionInMultipleWays("find_transaction_amount", transactionAmount);
//            }
//            else
//            {
//                throw new ArgumentException("Give the Transaction Amount");
//            }
//        }
//    }
//}
