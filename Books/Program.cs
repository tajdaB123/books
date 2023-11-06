using HtmlAgilityPack;
using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using static System.Net.Mime.MediaTypeNames;

public class Books
{
    private static string saveFolder = "";
    private const string websiteUrl = "https://books.toscrape.com/";
    private static readonly ILog log = LogManager.GetLogger(typeof(Books));
    static HttpClient client = new HttpClient();

    private static async Task Main(string[] args)
    {
        XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));
        Console.WriteLine("Type 'help' for instructions or provide a folder path to save files.");

        while (true)
        {
            Console.WriteLine("Enter folder path (or 'help' for instructions): ");
            saveFolder = Console.ReadLine();

            if (string.Equals(saveFolder, "help", StringComparison.OrdinalIgnoreCase))
            {
                DisplayHelpInstructions();
            }
            else
            {
                if (string.IsNullOrEmpty(saveFolder))
                    saveFolder = Directory.GetCurrentDirectory();

               

                Console.WriteLine($"Files will be saved to: {saveFolder}");
                // Main program
                try
                {
                    Console.WriteLine("Website content is downloading.");
                    if (!Directory.Exists(saveFolder))
                        Directory.CreateDirectory(saveFolder);

                    if (!saveFolder.EndsWith("/"))
                        saveFolder += "/";

                    await DownloadWebsiteAsync(websiteUrl, saveFolder);
                    Console.WriteLine("Website content downloaded successfully.");
                }
                catch (Exception e)
                {
                    ProgressInformation(e.Message, true);
                }
                break;
            }
        }
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    ///// <summary>
    ///// Displaying help instructions
    ///// </summary>
    private static void DisplayHelpInstructions()
    {
        Console.WriteLine("Instructions:");
        Console.WriteLine("1. Type a valid folder path to save files.");
        Console.WriteLine("2. Type 'help' to display these instructions.");
    }

    ///// <summary>
    ///// Writing progress information in log file and output
    ///// </summary>
    ///// <param name="text">Message to be written in log</param>
    ///// <param name="error">Define if an error was thrown</param>
    private static void ProgressInformation(string text, bool error = false)
    {
        if (error)
            log.Error(text);
        else
            log.Info(text);

    }

    /// <summary>
    /// Downloading file to disk
    /// </summary>
    /// <param name="url">Website URL address</param>
    /// <param name="path">Local disk path</param>
    /// <returns>Return true if the file was downloaded on the disk</returns>
    public static bool DownloadFile(string url, string path)
    {
        if (File.Exists(path))
            return false;

        if (!System.IO.Directory.Exists(Path.GetDirectoryName(path)))
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(path));
        
        ProgressInformation($"Downloading {Path.GetFileName(path)}...");
        using (var response = client.GetAsync(url).Result)
        {
            using (var content = response.Content.ReadAsStreamAsync().Result)
            {
                using (var stream = File.Create(path))
                {
                    content.CopyTo(stream);
                }
            }
        }
        return true;
    }

    ///// <summary>
    ///// Parsing and downloading website content
    ///// </summary>
    ///// <param name="url">Website url address</param>
    ///// <param name="path">Local disk path</param>
    public static async Task DownloadWebsiteAsync(string url, string path)
    {
        var web = new HtmlWeb();
        var doc = web.Load(url);

        Uri baseUrl = new Uri(url);
        Uri basePath = new Uri(path);
        Uri combinedUrl;
        Uri combinedPath;

        // Download images
        var imageNodes = doc.DocumentNode.SelectNodes("//img[@src]");
        if (imageNodes != null)
        {
            foreach (var imageNode in imageNodes)
            {
                string imageUrl = imageNode.GetAttributeValue("src", "");
                combinedPath = new Uri(baseUrl, imageUrl);
                combinedUrl = new Uri(basePath, imageUrl);
                DownloadFile(combinedPath.AbsoluteUri, combinedUrl.LocalPath);
                
            }
        }

        // Download linked scripts
        var linkNodes = doc.DocumentNode.SelectNodes("//link[@href]");
        if (linkNodes != null)
        {
            foreach (var linkNode in linkNodes)
            {
                string linkUrl = linkNode.GetAttributeValue("href", "");
                combinedPath = new Uri(baseUrl, linkUrl);
                combinedUrl = new Uri(basePath, linkUrl);
                DownloadFile(combinedPath.AbsoluteUri, combinedUrl.LocalPath);
            }
        }

        // Download linked pages
        var aNodes = doc.DocumentNode.SelectNodes("//a[@href]");
        if (aNodes != null)
        {
            foreach (var aNode in aNodes)
            {
                string aUrl = aNode.GetAttributeValue("href", "");
                combinedPath = new Uri(baseUrl, aUrl);
                combinedUrl = new Uri(basePath, aUrl);
                
                if (DownloadFile(combinedPath.AbsoluteUri, combinedUrl.LocalPath))
                {
                    await DownloadWebsiteAsync(combinedPath.AbsoluteUri, combinedUrl.LocalPath);
                }             
            }     
        }     
    }
}