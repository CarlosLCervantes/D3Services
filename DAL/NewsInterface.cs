using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class NewsInterface
    {
        private DialoNewsDBDataContext db;

        public NewsInterface()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["conStr"].ToString();
            db = new DialoNewsDBDataContext(connectionString);
        }

        #region ================================Retrieve================================

        public News GetNewsArticle(int articleID)
        {
            var newsArticle = (from newsRecord in db.News
                               where newsRecord.NewsID == articleID
                               select newsRecord);

            if (newsArticle == null || newsArticle.Count() != 1)
            {
                //uh. Somethign bad happened
                //TODO: Record error
                News defaultNews = new News() { NewsID = -1, Content = "", DateTime = DateTime.Now, ImageType = (int)ImageTypes.General, ImageData = new byte[0], Subject = "Error" };
                return defaultNews;
            }

            News returnArticle = newsArticle.SingleOrDefault();

            return returnArticle;
        }

        public IEnumerable<News> GetNews(int[] filters)
        {
            var news = (from newsRecord in db.News
                        select newsRecord);

            if (filters != null && filters.Length > 0)
            {
                news = news.Where(n => !filters.Contains(n.ArticleTypeID));
            }

            return news.OrderByDescending(n => n.DateTime);
        }

        public IEnumerable<News> GetNews(int pageStartIndex, int numToTake, int[] filters)
        {
            var news = GetNews(filters).Skip(pageStartIndex).Take(numToTake);

            return news;
        }

        public Image GetImage(int imgID)
        {
            Image image = (from img in db.Images
                           where img.ImageID == imgID
                           select img).SingleOrDefault();

            return image;
        }

        public IEnumerable<Image> GetImages()
        {
            var images = (from img in db.Images
                          select img);

            return images;
        }

        public NewsPreview GetNewsPreview(int id)
        {
            NewsPreview preview = (from np in db.NewsPreviews
                                   where np.NewsID == id
                                   select np).SingleOrDefault();

            return preview;

        }

        #endregion =====================================================================

        #region =================================Create=================================

        public bool InsertNews(string subject, string content, ArticleTypes articleType, ImageTypes imageType, byte[] imageData)
        {
            bool success = false;

            News newArticle = new News();
            newArticle.DateTime = DateTime.Now;
            newArticle.Subject = subject;
            newArticle.Content = content;
            newArticle.ImageType = (int)imageType;
            newArticle.ArticleTypeID = (int)articleType;

            if (imageData.Length > 0)
            {
                newArticle.ImageData = ImageHelper.ResizeImage(imageData, 250, 150);
            }
            else
            {
                newArticle.ImageData = imageData;
            }

            db.News.InsertOnSubmit(newArticle);
            db.SubmitChanges();

            return success;
        }

        public bool InsertImage(byte[] imageData, string description, int[] linkedArticleIDs, bool resize)
        {
            bool success = true;

            Image img = new Image();
            if (imageData.Length > 0)
            {
                if (resize)
                {
                    img.Data = ImageHelper.ResizeImage(imageData, 250, 150);
                }
                else
                {
                    img.Data = imageData;
                }
            }
            else { success = false; }
            img.Description = description;
            //img.ArticleIDs =


            if (success)
            {
                db.Images.InsertOnSubmit(img);
                db.SubmitChanges();
            }

            return success;
        }

        public int InsertPreview(string subject, string content )
        {
            NewsPreview preview = new NewsPreview();
            preview.DateTime = DateTime.Now;
            preview.Subject = subject;
            preview.Content = content;

            db.NewsPreviews.InsertOnSubmit(preview);
            db.SubmitChanges();

            return preview.NewsID;
        }

        #endregion=====================================================================

        #region =================================Update=================================

        //string subject, string content, ImageTypes imageType, byte[] imageData
        public bool EditNews(News newsItem)
        {
            bool success = false;

            News article = db.News.Where(x => x.NewsID == newsItem.NewsID).SingleOrDefault();
            article.Subject = newsItem.Subject;
            article.Content = newsItem.Content;
            article.ImageType = newsItem.ImageType;
            article.ArticleTypeID = newsItem.ArticleTypeID;

            db.SubmitChanges();

            return success;
        }

        #endregion ========================================================================


    }
}
