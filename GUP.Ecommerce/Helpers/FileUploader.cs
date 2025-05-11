using Microsoft.AspNetCore.WebUtilities;
using SixLabors.ImageSharp.Formats.Webp;
using System.Text.RegularExpressions;

namespace GUP.Ecommerce.Helpers
{
    public static class FileUploader
    {
        public static string SaveImage(string base64String, string webRootPath, string folderPath = "images", string fileName = null)
        {
            if (string.IsNullOrEmpty(base64String))
                return null;

            try
            {
                string originalFileExtension = ".jpg";
                string base64Data = base64String;

                // Extract the base64 data and detect original format
                if (base64String.Contains(","))
                {
                    var match = Regex.Match(base64String, @"data:image/(.+?);base64,(.+)");
                    if (match.Success)
                    {
                        originalFileExtension = "." + match.Groups[1].Value;
                        base64Data = match.Groups[2].Value;
                    }
                    else
                    {
                        base64Data = base64String.Split(',')[1];
                    }
                }

                // Create directory if it doesn't exist
                string directoryPath = Path.Combine(webRootPath, folderPath);
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                // Generate file name if not provided
                if (string.IsNullOrEmpty(fileName))
                {
                    // Create a unique name using timestamp and GUID
                    string timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
                    string uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);
                    fileName = $"{timestamp}-{uniqueId}";
                }

                // Always use .webp extension for output
                string webpFileName = fileName + ".webp";
                string webpFilePath = Path.Combine(directoryPath, webpFileName);

                // Convert image to WebP format
                byte[] imageBytes = Convert.FromBase64String(base64Data);

                using (var image = Image.Load(imageBytes))
                {
                    // Configure WebP encoder with desired settings
                    var encoder = new WebpEncoder
                    {
                        Quality = 80, // Adjust quality as needed (0-100)
                        FileFormat = WebpFileFormatType.Lossy
                    };

                    // Save as WebP
                    image.Save(webpFilePath, encoder);
                }

                // Return the relative path
                return Path.Combine(folderPath, webpFileName).Replace("\\", "/");
            }
            catch (Exception ex)
            {
                // Consider logging the exception message
                Console.WriteLine($"Error saving image: {ex.Message}");
                return null;
            }
        }

        public static bool DeleteImage(string imagePath, string webRootPath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return false;

            try
            {
                string fullPath = Path.Combine(webRootPath, imagePath.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting image: {ex.Message}");
                return false;
            }
        }
    }
}
