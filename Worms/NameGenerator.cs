namespace Worms
{
    public interface INameGenerator
    {
        public string getNextName();
    }


    public class JohnsNameGenerator : INameGenerator
    {
        private int i = 0;
        private string _baseName;

        public JohnsNameGenerator(string baseName = "John")
        {
            _baseName = baseName;
        }


        public string getNextName()
        {
            return _baseName + i;
        }
    }
}