using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Mail;
using System.Net;
using System.Net.Mail;
namespace BanHangOnline.WebServices
{
    /// <summary>
    /// Summary description for AuthenCation
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class AuthenCation : System.Web.Services.WebService
    {


        //Xac thuc tai khoan dang nhap
       [WebMethod]
       public bool authencation(string username,string password)
        {
            BanHangOnlineDataContext context = new BanHangOnlineDataContext();
            List<Account> ds = context.Accounts.ToList();
            foreach(Account ac in ds)
            {
               if(ac.UserName.CompareTo(username)==0 && ac.Pass.CompareTo(password) == 0)
                {
                    return true;
                }
            }
            return false;
        }


        //Them san pham
        [WebMethod]
        public bool insert_sanpham(string tensp,string anhsp,float giasp,string chitietsp,int matheloai,int mauser)
        {
            try
            {
                SanPham sp = new SanPham();
                sp.TenSP = tensp;
                sp.AnhSP = anhsp;
                sp.GiaSP = giasp;
                sp.ChiTietSP = chitietsp;
                sp.MaTheLoai = matheloai;
                sp.MaUserName = mauser;
                BanHangOnlineDataContext context = new BanHangOnlineDataContext();
                context.SanPhams.InsertOnSubmit(sp);
                context.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
            return false;
        }

        //insert user
        [WebMethod]
        public bool insert_user(string username,string pass)
        {
            if (check_user(username))
            {
                return false;
            }
            try
            {
                Account ac = new Account();
                ac.UserName = username;
                ac.Pass = pass;
                BanHangOnlineDataContext context = new BanHangOnlineDataContext();
                context.Accounts.InsertOnSubmit(ac);
                context.SubmitChanges();
                return true;
            }
            catch
            {

            }
            return false;
        }

        private void saveDonHang(string tenkhachhang,string donhang)
        {
            string madonhang = "";
            int MaSP = 0;
            string tenKh = "";
            string sdt = "";
            string soluong = "4";
            tenKh = tenkhachhang;

                soluong = donhang.Substring(donhang.IndexOf("Số lượng:"), donhang.IndexOf("Thành tiền:")).Trim();
                sdt = donhang.Substring(donhang.IndexOf("Số điện thoại:"),donhang.LastIndexOf("Mã sản phẩm:")).Trim();
                madonhang = donhang.Substring(donhang.IndexOf("ID Đơn hàng:"), donhang.Length);
                MaSP = Int32.Parse(donhang.Substring(donhang.IndexOf("Mã sản phẩm:"), donhang.IndexOf("Mã Thể Loại:")).Trim());
           
                BanHangOnlineDataContext context = new BanHangOnlineDataContext();
                DonHang dh = new DonHang();
            dh.TenKhachHang = tenkhachhang;
            dh.SoDienThoai = sdt;
            dh.MaSP = MaSP;
            dh.MaDonHang = madonhang;
            dh.SoLuong = soluong;
                context.DonHangs.InsertOnSubmit(dh);
                context.SubmitChanges();
        }

        //send email
        [WebMethod]
        public bool send_mail(string tenkhachhang,string donhang)
        {

            //save don hang
            try
            {
                saveDonHang(tenkhachhang, donhang);
            } catch (Exception e)
            {
              
            }
            //sent mail
            try
            {
                var from_email = new MailAddress("chi.yeu.minh.em.200895@gmail.com", "Nguyễn Công");
                var to_email = new MailAddress("nguyenthanhcong200895@gmail.com", "Đơn hàng");
                string subject = "Đơn hàng của " + tenkhachhang;
                string password = "01632211771";
                MailAddress cc1 = new MailAddress("tnthong.oplai@gmail.com");
                MailAddress cc2 = new MailAddress("ntcong95.oplai@gmail.com");
                var smtp = new SmtpClient
                {
                    Host = " smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(from_email.Address, password),
                };
                using (var message = new System.Net.Mail.MailMessage(from_email, to_email)
                {
                    Subject = subject,
                    Body = donhang,
                })
                {
                    message.CC.Add(cc1);
                    message.CC.Add(cc2);
                    smtp.Send(message);
                };
                return true;
            }
            catch
            {

            }
            return false;
        }

        //delete san pham
        [WebMethod]
        public bool delete_sanpham(int ma)
        {
            try
            {
                BanHangOnlineDataContext context = new BanHangOnlineDataContext();
                SanPham sp = context.SanPhams.FirstOrDefault(x => x.MaSP == ma);
                if (sp != null)
                {
                    context.SanPhams.DeleteOnSubmit(sp);
                    context.SubmitChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {

            }
            return false;
        }

        private bool check_user(string username)
        {
            BanHangOnlineDataContext context = new BanHangOnlineDataContext();
            List<Account> ds = context.Accounts.ToList();
            foreach(Account ac in ds)
            {
                if (ac.UserName.CompareTo(username) == 0)
                {
                    return true;
                }
            }
            return false;
        }
        
        [WebMethod]
        public bool edit_sanpham(int ma,string ten,string anh,
            string giasp,string chitietsp,
            string matheloai,string mausername)
        {
            BanHangOnlineDataContext context = new BanHangOnlineDataContext();
            SanPham sp = context.SanPhams.FirstOrDefault(x => x.MaSP == ma);
            try
            {
                if (sp != null)
                {
                    if (sp.MaSP == ma)
                    {
                        if (ten.Length > 0 && ten !=null)
                        {
                            sp.TenSP = ten;
                        }
                        if(anh.Length>0 && anh != null)
                        {
                            sp.AnhSP = anh;
                        }
                        if(giasp.Length>0 && giasp != null)
                        {
                            try
                            {
                                sp.GiaSP = Double.Parse(giasp);
                            }
                            catch (Exception)
                            {

                            }
                        }
                        if(chitietsp.Length>0 && chitietsp != null)
                        {
                            sp.ChiTietSP = chitietsp;
                        }
                    
                      return true;
                       
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        } 
    }
}