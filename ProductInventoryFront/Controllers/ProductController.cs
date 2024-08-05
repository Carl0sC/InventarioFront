using Microsoft.AspNetCore.Mvc;
using ProductInventoryFront.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProductInventoryFront.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? state)
        {
            var url = "/api/products";
            if (state.HasValue)
            {
                url += $"?state={state.Value}";
            }

            var response = await _httpClient.GetAsync(url);
            var content = response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : "[]";

            var products = JsonConvert.DeserializeObject<List<ProductModel>>(content);

            var viewModel = new ProductIndexViewModel
            {
                Products = products,
                SelectedState = state
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProductIndexViewModel model)
        {
            return await Index(model.SelectedState);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/products", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Error al registrar el producto.");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"/api/products/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<ProductModel>(content);
                return View(product);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/products/{model.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Error al actualizar el producto.");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"/api/products/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<ProductModel>(content);
                return View(product);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/products/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Error al eliminar el producto.");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ManageStock(int id, int quantity)
        {
            var product = new { Id = id, Quantity = quantity };
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/api/products/{id}/manage", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Error al gestionar la salida del producto.");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsDefective(int id)
        {
            var product = new { State = 2 }; // Estado 2 para defectuoso
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/api/products/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Error al marcar el producto como defectuoso.");
            return RedirectToAction("Index");
        }
    }
}
