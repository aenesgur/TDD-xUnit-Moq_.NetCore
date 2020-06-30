using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StationaryStore.DAL.Abstractions;
using StationaryStore.Entities;

namespace StationaryStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Product product)
        {
            if (!id.Equals(product.Id))
                return BadRequest();

            var isUpdated = await _productService.UpdateAsync(id,product);
            if (isUpdated)
                return NoContent(); //Status Code : 204

            return NotFound();
            
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var isCreated = await _productService.CreateAsync(product);
            if (isCreated)
                return CreatedAtAction("Get", new { id = product.Id }, product);

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            var isDeleted = await _productService.DeleteAsync(id);
            if (isDeleted)
                return NoContent();

            return NotFound();
        }
    }
}