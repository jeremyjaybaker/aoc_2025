using System.Text;
string fileContent = File.ReadAllText("./day4/input.txt");
// For the second part, you need 2 copies of the roll grid because one is 
// used during the program as a reference, and the other grid keeps track of
// which rolls have been removed. If you use a single grid and remove as you
// go, it changes the downstream answers which isn't how the puzzle works.
string[] rows = fileContent.Split(new[] { "\n" }, StringSplitOptions.None);
string[] processedRows = new string[rows.Length];
// By default, arrays are assigned by ref, so we need to make an independent copy
Array.Copy(rows, processedRows, rows.Length);

// false for part 1, true for part 2
bool part2 = false;

int accessibleRollCount = 0;
// Keeps track of how many rolls got removed in a single iteration. If
// zero, it means there's no more rolls that can be removed
int removedCount = 0;
do
{
  removedCount = 0;
  for(int y = 0; y < rows.Length; y++)
    for(int x = 0; x < rows[0].Length; x++)
      if (HasRoll(x, y, rows) && Accessible(x, y, rows))
      {
        if (part2)
        {
          removedCount++;
          // Strings are read-only in C#, so simple char replacement at a specific
          // index must be done by replacing the entire string
          StringBuilder sb = new StringBuilder(processedRows[y]);
          sb[x] = 'x';
          processedRows[y] = sb.ToString();
        }
        accessibleRollCount++;
      }
  // Update the reference grid with the processed grid
  Array.Copy(processedRows, rows, rows.Length);
} while (removedCount > 0);

Console.WriteLine("The wrapping paper rolls now look like this:");
foreach(string row in processedRows)
  Console.WriteLine($"{row}");
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