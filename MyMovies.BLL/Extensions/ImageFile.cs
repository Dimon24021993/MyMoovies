using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MyMovies.BLL.Connectors
{
    public static class ImageFile
    {
        public static async Task<string> UploadUserImageAsync(string fileName, string rootPath, Guid id, byte[] imageBytes)
        {
            //Image image = Image.FromStream(new MemoryStream(imageBytes));
            //todo add cut method to configurable size
            return await UploadImageAsync(fileName, rootPath, id, imageBytes);
        }

        public static async Task<string> UploadImageAsync(string fileName, string rootPath, Guid id, byte[] imageBytes)
        {
            var extension = Path.GetExtension(fileName).ToLower();

            if (extension != ".png" && extension != ".jpg" && extension != ".jpeg" && extension != ".jpe")
            {
                throw new Exception("Неверный формат файла");
            }

            Directory.CreateDirectory(rootPath);

            var filePath = rootPath + id + extension;

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await fs.WriteAsync(imageBytes, 0, imageBytes.Length);
            }

            return id + extension;
        }

        public static void DeleteImage(string webPath, string relativePath)
        {
            var fileName = Path.GetFileName(webPath);

            var fullPath = Extensions.PathUtil.Combine(relativePath, fileName);

            fullPath = Path.GetFullPath(fullPath);
            var exist = File.Exists(fullPath);
            if (exist) File.Delete(fullPath);
        }

        //public static IEnumerable<Image> GetThunmnailImage(Image image)
        //{
        //    decimal size = image.Width / 1.00M / image.Height;

        //    var newWidth = int.Parse(ConfigurationManager.AppSettings["ThumbnailWidthMedium"]);
        //    var newWidth2 = int.Parse(ConfigurationManager.AppSettings["ThumbnailWidthSmall"]);
        //    var thumbnailMedium = image.GetThumbnailImage(newWidth, (int) (newWidth / size), () => false, IntPtr.Zero);
        //    var thumbnailSmall = image.GetThumbnailImage(newWidth2, (int) (newWidth2 / size), () => false, IntPtr.Zero);
        //    List<Image> list = new List<Image>
        //    {
        //        thumbnailMedium,
        //        thumbnailSmall
        //    };
        //    return list;
        //}

        //public static Image ArrayToImage(byte[] array)
        //{
        //    using (var ms = new MemoryStream(array))
        //    {
        //        return Image.FromStream(ms);
        //    }
        //}

        //public static byte[] ImageToByteArray(Image imageIn)
        //{
        //    using (var ms = new MemoryStream())
        //    {
        //        imageIn.Save(ms, ImageFormat.Jpeg);
        //        return ms.ToArray();
        //    }
        //}
    }
}
