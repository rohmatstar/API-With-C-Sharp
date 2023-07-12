using Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Client.Controllers
{
    public class LatihanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ArrayJS()
        {
            return View();
        }

		public IActionResult ConsumeAPI()
		{
			return View();
		}

        public IActionResult DataTable()
        {
            return View();
        }

        public IActionResult DataTableLocal()
        {
            return View();
        }

		public IActionResult ChartVisualization()
		{
			return View();
		}
	}
}