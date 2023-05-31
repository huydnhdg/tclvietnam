using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCL_Voucher.Utils
{
    public class SmsTemp
    {
        public static string SYNTAX_INVALID(string chanel)
        {
            string mtReturn = "TIN NHAN SAI CU PHAP. VUI LONG SOAN LAI THEO: \"TCLKM IMEI TEN NAM SINH\" GUI 8077. CHI TIET LH 028 3836 6111 (EXT 498) HOAC http://manhinhsieulon.tcl.com";
            return mtReturn;
        }

        public static string IMEI_INVALID(string chanel)
        {
            string mtReturn = "SO IMEI DA DANG KI HOAC NHAP SAI. VUI LONG KIEM TRA LAI SO IMEI VA SOAN: \"TCLKM IMEI TEN NAM SINH\" GUI 8077.LH 1800 588 880 HOAC http://manhinhsieulon.tcl.com";
            return mtReturn;
        }

        public static string WIN_REWARD_1st(string chanel, string code)
        {
            string mtReturn = "TCL CHUC MUNG QUY KHACH CO MA DU THUONG {0} DA TRUNG 1 BIEU TUONG VANG 24K TU CT \"MAN HINH SIEU LON - QUA SIEU TO\". VUI LONG CUNG CAP HINH ANH DE LAM THU TUC NHAN THUONG: CMND, HOA DON, SO EMEI QUA HOP THU ngan.langngockim@tclvn.com.vn. CHI TIET LH 028 3836 6111 (EXT 498) DE DUOC HUONG DAN NHAN THUONG. XEM http://manhinhsieulon.tcl.com";
            mtReturn = string.Format(mtReturn, code);
            return mtReturn;
        }

        public static string WIN_REWARD_2nd(string chanel, string code)
        {
            string mtReturn = "TCL CHUC MUNG QUY KHACH CO MA DU THUONG {0} DA TRUNG 1 DONG TIEN VANG 1 CHI TU CT \"MAN HINH SIEU LON - QUA SIEU TO\". VUI LONG CUNG CAP HINH ANH DE LAM THU TUC NHAN THUONG: CMND, HOA DON, SO EMEI QUA HOP THU ngan.langngockim@tclvn.com.vn. CHI TIET LH 028 3836 6111 (EXT 498) DE DUOC HUONG DAN NHAN THUONG. XEM http://manhinhsieulon.tcl.com";
            mtReturn = string.Format(mtReturn, code);
            return mtReturn;
        }

        public static string WIN_REWARD_3rd(string chanel, string code)
        {
            string mtReturn = "TCL CHUC MUNG QUY KHACH CO MA DU THUONG {0} DA TRUNG 500,000 VND THE NAP DIEN THOAI TU CT \"MAN HINH SIEU LON - QUA SIEU TO\". VUI LONG CUNG CAP HINH ANH DE LAM THU TUC NHAN THUONG: CMND, HOA DON, SO EMEI QUA HOP THU ngan.langngockim@tclvn.com.vn. CHI TIET LH 028 3836 6111 (EXT 498) DE DUOC HUONG DAN NHAN THUONG. XEM http://manhinhsieulon.tcl.com";
            mtReturn = string.Format(mtReturn, code);
            return mtReturn;
        }

        public static string LOSER(string chanel, string code)
        {
            string mtReturn = "TCL THONG BAO CHUC QUY KHACH CO MA DU THUONG {0} MAY MAN LAN SAU. CHI TIET LH 1800 588 880 HOAC 028 3836 6111 (EXT 498), XEM http://manhinhsieulon.tcl.com";
            mtReturn = string.Format(mtReturn, code);
            return mtReturn;
        }
    }
}