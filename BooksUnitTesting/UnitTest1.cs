using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace BooksUnitTesting
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public async Task DownloadFile_FileDoesNotExist_ReturnsTrueAsync()
        {
            // Arrange
            string url = "https://books.toscrape.com/";
            string folderPath = "C:/test/";
            string filePath = "C:/test/catalogue/category/books/humor_30/test.html";

        
            // Act
            await Books.DownloadWebsiteAsync(url, folderPath);
            var result = File.Exists(filePath);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task DownloadFile_FileExists_ReturnsFalseAsync()
        {
            // Arrange
            string url = "https://books.toscrape.com/";
            string folderPath = "C:/test/";
            string filePath = "C:/test/media/cache/0b/bc/0bbcd0a6f4bcd81ccb1049a52736406e.jpg";

            // Act
            await Books.DownloadWebsiteAsync(url, folderPath);
            var result = File.Exists(filePath);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
