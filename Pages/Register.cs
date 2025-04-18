using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Allure.NUnit.Attributes;
using ParaBank_CSharp.Utilities;
using Allure.Net.Commons;
using NUnit.Framework.Internal;


namespace ParaBank_CSharp.Pages
{
    public class Register
    {
        private readonly IWebDriver driver;
        private readonly WebUtils utils;

        public Register(IWebDriver driver, WebUtils utils)
        {
            this.driver = driver;
            this.utils = utils;

        }

        private readonly By register_link = By.LinkText("Register");
        private readonly By first_name = By.Id("customer.firstName");
        private readonly By last_name = By.Id("customer.lastName");
        private readonly By address = By.Id("customer.address.street");
        private readonly By city = By.Id("customer.address.city");
        private readonly By state = By.Id("customer.address.state");
        private readonly By zip_code = By.Id("customer.address.zipCode");
        private readonly By phone = By.Id("customer.phoneNumber");
        private readonly By ssn = By.Id("customer.ssn");
        private readonly By username = By.Id("customer.username");
        private readonly By password = By.Id("customer.password");
        private readonly By confirm = By.Id("repeatedPassword");
        private readonly By register_button = By.XPath("//input[@value='Register']");
        private readonly By sign_up_text = By.XPath("//h1[normalize-space()='Signing up is easy!']");


        [AllureStep("Verify the register page")]

        public bool VerifyTheRegisterPage()
        {
            bool status = utils.IsElementVisible(sign_up_text);
            if (status)
            {
                Console.WriteLine("Navigate to the Register page");
            }
            else
            {
               Console.WriteLine("Unable to navigate to the Register page");
            }
            return status;

        }


        [AllureStep("Register to the application: {0}")]
        public void RegisterToApplication(string fname, string lname, string useraddress, string usercity, 
            string userstate, string userzipcode, string userphone, string userssn, string uname, string pwd, string confirmpwd)

        {
            //Thread.Sleep(3000);
            utils.ClickOnElement(register_link);
            VerifyTheRegisterPage();
            utils.EnterTextInField(first_name, fname);
            utils.EnterTextInField(last_name, lname);
            utils.EnterTextInField(address, useraddress);
            utils.EnterTextInField(city, usercity);
            utils.EnterTextInField(state, userstate);
            utils.EnterTextInField(zip_code, userzipcode);
            utils.EnterTextInField(phone, userphone);
            utils.EnterTextInField(ssn, userssn);
            utils.EnterTextInField(username, uname);
            utils.EnterTextInField(password, pwd);
            utils.EnterTextInField(confirm, confirmpwd);
            TestDataGenerator.AttachScreenshot(driver, "RegisterPageFilled");
            utils.ClickOnElement(register_button);
            //return (fname, uname);
        }
    }
}
