using FluentAssertions;
using Moq;
using NUnit.Framework;
using refactor_me.Model.Contract;
using System;
using System.Linq;

namespace refactor_me.Model.Implementation.Tests
{
    [TestFixture]
    public class TestProductsService
    {
        private Mock<IProductsRepository> _repo;

        [SetUp]
        public void SetUp()
        {
            _repo = new Mock<IProductsRepository>(MockBehavior.Strict);
        }

        [Test]
        public void GetAllAlwaysNotNull()
        {
            var service = new ProductsService(_repo.Object);
            _repo.Setup(r => r.All()).Returns(new Guid[] { });

            var result = service.GetAll();

            result.Should().NotBeNull();
        }

        private readonly Guid _id1 = new Guid("AC81FB42-1835-4C3E-9793-BA9F3C1F5623");
        private readonly Guid _id2 = new Guid("41ED9A13-D656-4086-9C8A-DB7A3D35DCEE");

        [Test]
        public void GetAllReturnsNumFound()
        {
            _repo.Setup(r => r.All()).Returns(new Guid[] { _id1, _id2 });
            _repo.Setup(r => r.Get(It.IsAny<Guid>())).Returns<IProduct>(null);

            var service = new ProductsService(_repo.Object);

            var result = service.GetAll();

            result.Items.Should().HaveCount(2);
        }

        [Test]
        public void GetAllReturnsProducts()
        {
            var prod1 = new Mock<IProduct>(MockBehavior.Strict);
            var prod2 = new Mock<IProduct>(MockBehavior.Strict);

            _repo.Setup(r => r.All()).Returns(new Guid[] { _id1, _id2 });
            _repo.Setup(r => r.Get(_id1)).Returns(prod1.Object);
            _repo.Setup(r => r.Get(_id2)).Returns(prod2.Object);

            var service = new ProductsService(_repo.Object);

            var result = service.GetAll();

            result.Items.Should().BeEquivalentTo(prod1.Object, prod2.Object);
        }
    }
}
