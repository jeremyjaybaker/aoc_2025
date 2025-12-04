string fileContent = File.ReadAllText("./day4/input.txt");
string[] rows = fileContent.Split(new[] { "\n" }, StringSplitOptions.None);

int accessibleRollCount = 0;
for(int y = 0; y < rows.Length; y++)
  for(int x = 0; x < rows[0].Length; x++)
    if (HasRoll(x, y, rows) && Accessible(x, y, rows))
      accessibleRollCount++;

Console.WriteLine($"There are {accessibleRollCount} accessible rolls");

static bool Accessible(int x, int y, string[] rows)
{
  int adjacentRollCount = 0;
  List<int[]> adjacentCoords = [
    [y-1, x],
    [y+1, x],
    [y, x-1],
    [y, x+1],
    [y-1, x-1],
    [y-1, x+1],
    [y+1, x-1],
    [y+1, x+1]
  ];

  foreach(int[] pair in adjacentCoords)
    if (ValidCoords(pair[1], pair[0], rows) && HasRoll(pair[1], pair[0], rows))
      adjacentRollCount++;
  
  return adjacentRollCount < 4;
}

static bool HasRoll(int x, int y, string[] rows)
{
  return rows[y][x] == '@';
}

static bool ValidCoords(int x, int y, string[] rows)
{
  int xMax = rows[0].Length;
  int yMax = rows.Length;
  return (x >= 0 && y >= 0 && x < xMax && y < yMax);
}