using System;
using OpenQA.Selenium;
using Allure.NUnit.Attributes;
using ParaBank_CSharp.Utilities;
using NUnit.Framework;

namespace ParaBank_CSharp.Pages
{
    public class UpdateContactInfo
    {
        private readonly IWebDriver driver;
        private readonly WebUtils utils;

        // Locators
        private readonly By UpdateContactInfoLink = By.LinkText("Update Contact Info");
        private readonly By FirstNameField = By.Id("customer.firstName");
        private readonly By LastNameField = By.Id("customer.lastName");
        private readonly By StreetAddressField = By.Id("customer.address.street");
        private readonly By CityField = By.Id("customer.address.city");
        private readonly By StateField = By.Id("customer.address.state");
        private readonly By ZipCodeField = By.Id("customer.address.zipCode");
        private readonly By PhoneNumberField = By.Id("customer.phoneNumber");
        private readonly By UpdateProfileButton = By.XPath("//input[@value='Update Profile']");
        private readonly By ProfileUpdatedHeader = By.XPath("//h1[normalize-space()='Profile Updated']");
        private readonly By ErrorText = By.XPath("//p[@class='error']");

        public UpdateContactInfo(IWebDriver driver, WebUtils utils)
        {
            this.driver = driver;
            this.utils = utils;
        }

        [AllureStep("Update Contact Info with provided user details")]
        public void UpdateContactInformation(string firstName, string lastName, string street, string city, string state, string zip, string phone)
        {
            ClickOnUpdateContactInfo();
            EnterFirstName(firstName);
            EnterLastName(lastName);
            EnterStreetAddress(street);
            EnterCity(city);
            EnterState(state);
            EnterZipCode(zip);

            System.Threading.Thread.Sleep(3000); // Replace with explicit wait ideally
            EnterPhoneNumber(phone);

            System.Threading.Thread.Sleep(5000); // Replace with explicit wait ideally
            ClickOnUpdateProfileButton();
            VerifyProfileIsUpdated();
        }

        [AllureStep("Click on Update Contact Info link")]
        public void ClickOnUpdateContactInfo()
        {
            utils.ClickOnElement(UpdateContactInfoLink);
        }

        [AllureStep("Enter First Name")]
        public void EnterFirstName(string firstName)
        {
            utils.EnterTextInField(FirstNameField, firstName);
        }

        [AllureStep("Enter Last Name")]
        public void EnterLastName(string lastName)
        {
            utils.EnterTextInField(LastNameField, lastName);
        }

        [AllureStep("Enter Street Address")]
        public void EnterStreetAddress(string street)
        {
            utils.EnterTextInField(StreetAddressField, street);
        }

        [AllureStep("Enter City")]
        public void EnterCity(string city)
        {
            utils.EnterTextInField(CityField, city);
        }

        [AllureStep("Enter State")]
        public void EnterState(string state)
        {
            utils.EnterTextInField(StateField, state);
        }

        [AllureStep("Enter Zip Code")]
        public void EnterZipCode(string zip)
        {
            utils.EnterTextInField(ZipCodeField, zip);
        }

        [AllureStep("Enter Phone Number")]
        public void EnterPhoneNumber(string phone)
        {
            utils.EnterTextInField(PhoneNumberField, phone);
        }

        [AllureStep("Click on Update Profile button")]
        public void ClickOnUpdateProfileButton()
        {
            utils.ClickOnElement(UpdateProfileButton);
        }

        [AllureStep("Verify that profile is updated")]
        public void VerifyProfileIsUpdated()
        {
            if (utils.IsElementVisible(ProfileUpdatedHeader))
            {
                TestDataGenerator.AttachScreenshot(driver, "Profile Updated");
            }
            else if (utils.IsElementVisible(ErrorText))
            {
                var errorMessage = utils.GetTextFromElement(ErrorText);
                Console.WriteLine($"ERROR: {errorMessage}");
                TestDataGenerator.AttachScreenshot(driver, errorMessage);
                Assert.Fail($"Profile update failed with error: {errorMessage}");
            }
        }
    }
}
