
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Schema;
using Microsoft.VisualBasic;

namespace DSA
{
    internal class DSA
    {

        static void Main(string[] args)
        {

            //string input = new StreamReader("./inputs/7 test input.txt").ReadToEnd();
            string input = new StreamReader("./inputs/7 input.txt").ReadToEnd();
            System.Console.WriteLine(input.Length);
            //System.Console.WriteLine(new Day7().Puzzle1(input.Split("\r\n")));
            System.Console.WriteLine(new Day7().Puzzle2(input.Split("\r\n")));
        }
    }
    public class Day8
    {
        public long Puzzle2(string[] args)
        {
            return 0;
        }
        public long Puzzle1(string[] args)
        {
            return 0;
        }
    }
    public class Day7
    {
        List<char> cardStr = new() { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
        // K88QK - 12 x 10000 + 7 x 1000 +  7 x 100 + 11 x 10 + 12 x 1 = 1 278 220
        // K4J67 - 12 x 10000 + 3 x 1000 + 10 x 100 +  5 x 10 +  6 x 1 = 1 240 560
        // K4J76 - 12 x 10000 + 3 x 1000 + 10 x 100 +  6 x 10 +  5 x 1 = 1 240 650
        List<char> cardStr2 = new() { 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };

        public long Puzzle2(string[] games)
        {
            Dictionary<string, int> bids = new();
            Dictionary<string, int> gameValues = new();
            Dictionary<string, long> fivekind = new();
            Dictionary<string, long> fourkind = new();
            Dictionary<string, long> fullhouse = new();
            Dictionary<string, long> threekind = new();
            Dictionary<string, long> twopair = new();
            Dictionary<string, long> onepair = new();
            Dictionary<string, long> highcard = new();

            for (int i = 0; i < games.Length; i++)
            {
                // create bids dict
                string[] gameDetails = games[i].Split(' ');
                bids.Add(gameDetails[0], Convert.ToInt32(gameDetails[1]));
                string cards = gameDetails[0];
                long orderValue = 0;
                // count cards
                Dictionary<char, int> cardCount = new();
                for (int j = 0; j < cards.Length; j++)
                {
                    char card = cards[j];
                    if (cardCount.ContainsKey(card))
                    {
                        cardCount[card]++;
                    }
                    else
                    {
                        cardCount.Add(card, 1);
                    }
                    orderValue += (cardStr2.IndexOf(card) + 1) * (long)Math.Pow(10, (cards.Length - j) * 2);
                }


                // evaluate cards
                int count = cardCount.Count;
                if (cardCount.ContainsKey('J'))
                {
                    int jCount = cardCount['J'];
                    if (jCount < 5)
                    {
                        count--;
                        cardCount.Remove('J');
                        var maxKVP = cardCount.MaxBy(kvp => kvp.Value);
                        cardCount[maxKVP.Key] += jCount;
                    }
                }
                switch (count)
                {
                    case 1:
                        // Five of a kind, where all five cards have the same label: AAAAA
                        fivekind.Add(cards, orderValue);
                        break;
                    case 2:
                        if (cardCount.ContainsValue(4))
                        {
                            // Four of a kind, where four cards have the same label and one card has a different label: AA8AA
                            fourkind.Add(cards, orderValue);
                        }
                        else
                        {
                            // Full house, where three cards have the same label, and the remaining two cards share a different label: 23332
                            fullhouse.Add(cards, orderValue);
                        }
                        break;
                    case 3:
                        if (cardCount.ContainsValue(3))
                        {
                            // Three of a kind, where three cards have the same label, and the remaining two cards are each different from any other card in the hand: TTT98
                            threekind.Add(cards, orderValue);
                        }
                        else
                        {
                            // Two pair, where two cards share one label, two other cards share a second label, and the remaining card has a third label: 23432
                            twopair.Add(cards, orderValue);
                        }
                        break;
                    case 4:
                        // One pair, where two cards share one label, and the other three cards have a different label from the pair and each other: A23A4
                        onepair.Add(cards, orderValue);
                        break;
                    case 5:
                        // High card, where all cards' labels are distinct: 23456
                        highcard.Add(cards, orderValue);
                        break;
                }
            }

            // sort game values
            var sortedDict = from game in fivekind orderby game.Value ascending select game;
            fivekind = sortedDict.ToDictionary<KeyValuePair<string, long>, string, long>(pair => pair.Key, pair => pair.Value);

            sortedDict = from game in fourkind orderby game.Value ascending select game;
            fourkind = sortedDict.ToDictionary<KeyValuePair<string, long>, string, long>(pair => pair.Key, pair => pair.Value);

            sortedDict = from game in fullhouse orderby game.Value ascending select game;
            fullhouse = sortedDict.ToDictionary<KeyValuePair<string, long>, string, long>(pair => pair.Key, pair => pair.Value);

            sortedDict = from game in threekind orderby game.Value ascending select game;
            threekind = sortedDict.ToDictionary<KeyValuePair<string, long>, string, long>(pair => pair.Key, pair => pair.Value);

            sortedDict = from game in twopair orderby game.Value ascending select game;
            twopair = sortedDict.ToDictionary<KeyValuePair<string, long>, string, long>(pair => pair.Key, pair => pair.Value);

            sortedDict = from game in onepair orderby game.Value ascending select game;
            onepair = sortedDict.ToDictionary<KeyValuePair<string, long>, string, long>(pair => pair.Key, pair => pair.Value);

            sortedDict = from game in highcard orderby game.Value ascending select game;
            highcard = sortedDict.ToDictionary<KeyValuePair<string, long>, string, long>(pair => pair.Key, pair => pair.Value);

            // merge all games
            var allGames = new List<string>(highcard.Count + onepair.Count + twopair.Count + threekind.Count + fullhouse.Count + fourkind.Count + fivekind.Count);
            allGames.AddRange(highcard.Keys.ToList());
            allGames.AddRange(onepair.Keys.ToList());
            allGames.AddRange(twopair.Keys.ToList());
            allGames.AddRange(threekind.Keys.ToList());
            allGames.AddRange(fullhouse.Keys.ToList());
            allGames.AddRange(fourkind.Keys.ToList());
            allGames.AddRange(fivekind.Keys.ToList());

            // loop and calculate total bid
            long total = 0;
            for (int i = 0; i < allGames.Count; i++)
            {
                total += (i + 1) * bids[allGames[i]];
            }
            return total;
        }
        public long Puzzle1(string[] games)
        {
            Dictionary<string, int> bids = new();
            Dictionary<string, int> gameValues = new();
            Dictionary<string, long> fivekind = new();
            Dictionary<string, long> fourkind = new();
            Dictionary<string, long> fullhouse = new();
            Dictionary<string, long> threekind = new();
            Dictionary<string, long> twopair = new();
            Dictionary<string, long> onepair = new();
            Dictionary<string, long> highcard = new();

            for (int i = 0; i < games.Length; i++)
            {
                // create bids dict
                string[] gameDetails = games[i].Split(' ');
                bids.Add(gameDetails[0], Convert.ToInt32(gameDetails[1]));
                string cards = gameDetails[0];
                long orderValue = 0;
                // count cards
                Dictionary<char, int> cardCount = new();
                for (int j = 0; j < cards.Length; j++)
                {
                    char card = cards[j];
                    if (cardCount.ContainsKey(card))
                    {
                        cardCount[card]++;
                    }
                    else
                    {
                        cardCount.Add(card, 1);
                    }
                    orderValue += (cardStr.IndexOf(card) + 1) * (long)Math.Pow(10, (cards.Length - j) * 2);
                }


                // evaluate cards
                switch (cardCount.Count)
                {
                    case 1:
                        // Five of a kind, where all five cards have the same label: AAAAA
                        fivekind.Add(cards, orderValue);
                        break;
                    case 2:
                        if (cardCount.ContainsValue(4))
                        {
                            // Four of a kind, where four cards have the same label and one card has a different label: AA8AA
                            fourkind.Add(cards, orderValue);
                        }
                        else
                        {
                            // Full house, where three cards have the same label, and the remaining two cards share a different label: 23332
                            fullhouse.Add(cards, orderValue);
                        }
                        break;
                    case 3:
                        if (cardCount.ContainsValue(3))
                        {
                            // Three of a kind, where three cards have the same label, and the remaining two cards are each different from any other card in the hand: TTT98
                            threekind.Add(cards, orderValue);
                        }
                        else
                        {
                            // Two pair, where two cards share one label, two other cards share a second label, and the remaining card has a third label: 23432
                            twopair.Add(cards, orderValue);
                        }
                        break;
                    case 4:
                        // One pair, where two cards share one label, and the other three cards have a different label from the pair and each other: A23A4
                        onepair.Add(cards, orderValue);
                        break;
                    case 5:
                        // High card, where all cards' labels are distinct: 23456
                        highcard.Add(cards, orderValue);
                        break;
                }
            }

            // sort game values
            var sortedDict = from game in fivekind orderby game.Value ascending select game;
            fivekind = sortedDict.ToDictionary<KeyValuePair<string, long>, string, long>(pair => pair.Key, pair => pair.Value);

            sortedDict = from game in fourkind orderby game.Value ascending select game;
            fourkind = sortedDict.ToDictionary<KeyValuePair<string, long>, string, long>(pair => pair.Key, pair => pair.Value);

            sortedDict = from game in fullhouse orderby game.Value ascending select game;
            fullhouse = sortedDict.ToDictionary<KeyValuePair<string, long>, string, long>(pair => pair.Key, pair => pair.Value);

            sortedDict = from game in threekind orderby game.Value ascending select game;
            threekind = sortedDict.ToDictionary<KeyValuePair<string, long>, string, long>(pair => pair.Key, pair => pair.Value);

            sortedDict = from game in twopair orderby game.Value ascending select game;
            twopair = sortedDict.ToDictionary<KeyValuePair<string, long>, string, long>(pair => pair.Key, pair => pair.Value);

            sortedDict = from game in onepair orderby game.Value ascending select game;
            onepair = sortedDict.ToDictionary<KeyValuePair<string, long>, string, long>(pair => pair.Key, pair => pair.Value);

            sortedDict = from game in highcard orderby game.Value ascending select game;
            highcard = sortedDict.ToDictionary<KeyValuePair<string, long>, string, long>(pair => pair.Key, pair => pair.Value);

            // merge all games
            var allGames = new List<string>(highcard.Count + onepair.Count + twopair.Count + threekind.Count + fullhouse.Count + fourkind.Count + fivekind.Count);
            allGames.AddRange(highcard.Keys.ToList());
            allGames.AddRange(onepair.Keys.ToList());
            allGames.AddRange(twopair.Keys.ToList());
            allGames.AddRange(threekind.Keys.ToList());
            allGames.AddRange(fullhouse.Keys.ToList());
            allGames.AddRange(fourkind.Keys.ToList());
            allGames.AddRange(fivekind.Keys.ToList());

            // loop and calculate total bid
            long total = 0;
            for (int i = 0; i < allGames.Count; i++)
            {
                total += (i + 1) * bids[allGames[i]];
            }
            return total;
        }
        public List<string> SortGames(List<string> games)
        {
            foreach (string game in games)
            {

            }
            return new List<string> { };
        }
    }
    public class Day6
    {
        public long Puzzle2(string[] races)
        {
            long time = Convert.ToInt64(races[0].Split(": ")[1].Replace(" ", ""));
            long record = Convert.ToInt64(races[1].Split(": ")[1].Replace(" ", ""));

            long passedCount = 0;
            for (long j = 1; j < time; j++)
            {
                long distanceTraveled = (time - j) * j;
                if (distanceTraveled > record)
                {
                    passedCount++;
                }
            }
            return passedCount;
        }
        public int Puzzle1(string[] races)
        {
            int[] times = races[0].Split(": ")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => Convert.ToInt32(n)).ToArray();
            int[] records = races[1].Split(": ")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => Convert.ToInt32(n)).ToArray();
            List<int> variants = new();
            for (int i = 0; i < times.Length; i++)
            {
                int passedCount = 0;
                for (int j = 1; j <= times[i]; j++)
                {
                    int distanceTraveled = (times[i] - j) * j;
                    if (distanceTraveled > records[i])
                    {
                        passedCount++;
                    }
                }
                variants.Add(passedCount);
            }
            int total = 1;
            foreach (int count in variants)
            {
                total *= count;
            }


