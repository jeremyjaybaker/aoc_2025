string fileContent = File.ReadAllText("./day2/input.txt");
string[] ranges = fileContent.Split(new[] { "," }, StringSplitOptions.None);

// Controls whether this program is solving for part 1 or part 2 of the puzzle
// since they use different definitions
bool partTwo = true;

List<long> allInvalidIds = [];
foreach (string range in ranges)
{
  List<long> invalidIdsInRange = [];
  string[] stringPair = range.Split(new[] { "-" }, StringSplitOptions.None);
  (long Start, long End) intRange = (Start: long.Parse(stringPair[0]), End: long.Parse(stringPair[1]));

  for (long productId = intRange.Start; productId <= intRange.End; productId++)
  {
    string stringId = productId.ToString();

    if ((partTwo && invalidUnderPartTwoRules(stringId)) || invalidUnderPartOneRules(stringId))
    {
      invalidIdsInRange.Add(productId);
      allInvalidIds.Add(productId);
    }
  }

  if (invalidIdsInRange.Count > 0)
    Console.WriteLine($"{range} has {invalidIdsInRange.Count} invalid IDs: {string.Join(",", invalidIdsInRange)}");
  else
    Console.WriteLine($"{range} contains no invalid IDs.");
}

static bool invalidUnderPartOneRules(string id)
{
  // Odd-length numbers must be valid since they cannot have symmetrical halves
  int idLength = id.Length;
  if (idLength % 2 != 0) return false;

  bool halvesAreEqual = id.Substring(0, idLength / 2) == id.Substring(idLength / 2);
  if (halvesAreEqual) return true;

  return false;
}

// Methodology:
// - For each ID, construct substrings of growing length starting at the beginning of the ID
//   Ie, for "123123123", substrings used to test are "1", "12", "123", "1231"
// - For each substring, try to reconstruct the original string using only the substring
//   Reconstruction with "1" fails, "111111111" != "123123123"
//   Reconstructon with "12" fails, 2 does not cleanly divide 9
//   Reconstruction with "123" succeeds, "123123123" == "123123123", so the ID must be invalid
//   Reconstruction with "1231" does not happen as we've returned early
static bool invalidUnderPartTwoRules(string id)
{
  for (int substrLength = 1; substrLength <= id.Length / 2; substrLength++)
  {
    // If the current substring length doesn't cleanly divide the number of
    // digits in the ID, then the ID cannot be made up of said substring and
    // therefore the ID is valid
    if (id.Length % substrLength != 0) continue;

    // Attempt to reconstruct the given id using a substring of
    // `substrLength` length
    int repetitionCount = id.Length / substrLength;
    string substring = id.Substring(0, substrLength);

    List<string> idParts = [];
    for(int i = 0; i < repetitionCount; i++)
      idParts.Add(substring);

    // If able to reconstruct the ID, it must be invalid
    if (string.Join("", idParts) == id)
      return true;
  }

  return false;
}

Console.WriteLine($"The sum of all invalid IDs is {allInvalidIds.Sum()}");
