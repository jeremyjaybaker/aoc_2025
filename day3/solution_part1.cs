using System.Linq;
string fileContent = File.ReadAllText("./day3/input.txt");
string[] banks = fileContent.Split(new[] { "\n" }, StringSplitOptions.None);

long[] bankJoltages = new long[banks.Length];
int joltageSize = 2;

for(int i = 0; i < banks.Length; i++)
{
  string bank = banks[i];
  int[] digits = new int[joltageSize];
  
  // The first digit is always the largest number in the bank IGNORING the
  // last number.
  // There may be multiple instances of this max number, and for the purposes
  // of determining the second digit, you should always select the left-most
  // instance of the first digit.
  // The second digit is the largest number to the right of this first digit.
  int substringEnd = bank.Length - (joltageSize - 1);
  int firstDigit = intArrayFromSubstring(bank, 0, substringEnd).Max();
  digits[0] = firstDigit;

  // +1 to IndexOf because Substring is inclusive and you don't want the firstDigit 
  // itself to be in the substring
  int secondDigit = intArrayFromSubstring(bank, bank.IndexOf($"{firstDigit}")+1).Max();
  digits[1] = secondDigit;

  bankJoltages[i] = long.Parse(string.Join("", digits.Select(d => $"{d}")));
  Console.WriteLine($"Largest joltage for bank {bank} is {bankJoltages[i]}");
}

static int[] intArrayFromSubstring(string numbers, int start, int? end = null)
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

Console.WriteLine($"Sum of all bank joltages is {bankJoltages.Sum()}");