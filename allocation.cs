using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace AllotracAutomation
{
    public class JobAllocationTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://dummy.allotrac.com/login");
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void CreateAndAllocateJobToTruck()
        {
            // Login
            driver.FindElement(By.Id("username")).SendKeys("testuser");
            driver.FindElement(By.Id("password")).SendKeys("password123");
            driver.FindElement(By.Id("loginBtn")).Click();
            wait.Until(d => d.Url.Contains("dashboard"));

            // Navigate to Job Creation
            driver.FindElement(By.Id("createJobBtn")).Click();
            wait.Until(d => d.Url.Contains("job-creation"));

            // Fill Job Details
            driver.FindElement(By.Id("jobTitle")).SendKeys("Test Job");
            driver.FindElement(By.Id("jobDescription")).SendKeys("Automated job creation");
            driver.FindElement(By.Id("submitJobBtn")).Click();
            wait.Until(d => d.Url.Contains("job-list"));

            // Navigate to Allocation Screen
            driver.FindElement(By.Id("allocationMenu")).Click();
            wait.Until(d => d.Url.Contains("allocation"));

            // Allocate Job to Truck
            driver.FindElement(By.XPath("//td[contains(text(), 'Test Job')]/following-sibling::td/button[text()='Allocate']")).Click();
            wait.Until(d => d.FindElement(By.Id("truckSelectionDropdown")).Displayed);
            driver.FindElement(By.Id("truckSelectionDropdown")).SendKeys("Truck 1");
            driver.FindElement(By.Id("confirmAllocationBtn")).Click();
            
            // Verify Allocation
            Assert.IsTrue(driver.PageSource.Contains("Job successfully allocated"));
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
