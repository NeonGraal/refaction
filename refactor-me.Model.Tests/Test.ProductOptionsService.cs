using FluentAssertions;
using Moq;
using NUnit.Framework;
using refactor_me.Model.Contract;
using System;

namespace refactor_me.Model.Implementation.Tests
{
    [TestFixture]
    public class TestOptionOptionsService
    {
        private Mock<IProductOptionsRepository> _repo;

        private IProductOptionsService _service;

        [SetUp]
        public void SetUp()
        {
            _repo = new Mock<IProductOptionsRepository>(MockBehavior.Strict);
            _service = new ProductOptionsService(_repo.Object);
        }

        private readonly Guid _prodId = new Guid("C488B49D-C5C0-4C88-920A-6F1BDBF4E555");

        [Test]
        public void GetAllAlwaysNotNull()
        {
            _repo.Setup(r => r.All(_prodId)).Returns(new Guid[] { });

            var result = _service.GetAll(_prodId);

            result.Should().NotBeNull();
        }

        private readonly Guid _id1 = new Guid("AC81FB42-1835-4C3E-9793-BA9F3C1F5623");
        private readonly Guid _id2 = new Guid("41ED9A13-D656-4086-9C8A-DB7A3D35DCEE");

        [Test]
        public void GetAllReturnsNumFound()
        {
            _repo.Setup(r => r.All(_prodId)).Returns(new Guid[] { _id1, _id2 });
            _repo.Setup(r => r.Get(It.IsAny<Guid>())).Returns<IProductOption>(null);

            var result = _service.GetAll(_prodId);

            result.Items.Should().HaveCount(2);
        }

        [Test]
        public void GetAllReturnsProductOptions()
        {
            var opt1 = new Mock<IProductOption>(MockBehavior.Strict);
            var opt2 = new Mock<IProductOption>(MockBehavior.Strict);

            _repo.Setup(r => r.All(_prodId)).Returns(new Guid[] { _id1, _id2 });
            _repo.Setup(r => r.Get(_id1)).Returns(opt1.Object);
            _repo.Setup(r => r.Get(_id2)).Returns(opt2.Object);

            var result = _service.GetAll(_prodId);

            result.Items.Should().BeEquivalentTo(opt1.Object, opt2.Object);
        }


        [Test]
        public void GetUnknownReturnsNull()
        {
            _repo.Setup(r => r.Get(_id1)).Returns<IProductOption>(null);

            var result = _service.Get(_id1);

            result.Should().BeNull();
        }

        [Test]
        public void GetKnownReturnsProductOption()
        {
            var opt = new Mock<IProductOption>(MockBehavior.Strict);

            _repo.Setup(r => r.Get(_id1)).Returns(opt.Object);

            var result = _service.Get(_id1);

            result.Should().Be(opt.Object);
        }

        [Test]
        public void CreateSavesNewProductOption()
        {
            var name = "Test Name";
            var description = "Test Description";

            var opt1 = new Mock<IProductOption>(MockBehavior.Strict);

            opt1.SetupGet(p => p.Id).Returns(_id1);
            opt1.SetupGet(p => p.ProductId).Returns(_id2); //  Yes not _prodId
            opt1.SetupGet(p => p.Name).Returns(name);
            opt1.SetupGet(p => p.Description).Returns(description);

            IProductOption opt2 = null;

            _repo.Setup(r => r.Get(_id1)).Returns<IProductOption>(null);
            _repo.Setup(r => r.Save(It.IsAny<IProductOption>(), true)).Verifiable();
            _repo.Setup(r => r.Save(It.IsAny<IProductOption>(), false)).Callback<IProductOption, bool>((o, _) => opt2 = o);

            _service.Create(_prodId, opt1.Object);

            _repo.Verify(r => r.Get(_id1), Times.Once);
            _repo.Verify(r => r.Save(It.IsAny<IProductOption>(), true), Times.Never);
            _repo.Verify(r => r.Save(It.IsAny<IProductOption>(), false), Times.Once);

            opt2.Id.Should().Be(_id1);
            opt2.ProductId.Should().Be(_prodId);
            opt2.Name.Should().Be(name);
            opt2.Description.Should().Be(description);
        }

