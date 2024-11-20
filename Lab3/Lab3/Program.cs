using System;
using System.Collections.Generic;
using System.IO;

namespace lab3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var edges = new List<Edge>();
            int n = 0, m = 0;

            using (StreamReader sr = new StreamReader("input.txt"))
            {
                var firstLine = sr.ReadLine().Split();
                n = int.Parse(firstLine[0]);
                m = int.Parse(firstLine[1]);

                for (int i = 0; i < m; i++)
                {
                    var line = sr.ReadLine().Split();
                    int u = int.Parse(line[0]);
                    int v = int.Parse(line[1]);
                    int c = int.Parse(line[2]);

                    edges.Add(new Edge(u, v, c, i + 1));
                }
            }

            int minCost = int.MaxValue;
            List<int> minEdges = null;

            int totalSubsets = 1 << m;

            for (int mask = 1; mask < totalSubsets; mask++)
            {
                var subsetEdges = new List<Edge>();
                int cost = 0;

                for (int i = 0; i < m; i++)
                {
                    if ((mask & (1 << i)) != 0)
                    {
                        subsetEdges.Add(edges[i]);
                        cost += edges[i].Cost;
                    }
                }

                if (cost > minCost)
                    continue;

                if (IsAllReachable(n, subsetEdges))
                {
                    if (cost < minCost || (cost == minCost && subsetEdges.Count < minEdges.Count))
                    {
                        minCost = cost;
                        minEdges = new List<int>();
                        foreach (var edge in subsetEdges)
                        {
                            minEdges.Add(edge.Index);
                        }
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                if (minEdges != null)
                {
                    sw.WriteLine($"{minCost} {minEdges.Count}");
                    minEdges.Sort();
                    sw.WriteLine(string.Join(" ", minEdges));
                }
                else
                {
                    sw.WriteLine("0 0");
                }
            }
        }

        public static bool IsAllReachable(int n, List<Edge> edges)
        {
            var adj = new Dictionary<int, List<int>>();

            for (int i = 1; i <= n; i++)
                adj[i] = new List<int>();

            foreach (var edge in edges)
            {
                adj[edge.From].Add(edge.To);
            }

            var visited = new HashSet<int>();
            var stack = new Stack<int>();
            stack.Push(1);
            visited.Add(1);

            while (stack.Count > 0)
            {
                int node = stack.Pop();
                foreach (var neighbor in adj[node])
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        stack.Push(neighbor);
                    }
                }
            }

            return visited.Count == n;
        }
    }
}
