using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanXeOnline.Models;

namespace WebBanXeOnline.Controllers
{
    public class QLHoaDonController : Controller
    {
        private static int mahd;
        WebBanXeOnlineEntities db = new WebBanXeOnlineEntities();
        // GET: QLHoaDon
        public ActionResult Index()
        {
            var model = new DonHangViewModel()
            {
                DonHangChuaDuyet = db.HoaDon.Where(x => x.TrangThai == false).ToList(),
                DonHangDaDuyet = db.HoaDon.Where(x => x.TrangThai == true).ToList()
            };
            return View(model);
        }

        public ActionResult ChiTietHoaDon(int id)
        {
            mahd = id;
            ViewBag.mahd = id;
            var listCTHoaDon = db.ChiTietHoaDon.Where(x => x.MaHoaDon == id).OrderBy(c => c.ChiTietHoaDon1).ToList();
            return View(listCTHoaDon);
        }

        public ActionResult DuyetHD(int id)
        {
            var hoaDon = db.HoaDon.Find(id).TrangThai = true;
            //Thiếu Mã tài khoản
            db.SaveChanges();
            var model = new DonHangViewModel()
            {
                DonHangChuaDuyet = db.HoaDon.Where(x => x.TrangThai == false).ToList(),
                DonHangDaDuyet = db.HoaDon.Where(x => x.TrangThai == true).ToList()
            };
            return RedirectToAction("Index", model);
        }

        [HttpPost]
        public ActionResult ThemChiTietHd(int SanPham, int SoLuong)
        {
            var sp = db.SanPham.SingleOrDefault(s => s.MaSP == SanPham);
            var hd = db.HoaDon.SingleOrDefault(h => h.MaHoaDon == mahd);
            ChiTietHoaDon ct = new ChiTietHoaDon();
            ct.MaHoaDon = mahd;
            ct.MaSP = SanPham;
            ct.SoLuong = SoLuong;
            ct.TongTien = SoLuong * sp.GiaBan;
            db.ChiTietHoaDon.Add(ct);
            hd.TongTien += ct.TongTien;
            db.SaveChanges();
            return RedirectToAction("ChiTietHd", new { id = mahd });

        }

        [HttpGet]
        public ActionResult SuaHD(int id)
        {
            var model = db.HoaDon.Find(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult SuaHD(HoaDon hoaDon)
        {
            var model = db.HoaDon.Find(hoaDon.MaHoaDon);
            model.HovaTen = hoaDon.HovaTen;
            model.DiaChi = hoaDon.DiaChi;
            model.Phone = hoaDon.Phone;
            model.Email = hoaDon.Email;
            model.DiaChiGiaoHang = hoaDon.DiaChiGiaoHang;
            model.ThoiGianGiaoHang = hoaDon.ThoiGianGiaoHang;
            db.SaveChanges();
            var model1 = new DonHangViewModel()
            {
                DonHangChuaDuyet = db.HoaDon.Where(x => x.TrangThai == false).ToList(),
                DonHangDaDuyet = db.HoaDon.Where(x => x.TrangThai == true).ToList()
            };
            return RedirectToAction("Index", model1);
        }

        [HttpPost]
        public ActionResult SuaChiTietHD(int id, int soluong)
        {
            var model = db.ChiTietHoaDon.Find(id);
            var hoaDon = db.HoaDon.Find(model.MaHoaDon);
            var sanPham = db.SanPham.Find(model.MaSP);
            sanPham.SoLuong = sanPham.SoLuong - soluong + model.SoLuong;
            hoaDon.TongTien += (soluong * db.SanPham.Find(model.MaSP).GiaBan) - model.TongTien;
            model.SoLuong = soluong;
            model.TongTien = model.SoLuong * db.SanPham.Find(model.MaSP).GiaBan;
            db.SaveChanges();
            JsonSerializerSettings json = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject("Cập nhật thành công", Formatting.Indented, json);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }


        public void XoaChiTietHD(int id)
        {
            var count = db.ChiTietHoaDon.ToList().Count;
            var modelend = db.ChiTietHoaDon.Find(count);

            var model = db.ChiTietHoaDon.Find(id);
            var sp = db.SanPham.Find(model.MaSP).SoLuong + model.SoLuong;
            var hoadon = db.HoaDon.Find(model.MaHoaDon).TongTien - model.TongTien;
            model.MaSP = modelend.MaSP;
            model.MaHoaDon = modelend.MaHoaDon;
            model.SoLuong = modelend.SoLuong;
            model.TongTien = modelend.TongTien;

            db.ChiTietHoaDon.Remove(modelend);
            db.SaveChanges();
        }

        [HttpPost]
        public ActionResult DeleteChiTietHD(int id)
        {
            XoaChiTietHD(id);
            JsonSerializerSettings json = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject("Xóa thành công", Formatting.Indented, json);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteHD(int id)
        {
            // Tìm  một đối tượng mà có khóa chính Find
            //Tìm một đối tượng mà có khóa phụ SingeOrDefau
            //Tim một list mà có khóa phụ Where
            var listChiTietHD = db.ChiTietHoaDon.Where(x => x.MaHoaDon == id).ToList();
            foreach (var item in listChiTietHD)
            {
                XoaChiTietHD(item.ChiTietHoaDon1);
            }
            db.HoaDon.Remove(db.HoaDon.Find(id));
            db.SaveChanges();
            JsonSerializerSettings json = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject("Xóa thành công", Formatting.Indented, json);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}