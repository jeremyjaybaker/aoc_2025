// NOTE: this solution works for both part 2 and part 1

using System.Linq;
public static class MyExtensions
{
  // Returns a substring for the given number string, and also converts said 
  // substring into an array of ints
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
    // Toggle this between 2 (part 1) and 12 (part 2)
    int joltageSize = 12;
    // Whether or not to turn on debug logs
    bool debug = false;

    foreach(string bank in banks)
    {
      long maxJoltage = long.Parse(RecurseSubstring(bank, joltageSize, debug));
      Console.WriteLine($"Max joltage for bank {bank} is {maxJoltage}");
      maxJoltages.Add(maxJoltage);
    }

    Console.WriteLine($"Sum of max joltages is {maxJoltages.Sum()}");
  }

  // The general algorithm for this is as follows using the number
  // 234234234234278 and finding 12-digit joltage.
  //
  // - First, we must ignore the last 11 digits because they cannot contain the first
  //   digit of the largest joltage. Therefore, we must choose the largest digit amonst
  //   2342 to be our largest joltage's first digit. This is 4 at index 2. Note that if
  //   there's more than once instance of the largest number, you must always select the
  //   left-most instance of the number.
  // - We know the rest of the digits in our largest joltage are to the right of 4, ie
  //   234234234278. We apply the same algorithm to this substring, ignoring the last 10
  //   digits and considering only 23. 3 is the largest of this set, so we select it and
  //   know that the third digit must come from all numbers to the right, ie
  //   4234234278.
  // - Continue applying this algorithm recursively, selecting a single digit each time.
  //   The list of numbers assembled in this fashion is the largest joltage for the given
  //   battery bank.
  static string RecurseSubstring(string number, int substringLength, bool debug = false)
  {
    int substringEnd = number.Length - (substringLength - 1);
    int firstDigit = number.IntArrayFromSubstring(0, substringEnd).Max();
    int index = number.IndexOf($"{firstDigit}");

    if (debug)
      Console.WriteLine($"Battery {number} with {substringLength} digits of joltage has a first digit of {firstDigit} at {index}");

    string currentState = $"{firstDigit}";
    int newSubstringLength = substringLength - 1;
    string newNumber = number.Substring(index + 1);
    if (newSubstringLength == 0)
      return currentState;
    else
      return currentState += RecurseSubstring(newNumber, newSubstringLength, debug);
  }
}