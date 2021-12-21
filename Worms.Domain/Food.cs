namespace Worms.Domain
{
    public class Food
    {
        public int ExpiresIn { get; set; }
        public Position Position { get; set; }

        public override string ToString()
        {
            return $"{nameof(ExpiresIn)}: {ExpiresIn}, {nameof(Position)}: {Position}";
        }
    }
}