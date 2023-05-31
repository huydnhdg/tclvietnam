using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCL_Voucher.Areas.Admin.Data;
using TCL_Voucher.Models;

namespace TCL_Voucher.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        TCL_VoucherEntities db = new TCL_VoucherEntities();
        public ActionResult Index(string str_date, string end_date, string current_str, string current_end)
        {
            if (str_date != null)
            {

            }
            else
            {
                str_date = current_str;
            }
            ViewBag.str_date = str_date;
            if (end_date != null)
            {

            }
            else
            {
                end_date = current_end;
            }
            ViewBag.end_date = end_date;

            var model = from a in db.Contacts
                        select a;

            if (!String.IsNullOrEmpty(str_date))
            {
                DateTime d = DateTime.Parse(str_date);
                model = model.Where(s => s.Createdate >= d);
            }
            if (!String.IsNullOrEmpty(end_date))
            {
                DateTime d = DateTime.Parse(end_date);
                model = model.Where(s => s.Createdate <= d);
            }
            model = model.OrderByDescending(a => a.Createdate);
            var contacts = new List<ContactView>();
            foreach (var item in model)
            {
                var contact = new ContactView();
                contact.Name = item.Name;
                contact.Phone = item.Phone;
                contact.BuyAdr = item.BuyAdr;
                contact.Province = item.Province;
                contact.Product = item.Product;
                contact.EMEI = item.EMEI;
                contact.Model = item.Model;
                contact.Createdate = item.Createdate;
                var word = item.FileUpload.Split(';');
                if (word.Count() == 3)
                {
                    contact.image1 = word[0];
                    contact.image2 = word[1];
                    contact.image3 = word[2];
                }
                else
                {
                    contact.image1 = word[0];
                }
                contacts.Add(contact);
            }
            return View(contacts);
        }

    }
}