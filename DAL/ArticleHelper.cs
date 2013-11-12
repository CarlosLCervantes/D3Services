using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class ArticleHelper
    {
        //TODO: Remove Version from Service
        public string FormatArticleForIPhone(News article, string htmlFormat, string errorFormat)
        {
            string formattedArticle = String.Empty;
            if (article != null)
            {
                formattedArticle = htmlFormat.Replace("[ARTICLE_HEADER]", article.Subject);
                formattedArticle = formattedArticle.Replace("[ARTICLE_TITLE]", article.DateTime.ToString("MMMMM dd, yyyy"));
                formattedArticle = formattedArticle.Replace("[ARTICLE_CONTENT]", article.Content);
            }
            else
            {
                formattedArticle = errorFormat;
            }

            return formattedArticle;
        }

        public string FormatPreviewForIPhone(NewsPreview article, string htmlFormat, string errorFormat)
        {
            string formattedArticle = String.Empty;
            if (article != null)
            {
                formattedArticle = htmlFormat.Replace("[ARTICLE_HEADER]", article.Subject);
                formattedArticle = formattedArticle.Replace("[ARTICLE_TITLE]", article.DateTime.ToString("MMMMM dd, yyyy"));
                formattedArticle = formattedArticle.Replace("[ARTICLE_CONTENT]", article.Content);
            }
            else
            {
                formattedArticle = errorFormat;
            }

            return formattedArticle;
        }

    }
}
