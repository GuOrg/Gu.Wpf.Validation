namespace Gu.Wpf.Validation.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Markup;
    using NUnit.Framework;

    public class NamespacesTests
    {
        private Assembly _assembly;
        private const string Uri = @"http://gu.se/Validation";
        private readonly string[] _skip = { "Annotations", "Properties", "XamlGeneratedNamespace" };
        [SetUp]
        public void SetUp()
        {
            _assembly = typeof(Validation.Input).Assembly;
        }

        [Test]
        public void XmlnsDefinitions()
        {
            var strings = _assembly.GetTypes()
                .Where(x=>x.IsPublic)
                .Select(x => x.Namespace)
                .Distinct()
                .Where(x => !_skip.Any(x.EndsWith))
                .OrderBy(x => x)
                .ToArray();
            var attributes = _assembly.CustomAttributes.Where(x => x.AttributeType == typeof(XmlnsDefinitionAttribute))
                .ToArray();

            var actuals = attributes.Select(a => a.ConstructorArguments[1].Value)
                                    .OrderBy(x => x)
                                    .ToArray();
            Console.WriteLine("Expected");
            foreach (var s in strings)
            {
                Console.WriteLine(@"[assembly: XmlnsDefinition(""{0}"", ""{1}"")]", Uri, s);
            }
            CollectionAssert.AreEqual(strings, actuals);
            foreach (var attribute in attributes)
            {
                Assert.AreEqual(Uri, attribute.ConstructorArguments[0].Value);
            }
        }

        [Test]
        public void XmlnsPrefix()
        {
            var prefixAttribute = _assembly.CustomAttributes.Single(x => x.AttributeType == typeof(XmlnsPrefixAttribute));
            Assert.AreEqual(Uri, prefixAttribute.ConstructorArguments[0].Value);
        }
    }
}