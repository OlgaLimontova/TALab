using NUnit.Framework;
using System.Collections.Generic;
using System.Configuration;

namespace SiteChecker
{
    public class CheckingClass
    {
        ExcelDataHandler handler;

        public CheckingClass()
        {
            handler = new ExcelDataHandler();
            handler.SetData();
        }
        
        void TestBasePageLogin(Page page)
        {
            page.NavigateHere(ConfigurationManager.AppSettings["StartURL"]);
            page.BaseLogin(ConfigurationManager.AppSettings["Login"], ConfigurationManager.AppSettings["Password"]);
        }

        void TestAllJournals(Page page, string startString)
        {
            Dictionary<string, List<KeyValuePair<string, List<string>>>> mainData = handler.GetMainData();
            foreach (string journalName in mainData.Keys)
                if (journalName.StartsWith(startString))
                    TestJournal(page, journalName);
        }

        void TestNavigateToJournal(Page page, string journalName)
        {
            page.NavigateToJournal(journalName);
        }

        void TestJournalPageLogin(Page page)
        {
            page.JournalPageLogin(ConfigurationManager.AppSettings["Login"], ConfigurationManager.AppSettings["Password"]);
            if (page.GetLoginName().Text.Trim().Equals(""))
                Assert.IsNotNull(page.GetLoginName().Text.Trim());
            else
                page.GetLoginName().Click();
        }

        void TestJournalPageLogout(Page page)
        {
            page.JournalPageLogout();
        }

        void TestSearch(Page page, string stringForSearching)
        {
            page.SearchString(stringForSearching);
        }

        void TestCheckCategoryPoints(Page page, KeyValuePair<string, List<string>> mainCategoryKeyValue)
        {
            foreach (string categoryPoint in mainCategoryKeyValue.Value)
                Assert.IsNotNull(page.GetDropdownToggleCategory(categoryPoint).Text);
        }

        [Timeout(10000)]
        void TestCheckMainCategories(Page page, string journalName)
        {
            foreach (KeyValuePair<string, List<string>> categoryDictionary in handler.GetMainCategories(journalName))
            {
                Assert.IsNotNull(page.GetDropdownToggle(categoryDictionary.Key).Text);
                page.CheckMainCategories(categoryDictionary.Key);
                TestCheckCategoryPoints(page, categoryDictionary);
            }
        }

        [Timeout(10000)]
        void TestCheckTestCasesCategories(Page page, string journalName)
        {
            page.AdvancedSearchLink.Click();
            foreach (KeyValuePair<string, List<string>> categoryDictionary in handler.GetTestCasesCategories(journalName))
            {
                Assert.IsTrue(page.GetSelectToggle(categoryDictionary.Key).Enabled);
                page.CheckTestCasesCategories(categoryDictionary);
                page.AdvancedSearchInOtherJournals(journalName);
            }
            page.AdvancedSearchButton.Click();
            Assert.IsTrue(page.SaveSearchButton.Displayed);
        }
        
        void TestJournal(Page page, string journalName)
        {
//            if (journalName.StartsWith("mc"))
            {
                TestNavigateToJournal(page, journalName);
                TestJournalPageLogin(page);
                TestCheckMainCategories(page, journalName);
                TestSearch(page, "melanoma");
                TestCheckTestCasesCategories(page, journalName);
                TestJournalPageLogout(page);
            }
        }

        [TestCaseSource("testCasesData")]
        public void StartTests(Page page, string startString)
        {
            TestAllJournals(page, startString);
        }
    }
}