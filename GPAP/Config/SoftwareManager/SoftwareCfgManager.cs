using GPAP.Config.SoftwareManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPAP.Config.SoftwareManager
{
    public class SoftwareCfgManager
    {
        public WorkFlowConfig[] WorkFlowConfigs { get; set; }
        public IoNameCfg[] IONames { get; set; }
    }
}
