using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Bmp;

namespace MotorCycleRentail.Common.Helpers
{
    public static class Base64Validator
    {
        public static bool IsValidBase64Image(string base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String))
                return false;

            // Verifica se a string tem um formato válido de Base64
            if ((base64String.Length % 4 != 0) || base64String.Contains(' ') || base64String.Contains('\t') || base64String.Contains('\r') || base64String.Contains('\n'))
                return false;

            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    // Corrigido para carregar corretamente a imagem no ImageSharp
                    using (Image image = Image.Load(ms))
                    {
                        IImageFormat format = image.Metadata.DecodedImageFormat;
                        return format is PngFormat || format is BmpFormat;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
