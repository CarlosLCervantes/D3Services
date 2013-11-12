using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestProject1.D3Service;

namespace TestProject1
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class D3WSTests
    {
        private D3Service.ServiceSoapClient d3Service;
        public D3WSTests()
        {
            d3Service = new D3Service.ServiceSoapClient();
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /// <summary>
        /// Tests the the basic get news feature
        /// </summary>
        [TestMethod]
        public void GetNewsForIPhone()
        {
            NewsForIPhone[] news = d3Service.GetNewsForIPhone(0, 10, "");
            Assert.IsNotNull(news, "No Results for Basic Get News From IPhone Call");
            Assert.IsFalse(String.IsNullOrEmpty(news[0].Content), "There is no Content for the first Article in the news result");
            Assert.IsTrue(news.Length == 10, "Requested 10 news items but a number other than 10 was returned");
        }

        /// <summary>
        /// Tests get latest news article for iPhone
        /// </summary>
        [TestMethod]
        public void GetLatestNewsForIPhone()
        {
            string filter = "";
            NewsForIPhone news = d3Service.GetNewsForIPhone(0, 1, filter).FirstOrDefault();
            News lastestNews = d3Service.GetLatestNewsArticleForIPhone(filter);

            Assert.IsTrue(news.NewsID == lastestNews.NewsID, "The latest Get News return did not match the GetLatestNewsArticle result");
        }

        /// <summary>
        /// Test get news article for iPhone
        /// </summary>
        [TestMethod]
        public void GetNewsArticleForIPhone()
        {
            News news = d3Service.GetNewsArticleForIPhone(0);

            Assert.IsNotNull(news, "No article was returned for the first article");
            Assert.IsFalse(String.IsNullOrEmpty(news.Content), "article had no content");
        }

        /// <summary>
        /// Get Random News Article for IPhone test
        /// </summary>
        [TestMethod]
        public void GetRandomArticleForIPhone()
        {
            NewsForIPhone news = d3Service.GetRandomArticleForIPhone();
            Assert.IsNotNull(news, "No article was returned for the first article");
            Assert.IsFalse(String.IsNullOrEmpty(news.Content), "article had no content");

            NewsForIPhone newsOther = d3Service.GetRandomArticleForIPhone();
            Assert.IsFalse(news.NewsID != newsOther.NewsID, "Requested 2 articles and they were the same. This could have been chance. But maybe you should check.");
        }

    }
}
