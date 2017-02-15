using NUnit.Framework;

namespace SiteChecker
{
    [TestFixture]
    [Parallelizable(ParallelScope.None)]
//    [TestClass]
    public class MainTestClass
    {
        [Test]
//        [TestMethod]
        public void MainTestMethodL()
        {
            var homePage = new Page();
            CheckingClass checker = new CheckingClass();
            checker.StartTests(homePage, "l");    
        }

        [Test]
//        [TestMethod]
        public void MainTestMethodM()
        {
            var homePage = new Page();
            CheckingClass checker = new CheckingClass();
            checker.StartTests(homePage, "m");
        }

        /*
        [Test]
//        [TestMethod]
        public void MainTestMethodN()
        {
            var homePage = new Page();
            CheckingClass checker = new CheckingClass();
            checker.StartTests(homePage, "n");
        }

        [Test]
//        [TestMethod]
        public void MainTestMethodO()
        {
            var homePage = new Page();
            CheckingClass checker = new CheckingClass();
            checker.StartTests(homePage, "o");
        }

        [Test]
//        [TestMethod]
        public void MainTestMethodP()
        {
            var homePage = new Page();
            CheckingClass checker = new CheckingClass();
            checker.StartTests(homePage, "p");
        }
        */

        [TearDown]
//        [ClassCleanup]
        public static void Cleanup()
        {
//            WebDriver.KillDriver();
        }
    }
}
