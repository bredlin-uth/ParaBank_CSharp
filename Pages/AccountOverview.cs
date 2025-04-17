using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using ParaBank_CSharp.Utilities;

namespace ParaBank_CSharp.Pages
{
    public class AccountOverview
    {

        private readonly IWebDriver driver;
        private readonly WebUtils utils;

        public AccountOverview(IWebDriver driver, WebUtils utils)
        {
            this.driver = driver;
            this.utils = utils;

        }
    }
}
