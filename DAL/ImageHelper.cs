using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DAL
{
    public static class ImageHelper
    {
        public static byte[] ToByteArray(System.Drawing.Image p_ImageIn)
        {
            System.Drawing.ImageConverter imgConverter = new System.Drawing.ImageConverter();
            Byte[] result = (Byte[])imgConverter.ConvertTo(p_ImageIn, typeof(Byte[]));

            return result;
        }

        public static System.Drawing.Image ConvertBytesToImage(byte[] imageData)
        {
            MemoryStream ms = new MemoryStream(imageData);
            System.Drawing.Image originalImg = System.Drawing.Image.FromStream(ms);
            return originalImg;
        }

        /// <summary> 
        /// Resize the image to the specified width and height. 
        /// </summary> 
        /// <param name="image">The image to resize.</param> 
        /// <param name="width">The width to resize to.</param> 
        /// <param name="height">The height to resize to.</param> 
        /// <returns>The resized image.</returns> 
        public static System.Drawing.Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            //a holder for the result 
            System.Drawing.Bitmap result = new System.Drawing.Bitmap(width, height);

            //use a graphics object to draw the resized image into the bitmap 
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(result))
            {
                //set the resize quality modes to high quality 
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //draw the image into the target bitmap 
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            //return the resulting bitmap 
            return result;
        }

        public static byte[] ResizeImage(byte[] originalImageData, int width, int height)
        {
            System.Drawing.Image imageFromData = ConvertBytesToImage(originalImageData);
            System.Drawing.Image resizedImage = ImageHelper.ResizeImage(imageFromData, width, height);
            byte[] resizedImageData = ToByteArray(resizedImage);
            return resizedImageData;
        }
    }
}
