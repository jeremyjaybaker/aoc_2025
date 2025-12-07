public readonly record struct Range
{
  public required long Start { get; init; }
  public required long End { get; init; }

  public Range(long start, long end)
  {
    Start = start;
    End = end;
  }

  // Returns the number of IDs that are the same in both ranges
  public long OverlapAmount(Range range)
  {
    long overlapStart = Math.Max(Start, range.Start);
    long overlapEnd = Math.Min(End, range.End);

    return Math.Max(0, overlapEnd - overlapStart + 1);
  }

  public bool DoesOverlap(Range range)
  {
    return OverlapAmount(range) > 0;
  }

  public bool IsEmpty()
  {
    return (Start == 0 && End == 0);
  }

  public override string ToString()
  {
    if (IsEmpty())
      return "[]";
    else
      return $"[{Start},{End}]";
  }

  public long Length()
  {
    if (IsEmpty()) 
      return 0;
    else 
      return End - Start + 1;
  }

  public Range Merge(Range otherRange)
  {
    if (DoesOverlap(otherRange))
    {
      long newStart = otherRange.Start > Start ? Start : otherRange.Start;
      long newEnd = otherRange.End < End ? End : otherRange.End; 
      return new Range { Start = newStart, End = newEnd };
    }
    else
      return new Range { Start = Start, End = End };
  }

  public static Range FromString(string str)
  {
    string[] halves = str.Split(new[] { "-" }, StringSplitOptions.None);
    return new Range { Start = long.Parse(halves[0]), End = long.Parse(halves[1]) };
  }

  // Using 0,0 as sentinel values is problematic for a variety of reasons,
  // but it works within the scope of this challenge
  public static Range Empty()
  {
    return new Range { Start = 0, End = 0 };
  }
}

public class Program
{
  static void Main()
  {
    string fileContent = File.ReadAllText("./day5/input.txt");
    Range[] ranges = CreateRanges(fileContent);
    long[] ingredientIds = CreateIngredientIds(fileContent);

    HashSet<long> freshIds = [];
    foreach (Range range in ranges)
      foreach(long id in ingredientIds)
      if(id >= range.Start && id <= range.End)
        freshIds.Add(id);

    Console.WriteLine($"There are {freshIds.Count} fresh ingredients available (part 1).");

    // Algorithm:
    // 1 Start on the first range and iterate through all other ranges. If the first line
    //   overlaps with the current range, merge the two together and replace the first 
    //   line with the new, larger range. Leave an empty range in the latter's place because
    //   we don't want to merge it more than once.
    // 2 Repeat this process for all other lines.
    // 3 Executing this algorithm once for all lines isn't sufficient because, in the process
    //   of merging, you might create ranges that overlap with previous ranges. Therefore,
    //   we need to run steps 1 and 2 over and over again until we've executed an iteration
    //   where no merges have taken place at all.
    (Range[] Ranges, int MergeCount) results = MergeAll(ranges);
    while(results.MergeCount > 0)
      results = MergeAll(results.Ranges);

    long sum = 0;
    for (int i = 0; i < ranges.Length; i++)
      sum += ranges[i].Length();

    Console.WriteLine($"There are {sum} fresh ingredients available (part 2).");
  }

  static (Range[] Ranges, int MergeCount) MergeAll(Range[] ranges)
  {
    int mergeCount = 0;
    for (int i = 0; i < ranges.Length; i++)
    {
      if (ranges[i].IsEmpty()) continue;

      for (int j = i + 1; j < ranges.Length; j++)
      {
        if (ranges[j].IsEmpty()) continue;

        if (ranges[i].DoesOverlap(ranges[j]))
        {
          mergeCount++;
          ranges[i] = ranges[i].Merge(ranges[j]);
          ranges[j] = Range.Empty();
        }
      }
    }

    return (ranges, mergeCount);
  }

  static Range[] CreateRanges(string fileContent)
  {
    string[] splitContent = fileContent.Split(new[] { "\n\n" }, StringSplitOptions.None);

    List<string> strRanges = splitContent[0].Split(new[] { "\n" }, StringSplitOptions.None).ToList();

    return strRanges.Select(str => Range.FromString(str)).ToArray();
  }

  static long[] CreateIngredientIds(string fileContent)
  {
    string[] splitContent = fileContent.Split(new[] { "\n\n" }, StringSplitOptions.None);
    List<string> freshStrIds = splitContent[1].Split(new[] { "\n" }, StringSplitOptions.None).ToList();
    return freshStrIds.Select(str => long.Parse(str)).ToArray();
  }
}