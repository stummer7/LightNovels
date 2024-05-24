using Core.Entities;

public class ImportController
{

    const string FileNameNovels = "Novels.csv";
    const string FileNameCategories = "Categories.csv";

    public async static Task<(IEnumerable<Novel>,IEnumerable<Category>)> ReadFromCsvAsync()
    {
        var lines = (await File.ReadAllLinesAsync(FileNameCategories))
            .Skip(1)
            .Select(l => l.Split(";"))
            .ToList();

        var lines2 = (await File.ReadAllLinesAsync(FileNameNovels))
            .Skip(1)
            .Select(l => l.Split(";"))
            .ToList();

        var categories = lines
            .GroupBy(l => l[0])
            .Select(g => new Category()
            {
                Name = g.Key,
            }).ToList();

        var novels = lines2.Select(l => new Novel()
        {
            Title = l[0],
            Author = l[1],
            Description = l[2],
            Status = l[3],
            Rank = int.Parse(l[4]),
            Views = int.Parse(l[5]),
            Bookmarked = int.Parse(l[6]),
            ImageURL = l[7],
        }).ToList();

        foreach (var item in lines)
        {
            if (item.Length > 1)
            {
                Novel? novel = novels.SingleOrDefault(n => n.Title == item[1]);
                Category category = categories.Single(c => c.Name == item[0]);

                novel?.Categories.Add(category);
            }
        }
        return (novels ,categories);
    }

    static void Main(string[] args)
    {

    }
}