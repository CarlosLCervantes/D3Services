namespace DAL
{
    partial class News
    {
        public string ContentSample;
        public News ConvertToSimple()
        {
            News simpleNews = new News();
            simpleNews.NewsID = this.NewsID;
            simpleNews.Subject = this.Subject.Truncate(35).RemoveHTML();
            simpleNews.DateTime = this.DateTime;
            simpleNews.ImageType = this.ImageType;
            if (this.ImageData != null && this.ImageData.Length > 0)
            {
                simpleNews.ImageData = ImageHelper.ResizeImage(this.ImageData, 50, 50);
            }
            else
            {
                simpleNews.ImageData = this.ImageData;
            }
            simpleNews.Content = this.Content.Truncate(90).RemoveHTML();

            return simpleNews;
        }
    }
}
