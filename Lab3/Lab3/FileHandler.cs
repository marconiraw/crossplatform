using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Lab3
{
    public class FileHandler
    {
        // Метод для читання та валідації вхідних даних
        public (int N, List<Edge> Edges, int Root) ReadInput(string inputFilePath)
        {
            if (!File.Exists(inputFilePath))
            {
                throw new FileNotFoundException("Input file does not exist.");
            }

            var lines = File.ReadAllLines(inputFilePath);
            if (lines.Length < 1)
            {
                throw new InvalidDataException("Input file must contain at least 1 line.");
            }

            // Парсинг n та m
            var firstLine = lines[0].Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (firstLine.Length < 2)
            {
                throw new InvalidDataException("The first line must contain at least 2 values: N and M.");
            }

            bool parseN = int.TryParse(firstLine[0], out int n);
            bool parseM = int.TryParse(firstLine[1], out int m);

            if (!parseN || !parseM || n < 1 || m < 0 || m > 22)
            {
                throw new InvalidDataException("Invalid values for N or M. Ensure that N >= 1 and 0 <= M <= 22.");
            }

            // Парсинг каналів
            var edges = new List<Edge>();
            for (int i = 1; i <= m && i < lines.Length; i++)
            {
                var parts = lines[i].Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 3)
                {
                    throw new InvalidDataException($"Line {i + 1} must contain exactly 3 values: U, V, C.");
                }

                bool parseU = int.TryParse(parts[0], out int u);
                bool parseV = int.TryParse(parts[1], out int v);
                bool parseC = int.TryParse(parts[2], out int c);

                if (!parseU || !parseV || !parseC || u < 1 || u > n || v < 1 || v > n || c < 0)
                {
                    throw new InvalidDataException($"Invalid values in line {i + 1}. Ensure that 1 <= U, V <= N and C >= 0.");
                }

                if (u == v)
                {
                    // Ігнорування самозв'язків
                    continue;
                }

                edges.Add(new Edge(u, v, c, i)); // Нумерація каналів починається з 1
            }

            return (n, edges, 1); // Припустимо, що корінь завжди 1. Якщо корінь може змінюватися, додайте його в вхідні дані.
        }

        // Метод для запису результату у вихідний файл
        public void WriteOutput(string outputFilePath, ArborescenceResult? result)
        {
            if (result == null)
            {
                File.WriteAllText(outputFilePath, "-1");
            }
            else
            {
                File.WriteAllText(outputFilePath, $"{result.TotalCost} {result.ChannelIndices.Count}");
                File.AppendAllText(outputFilePath, Environment.NewLine);
                File.AppendAllText(outputFilePath, string.Join(" ", result.ChannelIndices));
            }
        }
    }

    // Клас представляє канал між містами
    public class Edge
    {
        public int From { get; }
        public int To { get; }
        public int Cost { get; }
        public int Index { get; }

        public Edge(int from, int to, int cost, int index)
        {
            From = from;
            To = to;
            Cost = cost;
            Index = index;
        }
    }

    // Клас представляє результат арбореції
    public class ArborescenceResult
    {
        public int TotalCost { get; set; }
        public List<int> ChannelIndices { get; set; }

        public ArborescenceResult()
        {
            ChannelIndices = new List<int>();
        }
    }
}
