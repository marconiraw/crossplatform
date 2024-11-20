namespace lab3
{
    public class Edge
    {
        public int From { get; set; }
        public int To { get; set; }
        public int Cost { get; set; }
        public int Index { get; set; }

        public Edge(int from, int to, int cost, int index)
        {
            From = from;
            To = to;
            Cost = cost;
            Index = index;
        }
    }
}
