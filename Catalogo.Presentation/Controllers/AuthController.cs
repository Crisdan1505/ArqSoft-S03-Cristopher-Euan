using Microsoft.AspNetCore.Mvc;

namespace Catalogo.Presentation.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string correo, string password)
        {
            if (correo == "admin@gmail.com" &&
                password == "1234")
            {
                HttpContext.Session.SetString("Usuario", correo);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Correo o contraseña incorrectos";

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string nombre,
                                      string correo,
                                      string password)
        {
            ViewBag.Mensaje = "Cuenta creada correctamente";

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Usuario");

            return RedirectToAction("Index", "Home");
        }
    }
}