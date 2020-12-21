using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ExpertDecisionProblem
{
    class Program
    {
        static void Main()
        {
            List<List<int>> marks = new List<List<int>>();
            Console.WriteLine("Завантажити значення з json чи генерація даних випадковим чином?");
            Console.WriteLine("Rand r");
            Console.WriteLine("Json j");
            var str = Console.ReadLine();
            if (str == "r")
            {
                int n;
                Console.Write("Number of basics: ");
                n = Int16.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                Console.Write("Number of alternatives: ");
                int m = Int16.Parse(Console.ReadLine());
                List<List<int>> basics = new List<List<int>>();
                System.Globalization.NumberStyles style = System.Globalization.NumberStyles.AllowDecimalPoint |
                System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowExponent |
                System.Globalization.NumberStyles.AllowLeadingSign;
                for (int i = 0; i < n; i++)
                {
                    Console.WriteLine("{0} basic:", i + 1);
                    basics.Add(Console.ReadLine()?.Split(' ').Select(s => int.Parse(s, style)).ToList());
                }
                Console.Write("Write number of experts: ");
                int ex = Int16.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                Random rnd = new Random();
                for (int i = 0; i < ex; i++)
                {
                    marks.Add(basics[rnd.Next(0, basics.Count)]);
                }
            }
            else
            {
                if (str == "j")
                {
                    marks = JsonSerializer.Deserialize<List<List<int>>>(File.ReadAllText("marks.json"));
                }
                else
                {
                    Console.WriteLine("Недопустиме значення!");
                    return;
                }
            }
            if (MarksValidCheck(marks))
            {
                PrintMarks(marks);
                PrintSpace();
                List<int> leaders = GetLeaders(marks);
                List<int> absolute = GetAbsoluteList(leaders);
                Print(leaders.IndexOf(leaders.First()), CheckUnicLeader(absolute) && CheckAbsolute(leaders, absolute),
                "Absolute");
                Print(leaders.IndexOf(leaders.First()), CheckUnicLeader(absolute), "Releative");
                List<int> borda = GetBordaList(marks);
                Print(borda.IndexOf(borda.Max()), CheckUnicBorda(borda), "Borda");
                int kondra = GetKondorse(marks);
                Print(kondra, kondra != -1, "Kondorse");
                PrintSpace();
            }
            else
            {
                Console.WriteLine("Sorry, unreal marks");
            }
            
            var Votes = JsonSerializer.Deserialize<List<int>>(File.ReadAllText("votes.json"));
            var objectStateProbabilities = JsonSerializer.Deserialize<List<decimal>>(File.ReadAllText("objectStateProbabilities.json"));
            var failProbabilities = JsonSerializer.Deserialize<List<decimal>>(File.ReadAllText("failProbabilities.json"));
            if (VotesValidCheck(Votes, failProbabilities.Count) &&
            ObjectStateProbabilitiesValidCheck(objectStateProbabilities) &&
            FailProbabilitiesValidCheck(failProbabilities))
            {
                PrintVotes(Votes);
                PrintObjectStateProbabilities(objectStateProbabilities);
                PrintFailProbabilities(failProbabilities);
                int state = GetObjectState(Votes, objectStateProbabilities, failProbabilities);
                Print(state, true, "Object state");
            }
            else
            {
                Console.WriteLine("Sorry, unreal votes");
            }

            static bool VotesValidCheck(List<int> votes, int number)
            {
                foreach (int v in votes)
                {
                    if (v > number && v < 1)
                    {
                        return false;
                    }
                }
                return true;
            }
            static bool ObjectStateProbabilitiesValidCheck(List<decimal> objectStateProbabilities)
            {
                foreach (decimal d in objectStateProbabilities)
                {
                    if (d < 0)
                    {
                        return false;
                    }
                }
                decimal sum = 0;
                foreach (decimal d in objectStateProbabilities)
                {
                    sum = Decimal.Add(sum, d);
                }
                if (sum != 1)
                {
                    return false;
                }
                return true;
            }
            static bool FailProbabilitiesValidCheck(List<decimal> failProbabilities)
            {
                return failProbabilities.All(d => d >= 0 && d <= 1);
            }
            static bool MarksValidCheck(List<List<int>> marks)
            {
                for (int i = 0; i < marks.Count; i++)
                {
                    List<int> expertMarks = new List<int>();
                    for (int j = 0; j < marks[i].Count; j++)
                    {
                        expertMarks.Add(marks[i][j]);
                    }
                    expertMarks.Sort();
                    for (int j = 0; j < expertMarks.Count; j++)
                    {
                        if (expertMarks[j] != j)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        // Calculation
        static List<int> GetLeaders(List<List<int>> marks)
        {
             var leaders = new List<int>();
            for (int i = 0; i < marks.Count; i++)
            {
                leaders.Add(marks[i].IndexOf(marks[i].Max()));
            }
            return leaders;
        }
        static List<int> GetAbsoluteList(List<int> leaders)
        {
            return leaders.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp =>
            grp.Count()).ToList();
        }

        private static int GetRelative(List<int> leaders)
        {
            return leaders.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
        }
        static bool CheckUnicLeader(List<int> absolute)
        {
            if (absolute.Count > 1)
            {
                return (absolute[0] != absolute[1]);
            }
            else
            {
                return true;
            }
        }
        static bool CheckAbsolute(List<int> leaders, List<int> absolute)
        {
            if (absolute[0] >= leaders.Count / 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static List<int> GetBordaList(List<List<int>> marks)
        {
            List<int> borda = new List<int>();
            for (int j = 0; j < marks[0].Count; j++)
            {
                int sum = 0;
                for (int i = 0; i < marks.Count; i++)
                {
                    sum += marks[i][j];
                }
                borda.Add(sum);
            }
            return borda;
        }
        static bool CheckUnicBorda(List<int> borda)
        {
            borda.Sort();
            if (borda.Count > 1)
            {
                if (borda[borda.Count - 1] > borda[borda.Count - 2])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        static int GetKondorse(List<List<int>> marks)
        {
            List<List<int>> matrix = new List<List<int>>();
            for (int i = 0; i < marks[0].Count; i++)
            {
                matrix.Add(new List<int>());
                for (int j = 0; j < marks[0].Count; j++)
                {
                    if (i == j)
                    {
                        matrix[i].Add(-1);
                    }
                    else
                    {
                        matrix[i].Add(0);
                    }
                }
            }
            for (int j = 0; j < marks[0].Count; j++)
            {
                for (int k = 0; k < marks[0].Count; k++)
                {
                    if (k != j)
                    {
                        for (int i = 0; i < marks.Count; i++)
                        {
                            if (marks[i][j] > marks[i][k])
                            {
                                matrix[j][k]++;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < matrix.Count; i++)
            {
                bool fullRow = true;
                for (int j = 0; j < matrix.Count; j++)
                {
                    if (i != j)
                    {
                        if (matrix[i][j] >= matrix[j][i])
                        {
                        }
                        else
                        {
                            fullRow = false;
                            break;
                        }
                    }
                }
                if (fullRow)
                {
                    return i;
                }
            }
            return -1;
        }
        static int GetObjectState(List<int> votes, List<decimal> objectStateProbabilities, List<decimal> failProbabilities)
        {
            List<decimal> probabilities = new List<decimal>();
            for (int i = 1; i <= objectStateProbabilities.Count; i++)
            {
                decimal result = objectStateProbabilities[i - 1];
                List<bool> includes = new List<bool>();
                foreach (int v in votes)
                {
                    if (v == i)
                    {
                        includes.Add(true);
                    }
                    else
                    {
                        includes.Add(false);
                    }
                }
                for (int j = 0; j < votes.Count; j++)
                {
                    if (includes[j])
                    {
                        result = Decimal.Multiply(result, Decimal.Subtract(1, failProbabilities[j]));
                    }
                    else
                    {
                        result = Decimal.Multiply(result, failProbabilities[j]);
                    }
                }
                probabilities.Add(result);
            }
            return probabilities.IndexOf(probabilities.Max());
        }
        // Output
        static void PrintMarks(List<List<int>> marks)
        {
            Console.WriteLine("Marks :");
            for (int i = 0; i < marks.Count; i++)
            {
                Console.Write("Expert #{0} | ", i + 1);
                for (int j = 0; j < marks[i].Count; j++)
                {
                    Console.Write("{0} ", marks[i][j]);
                }
                Console.WriteLine("|");
            }
        }
        static void Print(int number, bool satisfy, string name)
        {
            if (satisfy)
            {
                Console.WriteLine("{0} : {1}", name, number + 1);
            }
            else
            {
                Console.WriteLine("{0} : {1}", name, "No way, srry");
            }
        }
        static void PrintSpace()
        {
            Console.WriteLine();
        }
        static void PrintVotes(List<int> votes)
        {
            Console.WriteLine("Votes : ");
            foreach (int v in votes)
            {
                Console.Write("{0} ", v);
            }
            Console.WriteLine();
        }
        static void PrintObjectStateProbabilities(List<decimal> objectStateProbabilities)
        {
            Console.WriteLine("Object State Probabilities :");
            foreach (decimal p in objectStateProbabilities)
            {
                Console.Write("{0} ", p);
            }
            Console.WriteLine();
        }
        static void PrintFailProbabilities(List<decimal> failProbabilities)
        {
            Console.WriteLine("Fail Probabilities :");
            foreach (decimal f in failProbabilities)
            {
                Console.Write("{0} ", f);
            }
            Console.WriteLine();
        }
    }
}
