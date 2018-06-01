using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MX.AIRobot.Util
{
    public class ReflexHelper
    {
        public static Dictionary<string, object> GetValueByModelProperty<T>(T model)
        {
            Dictionary<string, object> modelDic = new Dictionary<string, object>();
            var result = string.Empty;

            foreach (var p in model.GetType().GetProperties())
            {
                modelDic.Add(p.Name, p.GetValue(model));
            }
            return modelDic;
        }
    }
}
