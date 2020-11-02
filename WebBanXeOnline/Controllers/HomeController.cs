
using PagedList;
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

        public ActionResult TimKiem(string search, int? page)
        {
            ViewBag.KQ = search;
            var model = DanhSachTimKiem(search, page);
            return View(model);
        }

        public IPagedList<SanPham> DanhSachTimKiem(string search, int? page)
        {
            var model = shop.SanPham.Where(s => s.TenSP.ToLower().Contains(search.ToLower())).OrderByDescending(c => c.MaSP);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return model.ToPagedList(pageNumber, pageSize);
        }
    }
}