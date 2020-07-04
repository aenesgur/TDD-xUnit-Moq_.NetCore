using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StationaryStore.DAL.Abstractions;
using StationaryStore.Entities;

namespace StationaryStore.UI.Controllers.Admin
{
    [Route("admin/[controller]/[action]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();
            return View(products);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isCreated = await _productService.CreateAsync(model);
            if (isCreated)
            {
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        // GET: Product/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Product model)
        {
            if (!id.Equals(model.Id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var isUpdated = await _productService.UpdateAsync(id, model);
            if (isUpdated)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var isDeleted = await _productService.DeleteAsync(id);
            if (isDeleted)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}