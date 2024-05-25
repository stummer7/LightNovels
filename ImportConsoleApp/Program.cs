using Persistence;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Core.Entities;

//Console.WriteLine("Import der Movies und Categories in die Datenbank");
//await using var unitOfWork = new UnitOfWork();
//Console.WriteLine("Datenbank löschen");
//await unitOfWork.DeleteDatabaseAsync();
//Console.WriteLine("Datenbank migrieren");
//await unitOfWork.MigrateDatabaseAsync();

//var (novels, categories) = await ImportController.ReadFromCsvAsync();

//await unitOfWork.Novels.AddRangeAsync(novels);
//await unitOfWork.Categories.AddRangeAsync(categories);
//await unitOfWork.SaveChangesAsync();
//Console.WriteLine();
//Console.Write("Beenden mit Eingabetaste ...");
//Console.ReadLine();

string url = "https://www.lightnovelcave.com/browse/genre-all-25060123/order-new/status-all";
IList<Novel> novels = new List<Novel>();


var chromeOptions = new ChromeOptions();
chromeOptions.AddArguments("headless");
var driver = new ChromeDriver();

driver.Navigate().GoToUrl(url);

Thread.Sleep(5000);

IWebElement card = driver.FindElement(By.XPath("/html/body/div[1]/div/div/div/div[2]/div/button[2]"));
card.Click();

IList<IWebElement> elements = driver.FindElements(By.ClassName("novel-item"));

foreach (var item in elements)
{
    IList<IWebElement> links = item.FindElements(By.TagName("a"));
    IWebElement link = links.First();
    Console.WriteLine(link.Text);
    link.Click();
}


//driver.Quit();
