using FluentAssertions;
using Moq;
using NUnit.Framework;
using refactor_me.Model.Contract;
using System;

namespace refactor_me.Model.Implementation.Tests
{
    [TestFixture]
    public class TestProductsService
    {
        private Mock<IProductsRepository> _repo;

        private IProductsService _service;

        [SetUp]
        public void SetUp()
        {
            _repo = new Mock<IProductsRepository>(MockBehavior.Strict);
            _service = new ProductsService(_repo.Object);
        }

        [Test]
        public void GetAllAlwaysNotNull()
        {
            _repo.Setup(r => r.All()).Returns(new Guid[] { });

            var result = _service.GetAll();

            result.Should().NotBeNull();
        }

        private readonly Guid _id1 = new Guid("AC81FB42-1835-4C3E-9793-BA9F3C1F5623");
        private readonly Guid _id2 = new Guid("41ED9A13-D656-4086-9C8A-DB7A3D35DCEE");

        [Test]
        public void GetAllReturnsNumFound()
        {
            _repo.Setup(r => r.All()).Returns(new Guid[] { _id1, _id2 });
            _repo.Setup(r => r.Get(It.IsAny<Guid>())).Returns<IProduct>(null);

            var result = _service.GetAll();

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

            var result = _service.GetAll();

            result.Items.Should().BeEquivalentTo(prod1.Object, prod2.Object);
        }

        [Test]
        public void FindByNameAlwaysNotNull()
        {
            var name = "test";

            _repo.Setup(r => r.ByName(name)).Returns(new Guid[] { });

            var result = _service.FindByName(name);

            result.Should().NotBeNull();
        }

        [Test]
        public void FindByNameReturnsNumFound()
        {
            var name = "test";

            _repo.Setup(r => r.ByName(name)).Returns(new Guid[] { _id1, _id2 });
            _repo.Setup(r => r.Get(It.IsAny<Guid>())).Returns<IProduct>(null);

            var result = _service.FindByName(name);

            result.Items.Should().HaveCount(2);
        }

        [Test]
        public void FindByNameReturnsProducts()
        {
            var name = "test";
            var prod1 = new Mock<IProduct>(MockBehavior.Strict);
            var prod2 = new Mock<IProduct>(MockBehavior.Strict);

            _repo.Setup(r => r.ByName(name)).Returns(new Guid[] { _id1, _id2 });
            _repo.Setup(r => r.Get(_id1)).Returns(prod1.Object);
            _repo.Setup(r => r.Get(_id2)).Returns(prod2.Object);

            var result = _service.FindByName(name);

            result.Items.Should().BeEquivalentTo(prod1.Object, prod2.Object);
        }

        [Test]
        public void GetUnknownReturnsNull()
        {
            _repo.Setup(r => r.Get(_id1)).Returns<IProduct>(null);

            var result = _service.Get(_id1);

            result.Should().BeNull();
        }

        [Test]
        public void GetKnownReturnsProduct()
        {
            var prod1 = new Mock<IProduct>(MockBehavior.Strict);
            _repo.Setup(r => r.Get(_id1)).Returns(prod1.Object);

            var result = _service.Get(_id1);

            result.Should().Be(prod1.Object);
        }

        [Test]
        public void CreateSavesNewProduct()
        {
            var prod = new Mock<IProduct>(MockBehavior.Strict);
            prod.SetupGet(p => p.Id).Returns(_id1);

            _repo.Setup(r => r.Get(_id1)).Returns<IProduct>(null);
            _repo.Setup(r => r.Save(prod.Object, false)).Verifiable();

            _service.Create(prod.Object);

            _repo.Verify(r => r.Save(prod.Object, false), Times.Once);
        }

        [Test]
        public void CreateDoesntSaveExistingProduct()
        {
            var prod = new Mock<IProduct>(MockBehavior.Strict);
            prod.SetupGet(p => p.Id).Returns(_id1);

            _repo.Setup(r => r.Get(_id1)).Returns(prod.Object);
            _repo.Setup(r => r.Save(prod.Object, false)).Verifiable();

            _service.Create(prod.Object);

            _repo.Verify(r => r.Save(prod.Object, false), Times.Never);
        }
    }
}
