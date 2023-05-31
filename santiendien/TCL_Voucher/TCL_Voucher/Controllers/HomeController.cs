using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCL_Voucher.Models;

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
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Update()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendContact(Contact contact)
        {
            try
            {
                string model = JsonConvert.SerializeObject(contact);
                logger.Info(model);
                //check limited
                var checkcontact = db.Contacts;
                DateTime ldate = new DateTime(2021, 05, 16);
                var cphone = checkcontact.Where(a => a.Phone == contact.Phone && a.Createdate >= ldate);
                if (cphone.Count() >= 2)
                {
                    logger.Info("Số điện thoại đã đăng ký quá số lần quy định, vui lòng kiểm tra lại.");
                    return Json("Số điện thoại đã đăng ký quá số lần quy định, vui lòng kiểm tra lại.");
                }
                var cimei = checkcontact.Where(a => a.EMEI == contact.EMEI);
                if (cimei.Count() > 0)
                {
                    logger.Info("Số EMEI đã đăng ký, vui lòng kiểm tra lại.");
                    return Json("Số EMEI đã đăng ký, vui lòng kiểm tra lại.");
                }
                //check 3 image
                string[] words = contact.FileUpload.Split(';');
                if (words.Count() != 3)
                {
                    logger.Info("Tải lên hình ảnh hóa đơn mua sản phẩm, Mặt trước CMND/CCCC, Hình ảnh Số serial.");
                    return Json("Tải lên hình ảnh hóa đơn mua sản phẩm, Mặt trước CMND/CCCC, Hình ảnh Số serial.");
                }

                contact.Createdate = DateTime.Now;
                db.Contacts.Add(contact);
                db.SaveChanges();
                return Json(1);
            }
            catch (Exception ex)
            {
                logger.Info("Error occurred. Error details: " + ex.Message);
                return Json("Error occurred. Error details: " + ex.Message);
            }

        }
        [HttpPost]
        public ActionResult UploadImage()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    string url = "";
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];

                        string fname;
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        var rand = new Random();
                        string strrand = rand.Next(0, 999).ToString();
                        fname = strrand + "_" + fname;


                        var uploadRootFolderInput = AppDomain.CurrentDomain.BaseDirectory + "\\Uploads";
                        Directory.CreateDirectory(uploadRootFolderInput);
                        var directoryFullPathInput = uploadRootFolderInput;
                        string pathfname = Path.Combine(directoryFullPathInput, fname);
                        file.SaveAs(pathfname);
                        if (i > 0)
                        {
                            url = url + ";" + "/Uploads/" + fname;
                        }
                        else
                        {
                            url = "/Uploads/" + fname;
                        }
                        logger.Info(url);
                    }
                    return Json(url);
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

    }
}