using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Allure.Net.Commons;
using Allure.NUnit.Attributes;
using Allure.NUnit.Core;
using Faker;
using OpenQA.Selenium;


namespace ParaBank_CSharp.Utilities
{
    public static class TestDataGenerator
    {
        public static Dictionary<string, string> GenerateRandomUserForRegistration()
        {
            Random random = new Random();
            string name = Name.First().ToLower();
            int number = random.Next(10000, 99999);

            return new Dictionary<string, string>
            {
                {"fname", Name.First()},
                {"lname", Name.Last()},
                {"address", Address.StreetAddress()},
                {"city", Address.City()},
                {"state", Address.UsState()},
                {"zipcode", random.Next(100000, 999999).ToString()},
                {"phone", random.Next(600000000, 999999999).ToString() + random.Next(0, 9).ToString()},
                {"ssn", random.Next(1000, 9999).ToString()},
                {"uname", name + number},
                {"pwd", "Test@123"},
                {"pwd1", "Test@123"}
            };
        }

        public static Dictionary<string, string> GenerateRandomDetailsForPayBill(string amount)
        {
            Random random = new Random();

            return new Dictionary<string, string>
            {
                {"name", Name.First()},
                {"address", Address.StreetAddress()},
                {"city", Address.City()},
                {"state", Address.UsState()},
                {"zipcode", random.Next(100000, 999999).ToString()},
                {"phone", random.Next(600000000, 999999999).ToString() + random.Next(0, 9).ToString()},
                {"account", random.Next(1000, 9999).ToString()},
                {"amount", amount}
            };
        }

        public static bool CompareCurrencyWithNumber(string value1, string value2)
        {
            double Normalize(string value)
            {
                string numeric = Regex.Replace(value, @"[^\d.]", "");
                return double.Parse(numeric);
            }

            return Math.Round(Normalize(value1), 2) == Math.Round(Normalize(value2), 2);
        }

        public static string ConvertToFloat2Dp(object value)
        {
            if (value is int i)
                return ((float)i).ToString("F2");
            else if (value is float f)
                return f.ToString("F2");
            else if (value is double d)
                return d.ToString("F2");
            else
                throw new ArgumentException("Input must be int, float, or double");
        }

        //method to attach screenshots to Allure reports
        public static void AttachScreenshot(IWebDriver driver, string name)
        {
            // Capture screenshot from driver
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            string tempFilePath = Path.GetTempFileName() + ".png";
            screenshot.SaveAsFile(tempFilePath);

            // Attach screenshot
            AllureApi.AddAttachment(name, "image/png", tempFilePath);
            File.Delete(tempFilePath);
        }

        public static void AttachText(string name, string value)
        {
            AllureApi.AddAttachment($"{name}.txt", "text/plain", Encoding.UTF8.GetBytes(value)
            );
        }
    }
}
