using Microsoft.AspNetCore.Mvc;
using Moq;
using StationaryStore.DAL.Abstractions;
using StationaryStore.Entities;
using StationaryStore.UI.Controllers.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace StationaryStore.UI.Test.Admin
{
    public class ProductControllerTest
    {
        private readonly Mock<IProductService> _mock;
        private readonly ProductController _controller;
        private readonly List<Product> _products;

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
        public async void Index_ActionExecute_ReturnProductList()
        {
            _mock.Setup(x => x.GetAllAsync()).ReturnsAsync(_products);
            var result = await _controller.Index();

            var resultType = Assert.IsType<ViewResult>(result);
            var resultModel = Assert.IsAssignableFrom<List<Product>>(resultType.Model);
            Assert.Equal(2, resultModel.Count);
        }

        [Theory]
        [InlineData("0")]
        public async void Details_IdInValid_ReturnNotFound(string productId)
        {
            Product product = null;
            _mock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);
            var result = await _controller.Details(productId);

            var resultType = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, resultType.StatusCode);
        }

        [Theory]
        [InlineData("5ef9e23e1c4d3f78c4e72c1f")]
        public async void Details_ActionExecute_ReturnProduct(string productId)
        {
            var product = _products.First();
            _mock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);
            var result = await _controller.Details(productId);

            var resultType = Assert.IsType<ViewResult>(result);
            var resultModel = Assert.IsAssignableFrom<Product>(resultType.Model);
        }

        [Fact]
        public void Create_ActionResult_ReturnView()
        {
            var result = _controller.Create();

            var resultType = Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData("5ef9e23e1c4d3f78c4e72c1f")]
        public async void CreatePOST_InValidModelState_ReturnProduct(string productId)
        {
            var product = _products.First(x=>x.Id.Equals(productId));
            _controller.ModelState.AddModelError("Name", "Name field is required");
            var result = await _controller.Create(product);

            var resultType = Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData("5ef9e23e1c4d3f78c4e72c1f")]
        public async void CreatePOST_MethodReturnFalse_ReturnNotFound(string productId)
        {
            var product = _products.First(x => x.Id.Equals(productId));
            _mock.Setup(x => x.CreateAsync(product)).ReturnsAsync(false);
            var result = await _controller.Create(product);

            var resultType = Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData("5ef9e23e1c4d3f78c4e72c1f")]
        public async void CreatePOST_ActionExecute_RedirectToIndexAction(string productId)
        {
            var product = _products.First(x => x.Id.Equals(productId));
            _mock.Setup(x => x.CreateAsync(product)).ReturnsAsync(true);
            var result = await _controller.Create(product);

            var resultType = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", resultType.ActionName);
        }
    }
}
