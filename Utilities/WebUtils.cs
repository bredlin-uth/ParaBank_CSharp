using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Allure.NUnit.Attributes; 
using Allure.NUnit.Core;
using Allure.NUnit;

namespace ParaBank_CSharp.Utilities
{
    public class WebUtils
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;
        private readonly IJavaScriptExecutor js;
        public WebUtils(IWebDriver driver, int timeoutInSeconds = 10)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            js = (IJavaScriptExecutor)driver;
        }

        [AllureStep("Click on element: {locator}")]
        public void ClickOnElement(By locator)
        {
            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(locator)).Click();
            }
            catch (ElementClickInterceptedException)
            {
                var element = driver.FindElement(locator);
                js.ExecuteScript("arguments[0].click();", element);
            }
        }

        [AllureStep("Enter text '{value}' into field: {locator}")]
        public void EnterTextInField(By locator, string value)
        {
            try
            {
                var element = wait.Until(ExpectedConditions.ElementIsVisible(locator));
                element.Clear();
                element.SendKeys(value);
            }
            catch (ElementNotInteractableException)
            {
                var element = driver.FindElement(locator);
                js.ExecuteScript("arguments[0].value = arguments[1];", element, value);
            }
        }

        [AllureStep("Check if element is visible: {locator}")]
        public bool IsElementVisible(By locator)
        {
            try
            {
                bool visible = wait.Until(ExpectedConditions.ElementIsVisible(locator)).Displayed;

                // Attach screenshot when element is visible
                TestDataGenerator.AttachScreenshot(driver, $"Element visible: {locator}");

                Console.WriteLine($"Element {locator} visibility: {visible}");
                return visible;
            }
            catch (WebDriverTimeoutException)
            {
                // Attach screenshot when element is not visible (timeout)
                TestDataGenerator.AttachScreenshot(driver, $"Element NOT visible: {locator}");

                Console.WriteLine($"Element {locator} is not visible.");
                return false;
            }
        }

        [AllureStep("Wait until element is visible: {locator}")]
        public IWebElement WaitUntilElementIsVisible(By locator, int timeoutInSeconds = 10)
        {
            var localWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return localWait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        [AllureStep("Scroll to web element: {locator}")]
        public void ScrollToWebElement(By locator)
        {
            var element = driver.FindElement(locator);
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        [AllureStep("Get text from element: {locator}")]
        public string GetTextFromElement(By locator)
        {
            try
            {
                return wait.Until(ExpectedConditions.ElementIsVisible(locator)).Text.Trim();
            }
            catch (WebDriverTimeoutException)
            {
                return string.Empty;
            }
        }

        [AllureStep("Navigate back to previous page")]
        public void NavigateBackToPreviousPage()
        {
            driver.Navigate().Back();
        }

        [AllureStep("Select '{text}' from dropdown: {locator}")]
        public void SelectByVisibleText(By locator, string text)
        {
            var element = wait.Until(ExpectedConditions.ElementIsVisible(locator));
            var selectElement = new SelectElement(element);
            selectElement.SelectByText(text);
        }

        [AllureStep("Select value '{value}' from dropdown: {locator}")]
        public void SelectByValue(By locator, string value)
        {
            var element = wait.Until(ExpectedConditions.ElementIsVisible(locator));
            var selectElement = new SelectElement(element);
            selectElement.SelectByValue(value);
        }

        [AllureStep("Select index '{index}' from dropdown: {locator}")]
        public void SelectByIndex(By locator, int index)
        {
            var element = wait.Until(ExpectedConditions.ElementIsVisible(locator));
            var selectElement = new SelectElement(element);
            selectElement.SelectByIndex(index);
        }

        [AllureStep("Get values from dropdown: {locator}")]
        public List<string> GetValuesFromDropdown(By locator)
        {
            var element = wait.Until(ExpectedConditions.ElementIsVisible(locator));
            var selectElement = new SelectElement(element);
            return selectElement.Options.Select(option => option.Text.Trim()).ToList();
        }
    }
}
