using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using MX.AIRobot.ApiModel;
using System.Text.RegularExpressions;
using System.Configuration;
using MX.AIRobot.Log;
using System.Net;
using System.IO;
using MX.AIRobot.Model;

namespace MX.AIRobot.Util
{
   public class RegisterHelper
    {
       static string gdjRegister = "http://36.110.100.199/portal-ps-rail/user/userSignUp";
       static string gdjUpdate = "http://36.110.100.199/portal-ps- rail/user/ userUpdate";

       //public static RegisterReuslt Register(string userName, string passWord, string RoleID)
       //{
       //    var result = new RegisterReuslt();
       //    Hashtable ht = new Hashtable();
       //    ht.Add("username", userName);
       //    ht.Add("password", passWord);
       //    ht.Add("level", RoleLevel(RoleID));
       //    var doc = HttpClientHelper.QueryPostWebService(gdjRegister, ht);
       //    result = JsonHelper.JsonToObject<RegisterReuslt>(doc);

       //    return result;
       //}

       //public static RegisterReuslt Update(string userName, string oldPassword, string newPassword, string RoleID)
       //{
       //    {
       //        var result = new RegisterReuslt();
       //        Hashtable ht = new Hashtable();
       //        ht.Add("username", userName);
       //        ht.Add("oldPassword", oldPassword);
       //        ht.Add("newPassword", newPassword);
       //        ht.Add("level", RoleLevel(RoleID));
       //        var doc = HttpClientHelper.QueryPostWebService(gdjRegister, ht);
       //        result = JsonHelper.JsonToObject<RegisterReuslt>(doc);

       //        return result;
       //    }
       //}
       //public static string RoleLevel(string RoleID)
       //{
       //    string result = string.Empty;
       //    switch (RoleID)
       //    {
       //        case ConstClass.UserRoleAdmin:
       //            result = "ADMIN";
       //            break;
       //        case ConstClass.UserRoleRegulation:
       //            result = "ADMIN";
       //            break;
       //        default:
       //            result = "GUEST";
       //            break;
       //    }

       //    return result;
       //}
    }
}
