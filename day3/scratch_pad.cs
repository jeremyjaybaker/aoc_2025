using System.Linq;

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

  // Can both create a substring from a string, and turn said result into an int array
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

  // Simple method to help with debugging
  public static string Stringify(this List<int> numbers)
  {
    string result = "";
    for (int i = 0; i < numbers.Count; i++)
      result += $"{numbers[i]},";
    
    return result;
  }
}

// number = 811111111111119, substringLength = 2
// substringEnd = 13
// firstDigit = 8
// indexes = [0]
// first_iter
//   newSubstringLength = 1
//   newNumber = 11111111111119
public class Program
{
  public static void Main()
  {
    List<List<string>> test = RecurseSubstring("234234234234278", 12, true);
    List<string> options = [];

    List<string> qwer = test.Select(s => string.Join("", s)).ToList();

    Console.WriteLine($"Returned item is {string.Join("", qwer)}");
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