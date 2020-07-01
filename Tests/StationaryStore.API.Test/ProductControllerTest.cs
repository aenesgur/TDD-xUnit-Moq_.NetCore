using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StationaryStore.API.Controllers;
using StationaryStore.DAL.Abstractions;
using StationaryStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace StationaryStore.API.Test
{
    public class ProductControllerTest
    {
        private readonly Mock<IProductService> _mock;
        private readonly ProductController _controller;

        private List<Product> _products;

        public ProductControllerTest()
        {
            _mock = new Mock<IProductService>();
            _controller = new ProductController(_mock.Object);

            _products = new List<Product>()
            {
                new Product
                {
                    Id="5ef9e23e1c4d3f78c4e72c1f",
                    Name="Pen",
                    IsStock=true,
                    Price=25
                },
                new Product
                {
                    Id = "5ef9e23e1c4d3f78c4e72c1e",
                    Name = "Eraser",
                    IsStock = false,
                    Price = 5
                }
            };
        }

        [Fact]
        public async void Get_ActionExecute_ReturnOkResultWithProductList()
        {
            _mock.Setup(x => x.GetAllAsync()).ReturnsAsync(_products);
            var result = await _controller.Get();

            _mock.Verify(x => x.GetAllAsync(), Times.Once);
            var resultType = Assert.IsType<OkObjectResult>(result);
            var resultModel = Assert.IsAssignableFrom<List<Product>>(resultType.Value);
        }

        [Theory]
        [InlineData("0")]
        public async void Get_IdInValid_ReturnNotFound(string productId)
        {
            Product product = null;
            _mock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);
            var result = await _controller.Get(productId);

            _mock.Verify(x => x.GetByIdAsync(productId), Times.Once);
            var resultType = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, resultType.StatusCode);
        }

        [Theory]
        [InlineData("5ef9e23e1c4d3f78c4e72c1f")]
        public async void Get_ActionExecute_ReturnOkResultWithProduct(string productId)
        {
            var product = _products.First(x => x.Id.Equals(productId));
            _mock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);
            var result = await _controller.Get(productId);

            _mock.Verify(x => x.GetByIdAsync(productId), Times.Once);
            var resultType = Assert.IsType<OkObjectResult>(result);
            var resultModel = Assert.IsAssignableFrom<Product>(resultType.Value);
        }

        [Theory]
        [InlineData("0")]
        public async void Update_IdNotEqualModelId_ReturnBadRequest(string productId)
        {
            var product = _products.First();
            var result = await _controller.Update(productId, product);

            _mock.Verify(x => x.UpdateAsync(productId, product), Times.Never);
            var resultType = Assert.IsType<BadRequestResult>(result);
            Assert.NotEqual(productId, product.Id);
        }

        [Theory]
        [InlineData("5ef9e23e1c4d3f78c4e72c1f")]
        public async void Update_UpdateMethodReturnFalse_ReturnNotFound(string productId)
        {
            var product = _products.First(x => x.Id.Equals(productId));
            _mock.Setup(x => x.UpdateAsync(productId, product)).ReturnsAsync(false);
            var result = await _controller.Update(productId, product);

            var resultType = Assert.IsType<NotFoundResult>(result);
            _mock.Verify(x => x.UpdateAsync(productId, product), Times.Once);
            Assert.Equal(404, resultType.StatusCode);
        }

        [Theory]
        [InlineData("5ef9e23e1c4d3f78c4e72c1f")]
        public async void Update_ActionExecute_ReturnNoContent(string productId)
        {
            var product = _products.First(x => x.Id.Equals(productId));
            _mock.Setup(x => x.UpdateAsync(productId, product)).ReturnsAsync(true);
            var result = await _controller.Update(productId, product);

            _mock.Verify(x => x.UpdateAsync(productId, product), Times.Once);
            var resultType = Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void Create_InValidModelState_ReturnBadRequest()
        {
            var product = _products.First();
            _controller.ModelState.AddModelError("Error", "It is an error message for test");
            var result = await _controller.Create(product);

            _mock.Verify(x => x.CreateAsync(product), Times.Never);
            var resultType = Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void Create_CreateMethodReturnFalse_ReturnNotFound()
        {
            var product = _products.First();
            _mock.Setup(x => x.CreateAsync(product)).ReturnsAsync(false);
            var result = await _controller.Create(product);

            _mock.Verify(x => x.CreateAsync(product), Times.Once);
            var resultType = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Create_ActionExecute_ReturnNoContent()
        {
            var product = _products.First();
            _mock.Setup(x => x.CreateAsync(product)).ReturnsAsync(true);
            var result = await _controller.Create(product);

            _mock.Verify(x => x.CreateAsync(product), Times.Once);
            var resultType = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Get", resultType.ActionName);
        }

        [Theory]
        [InlineData("0")]
        public async void Delete_IdInValid_ReturnNotFound(string productId)
        {
            Product product = null;
            _mock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);
            var result = await _controller.Delete(productId);

            _mock.Verify(x => x.GetByIdAsync(productId), Times.Once);
            var resultType = Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData("5ef9e23e1c4d3f78c4e72c1f")]
        public async void Delete_DeleteMethodReturnFalse_ReturnNotFound(string productId)
        {
            var product = _products.First(x => x.Id.Equals(productId));
            _mock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);
            _mock.Setup(x => x.DeleteAsync(productId)).ReturnsAsync(false);
            var result = await _controller.Delete(productId);

            _mock.Verify(x => x.GetByIdAsync(productId), Times.Once);
            _mock.Verify(x => x.DeleteAsync(productId), Times.Once);
            var resultType = Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData("5ef9e23e1c4d3f78c4e72c1f")]
        public async void Delete_ActionExecute_ReturnNoContent(string productId)
        {
            var product = _products.First(x => x.Id.Equals(productId));
            _mock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);
            _mock.Setup(x => x.DeleteAsync(productId)).ReturnsAsync(true);
            var result = await _controller.Delete(productId);

            _mock.Verify(x => x.GetByIdAsync(productId), Times.Once);
            _mock.Verify(x => x.DeleteAsync(productId), Times.Once);
            var resultType = Assert.IsType<NoContentResult>(result);
        }
    }
}
