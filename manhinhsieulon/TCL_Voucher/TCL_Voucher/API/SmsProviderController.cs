using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using TCL_Voucher.Models;
using TCL_Voucher.Utils;

namespace TCL_Voucher.API
{
    [RoutePrefix("api/sms")]
    public class SmsProviderController : ApiController
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        TCL_VoucherEntities db = new TCL_VoucherEntities();

        [Route("receive")]
        [HttpGet]
        public HttpResponseMessage Receive(string Command_Code, string User_ID, string Service_ID, string Request_ID, string Message)
        {
            logger.Info(string.Format("[MO] @Command_Code= {0} @User_ID= {1} @Service_ID= {2} @Request_ID= {3} @Message= {4}", Command_Code, User_ID, Service_ID, Request_ID, Message));
            string mtReturn = "";
            string chanel = "SMS";
            // Check cú pháp tin nhắn
            string[] Words = Message.Split(' ');

            // Format định dạng 09xxx
            User_ID = MyControl.formatUserId(User_ID, 2);

            if (Words.Length > 3)
            {
                // Xử lý tin nhắn
                string Serial = Words[1].ToUpper();

                if (Serial.Length == 19 || Serial.Length == 20 || Serial.Length == 26)
                {
                    // IMEI đúng thì xử lý
                    // var check = db.Contacts.Where(a => a.Serial.Equals(Serial));
                    var check = db.Contacts.Where(a => a.Serial.ToUpper().Equals(Serial));
                    // Check trong Blacklist hệ thống
                    var blacklist = db.Blacklists.Where(a => a.IMEI.ToUpper().Equals(Serial));

                    if (check.SingleOrDefault() != null || blacklist.Count() > 0)//serial nay da dang ky nhan thuong
                    {
                        mtReturn = SmsTemp.IMEI_INVALID(chanel);
                    }
                    else
                    {

                        //lay ra 1 ma du thuong chua tick
                        int type = 0;
                        var model = new Maduthuong();

                        //chan 1 sdt chi duoc trung thuong 2 lan
                        //var datrungthuong = check.Where(a => a.Type != null);
                        //if (datrungthuong.Count() >= 2)
                        var datrungthuong = db.Contacts.Where(a => a.Type > 0 && a.Phone == User_ID);
                        if (datrungthuong.Count() > 1)
                        {
                            var makhongtrung = db.Maduthuongs.Where(a => a.Status == null && a.Type == null).OrderBy(a => Guid.NewGuid()).FirstOrDefault();
                            model = makhongtrung;
                        }
                        else
                        {
                            // Đoạn này fix giải nhất
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
                                // Tìm một mã dự thưởng bất kỳ
                                var maduthuong = db.Maduthuongs.Where(a => a.Status == null).OrderBy(a => Guid.NewGuid()).FirstOrDefault();
                                type = maduthuong.Type ?? default(int);
                                model = maduthuong;
                                logger.Info("Quay giải thưởng : " + type);
                            }

                        }

                        if (!string.IsNullOrEmpty(model.Code))
                        {
                            // Trúng giải thưởng thì Insert vào DB
                            switch (model.Type)
                            {
                                case 1:
                                    mtReturn = SmsTemp.WIN_REWARD_1st(chanel, model.Code);
                                    break;
                                case 2:
                                    mtReturn = SmsTemp.WIN_REWARD_2nd(chanel, model.Code);
                                    break;
                                case 3:
                                    mtReturn = SmsTemp.WIN_REWARD_3rd(chanel, model.Code);
                                    break;
                                default:
                                    mtReturn = SmsTemp.LOSER(chanel, model.Code);
                                    break;
                            }

                            // Đưa thông tin vào bảng Contact
                            var contact = new Contact()
                            {
                                Name = Words[2],
                                Type = model.Type,
                                Createdate = DateTime.Now,
                                Maduthuong = model.Code, // Mã dự thưởng
                                Phone = User_ID,
                                Serial = Serial,
                                Model = chanel,
                                Product = chanel
                            };
                            db.Contacts.Add(contact);

                            //gach ma trong danh sach du thuong
                            model.Status = 1;
                            model.Activedate = DateTime.Now;
                            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            //save data
                            db.SaveChanges();
                        }
                        else
                        {
                            mtReturn = SmsTemp.IMEI_INVALID(chanel);
                        }
                    }
                }
                else
                {
                    mtReturn = SmsTemp.IMEI_INVALID(chanel);
                }
            }
            else
            {
                mtReturn = SmsTemp.SYNTAX_INVALID(chanel);
            }
            // Trả tin nhắn cho khách hàng.
            var result = new Result()
            {
                status = "0",
                message = mtReturn,
                phone = User_ID
            };
            logger.Info(string.Format("[MT] @Command_Code= {0} @User_ID= {1} @Service_ID= {2} @Request_ID= {3} @Message= {4}", Command_Code, User_ID, Service_ID, Request_ID, mtReturn));
            return ResponseMessage(result);
        }
        private HttpResponseMessage ResponseMessage(Result result)
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");
            return response;
        }
    }
}