using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using System.Configuration;
using System.Collections.Generic;

namespace SiteChecker
{
    public class WebDriver
    {
        private static IWebDriver driver;

        private WebDriver() {}

        public static IWebDriver Driver
        {
            get
            {
                if (driver == null)
                {
                    driver = InitDriver();
                    driver.Manage().Window.Maximize();
                }
                return driver;
            }
        }

        IWebDriver GetRemoteDriver()
        {
            NameValueCollection caps = ConfigurationManager.GetSection("capabilities/" + profile) as NameValueCollection;
            NameValueCollection settings = ConfigurationManager.GetSection("environments/" + environment) as NameValueCollection;
            DesiredCapabilities capability = new DesiredCapabilities();
            foreach (string key in caps.AllKeys)
                capability.SetCapability(key, caps[key]);
            foreach (string key in settings.AllKeys)
                capability.SetCapability(key, settings[key]);
            String username = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
            if (username == null)
                username = ConfigurationManager.AppSettings.Get("user");
            String accesskey = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");
            if (accesskey == null)
                accesskey = ConfigurationManager.AppSettings.Get("key");
            capability.SetCapability("browserstack.user", username);
            capability.SetCapability("browserstack.key", accesskey);
            if (capability.GetCapability("browserstack.local") != null && capability.GetCapability("browserstack.local").ToString() == "true")
            {
                browserStackLocal = new Local();
                List<KeyValuePair<string, string>> bsLocalArgs = new List<KeyValuePair<string, string>>()
                { new KeyValuePair<string, string>("key", accesskey) };
                browserStackLocal.start(bsLocalArgs);
            }
            return new RemoteWebDriver(new Uri("http://"+ ConfigurationManager.AppSettings.Get("Server") +"/wd/hub/"), capability);
        }

        private static IWebDriver InitDriver()
        {
            switch (ConfigurationManager.AppSettings["BrowserName"])
            {
                case "chrome":
                    return new ChromeDriver(ConfigurationManager.AppSettings["DriverPath"]);
                case "ie":
                    return new InternetExplorerDriver(ConfigurationManager.AppSettings["DriverPath"]);
                case "firefox":
                    return new FirefoxDriver();
                case "remote":
                    return GetRemoteDriver();
                default:
                    return new ChromeDriver(ConfigurationManager.AppSettings["DriverPath"]);
            }
        }

        public static void KillDriver()
        {
            driver.Quit();
            driver = null;
        }
    }
}
