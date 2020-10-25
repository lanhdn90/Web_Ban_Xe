using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanXeOnline.Models;

namespace WebBanXeOnline.Controllers
{
    public class QLSanPhamController : Controller
    {
        WebBanXeOnlineEntities db = new WebBanXeOnlineEntities();
        // GET: QLSanPham
        public ActionResult Index()
        {
            var listQLSanPham = db.SanPham.OrderByDescending(x => x.MaSP).ToList();
            return View(listQLSanPham);
        }

        [HttpGet]
        public ActionResult ThemSanPham()
        {
            var listNCC = db.NhaCungCap.ToList();
            List<SelectListItem> list1 = listNCC.Select(t => new SelectListItem() { Text = t.TenNCC, Value = t.MaNCC.ToString() }).ToList();
            ViewBag.NCC = list1;
            var listMaLoai = db.LoaiSP.ToList();
            List<SelectListItem> list2 = listMaLoai.Select(t => new SelectListItem() { Text = t.TenLoaiSP, Value = t.MaLoai.ToString() }).ToList();
            ViewBag.MaLoai = list2;
            var listKhuyenMai = db.KhuyenMai.ToList();
            List<SelectListItem> list3 = listKhuyenMai.Select(t => new SelectListItem() { Text = t.TenKM, Value = t.MaKM.ToString() }).ToList();
            ViewBag.MaKM = list3;
            return View();
        }

        [HttpPost]
        public ActionResult ThemSanPham(SanPham sanPham)
        {
            var count = db.SanPham.ToList().Count;
            sanPham.MaSP =count +1;
            sanPham.MaTaiKhoan = 1;
            sanPham.NgayDang = DateTime.Now;
            db.SanPham.Add(sanPham);
            db.SaveChanges();
            var listQLSanPham = db.SanPham.OrderByDescending(x => x.MaSP).ToList();
            return RedirectToAction("Index", listQLSanPham);

        }

        [HttpGet]
        public ActionResult SuaSanPham(int id)
        {
            var listNCC = db.NhaCungCap.ToList();
            List<SelectListItem> list1 = listNCC.Select(t => new SelectListItem() { Text = t.TenNCC, Value = t.MaNCC.ToString() }).ToList();
            ViewBag.NCC = list1;
            var listMaLoai = db.LoaiSP.ToList();
            List<SelectListItem> list2 = listMaLoai.Select(t => new SelectListItem() { Text = t.TenLoaiSP, Value = t.MaLoai.ToString() }).ToList();
            ViewBag.MaLoai = list2;
            var listKhuyenMai = db.KhuyenMai.ToList();
            List<SelectListItem> list3 = listKhuyenMai.Select(t => new SelectListItem() { Text = t.TenKM, Value = t.MaKM.ToString() }).ToList();
            ViewBag.MaKM = list3;
            var model = db.SanPham.Find(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult SuaSanPham(SanPham sanPham)
        {
            var model = db.SanPham.Find(sanPham.MaSP);
            model.TenSP = sanPham.TenSP;
            model.MaNCC = sanPham.MaNCC;
            model.LoaiSP = sanPham.LoaiSP;
            model.GiaNhap = sanPham.GiaNhap;
            model.Anh = sanPham.Anh;
            if(sanPham.SoLuong == model.SoLuong)
            {
                model.SoLuong = sanPham.SoLuong;
            }
            else
            {
                model.SoLuong = model.SoLuong + sanPham.SoLuong;
            }
            model.GiaBan = sanPham.GiaBan;
            model.ChiTiet = sanPham.ChiTiet;
            model.MaKM = sanPham.MaKM;
            model.NgayDang = DateTime.Now;
            model.MaTaiKhoan = model.MaTaiKhoan;
            db.SaveChanges();
            var listQLSanPham = db.SanPham.OrderByDescending(x => x.MaSP).ToList();
            return RedirectToAction("Index", listQLSanPham);

        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var count = db.SanPham.ToList().Count;
            var modelend = db.SanPham.Find(count);
            // Tìm model
            var model = db.SanPham.Find(id);
            // remove model đối đối tượng chứ không phải remove id
            model.TenSP = modelend.TenSP;
            model.MaNCC = modelend.MaNCC;
            model.LoaiSP = modelend.LoaiSP;
            model.GiaNhap = modelend.GiaNhap;
            model.Anh = modelend.Anh;
            model.SoLuong = modelend.SoLuong;        
            model.GiaBan = modelend.GiaBan;
            model.ChiTiet = modelend.ChiTiet;
            model.MaKM = modelend.MaKM;
            model.NgayDang = model.NgayDang;
            model.MaTaiKhoan = modelend.MaTaiKhoan;
            db.SanPham.Remove(modelend);
            db.SaveChanges();
            JsonSerializerSettings json = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore};
            var result = JsonConvert.SerializeObject("Xóa thành công", Formatting.Indented, json);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}