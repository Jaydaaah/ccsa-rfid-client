using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;
using System.Drawing;

namespace ccsa_rfid_client.Action
{
    internal static class TesseractAction
    {
        internal static Image TakeScreenshot()
        {
            return Pranas.ScreenshotCapture.TakeScreenshot(true);
        }

        private static Bitmap toBitmap(Image image)
        {
            // Create a new Bitmap object with the same width and height
            Bitmap bitmap = new Bitmap(image.Width, image.Height);

            // Draw the Image onto the Bitmap
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(image, 0, 0, image.Width, image.Height);
            }

            return bitmap;
        }

        internal static string readFromImage(Image image)
        {
            using (var Engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
            {
                Bitmap bitmap = toBitmap(image);
                using (var page = Engine.Process(bitmap))
                {
                    return page.GetText();
                }
            }
        }
    }
}
