using Allure.NUnit.Attributes;
using ParaBank_CSharp.Pages;
using ParaBank_CSharp.Utilities;

namespace ParaBank_CSharp.Tests
{
    [TestFixture]
    public class Tests_ParaBank : BaseTest 
    {
        [Test]
        [AllureStep("---------")]
        public void LoginToTheParaBankApplication()
        {
            Login loginpage = new Login(Driver, Utils);
            loginpage.LoginToApplication("fytfy","yfytf");
            Assert.Pass();
        }
    }
}