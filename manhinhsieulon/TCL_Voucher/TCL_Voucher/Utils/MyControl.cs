﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCL_Voucher.Utils
{
    public class MyControl
    {
        public static String formatUserId(String userId, int formatType)
        {
            //bool check = IsPhoneNumber(userId);
            //if (userId == null || "".Equals(userId) || check == false)
            //{
            //    return null;
            //}
            String temp = userId;
            switch (formatType)
            {
                case 0://Constants.USERID_FORMAT_INTERNATIONAL:
                    if ((temp.StartsWith("9") || temp.StartsWith("8") || temp.StartsWith("7") || temp.StartsWith("5") || temp.StartsWith("3")) && temp.Length == 9)
                    {
                        temp = "84" + temp;
                    }
                    else if (temp.StartsWith("1") && temp.Length == 10)
                    {
                        temp = "84" + temp;
                    }
                    else if (temp.StartsWith("0"))
                    {
                        temp = "84" + temp.Substring(1);
                    } // els  
                    break;
                case 1://Constants.USERID_FORMAT_NATIONAL_NINE:
                    if (temp.StartsWith("84"))
                    {
                        temp = temp.Substring(2);
                    }
                    else if (temp.StartsWith("0"))
                    {
                        temp = temp.Substring(1);
                    } // else startsWith("9")
                    break;
                case 2://Constants.USERID_FORMAT_NATIONAL_ZERO:
                    if (temp.StartsWith("84"))
                    {
                        temp = "0" + temp.Substring(2);
                    }
                    else if (!temp.StartsWith("0"))
                    {
                        temp = "0" + temp;
                    } // else startsWith("09")
                    break;
                default:

                    return null;
            }
            return temp;
        }
    }
}