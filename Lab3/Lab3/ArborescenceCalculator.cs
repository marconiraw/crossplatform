using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab3
{
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
}
