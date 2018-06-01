using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MX.AIRobot.Model;

namespace MX.AIRobot.Util
{
    public static class SessionHelper
    {
        #region session存储
        /// <summary>
        /// 登录对象
        /// </summary>
        /// <param name="session"></param>
        /// <param name="User"></param>
        //public static void SetUser(this HttpSessionStateBase session, DicUser User)
        //{
        //    session[SessionKeys.User] = User;
        //}

        //public static DicUser GetUser(this HttpSessionStateBase session)
        //{
        //    if (session != null)
        //        return session[SessionKeys.User] == null ? null : (DicUser)session[SessionKeys.User];
        //    else
        //        return null;
        //}

        /// <summary>
        /// 验证码
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static string VerifyCode(this HttpSessionStateBase session)
        {
            return session[SessionKeys.VerifyCode] == null ? string.Empty : (string)session[SessionKeys.VerifyCode];
        }

        public static void SetVerifyCode(this HttpSessionStateBase session, string VerifyCode)
        {
            session[SessionKeys.VerifyCode] = VerifyCode;
        }

        /// <summary>
        /// 字典管理返回标识
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static string GetDictionaryTag(this HttpSessionStateBase session)
        {
            return session[SessionKeys.DictionaryTag] == null ? string.Empty : (string)session[SessionKeys.DictionaryTag];
        }

        public static void SetDictionaryTag(this HttpSessionStateBase session, string DictionaryTag)
        {
            session[SessionKeys.DictionaryTag] = DictionaryTag;
        }

        /// <summary>
        /// 信息发布管理返回标识
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static string GetNewsTag(this HttpSessionStateBase session)
        {
            return session[SessionKeys.NewsTag] == null ? string.Empty : (string)session[SessionKeys.NewsTag];
        }

        public static void SetNewsTag(this HttpSessionStateBase session, string NewsTag)
        {
            session[SessionKeys.NewsTag] = NewsTag;
        }

        /// <summary>
        /// 双语切换
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static string GetLanguage(this HttpSessionStateBase session)
        {
            return session[SessionKeys.Language] == null ? string.Empty : (string)session[SessionKeys.Language];
        }

        public static void SetLanguage(this HttpSessionStateBase session, string NewsTag)
        {
            session[SessionKeys.Language] = NewsTag;
        }

        #endregion

        #region 获取当前会话的用户名
        //public static string GetShowName(this HttpSessionStateBase session)
        //{
        //    var result = string.Empty;
        //    var user = session.GetUser();
        //    if (user != null)
        //    {
        //        result = user.UserName;
        //    }
        //    return result;
        //}
        #endregion

        //#region 系统中用于判断当前用户权限
        ///// <summary>
        ///// （管理员、个人用户、企业用户)
        ///// </summary>
        ///// <returns></returns>
        //public static bool AuthAPC(this HttpSessionStateBase Session)
        //{
        //    return (Session.GetUser().RoleObj.RoleID == ConstClass.UserRoleEnterprise || Session.GetUser().RoleObj.RoleID == ConstClass.UserRoleEnterprise || Session.GetUser().RoleObj.RoleID == ConstClass.UserRoleAdmin) ? true : false;
        //}
        ///// <summary>
        ///// （个人用户、企业用户)
        ///// </summary>
        ///// <returns></returns>
        //public static bool AuthPC(this HttpSessionStateBase Session)
        //{
        //    return (Session.GetUser().RoleObj.RoleID == ConstClass.UserRoleEnterprise) ? true : false;
        //}
        ///// <summary>
        ///// （知产服务用户)
        ///// </summary>
        ///// <returns></returns>
        //public static bool AuthIP(this HttpSessionStateBase Session)
        //{
        //    return (Session.GetUser().RoleObj.RoleID == ConstClass.UserRoleIPService) ? true : false;
        //}
        ///// <summary>
        ///// （管理员、超级管理员)
        ///// </summary>
        ///// <returns></returns>
        //public static bool AuthAdmin(this HttpSessionStateBase Session)
        //{
        //    return (Session.GetUser().RoleObj.RoleID == ConstClass.UserRoleAdmin) ? true : false;
        //}
        //#endregion
    }

    public static class SessionKeys
    {
        public const string User = "User";
        /// <summary>
        /// 验证码
        /// </summary>
        public const string VerifyCode = "VerifyCode";
        /// <summary>
        /// 字典管理返回标识
        /// </summary>
        public const string DictionaryTag = "DictionaryTag";
        /// <summary>
        /// 信息发布管理返回标识
        /// </summary>
        public const string NewsTag = "NewsTag";
        /// <summary>
        /// 双语切换
        /// </summary>
        public const string Language = "Language";
    }


}
