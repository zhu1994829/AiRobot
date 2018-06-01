using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MX.AIRobot.Interface
{

    /// <summary>
    /// 权限控制接口
    /// </summary>
    public interface IAuth
    {

        bool VerifyPermission();
    }
}
