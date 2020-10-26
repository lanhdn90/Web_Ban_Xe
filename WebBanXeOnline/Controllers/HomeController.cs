using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanXeOnline.Models;

namespace WebBanXeOnline.Controllers
{
    public class HomeController : Controller
    {
        WebBanXeOnlineEntities shop = new WebBanXeOnlineEntities();

        public ActionResult Index()
        {
            var model = shop.NhaCungCap.OrderByDescending(h => h.MaNCC).Skip(0).Take(5).ToList();
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}