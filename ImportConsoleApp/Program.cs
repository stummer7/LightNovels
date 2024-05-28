using Persistence;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Core.Entities;

Console.WriteLine("Import der Categories in die Datenbank");
await using var unitOfWork = new UnitOfWork();
Console.WriteLine("Datenbank löschen");
await unitOfWork.DeleteDatabaseAsync();//
Console.WriteLine("Datenbank migrieren");
await unitOfWork.MigrateDatabaseAsync();//

var (novelsImport, categories) = await ImportController.ReadFromCsvAsync();

//await unitOfWork.Novels.AddRangeAsync(novels);
await unitOfWork.Categories.AddRangeAsync(categories); //
await unitOfWork.SaveChangesAsync(); //
Console.WriteLine();

string url = "https://www.lightnovelcave.com/browse/genre-all-25060123/order-new/status-all";
IList<Novel> novels = new List<Novel>();
IList<Chapter> chapters = new List<Chapter>();


var chromeOptions = new ChromeOptions();
chromeOptions.AddArguments("headless");
var driver = new ChromeDriver();

driver.Navigate().GoToUrl(url);

Thread.Sleep(3000);

IWebElement card = driver.FindElement(By.XPath("/html/body/div[1]/div/div/div/div[2]/div/button[2]")); // get rid of cookies card
card.Click();

IList<IWebElement> elements = driver.FindElements(By.ClassName("novel-item"));

for(int i = 0; i < elements.Count; i++)
{
    elements = driver.FindElements(By.ClassName("novel-item"));
    var item = elements[i];

    IList<IWebElement> links = item.FindElements(By.TagName("a"));
    IWebElement link = links.First();
    link.Click(); //click on the novel card to go to the novel overview page

    Novel novel = GetNovelDetails(driver, categories);

    var content_nav = driver
        .FindElement(By.ClassName("content-nav"))
        .FindElements(By.TagName("a"))
        .First();

    content_nav.Click();

    var chapterList_parent = driver
        .FindElement(By.ClassName("chapter-list"))
        .FindElements(By.TagName("li"));

    for (int j = 0; j < chapterList_parent.Count /10; j++)
    {
        chapterList_parent = driver
        .FindElement(By.ClassName("chapter-list"))
        .FindElements(By.TagName("li"));

        var chapter_link = chapterList_parent[j].FindElement(By.TagName("a"));

        var title = chapter_link
            .FindElement(By.TagName("strong"))
            .Text;
        var released = chapter_link
            .FindElement(By.TagName("time"))
            .GetAttribute("datetime");

        Chapter chapter = new Chapter()
        {
            Novel = novel,
            Title = title,
            Released = DateTime.Parse(released)
        };

        chapter_link.Click();

        Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

        var chapter_container = driver.FindElement(By.Id("chapter-container"));
        var paragraphs = chapter_container.FindElements(By.TagName("p"));
        var text = string.Join("\n", paragraphs.Select(i => i.Text));

        chapter.Text = text;
        chapters.Add(chapter);

        driver.Navigate().Back();
    }

    novels.Add(novel);
    driver.Navigate().GoToUrl(url);
}

driver.Quit();

await unitOfWork.Novels.AddRangeAsync(novels);
await unitOfWork.Chapters.AddRangeAsync(chapters);
await unitOfWork.SaveChangesAsync();

static Novel GetNovelDetails(ChromeDriver driver, IEnumerable<Category> categories)
{
    var title = driver
        .FindElements(By.ClassName("novel-title"))
        .First()
        .Text;
    var authorParent = driver
        .FindElements(By.ClassName("author"))
        .First();
    var author = authorParent
        .FindElements(By.TagName("span"))
        .ElementAt(1)
        .Text;
    var ratingParent = driver
        .FindElements(By.ClassName("rating"))
        .First();
    var rank = ratingParent
        .FindElements(By.TagName("strong"))
        .First()
        .Text
        .Split(' ')[1];
    var header_statsParent = driver
        .FindElements(By.ClassName("header-stats"))
        .First();
    var viewsParent = header_statsParent
        .FindElements(By.TagName("span"))
        .ElementAt(1)
        .FindElement(By.TagName("strong"))
        .Text;
    var bookmarkedParent = header_statsParent
        .FindElements(By.TagName("span"))
        .ElementAt(2)
        .FindElement(By.TagName("strong"))
        .Text;
    var status = header_statsParent
        .FindElements(By.TagName("span"))
        .Last()
        .FindElement(By.TagName("strong")).Text;

    var views = ConvertNovelNumbersToString(viewsParent);
    var bookmarked = ConvertNovelNumbersToString(bookmarkedParent);

    var novelCategories = driver
        .FindElement(By.ClassName("categories"))
        .FindElements(By.TagName("li")).Select(i => i.Text)
        .ToList();

    IList<Category> objCategories = new List<Category>();

    foreach (var category in novelCategories)
    {
        var insert = categories.SingleOrDefault(c => c.Name.ToLower() == category.ToLower());
        if (insert != null)
        {
            objCategories.Add(insert);
        }
    }

    var imgUrl = driver
        .FindElement(By.ClassName("cover"))
        .FindElement(By.TagName("img"))
        .GetAttribute("src");
    var paragraphs = driver
        .FindElements(By.ClassName("content"))
        .First()
        .FindElements(By.TagName("p"));
    var summary = string.Join("\n", paragraphs.Select(i => i.Text));

    return new Novel()
    {

        Title = title,
        Author = author,
        Rank = int.Parse(rank),
        Views = int.Parse(views),
        Bookmarked = int.Parse(bookmarked),
        Status = status,
        Categories = objCategories,
        ImageURL = imgUrl,
        Summary = summary,
    };
};


static string ConvertNovelNumbersToString(string parent)
{
    string output = "";

    if (parent[parent.Length - 1] == 'K' || parent[parent.Length - 1] == 'M')
    {
        parent = parent.Remove(parent.Length - 1);
        parent = parent.Replace('.', ',');

        output = (double.Parse(parent) * 1000000).ToString();

        if (parent[parent.Length - 1] == 'K')
        {
            output = (double.Parse(parent) * 1000).ToString();
        }
    }
    else
    {
        parent = parent.Remove(parent.Length - 1);
        output = parent;
    }

    return output;
}