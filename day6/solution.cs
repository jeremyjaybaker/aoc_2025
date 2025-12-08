using System.Text.RegularExpressions;

string fileContent = File.ReadAllText("./day6/input.txt");
string[] lines = fileContent.Split(new[] { "\n" }, StringSplitOptions.None);

bool part2 = true;

// Initial setup includes:
// - array of operators
// - indexes where each column of numbers needs to be split
// - a 2-dimensional array of stringified numbers for easier access and
//   iteration through the input list of numbers
string[] operators = GetOperatorArray(lines);
int[] columnDelineators = GetColumnDelineators(lines[lines.Length-1]);
string[] linesWithoutOperators = new string[lines.Length - 1];
Array.Copy(lines, 0, linesWithoutOperators, 0, lines.Length - 1);
string[,] numberGrid = CreateGrid(linesWithoutOperators, columnDelineators);

long totalSum = 0;
long columnResult;
for (int column = 0; column < operators.Length; column++)
{
  // Create the set of numbers to operate on for the current column
  string[] columnNumbers = new string[linesWithoutOperators.Length];
  for (int row = 0; row < linesWithoutOperators.Length; row++)
    columnNumbers[row] = numberGrid[row, column];

  if (part2)
    columnNumbers = ColumnTransform(columnNumbers);
  //Console.WriteLine($"After transformation, col nums are {string.Join(",", columnNumbers.ToList().Select(a => $"{a}"))}");

  if (operators[column] == "+")
  {
    columnResult = columnNumbers.ToList().Select(a => long.Parse(a)).Sum();
  }
  else if (operators[column] == "*")
  {
    columnResult = 1;
    foreach (string num in columnNumbers)
      columnResult *= long.Parse(num);
  }
  else
    throw new InvalidOperationException($"Operations should be either + or *, but is {operators[column]}");

  totalSum += columnResult;
}

Console.WriteLine($"Total sum is {totalSum}");

static string[] ColumnTransform(string[] column)
{
  // Count of the numbers that will be present in the entire column's
  // addition or multiplication
  int componentCount = $"{column.ToList().Select(str => long.Parse(str)).Max()}".Length;

  Console.WriteLine($"Component count is {componentCount}");
  List<string> components = [];

  for (int i = 0; i < componentCount; i++)
  {
    List<char> component = [];
    foreach (string str in column)
    {
      // null/empty check
      if (str[i] == ' ') continue;
      component.Add(str[i]);
    }

    Console.WriteLine($"Setting component[{i}]: {string.Join("", component)}");
    components.Add(string.Join("", component));
  }

  return components.ToArray();
}

// Returns a stringified number with leading dashes depending on digitCount.
// Example: number = 6, digitCount = 4, return = "---6"
static string PaddedNumberAsString(int number, int digitCount)
{
  int dashCount = digitCount - $"{number}".Length;
  string dashes = string.Join("", Enumerable.Repeat("-", dashCount).ToArray());
  return $"{dashes}{number}";
}

/*static int[] MakeNumberArrayFromLine(string line)
{
  List<string> numList = Regex.Split(line, @"\s+").ToList();
  numList.RemoveAll(str => string.IsNullOrEmpty(str));
  
  return numList
    .Select(str => int.Parse(str))
    .ToArray();
}*/

static string[] GetOperatorArray(string[] lines)
{
  List<string> split = Regex.Split(lines[lines.Length - 1], @"\s+").ToList();
  split.RemoveAll(str => string.IsNullOrEmpty(str));

  return split.ToArray();
}

static string[,] CreateGrid(string[] lines, int[] columnDelineators)
{
  string[,] numberGrid = new string[lines.Length, columnDelineators.Length + 1];

  for (int i = 0; i < lines.Length; i++)
  {
    string line = lines[i];
    string[] numbers = SplitStringOnAllIndexes(line, columnDelineators);

    for (int j = 0; j < numbers.Length; j++)
      numberGrid[i,j] = numbers[j];
  }

  return numberGrid;
}

static int[] GetColumnDelineators(string operators)
{
  List<int> plusIndexes = AllIndexesOf(operators, "+");
  List<int> timesIndexes = AllIndexesOf(operators, "*");

  // The index directly to the left of each operator except for
  // the first operator is a column delineator. Ignoring the first
  // operator is why we need to remove index 0
  plusIndexes.AddRange(timesIndexes);
  plusIndexes.Remove(0);
  plusIndexes.Sort();

  // Subtract 1 to get the index to the left of the operator
  return plusIndexes.Select(i => i - 1).ToArray();
}

static string[] SplitStringOnAllIndexes(string line, int[] indexes)
{
  int startIndex = 0;
  int endIndex = indexes[0] - 1;
  string substr = line.Substring(startIndex, endIndex - startIndex + 1);
  List<string> strings = new List<string> { substr };

  for (int i = 0; i < indexes.Length - 1; i++)
  {
    startIndex = indexes[i] + 1;
    endIndex = indexes[i+1] - 1;
    substr = line.Substring(startIndex, endIndex - startIndex + 1);
    strings.Add(substr);
  }

  startIndex = indexes[indexes.Length - 1] + 1;
  endIndex = line.Length;
  substr = line.Substring(startIndex, endIndex - startIndex);
  strings.Add(substr);

  return strings.ToArray();
}

static List<int> AllIndexesOf(string subject, string pattern)
{
  List<int> Indexes = [];

  int currentIndex = 0;
  while ((currentIndex = subject.IndexOf(pattern, currentIndex)) != -1)
  {
    Indexes.Add(currentIndex);
    currentIndex++;
  }

  return Indexes;
}