using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BanHangOnline.Controllers
{
    public class SanPhamController : ApiController
    {
        [HttpGet]
        public List<SanPham> tatcadanhsach()
        {
            BanHangOnlineDataContext context = new BanHangOnlineDataContext();
            List<SanPham> dssp = context.SanPhams.ToList();
            foreach(SanPham sp in dssp)
            {
                sp.TheLoai.MaTheLoai = -1;
                sp.Account.MaUserName = -1;
            }
            return dssp;
        }
        [HttpGet]
        public List<SanPham>danhsachtheolist(int id)
        {
            BanHangOnlineDataContext context = new BanHangOnlineDataContext();
            List<SanPham> dssp = context.SanPhams.Where(x => x.MaTheLoai == id).ToList();
            foreach(SanPham sp in dssp)
            {
                sp.TheLoai.MaTheLoai = -1;
                sp.Account.MaUserName = -1;
            }
            return dssp;
        }
    }
}