            return total;
        }
    }
    public class Day5
    {
        public long Puzzle2(string[] almanac)
        {
            // parse almanac data
            long[] seeds = almanac[0].Split(": ")[1].Split(" ").Select(n => Convert.ToInt64(n)).ToArray();
            List<KeyValuePair<long, long>> seedIntervals = new();
            for (int j = 0; j < seeds.Length; j += 2)
            {
                seedIntervals.Add(new KeyValuePair<long, long>(seeds[j], seeds[j + 1]));
            }


            int i = 1;
            Dictionary<string, List<long[]>> maps = new();
            string[] mapCategories = new string[] { "seed-to-soil", "soil-to-fertilizer", "fertilizer-to-water", "water-to-light", "light-to-temperature", "temperature-to-humidity", "humidity-to-location" };
            foreach (string mapCategory in mapCategories)
            {
                maps.Add(mapCategory, new List<long[]>());
            }

            while (i < almanac.Length)
            {
                if (almanac[i].Length == 0)
                {
                    i++;
                    continue;
                }
                string mapName = almanac[i].Split(" ")[0];
                if (maps.ContainsKey(mapName))
                {
                    // skip map title
                    i++;
                    while (i < almanac.Length && almanac[i].Length != 0)
                    {
                        // parse the data and loop to fill mapping list
                        string[] mapDetails = almanac[i].Split(" ");

                        long sourceStart = long.Parse(mapDetails[1]);
                        long destinationStart = long.Parse(mapDetails[0]);
                        long count = long.Parse(mapDetails[2]);
                        maps[mapName].Add(new long[] { sourceStart, destinationStart, count });
                        i++;
                    }
                }
            }

            List<KeyValuePair<long, long>> soil = GetMappingInterval(maps["seed-to-soil"], seedIntervals);
            List<KeyValuePair<long, long>> fertilizer = GetMappingInterval(maps["soil-to-fertilizer"], soil);
            List<KeyValuePair<long, long>> water = GetMappingInterval(maps["fertilizer-to-water"], fertilizer);
            List<KeyValuePair<long, long>> light = GetMappingInterval(maps["water-to-light"], water);
            List<KeyValuePair<long, long>> temperature = GetMappingInterval(maps["light-to-temperature"], light);
            List<KeyValuePair<long, long>> humidity = GetMappingInterval(maps["temperature-to-humidity"], temperature);
            List<KeyValuePair<long, long>> location = GetMappingInterval(maps["humidity-to-location"], humidity);
            long locMin = long.MaxValue;
            for (int j = 0; j < location.Count; j++)
            {
                if (locMin > location[j].Key)
                {
                    locMin = location[j].Key;
                }
            }

            return locMin;
        }

        public long Puzzle1(string[] almanac)
        {
            // parse almanac data
            long[] seeds = almanac[0].Split(": ")[1].Split(" ").Select(n => Convert.ToInt64(n)).ToArray();
            int i = 1;
            Dictionary<string, List<long[]>> maps = new();
            string[] mapCategories = new string[] { "seed-to-soil", "soil-to-fertilizer", "fertilizer-to-water", "water-to-light", "light-to-temperature", "temperature-to-humidity", "humidity-to-location" };
            foreach (string mapCategory in mapCategories)
            {
                maps.Add(mapCategory, new List<long[]>());
            }

            while (i < almanac.Length)
            {
                if (almanac[i].Length == 0)
                {
                    i++;
                    continue;
                }
                string mapName = almanac[i].Split(" ")[0];
                if (maps.ContainsKey(mapName))
                {
                    // skip map title
                    i++;
                    while (i < almanac.Length && almanac[i].Length != 0)
                    {
                        // parse the data and loop to fill mapping list
                        string[] mapDetails = almanac[i].Split(" ");

                        long sourceStart = long.Parse(mapDetails[1]);
                        long destinationStart = long.Parse(mapDetails[0]);
                        long count = long.Parse(mapDetails[2]);
                        maps[mapName].Add(new long[] { sourceStart, destinationStart, count });
                        i++;
                    }
                }
            }
            List<long> locations = new();
            foreach (long seed in seeds)
            {
                long soil = GetMapping(maps["seed-to-soil"], seed);
                long fertilizer = GetMapping(maps["soil-to-fertilizer"], soil);
                long water = GetMapping(maps["fertilizer-to-water"], fertilizer);
                long light = GetMapping(maps["water-to-light"], water);
                long temperature = GetMapping(maps["light-to-temperature"], light);
                long humidity = GetMapping(maps["temperature-to-humidity"], temperature);
                long location = GetMapping(maps["humidity-to-location"], humidity);
                locations.Add(location);
            }
            return locations.Min();
        }

        private long GetMapping(List<long[]> maps, long val)
        {
            foreach (long[] map in maps)
            {
                long source = map[0];
                long target = map[1];
                long count = map[2];
                if (val >= source && val <= source + count - 1)
                {
                    long offset = val - source;
                    return target + offset;
                }
            }
            return val;

        }
        private List<KeyValuePair<long, long>> GetMappingInterval(List<long[]> maps, List<KeyValuePair<long, long>> intervals)
        {
            List<KeyValuePair<long, long>> outIntervals = new();
            foreach (KeyValuePair<long, long> interval in intervals)
            {
                long intervalStart = interval.Key;
                long intervalLength = interval.Value;
                foreach (long[] map in maps)
                {
                    long source = map[0];
                    long target = map[1];
                    long count = map[2];
                    if (intervalStart < source && intervalStart + intervalLength < source)
                    {
                        // outside of all mapping range
                        continue;
                    }

                    if (intervalStart > source + count - 1)
                    {
                        // outside of all mapping range
                        continue;
                    }

                    // some parts outside, some parts inside
                    long outsideLeftPart = source - intervalStart;
                    long outsideRightPart = intervalStart + intervalLength - (source + count);
                    if (outsideLeftPart > 0)
                    {
                        //Debug.Assert(intervalStart != 0);
                        outIntervals.Add(new KeyValuePair<long, long>(intervalStart, outsideLeftPart));
                    }
                    if (outsideRightPart > 0)
                    {
                        //Debug.Assert(source + count != 0);
                        outIntervals.Add(new KeyValuePair<long, long>(source + count, outsideRightPart));
                    }

                    long startOffset = intervalStart > source ? intervalStart - source : 0;
                    long endOffset = intervalStart + intervalLength < source + count ? source + count - (intervalStart + intervalLength) : 0;
                    long start = target + startOffset;
                    long end = count - startOffset - endOffset;
                    //Debug.Assert(start != 0);
                    outIntervals.Add(new KeyValuePair<long, long>(start, end));
                }
                if (outIntervals.Count == 0)
                {
                    //Debug.Assert(intervalStart != 0);
                    outIntervals.Add(new KeyValuePair<long, long>(intervalStart, intervalLength));
                }
            }

            return outIntervals;

        }
    }
    public class Day4
    {
        public int Puzzle2(string[] cards)
        {
            int[] total = new int[cards.Length];
            Array.Fill(total, 1);

            for (int i = 0; i < cards.Length; i++)
            {
                string[] numbers = cards[i].Split(": ")[1].Split(" | ");
                string[] winning = numbers[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                string[] owned = numbers[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                int found = 0;
                foreach (string num in owned)
                {
                    if (winning.Contains(num))
                    {
                        found++;
                    }
                }

                for (int j = i + 1; j <= i + found; j++)
                {
                    total[j] += total[i];
                }
            }
            return total.Sum();
        }
        public int Puzzle1(string[] cards)
        {
            int total = 0;
            foreach (string card in cards)
            {
                string[] numbers = card.Split(": ")[1].Split(" | ");
                string[] winning = numbers[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                string[] owned = numbers[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                int found = 0;
                foreach (string num in owned)
                {
                    if (winning.Contains(num))
                    {
                        found++;
                    }
                }
                if (found != 0)
                {
                    total += (int)Math.Pow(2, found - 1);
                }
            }
            return total;
        }
    }
    public class Day3
    {
        public int Puzzle2(string[] schematic)
        {
            int rowCount = schematic.Length;
            int total = 0;
            int columnCount = schematic[0].Length;

            for (int row = 0; row < rowCount; row++)
            {
                string line = schematic[row];
                for (int i = 0; i < columnCount; i++)
                {
                    if (line[i] == '*')
                    {
                        List<int> nums = new();
                        Target t = new(row, i);
                        bool movedLeft = false;
                        bool movedUp = false;
                        bool skip = false;
                        bool skipAll = false;
                        if (t.column > 0)
                        {
                            // move left 1
                            t.column--;
                            movedLeft = true;
                            if (IsNumber(schematic[t.row][t.column]))
                            {
                                string digits = "";
                                digits += schematic[t.row][t.column];
                                int tLeft = t.column - 1;
                                while (tLeft > -1 && IsNumber(schematic[t.row][tLeft]))
                                {
                                    digits = schematic[t.row][tLeft] + digits;
                                    tLeft--;
                                }
                                nums.Add(int.Parse(digits));
                            }
                        }
                        if (t.row > 0)
                        {
                            // move up 1
                            t.row--;
                            movedUp = true;
                            if (IsNumber(schematic[t.row][t.column]))
                            {
                                string digits = "";
                                digits += schematic[t.row][t.column];
                                int tLeft = t.column - 1;
                                int tRight = t.column + 1;
                                while (tLeft > -1 && IsNumber(schematic[t.row][tLeft]))
                                {
                                    digits = schematic[t.row][tLeft] + digits;
                                    tLeft--;
                                }
                                while (tRight < columnCount && IsNumber(schematic[t.row][tRight]))
                                {
                                    digits += schematic[t.row][tRight];
                                    tRight++;
                                }
                                if ((tRight - (t.column + 1)) == 1)
                                {
                                    // move right 1
                                    t.column++;
                                    skip = true;
                                }
                                else if ((tRight - (t.column + 1)) >= 2)
                                {
                                    t.column += 2;
                                    skipAll = true;
                                }
                                nums.Add(int.Parse(digits));
                            }
                        }
                        if (!skipAll)
                        {
                            // move right 1
                            t.column++;
                            // check symbol
                            if (IsNumber(schematic[t.row][t.column]))
                            {
                                string digits = "";
                                digits += schematic[t.row][t.column];
                                int tRight = t.column + 1;
                                while (tRight < columnCount && IsNumber(schematic[t.row][tRight]))
                                {
                                    digits += schematic[t.row][tRight];
                                    tRight++;
                                }
                                if ((tRight - (t.column + 1)) >= 1)
                                {
                                    // move right 1
                                    t.column++;
                                    skip = true;
                                }
                                nums.Add(int.Parse(digits));
                            }
                        }
                        if (t.column < columnCount - 1 && movedLeft && !skip && !skipAll)
                        {
                            // move right 1
                            t.column++;
                            // check symbol
                            if (IsNumber(schematic[t.row][t.column]))
                            {
                                string digits = "";
                                digits += schematic[t.row][t.column];
                                int tRight = t.column + 1;
                                while (tRight < columnCount && IsNumber(schematic[t.row][tRight]))
                                {
                                    digits += schematic[t.row][tRight];
                                    tRight++;
                                }
                                nums.Add(int.Parse(digits));
                            }
                        }
                        if (t.row < rowCount - 1 && movedUp)
                        {
                            // move down 1
                            t.row++;
                            // check symbol
                            if (IsNumber(schematic[t.row][t.column]))
                            {
                                string digits = "";
                                digits += schematic[t.row][t.column];
                                int tRight = t.column + 1;
                                while (tRight < columnCount && IsNumber(schematic[t.row][tRight]))
                                {
                                    digits += schematic[t.row][tRight];
                                    tRight++;
                                }
                                nums.Add(int.Parse(digits));
                            }
                        }
                        skip = false;
                        skipAll = false;
                        if (t.row < rowCount - 1)
                        {
                            // move down 1
                            t.row++;
                            // check symbol
                            if (IsNumber(schematic[t.row][t.column]))
                            {
                                string digits = "";
                                digits += schematic[t.row][t.column];
                                int tLeft = t.column - 1;
                                int tRight = t.column + 1;
                                while (tRight < columnCount && IsNumber(schematic[t.row][tRight]))
                                {
                                    digits += schematic[t.row][tRight];
                                    tRight++;
                                }
                                while (tLeft > -1 && IsNumber(schematic[t.row][tLeft]))
                                {
                                    digits = schematic[t.row][tLeft] + digits;
                                    tLeft--;
                                }
                                if ((tLeft - (t.column - 1)) == -1)
                                {
                                    // move left 1
                                    t.column--;
                                    skip = true;
                                }
                                else if ((tLeft - (t.column - 1)) <= -2)
                                {
                                    t.column -= 2;
                                    skipAll = true;
                                }
                                nums.Add(int.Parse(digits));
                            }
                        }
                        if (!skipAll)
                        {
                            // move left 1
                            t.column--;
                            // check symbol
                            if (IsNumber(schematic[t.row][t.column]))
                            {
                                string digits = "";
                                digits += schematic[t.row][t.column];
                                int tLeft = t.column - 1;
                                while (tLeft > -1 && IsNumber(schematic[t.row][tLeft]))
                                {
                                    digits = schematic[t.row][tLeft] + digits;
                                    tLeft--;
                                }
                                if ((tLeft - (t.column - 1)) <= -1)
                                {
                                    // move left 1
                                    t.column--;
                                    skip = true;
                                }
                                nums.Add(int.Parse(digits));
                            }
                        }
                        if (t.row > 0 && t.column > 0 && !skip && !skipAll)
                        {
                            // move left 1
                            t.column--;
                            // check symbol
                            if (IsNumber(schematic[t.row][t.column]))
                            {
                                string digits = "";
                                digits += schematic[t.row][t.column];
                                int tLeft = t.column - 1;
                                while (tLeft > -1 && IsNumber(schematic[t.row][tLeft]))
                                {
                                    digits = schematic[t.row][tLeft] + digits;
                                    tLeft--;
                                }
                                nums.Add(int.Parse(digits));
                            }
                        }

                        if (nums.Count == 2)
                        {
                            System.Console.WriteLine($"Adding ratio to total: {nums[0]} + {nums[1]}");
                            total += nums[0] * nums[1];
                        }
                        else
                        {
                            System.Console.WriteLine($"Not adding to total: {string.Join(", ", nums)}");
                        }

                    }
                }
            }

            return total;
        }
        private readonly char[] nonSymbols = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.' };
        private readonly char[] numbers = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        private class Target
        {
            public int column;
            public int row;
            public Target(int r, int c)
            {
                row = r;
                column = c;
            }
        }
        public int Puzzle1(string[] schematic)
        {
            int rowCount = schematic.Length;
            int columnCount = schematic[0].Length;
            List<int> partNumbers = new();

            for (int row = 0; row < schematic.Length; row++)
            {
                string line = schematic[row];
                for (int i = 0; i < columnCount; i++)
                {
                    if (int.TryParse(line[i].ToString(), out int n))
                    {
                        string digits = line[i].ToString();
                        for (int j = i + 1; j < rowCount; j++)
                        {
                            if (int.TryParse(line[j].ToString(), out _))
                            {
                                digits += line[j];
                            }
                            else
                            {
                                break;
                            }
                        }

                        Target t = new(row, i);
                        bool movedLeft = false;
                        bool movedUp = false;
                        if (t.column > 0)
                        {
                            // move left 1
                            t.column--;
                            movedLeft = true;
                            if (IsSymbol(schematic[t.row][t.column]))
                            {
                                partNumbers.Add(int.Parse(digits));
                                i += digits.Length;
                                continue;
                            }
                        }

                        if (t.row > 0)
                        {
                            // move up 1
                            t.row--;
                            movedUp = true;
                            if (IsSymbol(schematic[t.row][t.column]))
                            {
                                partNumbers.Add(int.Parse(digits));
                                i += digits.Length;
                                continue;
                            }
                        }

                        for (int j = 0; j < digits.Length; j++)
                        {
                            // for digit length
                            // move right 1
                            t.column++;
                            // check symbol
                            if (IsSymbol(schematic[t.row][t.column]))
                            {
                                partNumbers.Add(int.Parse(digits));
                                continue;
                            }
                        }

                        if (t.column < columnCount - 1 && movedLeft)
                        {
                            // move right 1
                            t.column++;
                            // check symbol
                            if (IsSymbol(schematic[t.row][t.column]))
                            {
                                partNumbers.Add(int.Parse(digits));
                                i += digits.Length;
                                continue;
                            }
                        }

                        if (t.row < rowCount - 1 && movedUp)
                        {
                            // move down 1
                            t.row++;
                            // check symbol
                            if (IsSymbol(schematic[t.row][t.column]))
                            {
                                partNumbers.Add(int.Parse(digits));
                                i += digits.Length;
                                continue;
                            }
                        }

                        if (t.row < rowCount - 1)
                        {
                            // move down 1
                            t.row++;
                            // check symbol
                            if (IsSymbol(schematic[t.row][t.column]))
                            {
                                partNumbers.Add(int.Parse(digits));
                                i += digits.Length;
                                continue;
                            }
                        }
                        for (int j = 0; j < digits.Length; j++)
                        {
                            // move left 1
                            t.column--;
                            // check symbol
                            if (IsSymbol(schematic[t.row][t.column]))
                            {
                                partNumbers.Add(int.Parse(digits));
                                continue;
                            }
                        }

                        if (t.row > 0 && t.column > 0)
                        {
                            // move left 1
                            t.column--;
                            // check symbol
                            if (IsSymbol(schematic[t.row][t.column]))
                            {
                                partNumbers.Add(int.Parse(digits));
                                i += digits.Length;
                                continue;
                            }
                        }

                        i += digits.Length;
                    }
                }
            }
            return partNumbers.Sum();

        }
        public bool IsSymbol(char c)
        {
            return !nonSymbols.Contains(c);
        }
        public bool IsNumber(char c)
        {
            return numbers.Contains(c);
        }
    }
    public class Day2
    {
        public int Puzzle2(string[] games)
        {
            int total = 0;
            foreach (string game in games)
            {
                int redMin = 0;
                int greenMin = 0;
                int blueMin = 0;
                string[] gameDetails = game.Split(": ");
                int gameId = int.Parse(gameDetails[0][5..]);
                string[] reveals = gameDetails[1].Split("; ");
                foreach (string reveal in reveals)
                {
                    string[] cubes = reveal.Split(", ");
                    foreach (string cube in cubes)
                    {
                        int count = int.Parse(cube.Split(" ")[0]);
                        switch (cube[^1])
                        {
                            case 'd':
                                if (count > redMin)
                                {
                                    redMin = count;
                                }
                                break;

                            case 'e':
                                if (count > blueMin)
                                {
                                    blueMin = count;
                                }
                                break;
                            case 'n':
                                if (count > greenMin)
                                {
                                    greenMin = count;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                total += redMin * greenMin * blueMin;
            }
            return total;
        }

        public int Puzzle1(string[] games)
        {
            int redMax = 12;
            int greenMax = 13;
            int blueMax = 14;
            int total = 0;
            foreach (string game in games)
            {
                string[] gameDetails = game.Split(": ");
                int gameId = int.Parse(gameDetails[0][5..]);
                string[] reveals = gameDetails[1].Split("; ");
                bool possible = true;
                foreach (string reveal in reveals)
                {
                    string[] cubes = reveal.Split(", ");
                    foreach (string cube in cubes)
                    {
                        int count = int.Parse(cube.Split(" ")[0]);
                        int max = 0;
                        switch (cube[^1])
                        {
                            case 'd':
                                max = redMax;
                                break;

                            case 'e':
                                max = blueMax;
                                break;
                            case 'n':
                                max = greenMax;
                                break;
                            default:
                                break;
                        }
                        if (count > max)
                        {
                            possible = false;
                            break;
                        }
                    }
                }
                if (possible)
                {
                    System.Console.WriteLine($"game possible {gameId}");
                    total += gameId;
                }
            }
            return total;
        }
    }
}

// public class DayX
// {
//     public long Puzzle2(string[] args)
//     {
//         return 0;
//     }
//     public long Puzzle1(string[] args)
//     {
//         return 0;
//     }
// }