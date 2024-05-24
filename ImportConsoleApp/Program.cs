using Persistence;

Console.WriteLine("Import der Movies und Categories in die Datenbank");
await using var unitOfWork = new UnitOfWork();
Console.WriteLine("Datenbank löschen");
await unitOfWork.DeleteDatabaseAsync();
Console.WriteLine("Datenbank migrieren");
await unitOfWork.MigrateDatabaseAsync();

var (novels, categories) = await ImportController.ReadFromCsvAsync();

await unitOfWork.Novels.AddRangeAsync(novels);
await unitOfWork.Categories.AddRangeAsync(categories);
await unitOfWork.SaveChangesAsync();
Console.WriteLine();
Console.Write("Beenden mit Eingabetaste ...");
Console.ReadLine();