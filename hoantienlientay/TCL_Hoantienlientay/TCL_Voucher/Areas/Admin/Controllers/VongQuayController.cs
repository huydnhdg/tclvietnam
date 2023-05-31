using Newtonsoft.Json;
using NLog;
using OfficeOpenXml;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCL_Voucher.Models;

namespace TCL_Voucher.Areas.Admin.Controllers
{
    [Authorize]
    public class VongQuayController : Controller
    {
        TCL_VoucherEntities db = new TCL_VoucherEntities();
        Logger logger = LogManager.GetCurrentClassLogger();
        static IEnumerable<VongQuay> listModel = null;
        static string url = "";
        public ActionResult Index(int? page, string textSearch, string status, string start_date, string end_date, int pageSize = 10)
        {
            url = Request.Url.AbsoluteUri;
            var model = from a in db.VongQuays
                        select a;
            if (!string.IsNullOrEmpty(textSearch))
            {
                model = model.Where(a => a.Name.Contains(textSearch) || a.Phone.Contains(textSearch) || a.SERIAL.Contains(textSearch));
                ViewBag.textSearch = textSearch;
            }
            if (!string.IsNullOrEmpty(status))
            {
                int _status = int.Parse(status);
                model = model.Where(a => a.Status == _status);
            }
            if (!string.IsNullOrEmpty(start_date))
            {
                DateTime s = DateTime.Parse(start_date);
                model = model.Where(a => a.Createdate >= s);
                ViewBag.start_date = start_date;
            }
            if (!string.IsNullOrEmpty(end_date))
            {
                DateTime s = DateTime.Parse(end_date);
                s = s.AddDays(1);
                model = model.Where(a => a.Createdate <= s);
                ViewBag.end_date = end_date;
            }
            listModel = model as IEnumerable<VongQuay>;
            int pageNumber = (page ?? 1);
            return View(listModel.OrderByDescending(a => a.Createdate).ToPagedList(pageNumber, pageSize));
        }
        string domain = "http://sansetiendien.tcl.com";
        public void export_vongquay()
        {
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "STT";
            Sheet.Cells["B1"].Value = "Ngày tạo";
            Sheet.Cells["C1"].Value = "Số điện thoại";
            Sheet.Cells["D1"].Value = "Họ tên";
            Sheet.Cells["E1"].Value = "Thành phố";
            Sheet.Cells["F1"].Value = "Nơi mua hàng";
            Sheet.Cells["G1"].Value = "Sản phẩm";
            Sheet.Cells["H1"].Value = "Model";
            Sheet.Cells["I1"].Value = "Size";
            Sheet.Cells["J1"].Value = "Hóa đơn";
            Sheet.Cells["K1"].Value = "Giải thưởng";

            int index = 1;
            int row = 2;
            foreach (var item in listModel)
            {

                Sheet.Cells[string.Format("A{0}", row)].Value = index++;
                Sheet.Cells[string.Format("B{0}", row)].Value = (item.Createdate != null) ? item.Createdate.ToString("dd/MM/yyyy") : "";
                Sheet.Cells[string.Format("C{0}", row)].Value = item.Phone;
                Sheet.Cells[string.Format("D{0}", row)].Value = item.Name;
                Sheet.Cells[string.Format("E{0}", row)].Value = item.Province;
                Sheet.Cells[string.Format("F{0}", row)].Value = item.BuyAdr;
                Sheet.Cells[string.Format("G{0}", row)].Value = item.PRODUCT;
                Sheet.Cells[string.Format("H{0}", row)].Value = item.MODEL;
                Sheet.Cells[string.Format("I{0}", row)].Value = item.SIZE;
                Sheet.Cells[string.Format("J{0}", row)].Value = domain + item.INVOICE;
                Sheet.Cells[string.Format("K{0}", row)].Value = item.PAYMENT;

                row++;
            }


            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Report.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();
        }
        public ActionResult Delete(long Id)
        {
            try
            {
                var model = db.VongQuays.Find(Id);
                string json = JsonConvert.SerializeObject(model);
                logger.Info(string.Format("[Delete Vongquay] @Contact={0}", json));
                db.VongQuays.Remove(model);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            return Redirect(url);
        }
    }
}