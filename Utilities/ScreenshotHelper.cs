using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;

namespace ParaBank_CSharp.Utilities
{
    public class ScreenshotHelper
    {
        protected IWebDriver Driver;

        // Constructor to initialize the driver
        public ScreenshotHelper(IWebDriver driver)
        {
            this.Driver = driver;
        }

        // Method to capture screenshot
        public string CaptureScreenshot(string testName)
        {
            // Cast the WebDriver instance to ITakesScreenshot
            ITakesScreenshot ts = (ITakesScreenshot)Driver;
            Screenshot screenshot = ts.GetScreenshot();

            // Define the path for the report directory
            string reportDirectory = AppDomain.CurrentDomain.BaseDirectory + @"allure-results\";

            // Ensure the directory exists
            if (!Directory.Exists(reportDirectory))
            {
                Directory.CreateDirectory(reportDirectory);
            }

            // Define the full path to save the screenshot
            string screenshotPath = reportDirectory + testName + ".png";

            // Save the screenshot in PNG format (no need for ScreenshotImageFormat)
            screenshot.SaveAsFile(screenshotPath);

            return screenshotPath;
        }
    }
}
