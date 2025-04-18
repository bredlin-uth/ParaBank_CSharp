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
            // Generate random user data
            userData = TestDataGenerator.GenerateRandomUserForRegistration();
            foreach (KeyValuePair<string, string> kvp in userData)
            {
                Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
            }

            // Register to the ParaBank application
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
                //Assert.IsTrue(accountOverviewPage.VerifyAndClickAccountNumber(newAccountNumber));

                //accountOverviewPage.VerifyAccountDetails(newAccountNumber, accountType, 100, 100);
            }

        }

    }
 
}