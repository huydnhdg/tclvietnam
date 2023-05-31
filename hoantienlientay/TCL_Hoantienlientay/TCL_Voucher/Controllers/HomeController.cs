using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
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
            return RedirectToAction("Index", "Vongquay");
            if (!string.IsNullOrEmpty(msg))
            {
                ViewBag.msg = msg;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Send(Contact contact, HttpPostedFileBase Invoice)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string mess = "";
                    //check trung serial
                    //if (contact.Product == "Tivi")
                    //{
                    //    if (contact.Serial.Length != 19)
                    //    {
                    //        return RedirectToAction("Index", "Home", new { msg = "Serial Tivi chưa đúng. Hãy kiểm tra lại" });
                    //    }
                    //}
                    //else if (contact.Product == "Máy lạnh")
                    //{
                    //    if (contact.Serial.Length != 20)
                    //    {
                    //        return RedirectToAction("Index", "Home", new { msg = "Serial Máy lạnh chưa đúng. Hãy kiểm tra lại" });
                    //    }

                    //}
                    //else if (contact.Product == "Máy giặt")
                    //{
                    //    if (contact.Serial.Length != 26)
                    //    {
                    //        return RedirectToAction("Index", "Home", new { msg = "Serial Máy giặt chưa đúng. Hãy kiểm tra lại" });
                    //    }
                    //}


                    //string imei = UPLOAD(Imei);
                    string invoice = UPLOAD(Invoice);
                    //string cmndb = UPLOAD(Cmndb);
                    //string cmnda = UPLOAD(Cmnda);

                    //string extra = null;
                    //string extra1 = null;
                    //string extra2 = null;
                    //if (Extra != null)
                    //{
                    //    extra = UPLOAD(Extra);
                    //}
                    //if (Extra1 != null)
                    //{
                    //    extra1 = UPLOAD(Extra1);
                    //}
                    //if (Extra2 != null)
                    //{
                    //    extra2 = UPLOAD(Extra2);
                    //}

                    var _contact = new Contact()
                    {
                        Name = contact.Name,
                        Phone = contact.Phone,
                        Province = contact.Province,
                        //EMEI = imei,
                        INVOICE = invoice,
                        //CMNDB = cmndb,
                        //CMNDA = cmnda,
                        Createdate = DateTime.Now,
                        Model = contact.Model,
                        BuyAdr = contact.BuyAdr,
                        Product = contact.Product,
                        //Serial = contact.Serial,
                        Extra = contact.Extra,
                        //Extra1 = extra1,
                        //Extra2 = extra2
                    };
                    db.Contacts.Add(_contact);
                    db.SaveChanges();

                    //dựa vào kích thước để trả thưởng cho khách hàng
                    mess = "OK";



                    return RedirectToAction("Index", "Home", new { msg = mess });
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    logger.Error(ex.InnerException);
                    return RedirectToAction("Index", "Home", new { msg = ex.Message });
                }
            }
            logger.Error("ModelState.IsValid");
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
            var rand = new Random();
            string strrand = rand.Next(0, 999).ToString();
            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(Server.MapPath("~/Uploads/"), strrand + "_" + fileName);
            file.SaveAs(path);
            string link = "/Uploads/" + strrand + "_" + fileName;
            return link;
        }

        static string msg_giainhat = "TCL CHUC MUNG QUY KHACH CO MA DU THUONG {0} DA TRUNG 1 BIEU TUONG VANG 24K TU CT ''MAN HINH SIEU LON - QUA SIEU TO''. LIEN HE 028 3836 6111 (NHANH 498) DE DUOC HUONG DAN NHAN THUONG. CHI TIET XEM TAI: http://manhinhsieulon.tcl.com";
        static string msg_giainhi = "TCL CHUC MUNG QUY KHACH CO MA DU THUONG {0} DA TRUNG 1 DONG TIEN VANG 1 CHI TU CT ''MAN HINH SIEU LON - QUA SIEU TO''. LIEN HE 028 3836 6111 (NHANH 498) DE DUOC HUONG DAN NHAN THUONG. CHI TIET XEM TAI: http://manhinhsieulon.tcl.com";
        static string msg_giaiba = "TCL CHUC MUNG QUY KHACH CO MA DU THUONG {0} DA TRUNG 500,000 VND TU CT ''MAN HINH SIEU LON - QUA SIEU TO''. LIEN HE 028 3836 6111 (NHANH 498) DE DUOC HUONG DAN NHAN THUONG. CHI TIET XEM TAI: http://manhinhsieulon.tcl.com";
        static string msg_kk = "TCL THONG BAO CHUC QUY KHACH CO MA DU THUONG {0} MAY MAN LAN SAU. CHI TIET LH 1800 588 880 HOAC 028 3836 6111 (NHANH 498), website: http://manhinhsieulon.tcl.com";
    }
}