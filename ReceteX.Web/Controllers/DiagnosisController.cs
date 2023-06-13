using Microsoft.AspNetCore.Mvc;

namespace ReceteX.Web.Controllers
{
    public class DiagnosisController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
