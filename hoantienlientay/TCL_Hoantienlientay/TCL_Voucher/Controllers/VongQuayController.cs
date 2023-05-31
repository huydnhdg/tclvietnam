using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using TCL_Voucher.Models;

namespace TCL_Voucher.Controllers
{
    public class VongQuayController : Controller
    {
        TCL_VoucherEntities db = new TCL_VoucherEntities();
        Logger logger = LogManager.GetCurrentClassLogger();
        public ActionResult Index()
        {
            var model = new VongQuay();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(VongQuay contact, HttpPostedFileBase Invoice)
        {
            try
            {
                string json = JsonConvert.SerializeObject(contact);
                logger.Info(string.Format("[Send Form] @Contact={0}", json));
                string invoice = UPLOAD(Invoice);
                var check_old = db.VongQuays.Where(a => a.Phone == contact.Phone);
                if (check_old.Count() < 3)
                {
                    //random ra vi tri trung giai
                    Random random = new Random();
                    int index = random.Next(0, 99);
                    logger.Info("RANDOM RA " + index);
                    //kiem tra xem con giai nay khong | neu khong thi lay 1 giai nao do con
                    int giai = checkgiai(index);
                    logger.Info("SAU KHI CHECK CON KHONG RA " + giai);
                    if (giai > 0)
                    {
                        var _contact = new VongQuay()
                        {
                            Name = contact.Name,
                            Phone = contact.Phone,
                            Province = contact.Province,
                            BuyAdr = contact.BuyAdr,
                            Createdate = DateTime.Now,
                            PRODUCT = contact.PRODUCT,
                            MODEL = contact.MODEL,
                            SIZE = contact.SIZE,
                            INVOICE = invoice,
                            PAYMENT = giai
                        };
                        db.VongQuays.Add(_contact);
                        db.SaveChanges();
                        return View(_contact);
                    }
                    //result = 0 khong con giai nao 
                    contact.PAYMENT = -2;
                    return View(contact);
                }
                else
                {
                    //so dien thoai dang ky 3 lan
                    contact.PAYMENT = -1;
                    return View(contact);
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.InnerException);
                contact.PAYMENT = -9;
                return View(contact);
            }
        }
        [HttpPost]
        public JsonResult savedata(long ID, string giai)
        {
            var _contact = db.VongQuays.Find(ID);
            _contact.PAYMENT = int.Parse(giai);
            db.SaveChanges();
            return Json(_contact, JsonRequestBehavior.AllowGet);
        }
        static string giai1000 = WebConfigurationManager.AppSettings["G1000"];
        static string giai500 = WebConfigurationManager.AppSettings["G500"];
        static string giai400 = WebConfigurationManager.AppSettings["G400"];
        static string giai300 = WebConfigurationManager.AppSettings["G300"];
        static string giai200 = WebConfigurationManager.AppSettings["G200"];
        static string giai100 = WebConfigurationManager.AppSettings["G100"];
        public int checkgiai(int index)
        {
            int result = 0;
            var giai = db.VongQuays.Where(a => a.PAYMENT > 0);
            var giai10 = db.VongQuays.Where(a => a.PAYMENT == 1000);
            var giai5 = db.VongQuays.Where(a => a.PAYMENT == 500);
            var giai4 = db.VongQuays.Where(a => a.PAYMENT == 400);
            var giai3 = db.VongQuays.Where(a => a.PAYMENT == 300);
            var giai2 = db.VongQuays.Where(a => a.PAYMENT == 200);
            var giai1 = db.VongQuays.Where(a => a.PAYMENT == 100);
            if (giai.Count() < 550)
            {
                if ((index == 0))
                {
                    if (giai10.Count() < int.Parse(giai1000))
                    {
                        result = 1000;
                    }
                    else
                    {
                        //xem con giai nao k, neu con thi tra giai do, neu het thi thoi
                        result = congiainaokhac();
                    }
                }
                else if (index > 0 && index <= 3)
                {
                    if (giai1.Count() < int.Parse(giai500))
                    {
                        result = 500;
                    }
                    else
                    {
                        //xem con giai nao k, neu con thi tra giai do, neu het thi thoi
                        result = congiainaokhac();
                    }
                }
                else if (index > 3 && index <= 19)
                {
                    if (giai1.Count() < int.Parse(giai400))
                    {
                        result = 400;
                    }
                    else
                    {
                        //xem con giai nao k, neu con thi tra giai do, neu het thi thoi
                        result = congiainaokhac();
                    }
                }
                else if (index > 19 && index <= 39)
                {
                    if (giai1.Count() < int.Parse(giai300))
                    {
                        result = 300;
                    }
                    else
                    {
                        //xem con giai nao k, neu con thi tra giai do, neu het thi thoi
                        result = congiainaokhac();
                    }
                }
                else if (index > 39 && index <= 69)
                {
                    if (giai1.Count() < int.Parse(giai200))
                    {
                        result = 200;
                    }
                    else
                    {
                        //xem con giai nao k, neu con thi tra giai do, neu het thi thoi
                        result = congiainaokhac();
                    }
                }
                else if (index > 69 && index <= 99)
                {
                    if (giai1.Count() < int.Parse(giai100))
                    {
                        result = 100;
                    }
                    else
                    {
                        //xem con giai nao k, neu con thi tra giai do, neu het thi thoi
                        result = congiainaokhac();
                    }
                }
            }
            return result;
        }
        public int congiainaokhac()
        {
            var giai10 = db.VongQuays.Where(a => a.PAYMENT == 1000);
            var giai5 = db.VongQuays.Where(a => a.PAYMENT == 500);
            var giai4 = db.VongQuays.Where(a => a.PAYMENT == 400);
            var giai3 = db.VongQuays.Where(a => a.PAYMENT == 300);
            var giai2 = db.VongQuays.Where(a => a.PAYMENT == 200);
            var giai1 = db.VongQuays.Where(a => a.PAYMENT == 100);

            if (giai1.Count() < int.Parse(giai100))
            {
                return 100;
            }
            else if (giai2.Count() < int.Parse(giai200))
            {
                return 200;
            }
            else if (giai3.Count() < int.Parse(giai300))
            {
                return 300;
            }
            else if (giai4.Count() < int.Parse(giai400))
            {
                return 400;
            }
            else if (giai5.Count() < int.Parse(giai500))
            {
                return 500;
            }
            else if (giai10.Count() < int.Parse(giai1000))
            {
                return 1000;
            }
            return 0;
        }
        string UPLOAD(HttpPostedFileBase file)
        {
            var rand = new Random();
            string strrand = rand.Next(0, 999).ToString();
            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(Server.MapPath("~/Uploads/VongQuay/"), strrand + "_" + fileName);
            file.SaveAs(path);
            string link = "/Uploads/VongQuay/" + strrand + "_" + fileName;
            return link;
        }
    }
}