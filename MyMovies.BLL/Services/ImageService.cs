using System.IO;
using System.Threading.Tasks;
using MyMovies.BLL.Interfaces;

namespace MyMovies.BLL.Services
{
    public class ImageService: IImageService
    {
        private static string CarImagesPath => "";
        private static string ExternalDiagPath => "";
        private static string InternalDiagPath => "";


        //public async Task RotateImage(Guid id, RotateFlipType flipType)
        //{
        //    var carImageFiles = Directory.GetFiles(Extensions.PathUtil.Combine(CarImagesPath)).Where(x => x.Contains(id.ToString())).ToList();
        //    var externalDiagFiles = Directory.GetFiles(Extensions.PathUtil.Combine(ExternalDiagPath)).Where(x => x.Contains(id.ToString())).ToList();
        //    var internalDiagFiles = Directory.GetFiles(Extensions.PathUtil.Combine(InternalDiagPath)).Where(x => x.Contains(id.ToString())).ToList();

        //    if (carImageFiles.Any())
        //    {
        //        foreach (var file in carImageFiles)
        //        {
        //            var bitmap = Image.FromFile(file);
        //            bitmap.RotateFlip(flipType);
        //            await UploadImage(file, bitmap);
        //        }
        //    }

        //    if (externalDiagFiles.Any())
        //    {
        //        foreach (var file in externalDiagFiles)
        //        {
        //            var bitmap = Image.FromFile(file);
        //            bitmap.RotateFlip(flipType);
        //            await UploadImage(file, bitmap);
        //        }
        //    }

        //    if (internalDiagFiles.Any())
        //    {
        //        foreach (var file in internalDiagFiles)
        //        {
        //            var bitmap = Image.FromFile(file);
        //            bitmap.RotateFlip(flipType);
        //            //bitmap.Save(file);
        //            await UploadImage(file, bitmap);
        //        }
        //    }
        //}

        public async Task UploadImage(string path)//, Image image)
        {
            var imageBytes = new byte[0];// ImageFileConnector.ImageToByteArray(image);

            using (var fs = new FileStream(path, FileMode.Create))
            {
                await fs.WriteAsync(imageBytes, 0, imageBytes.Length);
            }
        }
    }
}
