using FluentAssertions;
using NUnit.Framework;

namespace refactor_me.Model.Implementation.Tests
{
    [TestFixture]
    public class TestProducts
    {
        [Test]
        public void GetAll()
        {
            var products = new Products();

            var result = products.GetAll();

            result.Should().NotBeNull();
        }
    }
}
