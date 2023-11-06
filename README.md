# Web Scraper: Books To Scrape


This command-line application allows you to scrape content from the website https://books.toscrape.com/. It recursively traverses all pages, downloads and saves files (including pages and images) to your local disk, maintaining the original file structure. The application also provides progress information in the console, keeping you informed about the scraping process.


**Usage Instructions**

Run the Program:

Execute the program.
If you need instructions, type help and press Enter.
Enter a valid folder path where you want to save the downloaded files.
If you provide an empty path, the program will use the current directory.

Downloading Process:

The program will display progress information as it downloads the website content.
Images, linked pages, and scripts will be downloaded and saved in the specified folder.
The program will automatically create subdirectories to maintain the website's file structure.
Completion:

Once the website content is downloaded, the program will display a success message.
The downloaded files will be available in the specified folder.

**Program Features**

Recursive Scraping: The program recursively follows linked pages on the website and downloads their content.
File Preservation: Images, linked pages, and scripts are saved in their original website structure to maintain organization.
Error Handling: Errors are logged and displayed to provide feedback on any issues encountered during the download process.
Logging: Detailed logs are available in the logs folder, providing insights into the download process.

**Example Usage**

Run workflow file main.yml script to build the project and get executable file (Books.dll). Also Unit tests will be build and executed.

Run dotnet Books.dll

Type 'help' for instructions or provide a folder path to save files.

Enter folder path (or 'help' for instructions):

/path/to/save/files


Files will be saved to: /path/to/save/files


Website content is downloading.


Website content downloaded successfully.


Press any key to exit...

