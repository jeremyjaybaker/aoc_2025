using System.Text.RegularExpressions;

string fileContent = File.ReadAllText("./day6/input.txt");
string[] lines = fileContent.Split(new[] { "\n" }, StringSplitOptions.None);

bool part2 = true;

char[] operators = GetOperatorArray(lines);

// Column delineators are indexes that separate columns of numbers from the input
string rawOperatorLine = lines[lines.Length-1];
int[] columnDelineators = GetColumnDelineators(rawOperatorLine);

// A copy of the input without the last line containing operators
string[] linesWithoutOperators = new string[lines.Length - 1];
Array.Copy(lines, 0, linesWithoutOperators, 0, lines.Length - 1);

// a 2-dimensional representation of the input numbers as strings including spaces,
// ie the columns
// 123  64
//  23 334
//   2  23
// is represented as [['123', ' 64'], [' 23', '334'], ['  2', ' 23']
//
// The spaces can simply be ignored for Part 1, and for Part 2 their position
// is relevant to determining Cephalopod Math results
string[,] numberGrid = CreateGrid(linesWithoutOperators, columnDelineators);

long totalSum = 0;
long columnResult;

for (int column = 0; column < operators.Length; column++)
{
  // Collect the set of numbers in the current column
  string[] columnNumbers = new string[linesWithoutOperators.Length];
  for (int row = 0; row < linesWithoutOperators.Length; row++)
    columnNumbers[row] = numberGrid[row, column];

  // Part 2 can use the same addition/multiplication code as Part 1
  // as long as you transform the numbers in each column into a 
  // different orientation
  if (part2) columnNumbers = ColumnTransform(columnNumbers);

  if (operators[column] == '+')
    columnResult = columnNumbers.ToList().Select(a => long.Parse(a)).Sum();
  else if (operators[column] == '*')
  {
    columnResult = 1;
    foreach (string num in columnNumbers) columnResult *= long.Parse(num);
  }
  else
    throw new InvalidOperationException($"Operations should be either + or *, but is {operators[column]}");

  totalSum += columnResult;
}

Console.WriteLine($"Total sum is {totalSum}");

// Used exclusively in Part 2
static string[] ColumnTransform(string[] column)
{
  // Count of the numbers that will be present in the entire column's addition or multiplication
  // Ie, 4 + 431 + 623 has a component count of 3
  int componentCount = $"{column.ToList().Select(str => long.Parse(str)).Max()}".Length;

  List<string> components = [];

  for (int i = 0; i < componentCount; i++)
  {
    List<char> component = [];
    foreach (string str in column)
    {
      // This is when preserving spacing within each cell of the grid
      // becomes critical
      if (str[i] == ' ') continue;
      component.Add(str[i]);
    }

    // Console.WriteLine($"Setting component[{i}]: {string.Join("", component)}");
    components.Add(string.Join("", component));
  }

  return components.ToArray();
}

static char[] GetOperatorArray(string[] lines)
{
  List<char> split = Regex
    .Split(lines[lines.Length - 1], @"\s+")
    .ToList()
    .Select(str => {
      if (string.IsNullOrEmpty(str))
        return '\0';
      else
        return str.ToCharArray()[0];
    })
    .ToList();

  // Remove any blank values before returning,
  // usually blank values are due to right-most
  // whitespace on the last operator.
  split.RemoveAll(c => c == 0);

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
  // This algorithm could probably be simplified, but it's conceptually
  // easier to:
  // 1) build the first string
  // 2) iterate through the delineation indexes and split out individual strings
  //    as you go. You'll need the current deliniator plus the next one which is
  //    why it's awkward to handle the very last string as part of the loop
  // 3) build the last string
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