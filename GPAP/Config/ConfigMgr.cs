using GPAP.Config.SoftwareManager;
using GPAP.Config.HardwareManager;
using GPAP.Instrument;
using GPAP.WorkFlow;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using GPAP.Classes;

namespace GPAP.Config
{
    public enum EnumConfigType
    {
        HardwareCfg,
        SoftwareCfg,
        SystemParaCfg,
        UserCfg,
    }
    public class ConfigMgr
    {
        private ConfigMgr()
        {

        }
        private static readonly Lazy<ConfigMgr> _instance = new Lazy<ConfigMgr>(() => new ConfigMgr());
        public static ConfigMgr Instance
        {
            get { return _instance.Value; }
        }
        private readonly string File_HardwareCfg = FileHelper.GetCurFilePathString() + "Config\\HardwareCfg.json";
        private readonly string File_SoftwareCfg = FileHelper.GetCurFilePathString() + "Config\\SoftwareCfg.json";
        private readonly string File_SystemParaCfg = FileHelper.GetCurFilePathString() + "Config\\SystemParaCfg.json";
        private readonly string File_UserCfg = FileHelper.GetCurFilePathString() + "User.json";

        public  HardwareCfgManager HardwareCfgMgr = null;
        public  SoftwareCfgManager SoftwareCfgMgr = null;



        //public static 
        public void LoadConfig(out List<string> errList)
        {
            #region >>>>Hardware init
            errList = new List<string>();
            try
            {
                var json_string = File.ReadAllText(File_HardwareCfg);
                HardwareCfgMgr = JsonConvert.DeserializeObject<HardwareCfgManager>(json_string);
            }
            catch (Exception ex)
            {
                errList.Add($"Unable to load config file { File_HardwareCfg}, { ex.Message}");
            }

            InstrumentBase instrumentBase = null;


            Type hardWareMgrType = HardwareCfgMgr.GetType();
            foreach (var it in hardWareMgrType.GetProperties())
            {
                switch (it.Name)
                {
                    case "Instruments":
                        var instrumentCfgs = it.GetValue(HardwareCfgMgr) as InstrumentCfg[];
                        if (instrumentCfgs == null)
                            break;
                        foreach (var instrumentCfg in instrumentCfgs)
                        {
                            if (instrumentCfg.Enabled)
                            {
                                instrumentBase = hardWareMgrType.Assembly.CreateInstance("JPT_TosaTest.Instruments." + instrumentCfg.InstrumentName.Substring(0, instrumentCfg.InstrumentName.IndexOf("[")), true, BindingFlags.CreateInstance, null, null, null, null) as InstrumentBase;
                                if (instrumentBase != null)
                                {
                                    if (instrumentBase.Init())
                                    {

                                    }
                                }
                            }
                        }
                        break;
                    case "IOCards":
                    case "MotonCards":
                    case "Cameras":
                    case "Lights":
                    case "Comports":
                    case "Ethernets":
                    case "Gpibs":
                    case "Visas":
                        break;
                    default:
                        errList.Add("Invalid hardware type!");
                        break;

                }
            }

            #endregion

            #region >>>>Software init
            try
            {
                var json_string = File.ReadAllText(File_SoftwareCfg);
                SoftwareCfgMgr = JsonConvert.DeserializeObject<SoftwareCfgManager>(json_string);
            }
            catch (Exception ex)
            {
                errList.Add(String.Format("Unable to load config file {0}, {1}", File_SoftwareCfg, ex.Message));
            }

            Type tStationCfg = SoftwareCfgMgr.GetType();
            PropertyInfo[] pis = tStationCfg.GetProperties();
            WorkFlowConfig[] WorkFlowCfgs = null;
            WorkFlowBase workFlowBase = null;
            foreach (PropertyInfo pi in pis)
            {
                if (pi.Name == "WorkFlowConfigs")
                {
                    WorkFlowCfgs = pi.GetValue(SoftwareCfgMgr) as WorkFlowConfig[];
                    foreach (var it in WorkFlowCfgs)
                    {
                        if (it.Enable)
                        {
                            workFlowBase = tStationCfg.Assembly.CreateInstance("GPAP.WorkFlow." + it.Name, true, BindingFlags.CreateInstance, null, new object[] { it }, null, null) as WorkFlowBase;
                            if (workFlowBase == null)
                                errList.Add($"Station: {it.Name} Create instance failed!");
                            else
                                WorkFlowMgr.Instance.AddStation(it.Name, workFlowBase);
                        }
                    }
                }
            }
            #endregion

        }
        public void SaveConfig(EnumConfigType cfgType, object[] listObj)
        {
            if (listObj == null)
                throw new Exception(string.Format("保存的{0}数据为空", cfgType.ToString()));
            string fileSaved = null;
            object objSaved = null;
            switch (cfgType)
            {
                case EnumConfigType.HardwareCfg:
                    //fileSaved = File_HardwareCfg;
                    //objSaved=new HardwareCfgManager() {  }
                    break; 
                case EnumConfigType.SoftwareCfg:
                    fileSaved = File_SoftwareCfg;
                    break;
                case EnumConfigType.SystemParaCfg:
  
                    break;
                case EnumConfigType.UserCfg:

                    break;
                default:
                    break;
            }
            string json_str = JsonConvert.SerializeObject(objSaved);
            File.WriteAllText(fileSaved, json_str);
        }
    }
}
