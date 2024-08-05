using Microsoft.AspNetCore.Mvc;
using ProductInventoryFront.Models;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProductInventoryFront.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var response = await _httpClient.PostAsync("/api/auth/login", new StringContent(
                    JsonConvert.SerializeObject(model),
                    Encoding.UTF8,
                    "application/json"
                ));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<TokenResponse>(content);
                    // Maneja el token aquí, como guardarlo en el almacenamiento local o en una sesión
                    HttpContext.Session.SetString("AuthToken", result.Token);

                    // Redirige al índice de productos después del login exitoso
                    return RedirectToAction("Index", "Product");
                }

                ModelState.AddModelError("", "Invalid login attempt");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
            }

            return View(model);
        }
    }
}
