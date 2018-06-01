using MX.AIRobot.Interface;
using PetaPoco;
using MX.AIRobot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.SessionState;
using System.Web;
using MX.AIRobot.Util;

namespace MX.AIRobot.AOP
{
    public class Auth : IAuth
    {
        #region 公开成员

        public Database db = new Database("RTCIPConnect");

        public string RoleID { get; set; }
        public string CurrentPage { get; set; }

        /// <summary>
        /// 页面元素控制
        /// </summary>
        public IEnumerable<string> Elements { get; set; }

        #endregion

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <returns></returns>
        public bool VerifyPermission()
        {
            //var list = ResourceCache.FieldConversion(RoleID);

            //if (list.Count(c => c.ResourceObject.ResourceUrl == CurrentPage || c.ResourceObject.ResourceBackUpUrl == CurrentPage) > 0)
            //{
            //    Elements = list.Select(s => s.ResourceObject.ResourceUrl ?? s.ResourceObject.ResourceBackUpUrl);
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return false;
        }
    }

    public class AuthException
    {
        public static void AuthExceptionHandler()
        {
            HttpSessionState _session = HttpContext.Current.Session;
            //if (_session[SessionKeys.User] == null)
            //{
            //    throw new Exception("您登录已超时,请重新登录。");
            //}
            //else
            //{
            //    throw new Exception("您没有权限操作,请联系管理员。");
            //}
        }
    }
}
