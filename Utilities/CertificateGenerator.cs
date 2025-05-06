using SkiaSharp;
using SkiaSharp.Views.Maui.Controls;
using System.IO;


namespace Barangay_Office.Utilities
{
    public class CertificateGenerator
    {
        public static Stream GenerateCertificate(string certificatePath, string name, string date, string language, string address)
        {
            //load the certificate template
            using var inputStream = FileSystem.OpenAppPackageFileAsync(certificatePath).Result;
            using var bitmap = SKBitmap.Decode(inputStream);

            //create canvas to draw the image
            using var surface = SKSurface.Create(new SKImageInfo(bitmap.Width, bitmap.Height));
            var canvas = surface.Canvas;

            //draw the original image
            canvas.DrawBitmap(bitmap, 0, 0);

            //setup the text paint
            var paint = new SKPaint()
            {
                Color = SKColors.Black,
                TextSize = 50,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Arial")

            };

            

            //draw the name on the certificate
            canvas.DrawText(name, 100, 200, paint);
            //draw the date on the certificate
            canvas.DrawText(date, 100, 300, paint);
            //draw the language on the certificate
            canvas.DrawText(language, 100, 400, paint);
            //draw the address on the certificate
            canvas.DrawText(address, 100, 500, paint);

            //save the modified image to stream
            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            var outputStream = new MemoryStream();
            data.SaveTo(outputStream);
            outputStream.Seek(0, SeekOrigin.Begin); // Reset the stream position to the beginning

            return outputStream;
        }
    }
}
