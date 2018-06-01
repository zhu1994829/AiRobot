using MX.AIRobot.Interface;
using MX.AIRobot.Log;
using MX.AIRobot.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MX.AIRobot.Util
{
    public class MailHelper
    {
        //#region 用户注册激活
        ///// <summary>
        ///// 用户注册激活
        ///// </summary>
        ///// <param name="email">接收人的邮箱地址</param>
        ///// <param name="contact">联系人</param>
        ///// <param name="activeUrl">激活地址</param>
        ///// <param name="type">邮件类型</param>
        //public static void RegisterActive(string email, string contact, string activeUrl, string type = ConstClass.LanguageCn)
        //{
        //    MailService service = new MailService();
        //    string title=string.Empty;
        //    string content = string.Empty;
        //    if (type == ConstClass.LanguageCn)
        //    {
        //        title = getEmailBody("RegisterActive", "title");
        //        content = getEmailBody("RegisterActive", "DSSql").Replace("@TragetName", contact).Replace("@ActiveUrl", activeUrl);
        //    }
        //    else 
        //    {
        //        title = getEmailBody("RegisterActive_en", "title");
        //        content = getEmailBody("RegisterActive_en", "DSSql").Replace("@TragetName", contact).Replace("@ActiveUrl", activeUrl);
        //    }
        //    service.To = email;
        //    service.Title = title;
        //    service.Body = content;
        //    service.SendMail();
        //}

        ///// <summary>
        ///// 找回密码
        ///// </summary>
        ///// <param name="email">接收人的邮箱地址</param>
        ///// <param name="contact">联系人</param>
        ///// <param name="newpwd">新密码</param>
        ///// <param name="type">邮件类型</param>
        //public static void FindPassWord(string email, string contact, string updatePwdUrl, string type = ConstClass.LanguageCn)
        //{
        //    MailService service = new MailService();
        //    string title=string.Empty;
        //    string content = string.Empty;
        //    if (type == ConstClass.LanguageCn)
        //    {
        //        title = getEmailBody("FindPwd", "title");
        //        content = getEmailBody("FindPwd", "DSSql").Replace("@TragetName", contact).Replace("@updatePwdUrl", updatePwdUrl);
        //    }
        //    else 
        //    {
        //        title = getEmailBody("FindPwd_en", "title");
        //        content = getEmailBody("FindPwd_en", "DSSql").Replace("@TragetName", contact).Replace("@updatePwdUrl", updatePwdUrl);
        //    }
        //    service.To = email;
        //    service.Title = title;
        //    service.Body = content;
        //    service.SendMail();
        //}
        //#endregion

        #region 审核
        public static void VerifyUser(string email, string contact, string activeUrl, string reason)
        {
            MailService service = new MailService();
            string title = string.Empty;
            string content = string.Empty;
            if (string.IsNullOrEmpty(reason))
            {
                title = getEmailBody("VerifyTrue", "title");
                content = getEmailBody("VerifyTrue", "DSSql").Replace("@TragetName", contact).Replace("@ActiveUrl", activeUrl);
            }
            else
            {
                title = getEmailBody("VerifyFalse", "title");
                content = getEmailBody("VerifyFalse", "DSSql").Replace("@TragetName", contact).Replace("@ActiveUrl", activeUrl).Replace("@Reason", reason);
            }
            service.To = email;
            service.Title = title;
            service.Body = content;
            service.SendMail();
        }
        #endregion

        #region 公用方法
        /// <summary>
        /// 返回节点内容
        /// </summary>
        /// <param name="xpath">根节点id</param>
        /// <param name="name">子节点名</param>
        /// <returns>返回节点内容</returns>
        private static string getEmailBody(string xpath, string name)
        {
            try
            {
                XDocument doc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + PageHelper.MailTemplateUrl);

                IEnumerable<XElement> filters = doc.Root.Elements("FILTERS").Elements("FILTER");

                if (filters.Count(s => string.Compare(s.Attribute("id").Value, xpath, true) == 0) == 0)
                {
                    throw new ArgumentException("SearchConfigID");
                }

                //加载ID所对应的FITER SQL
                XElement filter = filters.First(s => string.Compare(s.Attribute("id").Value, xpath, true) == 0);
                return filter.Element(name).Value;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
    }

    public class MailService
    {
        ILogger log = new Logger();

        private string MailName = PageHelper.MailName;       //发送方用户名       
        private string MailPwd = PageHelper.MailPwd;         //发送方密码
        private string Smtp = PageHelper.MailSmtp;           //发送方主机名称或IP
        private string FromName = PageHelper.MailFromName;     //发件人姓名
        private MailMessage mailMessage;
        private SmtpClient smtpClient;

        /// <summary>
        /// 邮件发送地址
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// 邮件抄送地址
        /// </summary>
        public string CC { get; set; }
        /// <summary>
        /// 邮件内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 邮件标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 附件地址
        /// </summary>
        public string AttachmentPath { get; set; }

        #region 内部方法

        /// <summary>
        /// 构造消息体
        /// </summary>
        private void Message()
        {
            mailMessage = new MailMessage();
            To = To.Replace(';', ',');
            mailMessage.To.Add(To); //可添加多个收件人
            if (!string.IsNullOrEmpty(CC))
            {
                CC = CC.Replace(';', ',');
                mailMessage.CC.Add(CC); //可添加多个抄送人
            }
            mailMessage.From = new System.Net.Mail.MailAddress(MailName, this.FromName);
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            mailMessage.Subject = Title;
            mailMessage.Body = Body;
            mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
        }

        /// <summary>  
        /// 添加附件  
        /// </summary>  
        private void Attachments()
        {
            if (!string.IsNullOrEmpty(AttachmentPath))
            {
                string[] attachmentPathpath = AttachmentPath.Split(',');
                Attachment data;
                ContentDisposition disposition;
                for (int i = 0; i < attachmentPathpath.Length; i++)
                {
                    data = new Attachment(attachmentPathpath[i], MediaTypeNames.Application.Octet);//实例化附件  
                    data.Name = Path.GetFileNameWithoutExtension(data.Name).Split('+')[0].ToString() + ".pdf";
                    disposition = data.ContentDisposition;
                    mailMessage.Attachments.Add(data);//添加到附件中  
                }
            }
        }

        /// <summary>
        /// 同步发送邮件
        /// </summary>
        /// <returns></returns>
        private void Send()
        {
            smtpClient = new SmtpClient();
            smtpClient.Credentials = new System.Net.NetworkCredential(mailMessage.From.Address, MailPwd);//设置发件人身份的票据  
            smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            smtpClient.Host = Smtp;//"smtp.teipr.com";
            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                log.WriteLog(LogLevel.Fatal, "邮件发送失败！" + "\n" + "技术信息:\n" + ex.Message + "致命的错误");
            }
        }

        /// <summary>  
        /// 异步发送邮件  
        /// </summary>   
        /// <param name="CompletedMethod"></param>  
        /// <remarks>异步发送邮件必须要将页面的Async设置为true，让页面支持页面操作</remarks>
        private void SendAsync(SendCompletedEventHandler CompletedMethod)
        {
            if (mailMessage != null)
            {
                smtpClient = new SmtpClient();
                smtpClient.Credentials = new System.Net.NetworkCredential(mailMessage.From.Address, MailPwd);//设置发件人身份的票据  
                smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtpClient.Host = Smtp;
                smtpClient.SendCompleted += new SendCompletedEventHandler(CompletedMethod);//注册异步发送邮件完成时的事件 
                try
                {
                    smtpClient.SendAsync(mailMessage, mailMessage.Body);
                }
                catch (System.Net.Mail.SmtpException ex)
                {
                    log.WriteLog(LogLevel.Fatal, "邮件发送失败！" + "\n" + "技术信息:\n" + ex.Message + "致命的错误");
                }
            }
        }

        #region 发送邮件后所处理的函数

        public void client_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    log.WriteLog(LogLevel.Warn, "发送已取消！");
                }
                if (e.Error != null)
                {
                    log.WriteLog(LogLevel.Error, "邮件发送失败！" + "\n" + "技术信息:\n" + e.ToString() + "错误");
                }
                else
                {
                    log.WriteLog(LogLevel.Info, "邮件成功发出! 恭喜!");
                }
            }
            catch (Exception Ex)
            {
                log.WriteLog(LogLevel.Fatal, "邮件发送失败！" + "\n" + "技术信息:\n" + Ex.Message + "致命的错误");
                throw new Exception("邮件发送失败！" + "\n" + "技术信息:\n" + Ex.Message + "致命的错误");
            }

        }
        #endregion

        #endregion

        #region 外部调用接口

        /// <summary>
        /// 发送(同步)
        /// </summary>
        /// <param name="To">邮件发送地址</param>
        /// <param name="CC">邮件抄送地址</param>
        /// <param name="AttachmentPath">附件路径</param>
        public void SendMail()
        {
            if (PageHelper.MailSwitch)
            {
                this.Message();
                this.Attachments();
                this.Send();
            }
        }

        /// <summary>
        /// 发送(异步)
        /// </summary>
        /// <param name="To">邮件发送地址</param>
        /// <param name="CC">邮件抄送地址</param>
        /// <param name="AttachmentPath">附件路径</param>
        /// <remarks>异步发送邮件必须要将页面的Async设置为true，让页面支持页面操作</remarks>
        public void SendAsyncMail()
        {
            if (PageHelper.MailSwitch)
            {
                this.Message();
                this.Attachments();
                this.SendAsync(client_SendCompleted);
            }
        }

        #endregion


    }

}
