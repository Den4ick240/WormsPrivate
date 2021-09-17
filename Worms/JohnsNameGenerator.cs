using Worms.abstractions;

namespace Worms
{
    public class JohnsNameGenerator : INameGenerator
    {
        private int _i;
        private readonly string _baseName;

        public JohnsNameGenerator(string baseName = "John")
        {
            _baseName = baseName;
        }

        public string getNextName()
        {
            return _baseName + ++_i;
        }
    }
}