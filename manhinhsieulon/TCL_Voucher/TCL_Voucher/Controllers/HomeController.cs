using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCL_Voucher.Models;
using TCL_Voucher.Utils;

namespace TCL_Voucher.Controllers
{
    public class HomeController : Controller
    {
        TCL_VoucherEntities db = new TCL_VoucherEntities();
        Logger logger = LogManager.GetCurrentClassLogger();
        //public ActionResult Index(string key = "")
        //{
        //    if (key == "trang")
        //    {
        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToAction("Update", "Home");
        //    }

        //}
        public ActionResult Index(string msg = "")
        {
            if (!string.IsNullOrEmpty(msg))
            {
                ViewBag.msg = msg;
            }
            var _traothuong = db.Traothuongs.OrderBy(a => a.Giai);
            ViewBag.dstraothuong = _traothuong.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Send(Contact contact, HttpPostedFileBase Imei, HttpPostedFileBase Invoice,
            HttpPostedFileBase Cmndb, HttpPostedFileBase Cmnda, HttpPostedFileBase Extra, HttpPostedFileBase Extra1, HttpPostedFileBase Extra2)
        {
            if (ModelState.IsValid)
            {
                logger.Info("có khách hàng gửi form");
                try
                {
                    string mess = "";
                    //check trung serial
                    if (contact.Product == "Tivi")
                    {
                        if (contact.Serial.Length != 19)
                        {
                            return RedirectToAction("Index", "Home", new { msg = "Serial Tivi chưa đúng. Hãy kiểm tra lại" });
                        }
                    }
                    else if (contact.Product == "Máy lạnh")
                    {
                        if (contact.Serial.Length != 20)
                        {
                            return RedirectToAction("Index", "Home", new { msg = "Serial Máy lạnh chưa đúng. Hãy kiểm tra lại" });
                        }

                    }
                    else if (contact.Product == "Máy giặt")
                    {
                        if (contact.Serial.Length != 26)
                        {
                            return RedirectToAction("Index", "Home", new { msg = "Serial Máy giặt chưa đúng. Hãy kiểm tra lại" });
                        }
                    }
                    var check = db.Contacts.Where(a => a.Serial.Equals(contact.Serial));
                    var blacklist = db.Blacklists.Where(a => a.IMEI.Equals(contact.Serial));
                    if (check.SingleOrDefault() != null || blacklist.Count() > 0)//serial nay da dang ky nhan thuong
                    {
                        return RedirectToAction("Index", "Home", new { msg = "Serial này đã đăng ký nhận thưởng. Hãy thử nhập 1 serial khác." });
                    }
                    else
                    {

                        //lay ra 1 ma du thuong chua tick
                        int type = 0;
                        var model = new Maduthuong();

                        //chan 1 sdt chi duoc trung thuong 2 lan
                        // var datrungthuong = check.Where(a => a.Type != null);
                        var datrungthuong = db.Contacts.Where(a => a.Type > 0 && a.Phone == contact.Phone);
                        if (datrungthuong.Count() > 1)
                        {
                            var makhongtrung = db.Maduthuongs.Where(a => a.Status == null && a.Type == null).OrderBy(a => Guid.NewGuid()).FirstOrDefault();
                            model = makhongtrung;
                        }
                        else
                        {
                            //do
                            //{
                            int sogiainhat = int.Parse(ConfigurationManager.AppSettings["FIXGIATNHAT"]);
                            var _checkma = db.Maduthuongs.Where(a => a.Type == 1 && a.Activedate != null);
                            //neu chua trung du so giai nhat yeu cau thi fix cho trung luon
                            if (_checkma.Count() < sogiainhat)
                            {
                                var maduthuong = db.Maduthuongs.Where(a => a.Status == null && a.Type == 1).OrderBy(a => Guid.NewGuid()).FirstOrDefault();
                                type = maduthuong.Type ?? default(int);
                                model = maduthuong;

                                logger.Info("fix giai : " + type);
                            }
                            else
                            {
                                var maduthuong = db.Maduthuongs.Where(a => a.Status == null).OrderBy(a => Guid.NewGuid()).FirstOrDefault();
                                type = maduthuong.Type ?? default(int);
                                model = maduthuong;
                                logger.Info(string.Format("Quay giải thưởng Giải= {0} @Số điện thoại={1}",type, contact.Phone));
                            }

                            //} while (!DEMGIAI(type));
                        }


                        if (!string.IsNullOrEmpty(model.Code))
                        {
                            if (model.Type == 1)
                            {
                                //trung giai nhat
                                mess = String.Format(msg_giainhat, model.Code);
                            }
                            else if (model.Type == 2)
                            {
                                //trung gia nhi
                                mess = String.Format(msg_giainhi, model.Code);
                            }
                            else if (model.Type == 3)
                            {
                                //trung giai ba
                                mess = String.Format(msg_giaiba, model.Code);
                            }
                            else
                            {
                                mess = String.Format(msg_kk, model.Code);
                            }
                            //luu thong tin khach hang
                            string imei = UPLOAD(Imei);
                            string invoice = UPLOAD(Invoice);
                            string cmndb = UPLOAD(Cmndb);
                            string cmnda = UPLOAD(Cmnda);
                            string extra = null;
                            string extra1 = null;
                            string extra2 = null;
                            if (Extra != null)
                            {
                                extra = UPLOAD(Extra);
                            }
                            if (Extra1 != null)
                            {
                                extra1 = UPLOAD(Extra1);
                            }
                            if (Extra2 != null)
                            {
                                extra2 = UPLOAD(Extra2);
                            }

                            contact.Type = model.Type;
                            contact.Maduthuong = model.Code;
                            contact.EMEI = imei;
                            contact.INVOICE = invoice;
                            contact.CMNDB = cmndb;
                            contact.CMNDA = cmnda;
                            contact.Extra = extra;
                            contact.Extra1 = extra1;
                            contact.Extra2 = extra2;
                            contact.Createdate = DateTime.Now;
                            db.Contacts.Add(contact);

                            //gach ma trong danh sach du thuong
                            model.Status = 1;
                            model.Activedate = DateTime.Now;
                            db.Entry(model).State = System.Data.Entity.EntityState.Modified;

                            //save data
                            db.SaveChanges();

                            //sent brandname cho khach hang
                            int result = SendBrandName.SentMsg(contact.Phone, mess);
                            var brandname = new SentBrandname()
                            {
                                Phone = contact.Phone,
                                Message = mess,
                                Createdate = DateTime.Now,
                                Status = result
                            };
                            db.SentBrandnames.Add(brandname);
                            db.SaveChanges();
                        }
                    }

                    return RedirectToAction("Index", "Home", new { msg = mess });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Home", new { msg = ex.Message });
                }
            }
            return RedirectToAction("Index", "Home", new { msg = "Không thành công, hãy thử lại sau hoặc liên hệ hotline để được hỗ trợ." });
        }

        bool DEMGIAI(int type)
        {
            DateTime today = DateTime.Now;
            DateTime tuan1 = new DateTime(2021, 06, 06);
            tuan1 = tuan1.AddDays(1);//den cuoi ngay 06
            DateTime tuan2 = tuan1.AddDays(7);
            DateTime tuan3 = tuan2.AddDays(7);
            DateTime tuan4 = tuan3.AddDays(7);
            DateTime tuan5 = tuan4.AddDays(7);
            DateTime tuan6 = tuan5.AddDays(7);
            int sonhan = 1;
            if (today <= tuan1)
            {
                sonhan = 1;
            }
            else if (today <= tuan2)
            {
                sonhan = 2;
            }
            else if (today <= tuan3)
            {
                sonhan = 3;
            }
            else if (today <= tuan4)
            {
                sonhan = 4;
            }
            else if (today <= tuan5)
            {
                sonhan = 5;
            }
            else if (today <= tuan6)
            {
                sonhan = 6;
            }
            switch (type)
            {
                case 1:
                    int count1 = db.Maduthuongs.Where(a => a.Type == 3 && a.Status == 1).Count();
                    if (count1 < 1 * sonhan)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case 2:
                    int count2 = db.Maduthuongs.Where(a => a.Type == 2 && a.Status == 1).Count();
                    if (count2 < 5 * sonhan)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case 3:
                    int count3 = db.Maduthuongs.Where(a => a.Type == 3 && a.Status == 1).Count();
                    if (count3 < 50 * sonhan)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return true;
            }
        }

        string UPLOAD(HttpPostedFileBase file)
        {
            string link = "";
            if (file != null)
            {
                var rand = new Random();
                string strrand = rand.Next(0, 999).ToString();
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Uploads/"), strrand + "_" + fileName);
                file.SaveAs(path);
                link = "/Uploads/" + strrand + "_" + fileName;
            }
            return link;            
        }

        static string msg_giainhat = "TCL CHUC MUNG QUY KHACH CO MA DU THUONG {0} DA TRUNG 1 BIEU TUONG VANG 24K TU CT ''MAN HINH SIEU LON - QUA SIEU TO''. LIEN HE 028 3836 6111 (NHANH 498) DE DUOC HUONG DAN NHAN THUONG. CHI TIET XEM TAI: http://manhinhsieulon.tcl.com";
        static string msg_giainhi = "TCL CHUC MUNG QUY KHACH CO MA DU THUONG {0} DA TRUNG 1 DONG TIEN VANG 1 CHI TU CT ''MAN HINH SIEU LON - QUA SIEU TO''. LIEN HE 028 3836 6111 (NHANH 498) DE DUOC HUONG DAN NHAN THUONG. CHI TIET XEM TAI: http://manhinhsieulon.tcl.com";
        static string msg_giaiba = "TCL CHUC MUNG QUY KHACH CO MA DU THUONG {0} DA TRUNG 500,000 VND TU CT ''MAN HINH SIEU LON - QUA SIEU TO''. LIEN HE 028 3836 6111 (NHANH 498) DE DUOC HUONG DAN NHAN THUONG. CHI TIET XEM TAI: http://manhinhsieulon.tcl.com";
        static string msg_kk = "TCL THONG BAO CHUC QUY KHACH CO MA DU THUONG {0} MAY MAN LAN SAU. CHI TIET LH 1800 588 880 HOAC 028 3836 6111 (NHANH 498), website: http://manhinhsieulon.tcl.com";
    }
}