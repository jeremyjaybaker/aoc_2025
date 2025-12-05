//using System.Linq;
string fileContent = File.ReadAllText("./day5/input.txt");
(long Start, long End)[] ranges = CreateRanges(fileContent);
long[] ingredientIds = CreateIngredientIds(fileContent);

HashSet<long> freshIds = [];
foreach ((long Start, long End) range in ranges)
  foreach(long id in ingredientIds)
    if(id >= range.Start && id <= range.End)
      freshIds.Add(id);

Console.WriteLine($"There are {freshIds.Count} fresh ingredients.");

static (long Start, long End)[] CreateRanges(string fileContent)
{
  string[] splitContent = fileContent.Split(new[] { "\n\n" }, StringSplitOptions.None);

  List<string> strRanges = splitContent[0].Split(new[] { "\n" }, StringSplitOptions.None).ToList();

  return strRanges.Select(str =>
    {
      string[] halves = str.Split(new[] { "-" }, StringSplitOptions.None);
      return (Start: long.Parse(halves[0]), End: long.Parse(halves[1]));
    }
  ).ToArray();
}

static long[] CreateIngredientIds(string fileContent)
{
  string[] splitContent = fileContent.Split(new[] { "\n\n" }, StringSplitOptions.None);
  List<string> freshStrIds = splitContent[1].Split(new[] { "\n" }, StringSplitOptions.None).ToList();
  return freshStrIds.Select(str => long.Parse(str)).ToArray();
}