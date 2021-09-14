namespace Worms
{
    public class Food
    {
        private int _ttl;
        public Point Point { init; get; }
        public int FeedingPoints { init; get; }

        public Food(int ttl, Point point, int feedingPoints)
        {
            _ttl = ttl;
            Point = point;
            FeedingPoints = feedingPoints;
        }

        public void Age()
        {
            _ttl--;
        }

        public bool IsExpired()
        {
            return _ttl <= 0;
        }
    }

    public class FoodFactory
    {
        private const int Ttl = 10;
        private const int FeedingPoints = 10;

        public Food GetFood(Point point)
        {
            return new Food(Ttl, point, FeedingPoints);
        }
    }
}