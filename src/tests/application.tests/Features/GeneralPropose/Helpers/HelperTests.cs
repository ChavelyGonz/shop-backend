using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.GeneralPropose.Helpers;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Repositories;

namespace Application.Tests.Features.GeneralPropose.Helpers
{
    public class HelperTests
    {
        private readonly Mock<IEntityReadRepository<DummyEntity>> _readRepositoryMock;
        private readonly Mock<IEntityRepository<DummyEntity>> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Helper<DummyEntity> _helper;

        public HelperTests()
        {
            _readRepositoryMock = new Mock<IEntityReadRepository<DummyEntity>>();
            _repositoryMock = new Mock<IEntityRepository<DummyEntity>>();
            _mapperMock = new Mock<IMapper>();
            _helper = new Helper<DummyEntity>(
                _readRepositoryMock.Object,
                _repositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task TotalCount_ReturnsCorrectCount()
        {
            // Arrange
            var expectedCount = 5;
            _readRepositoryMock
                .Setup(r => r.CountAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedCount);

            // Act
            var result = await _helper.TotalCount();

            // Assert
            Assert.Equal(expectedCount, result);
        }

        [Fact]
        public async Task DeleteAsync_WhenEntityExists_DeletesEntity()
        {
            // Arrange
            var id = 1;
            var entity = new DummyEntity { Id = id };
            _readRepositoryMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            // Act
            await _helper.DeleteAsync(id);

            // Assert
            _repositoryMock.Verify(r => r.DeleteAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenEntityNotFound_ThrowsApiException()
        {
            // Arrange
            var id = 2;
            _readRepositoryMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((DummyEntity)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() => _helper.DeleteAsync(id));
            Assert.Equal($"Entity of type {typeof(DummyEntity)} with id {id} not found.", ex.Message);
        }

        [Fact]
        public async Task EditAsync_WhenEntityExists_UpdatesEntity()
        {
            // Arrange
            var dto = new DummyDto { Id = 1 };
            var existingEntity = new DummyEntity { Id = dto.Id };
            var mappedEntity = new DummyEntity { Id = dto.Id }; // new mapped entity

            _readRepositoryMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEntity);

            _mapperMock
                .Setup(m => m.Map<DummyDto, DummyEntity>(dto))
                .Returns(mappedEntity);

            // Act
            await _helper.EditAsync(dto);

            // Assert
            _repositoryMock.Verify(r => r.UpdateAsync(mappedEntity, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task EditAsync_WhenEntityNotFound_ThrowsApiException()
        {
            // Arrange
            var dto = new DummyDto { Id = 3 };
            _readRepositoryMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((DummyEntity)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApiException>(() => _helper.EditAsync(dto));
            Assert.Equal($"Entity of type {typeof(DummyEntity)} with id {dto.Id} not found.", ex.Message);
        }
    }

    // Dummy classes for testing
    public class DummyEntity : IHasId
    {
        public int Id { get; set; }
    }

    public class DummyDto : IHasId
    {
        public int Id { get; set; }
    }
}
