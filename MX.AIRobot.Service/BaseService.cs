using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MX.AIRobot.Model;
using MX.AIRobot.AOP;
using MX.AIRobot.Interface;
using MX.AIRobot.Log;
using PetaPoco;

namespace MX.AIRobot.Service
{
   public abstract class BaseService
    {
        public Database db = new Database("AIRobotConnect");
        protected ILogger log = new Logger();
    }
}