        [Test]
        public void CreateDoesntSaveExistingProductOption()
        {
            var opt = new Mock<IProductOption>(MockBehavior.Strict);

            opt.SetupGet(p => p.Id).Returns(_id1);

            _repo.Setup(r => r.Get(_id1)).Returns(opt.Object);
            _repo.Setup(r => r.Save(It.IsAny<IProductOption>(), It.IsAny<bool>())).Verifiable();

            _service.Create(_prodId, opt.Object);

            _repo.Verify(r => r.Get(_id1), Times.Once);
            _repo.Verify(r => r.Save(It.IsAny<IProductOption>(), It.IsAny<bool>()), Times.Never);
        }

        [Test]
        public void UpdateDoesntSaveNewProductOption()
        {
            var opt = new Mock<IProductOption>(MockBehavior.Strict);

            _repo.Setup(r => r.Get(_id1)).Returns<IProductOption>(null);
            _repo.Setup(r => r.Save(It.IsAny<IProductOption>(), It.IsAny<bool>())).Verifiable();

            _service.Update(_id1, opt.Object);

            _repo.Verify(r => r.Get(_id1), Times.Once);
            _repo.Verify(r => r.Save(It.IsAny<IProductOption>(), It.IsAny<bool>()), Times.Never);
        }

        [Test]
        public void UpdateSavesExistingProductOption()
        {
            var name = "Test Name";
            var description = "Test Description";

            var opt1 = new Mock<IProductOption>(MockBehavior.Strict);
            var opt2 = new Mock<IProductOption>(MockBehavior.Strict);

            opt2.SetupGet(p => p.Id).Returns(_id2);
            opt2.SetupGet(p => p.ProductId).Returns(_prodId);
            opt2.SetupGet(p => p.Name).Returns(name);
            opt2.SetupGet(p => p.Description).Returns(description);

            IProductOption prod3 = null;

            _repo.Setup(r => r.Get(_id1)).Returns(opt1.Object);
            _repo.Setup(r => r.Save(It.IsAny<IProductOption>(), false)).Verifiable();
            _repo.Setup(r => r.Save(It.IsAny<IProductOption>(), true)).Callback<IProductOption, bool>((p, _) => prod3 = p);

            _service.Update(_id1, opt2.Object);

            _repo.Verify(r => r.Get(_id1), Times.Once);
            _repo.Verify(r => r.Save(It.IsAny<IProductOption>(), false), Times.Never);
            _repo.Verify(r => r.Save(It.IsAny<IProductOption>(), true), Times.Once);

            prod3.Id.Should().Be(_id1);
            prod3.ProductId.Should().Be(_prodId);
            prod3.Name.Should().Be(name);
            prod3.Description.Should().Be(description);
        }

        [Test]
        public void DeleteDoesntRemoveNewProductOption()
        {
            _repo.Setup(r => r.Get(_id1)).Returns<IProductOption>(null);

            _service.Delete(_id1);

            _repo.Verify(r => r.Get(_id1), Times.Once);
            _repo.Verify(r => r.Remove(_id1), Times.Never);
        }

        [Test]
        public void DeleteRemovesExistingProductOption()
        {
            var opt = new Mock<IProductOption>(MockBehavior.Strict);

            _repo.Setup(r => r.Get(_id1)).Returns(opt.Object);
            _repo.Setup(r => r.Remove(_id1)).Verifiable();

            _service.Delete(_id1);

            _repo.Verify(r => r.Get(_id1), Times.Once);
            _repo.Verify(r => r.Remove(_id1), Times.Once);
        }
    }
}
