using Microsoft.AspNetCore.Mvc;

namespace AracKiralama.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Home/Error/{statusCode?}")]
        public IActionResult Error(int statusCode)
        {
            // Status code'a göre özel bir hata sayfası gösterebiliriz.
            if (statusCode == 404)
            {
                return View("NotFound"); // NotFound.cshtml dosyasını göster
            }
            return View("Error"); // Genel hata sayfasını göster
        }
    }
}
