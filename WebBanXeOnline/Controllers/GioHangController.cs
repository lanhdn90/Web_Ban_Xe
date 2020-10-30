using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanXeOnline.Models;

namespace WebBanXeOnline.Controllers
{
    public class GioHangController : Controller
    {
        WebBanXeOnlineEntities shop = new WebBanXeOnlineEntities();
        // GET: GioHang
        public ActionResult Index()
        {
            ViewBag.Title = "Giỏ hàng";
            GioHangViewModel model = new GioHangViewModel();
            model.GioHang = (GioHang)Session["Cart"];
            return View(model);
        }

        [HttpPost]
        public ActionResult ThemVaoGioHang(int id, int anount)
        {
            var sp = shop.SanPham.Find(id);
            GioHang objCart = (GioHang)Session["Cart"];
            if(objCart == null)
            {
                objCart = new GioHang();
            }
            GioHang.GioHangItem item = new GioHang.GioHangItem()
            {
                MaSP = sp.MaSP,
                TenSP = sp.TenSP,
                Anh = sp.Anh,
                SoLuong = anount,
                Gia = sp.GiaBan,
                Tong = Convert.ToDouble(sp.GiaBan) * anount
            };
            objCart.AddToCart(item);
            Session["Cart"] = objCart;
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject("Thêm thành công", Formatting.Indented, jss);
            return this.Json(result, JsonRequestBehavior.AllowGet); ;
        }

        public ActionResult XoaSanPham(int id)
        {
            GioHang objCart = (GioHang)Session["Cart"];
            if (objCart != null)
            {
                if(objCart.XoaSanPham(id)) Session["Cart"] = objCart;
            }
            return RedirectToAction("index");
        }

        public ActionResult CapNhatSoLuong(string maSp, int soLuong)
        {
            int id = Convert.ToInt32(maSp.Substring(7, maSp.Length - 7));
            GioHang objCart = (GioHang)Session["Cart"];
            if (objCart != null)
            {
                if(objCart.CapNhatSoLuong(id, soLuong)) Session["Cart"] = objCart;
            }
            return RedirectToAction("index");
        }

        public ActionResult NhapThongTin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThanhToan(HoaDon hoadon)
        {
            GioHangViewModel model = new GioHangViewModel();
            model.GioHang = (GioHang)Session["Cart"];
            decimal tong = model.GioHang.ListItem.Sum(item => item.Gia * item.SoLuong);
            HoaDon hd = new HoaDon();
            hd.HovaTen = hoadon.HovaTen;
            hd.DiaChi = hoadon.DiaChi;
            hd.Email = hoadon.Email;
            hd.Phone = hoadon.Phone;
            hd.DiaChiGiaoHang = hoadon.DiaChiGiaoHang;
            hd.ThoiGianGiaoHang = hoadon.ThoiGianGiaoHang;
            hd.NgayTao = DateTime.Now;
            hd.TongTien = tong;
            hd.TrangThai = false;
            shop.HoaDon.Add(hd);
            shop.SaveChanges();

            var hoaDon = (from h in shop.HoaDon orderby h.MaHoaDon descending select h).FirstOrDefault();
            foreach (var item in model.GioHang.ListItem)
            {
                ChiTietHoaDon ct = new ChiTietHoaDon();
                ct.ChiTietHoaDon1 = shop.ChiTietHoaDon.Count() + 1;
                ct.MaHoaDon = hoaDon.MaHoaDon;
                ct.MaSP = item.MaSP;
                ct.SoLuong = item.SoLuong;
                ct.TongTien = item.SoLuong * shop.SanPham.Find(item.MaSP).GiaBan;
                shop.ChiTietHoaDon.Add(ct);
                shop.SaveChanges();
                CapNhatSLSanPham(item.MaSP, item.SoLuong);
            }
            model.GioHang.ListItem.Clear();
            return View();
        }
        
        
        public void CapNhatSLSanPham(int id, int soluong)
        {
            var model = shop.SanPham.Find(id);
            model.SoLuong = model.SoLuong - soluong;
            shop.SaveChanges();
           
        }
    }
}