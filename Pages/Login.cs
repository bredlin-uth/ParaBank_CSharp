using OpenQA.Selenium;
using Allure.NUnit.Attributes;
using ParaBank_CSharp.Utilities;

namespace ParaBank_CSharp.Pages
{
    public class Login
    {
        private readonly IWebDriver driver;
        private readonly WebUtils utils;

        public Login(IWebDriver driver, WebUtils utils)
        {
            this.driver = driver;
            this.utils = utils;

        }
        private readonly By usernameField = By.XPath("//div[@class='login']/input[@name='username']");
        private readonly By passwordField = By.XPath("//div[@class='login']/input[@name='password']");
        private readonly By loginButton = By.XPath("//div[@class='login']/input[@value='Log In']");


        //[AllureStep("Login to the application with username and password: {0}")]
        public void LoginToApplication(string username, string password)
        {
            utils.EnterTextInField(usernameField, username);
            utils.EnterTextInField(passwordField, password);
            utils.ClickOnElement(loginButton);
        }
    }
}
