namespace Worms.abstractions
{
    public class Food
    {
        public int Ttl { get; private set; }
        public Point Point { get; }
        public int FeedingPoints { get; }

        public Food(int ttl, Point point, int feedingPoints)
        {
            Ttl = ttl;
            Point = point;
            FeedingPoints = feedingPoints;
        }

        public void Age()
        {
            Ttl--;
        }

        public bool IsExpired()
        {
            return Ttl <= 0;
        }
    }
}