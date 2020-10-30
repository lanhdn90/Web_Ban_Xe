using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace WebBanXeOnline.Models
{
    public class GioHang
    {
        public GioHang()
        {
            ListItem = new List<GioHangItem>();
        }

        public List<GioHangItem> ListItem { get; set; }

        public void AddToCart(GioHangItem item)
        {
            var updateCrat = CapNhatSoLuong(item.MaSP, item.SoLuong);
            if (updateCrat == false)
            {
                ListItem.Add(item);
            }
        }

        public bool CapNhatSoLuong(int lngProductSellID, int intQuantity)
        {
            GioHangItem existsItem = ListItem.Where(x => x.MaSP == lngProductSellID).SingleOrDefault();
            if (existsItem != null)
            {
                existsItem.SoLuong = intQuantity;
                existsItem.Tong = existsItem.SoLuong * Convert.ToDouble(existsItem.Gia);
                return true;
            }
            else
            {
                return false;
            }   
        }

        public bool XoaSanPham(int lngProductSellID)
        {
            GioHangItem existsItem = ListItem.Where(x => x.MaSP == lngProductSellID).SingleOrDefault();
            if (existsItem != null)
            {
                ListItem.Remove(existsItem);
                return true;
            }
            else
            {
                return false;
            }   
        }

        public bool GioHangRong()
        {
            ListItem.Clear();
            return true;
        }

        public class GioHangItem
        {
            public int MaSP { get; set; }
            public string TenSP { get; set; }
            public string Anh { get; set; }
            public int SoLuong { get; set; }
            public decimal Gia { get; set; }
            public double Tong { get; set; }
        }
    }
}