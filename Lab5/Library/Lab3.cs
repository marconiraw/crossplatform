using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Library
{
    public class FileHandler3
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

    public class ArborescenceCalculator
    {
        // Реалізація алгоритму Чу-Лію для мінімальної арбореції
        public ArborescenceResult? GetMinimumArborescence(int n, List<Edge> edges, int root)
        {
            var inEdges = new List<Edge>[n + 1];
            for (int i = 0; i <= n; i++)
                inEdges[i] = new List<Edge>();

            foreach (var edge in edges)
            {
                if (edge.To != root)
                {
                    inEdges[edge.To].Add(edge);
                }
            }

            // Об'єднуємо всі можливі входи та шукаємо мінімальні
            var parent = new int[n + 1];
            var minEdge = new Edge[n + 1];

            // Вибір мінімального входу для кожного вузла
            for (int i = 1; i <= n; i++)
            {
                if (i == root)
                    continue;

                if (inEdges[i].Count == 0)
                {
                    // Якщо немає входів для вузла, арбореція неможлива
                    return null;
                }

                // Вибираємо мінімальну вартость
                var min = inEdges[i].OrderBy(e => e.Cost).First();
                minEdge[i] = min;
                parent[i] = min.From;
            }

            // Перевірка на цикли
            var visited = new int[n + 1];
            int cycle = 0;
            int[] label = new int[n + 1];
            int[] predecessor = new int[n + 1];
            int[] id = new int[n + 1];
            for (int i = 1; i <= n; i++)
            {
                if (i == root)
                    continue;

                var u = i;
                while (visited[u] != i && id[u] == 0 && u != root)
                {
                    visited[u] = i;
                    u = parent[u];
                }

                if (u != root && id[u] == 0)
                {
                    cycle++;
                    while (label[u] != cycle)
                    {
                        label[u] = cycle;
                        u = parent[u];
                    }
                }
            }

            if (cycle == 0)
            {
                // Немає циклів, арбореція знайдена
                var result = new ArborescenceResult();
                int totalCost = 0;
                for (int i = 1; i <= n; i++)
                {
                    if (i == root)
                        continue;

                    totalCost += minEdge[i].Cost;
                    result.ChannelIndices.Add(minEdge[i].Index);
                }
                result.TotalCost = totalCost;
                return result;
            }
            else
            {
                return null;
            }
        }
    }

    public static class Lab3
    {
        public static void Run(string inputFilePath, string outputFilePath)
        {
            Console.WriteLine($"Input File Path: {inputFilePath}");
            Console.WriteLine($"Output File Path: {outputFilePath}");

            var fileHandler = new FileHandler3();
            var calculator = new ArborescenceCalculator();


            try
            {
                // Читання та валідація вхідних даних
                var (n, edges, root) = fileHandler.ReadInput(inputFilePath);

                // Обчислення мінімальної арбореції
                var result = calculator.GetMinimumArborescence(n, edges, root);

                // Запис результату у вихідний файл
                fileHandler.WriteOutput(outputFilePath, result);

                if (result != null)
                {
                    Console.WriteLine("Lab 3 executed successfully.");
                }
                else
                {
                    Console.WriteLine("Lab 3 executed with no valid arborescence found.");
                }
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine($"Invalid data: {ex.Message}");
                fileHandler.WriteOutput(outputFilePath, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                fileHandler.WriteOutput(outputFilePath, null);
            }
        }
    }
}