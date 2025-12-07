using System.Text.RegularExpressions;

string fileContent = File.ReadAllText("./day6/input.txt");
string[] lines = fileContent.Split(new[] { "\n" }, StringSplitOptions.None);

string[] operators = CreateOperatorArray(lines);
string[] linesWithoutOperators = new string[lines.Length - 1];
Array.Copy(lines, 0, linesWithoutOperators, 0, lines.Length - 1);
int[,] numberGrid = CreateNumberGrid(linesWithoutOperators, operators.Length);

long totalSum = 0;
long columnResult;
for (int column = 0; column < operators.Length; column++)
{
  // Create the set of numbers to operate on for the current column
  int[] columnNumbers = new int[linesWithoutOperators.Length];
  for (int row = 0; row < linesWithoutOperators.Length; row++)
    columnNumbers[row] = numberGrid[row, column];

  if (operators[column] == "+")
  {
    columnResult = (long)columnNumbers.ToList().Sum();
  }
  else if (operators[column] == "*")
  {
    columnResult = 1;
    foreach (int num in columnNumbers)
      columnResult *= (long)num;
  }
  else
    throw new InvalidOperationException($"Operations should be either + or *, but is {operators[column]}");

  totalSum += columnResult;
}

Console.WriteLine($"Total sum is {totalSum}");

static int[] MakeNumberArrayFromLine(string line)
{
  List<string> numList = Regex.Split(line, @"\s+").ToList();
  numList.RemoveAll(str => string.IsNullOrEmpty(str));
  
  return numList
    .Select(str => int.Parse(str))
    .ToArray();
}

static string[] CreateOperatorArray(string[] lines)
{
  List<string> split = Regex.Split(lines[lines.Length - 1], @"\s+").ToList();
  split.RemoveAll(str => string.IsNullOrEmpty(str));

  return split.ToArray();
}

static int[,] CreateNumberGrid(string[] lines, int columnCount)
{
  int[,] numberGrid = new int[lines.Length, columnCount];

  for (int i = 0; i < lines.Length; i++)
  {
    string line = lines[i];
    int[] numbers = MakeNumberArrayFromLine(line);

    for (int j = 0; j < numbers.Length; j++)
      numberGrid[i,j] = numbers[j];
  }

  return numberGrid;
}