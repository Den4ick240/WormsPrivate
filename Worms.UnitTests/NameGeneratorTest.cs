using System.Collections.Generic;
using NUnit.Framework;
using Worms.abstractions;

namespace Worms.UnitTests
{
    public class NameGeneratorTest
    {
        private readonly List<INameGenerator> _nameGenerators = new()
        {
            new JohnsNameGenerator()
        };

        private const int RequiredNumberOfUniqueNames = 100;

        [Test]
        public void uniqueNamesTest()
        {
            foreach (var nameGenerator in _nameGenerators)
            {
                var dict = new HashSet<string>();
                for (int i = 0; i < RequiredNumberOfUniqueNames; i++)
                {
                    var name = nameGenerator.getNextName();
                    Assert.IsFalse(dict.Contains(name));
                    dict.Add(name);
                }
            }
        }
    }
}