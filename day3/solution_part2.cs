using System.Linq;

// NOTE: this solution works for both part 2 and part 1
public static class MyExtensions
{
  // https://stackoverflow.com/questions/2641326/finding-all-positions-of-substring-in-a-larger-string-in-c-sharp
  public static int[] AllIndexesOf(this string str, string value) {
    if (String.IsNullOrEmpty(value))
      throw new ArgumentException("the string to find may not be empty", "value");
    List<int> indexes = new List<int>();
    for (int index = 0;; index += value.Length) {
    index = str.IndexOf(value, index);
      if (index == -1)
        return indexes.ToArray();
      indexes.Add(index);
    }
  }
  public static int[] IntArrayFromSubstring(this string numbers, int start, int? end = null)
  {
    string substring;
    if (end is not null)
      substring = numbers.Substring(start, (int)end);
    else
      substring = numbers.Substring(start);
    
    return substring
      .ToCharArray()
      .Select(s => (int)Char.GetNumericValue(s))
      .ToArray();
  }
}

public class Program
{
  public static void Main()
  {
    string fileContent = File.ReadAllText("./day3/input.txt");
    string[] banks = fileContent.Split(new[] { "\n" }, StringSplitOptions.None);

    List<long> maxJoltages = [];
    // Toggle this between 2 and 12 depending on the problem part
    int joltageSize = 12;
    // Whether or not to turn on debug logs
    bool debug = false;

    foreach(string bank in banks)
    {
      List<List<string>> result = RecurseSubstring(bank, joltageSize, debug);

      List<string> qwer = result.Select(s => string.Join("", s)).ToList();
      long maxJoltage = long.Parse(string.Join("", qwer));
      Console.WriteLine($"Max joltage for bank {bank} is {maxJoltage}");
      maxJoltages.Add(maxJoltage);
    }

    Console.WriteLine($"Sum of max joltages is {maxJoltages.Sum()}");
  }
  static List<List<string>> RecurseSubstring(string number, int substringLength, bool debug = false)
  {
    int substringEnd = number.Length - (substringLength - 1);
    int firstDigit = number.IntArrayFromSubstring(0, substringEnd).Max();
    int index = number.IndexOf($"{firstDigit}");

    if (debug)
      Console.WriteLine($"Battery {number} with {substringLength} digits of joltage has a first digit of {firstDigit} at {index}");

    List<List<string>> currentState = new List<List<string>> { new List<string> { $"{firstDigit}" } };
    int newSubstringLength = substringLength - 1;
    string newNumber = number.Substring(index+1);
    if (newSubstringLength == 0)
      return currentState;
    else
      return currentState.Concat(RecurseSubstring(newNumber, newSubstringLength, debug)).ToList();
  }
}