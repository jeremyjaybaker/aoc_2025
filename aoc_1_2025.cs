int current = 50;
string fileContent = File.ReadAllText("./aoc_1_2025_input.txt");
string[] instructions = fileContent.Split(new[] { "\n" }, StringSplitOptions.None);
(int ZeroCount, int CrossZeroCount) counts = (0, 0);

for (int counter = 0; counter < instructions.Length; counter++)
{
  // No error handling, file assumed to be pristine
  string instruction = instructions[counter];
  char direction = instruction.Trim()[0];
  int magnitude = int.Parse(instruction.Trim().Substring(1));

  var result = rotate(current, direction, magnitude);
  current = result.Current;
  
  if (result.Current == 0) counts.ZeroCount++;
  counts.CrossZeroCount += result.CrossZeroCount;

  if (result.CrossZeroCount > 0)
    Console.WriteLine($"Console is rotated {instruction} to point at {current}; during this rotation, it crosses zero without stopping {result.CrossZeroCount} times.");
  else
    Console.WriteLine($"Console is rotated {instruction} to point at {current}.");
}

// @param direction Left or Right, ie one of 'L' or 'R'
// @param magnitude can be any integer greater than zero, including well over 99
// @return Tuple including the current value plus how many times zero was rotated past
static (int Current, int CrossZeroCount) rotate(int current, char direction, int magnitude)
{
  int simplifiedMagnitude = magnitude % 100;
  int crossZeroCount = magnitude / 100;
  bool notStartedAtZero = current != 0;

  if (direction == 'L')
    current -= simplifiedMagnitude;
  else if (direction == 'R')
    current += simplifiedMagnitude;

  if (current < 0)
  {
    current += 100;
    if (notStartedAtZero && current != 0) crossZeroCount += 1;
  }
  else if (current > 99)
  {
    current -= 100;
    if (notStartedAtZero && current != 0) crossZeroCount += 1;
  }

    return (current, crossZeroCount);
}

Console.WriteLine($"There are {counts.ZeroCount} instances of the dial stopping at zero.");
Console.WriteLine($"There are {counts.CrossZeroCount} instances of the dial crossing zero without stopping.");
Console.WriteLine($"There are {counts.ZeroCount + counts.CrossZeroCount} total clicks at zero.");