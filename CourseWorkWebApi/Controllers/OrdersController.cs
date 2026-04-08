using Microsoft.AspNetCore.Mvc;

namespace CourseWorkWebApi.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
