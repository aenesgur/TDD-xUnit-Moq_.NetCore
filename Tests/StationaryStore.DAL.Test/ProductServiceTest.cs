using StationaryStore.DAL.Abstractions;
using StationaryStore.DAL.Concerete;
using StationaryStore.DAL.Test.Attributes;
using StationaryStore.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace StationaryStore.DAL.Test
{
    [TestCaseOrderer("StationaryStore.DAL.Test.Orderers.PriorityOrderer", "StationaryStore.DAL.Test")]
    public class ProductServiceTest
    {
        private readonly IProductService _service;
        private List<Product> _products;
      
        public ProductServiceTest()
        {
            IMongoDbSettings settings = new MongoDbSettings
            {
                ConnectionString = "mongodb://localhost:27017",
                Database = "BookDatabaseTest",
                Collection = "ProductService"
            };

            _service = new ProductService(settings);

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

        [Theory, TestPriority(0)]
        [InlineData("5ef9e23e1c4d3f78c4e72c1f")]
        public async void CreateAsync_MethodExecute_ReturnTrue(string productId)
        {
            var product = _products.First(x => x.Id.Equals(productId));
            var result = await _service.CreateAsync(product);
            var resultType = Assert.IsType<bool>(result);
            Assert.True(resultType);
        }

        [Fact, TestPriority(1)]
        public async void GetAllAsync_MethotExecute_ReturnProductList()
        {
            var result = await _service.GetAllAsync();

            Assert.IsAssignableFrom<List<Product>>(result);
        }

        [Theory, TestPriority(2)]
        [InlineData("5ef9e23e1c4d3f78c4e72c1f")]
        public async void UpdateAsync_MethodExecute_ReturnTrue(string productId)
        {
            var product = _products.First(x => x.Id.Equals(productId));
            product.Name = "Book";
            product.IsStock = false;

            var result = await _service.UpdateAsync(productId, product);
            var resultType = Assert.IsType <bool> (result);
            Assert.True(resultType);
        }

        [Theory, TestPriority(3)]
        [InlineData("5ef9e23e1c4d3f78c4e72c1f")]
        public async void GetByIdAsync_MethodExecute_ReturnTrue(string productId)
        {
            var result = await _service.GetByIdAsync(productId);
            var resultType = Assert.IsAssignableFrom<Product>(result);
            Assert.NotNull(result.Id);
            Assert.NotNull(result.Name);
        }

        [Theory, TestPriority(4)]
        [InlineData("5ef9e23e1c4d3f78c4e72c1f")]
        public async void DeleteAsync_MethodExecute_ReturnTrue(string productId)
        {
            var result = await _service.DeleteAsync(productId);
            var resultType = Assert.IsType<bool>(result);
            Assert.True(resultType);

        }

    }
}
