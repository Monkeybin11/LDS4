using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GPAP.Model;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using System;

namespace GPAP.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private int _errorCount = 0;
        private int _viewIndex = 0;
        private void ShowErrorinfo(string ErrorMsg)
        {
            if (!string.IsNullOrEmpty(ErrorMsg))
            {
                SystemErrorMessageCollection.Add(new MessageItem() { MsgType = EnumMessageType.Error, StrMsg = ErrorMsg });
            }
        }
        private List<string> ErrList = null;
        private object[] stationLock = new object[10];
        private int _progressValue1 = 0, _progressValue2 = 0, _progressValue3 = 0, _progressValue4 = 0;
        private void SystemErrorMessageCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var colls = from item in SystemErrorMessageCollection where item.MsgType == EnumMessageType.Error select item;
            if (colls != null)
                ErrorCount = colls.Count();
        }
        private void Value_OnStationInfoChanged1(int Index, string StationName, string Msg)
        {
            lock (stationLock[Index])
            {
                StationInfoCollection[Index] = $"{StationName}:{Msg}";
            }
        }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            SystemErrorMessageCollection = new ObservableCollection<MessageItem>();
            SystemErrorMessageCollection.CollectionChanged += SystemErrorMessageCollection_CollectionChanged;


            //加载配置文件
            Config.ConfigMgr.Instance.LoadConfig(out ErrList);
            StationInfoCollection = new ObservableCollection<string>();
            foreach (var stationCfg in Config.ConfigMgr.Instance.SoftwareCfgMgr.WorkFlowConfigs)
            {
                StationInfoCollection.Add(stationCfg.Name);
            }
            foreach (var station in WorkFlow.WorkFlowMgr.Instance.stationDic)
            {
                station.Value.OnStationInfoChanged += Value_OnStationInfoChanged1; ;
            }
            for (int i = 0; i < 10; i++)
                stationLock[i] = new object();

            StandardDdataCollection = new ObservableCollection<Point>();
            RealDataCollect1=new ObservableCollection<Point>();
            RealDataCollect2 = new ObservableCollection<Point>();
            RealDataCollect3 = new ObservableCollection<Point>();
            RealDataCollect4 = new ObservableCollection<Point>();

            TestResultDataCollect = new ObservableCollection<TestResultData>();
            for (int i = 0; i < 4; i++)
                TestResultDataCollect.Add(new TestResultData());


            Messenger.Default.Register<int>(this,"Progress",curValue=> {
                Application.Current.Dispatcher.Invoke(()=> {
                    int value = curValue & 0xFFFF;
                    int Index = (curValue >> 16) & 0xFFFF;
                    switch (Index)
                    {
                        case 1:
                            ProgressValue1 = value;
                            break;
                        case 2:
                            ProgressValue2 = value;
                            break;
                        case 3:
                            ProgressValue3 = value;
                            break;
                        case 4:
                            ProgressValue4 = value;
                            break;
                        default:
                            break;
                    }
                });
            });
            Messenger.Default.Register<object>(this, "Collection", o => {
                Application.Current.Dispatcher.Invoke(() => {
                    var tuple = o as Tuple<int, Point, Point,bool>;
                    int Index = tuple.Item1;
                    var StandardPt = tuple.Item2;
                    var RealPt = tuple.Item3;
                    bool bAdd = tuple.Item4;
                    switch (Index)
                    {
                        case 1:
                            if (!bAdd)
                                RealDataCollect1.Clear();
                            else
                                RealDataCollect1.Add(RealPt);
                            break;
                        case 2:
                            if (!bAdd)
                                RealDataCollect2.Clear();
                            else
                                RealDataCollect2.Add(RealPt);
                            break;
                        case 3:
                            if (!bAdd)
                                RealDataCollect3.Clear();
                            else
                                RealDataCollect3.Add(RealPt);
                            break;
                        case 4:
                            if (!bAdd)
                                RealDataCollect4.Clear();
                            else
                                RealDataCollect4.Add(RealPt);
                            break;         
                        default:
                            break;
                    }
                   

                });
            });
            Messenger.Default.Register<object>(this, "FinalResult", o => {
                Application.Current.Dispatcher.Invoke(() => {
                    var tuple = o as Tuple<int, string, string, string,string,bool>;
                    int Index = tuple.Item1;
                    var Speed = tuple.Item2;
                    var Score = tuple.Item3;
                    var Accuracy = tuple.Item4;
                    var FinalResultString = tuple.Item5;
                    var bAdd = tuple.Item6;
                    if (Index >= 1 && Index <= 4)
                    {
                        TestResultDataCollect[Index - 1].SpeedResult = Speed;
                        TestResultDataCollect[Index - 1].ScoreResult = Score;
                        TestResultDataCollect[Index - 1].AccuracyResult = Accuracy;
                        TestResultDataCollect[Index - 1].FinalResultString = FinalResultString;
                    }
                });
            });
        }
        #region Property
        public int ErrorCount
        {
            get { return _errorCount; }
            set
            {
                if (_errorCount != value)
                {
                    _errorCount = value;
                    RaisePropertyChanged();
                }
            }
        }
        public ObservableCollection<MessageItem> SystemErrorMessageCollection
        {
            get;
            set;
        }
        public int ViewIndex
        {
            get { return _viewIndex; }
            set
            {
                if (_viewIndex != value)
                {
                    _viewIndex = value;
                    RaisePropertyChanged();
                }
            }
        }
        public ObservableCollection<string> StationInfoCollection { get; set; }
        public int ProgressValue1
        {
            get { return _progressValue1; }
            set
            {
                if (_progressValue1 != value)
                {
                    _progressValue1 = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int ProgressValue2
        {
            get { return _progressValue2; }
            set
            {
                if (_progressValue2 != value)
                {
                    _progressValue2 = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int ProgressValue3
        {
            get { return _progressValue3; }
            set
            {
                if (_progressValue3 != value)
                {
                    _progressValue3 = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int ProgressValue4
        {
            get { return _progressValue4; }
            set
            {
                if (_progressValue4 != value)
                {
                    _progressValue4 = value;
                    RaisePropertyChanged();
                }
            }
        }
        public ObservableCollection<Point> StandardDdataCollection
        {
            get;
            set;
        }
        public ObservableCollection<Point> RealDataCollect1
        {
            get;
            set;
        }
        public ObservableCollection<Point> RealDataCollect2
        {
            get;
            set;
        }
        public ObservableCollection<Point> RealDataCollect3
        {
            get;
            set;
        }
        public ObservableCollection<Point> RealDataCollect4
        {
            get;
            set;
        }
        public ObservableCollection<TestResultData> TestResultDataCollect { get; set; }

        #endregion
        #region Command
        /// <summary>
        /// 主界面运行按钮
        /// </summary>
        public RelayCommand StartStationCommand
        {
            get { return new RelayCommand(() => WorkFlow.WorkFlowMgr.Instance.StartAllStation()); }
        }
        public RelayCommand StartStation1Command
        {
            get { return new RelayCommand(() => {
                var station=WorkFlow.WorkFlowMgr.Instance.FindStationByIndex(0);
                if (station != null)
                    station.Start();
            }); }
        }
        public RelayCommand StartStation2Command
        {
            get
            {
                return new RelayCommand(() => {
                    var station = WorkFlow.WorkFlowMgr.Instance.FindStationByIndex(1);
                    if (station != null)
                        station.Start();
                });
            }
        }
        public RelayCommand StartStation3Command
        {
            get
            {
                return new RelayCommand(() => {
                    var station = WorkFlow.WorkFlowMgr.Instance.FindStationByIndex(2);
                    if (station != null)
                        station.Start();
                });
            }
        }
        public RelayCommand StartStation4Command
        {
            get
            {
                return new RelayCommand(() => {
                    var station = WorkFlow.WorkFlowMgr.Instance.FindStationByIndex(3);
                    if (station != null)
                        station.Start();
                });
            }
        }
        public RelayCommand PauseStationCommand
        {
            get { return new RelayCommand(() => WorkFlow.WorkFlowMgr.Instance.PauseAllStation()); }
        }
        public RelayCommand StopStationCommand
        {
            get { return new RelayCommand(() => WorkFlow.WorkFlowMgr.Instance.StopAllStation()); }
        }
        public RelayCommand StopStation1Command
        {
            get
            {
                return new RelayCommand(() => {
                    var station = WorkFlow.WorkFlowMgr.Instance.FindStationByIndex(0);
                    if (station != null)
                        station.Stop();
                });
            }
        }
        public RelayCommand StopStation2Command
        {
            get
            {
                return new RelayCommand(() => {
                    var station = WorkFlow.WorkFlowMgr.Instance.FindStationByIndex(1);
                    if (station != null)
                        station.Stop();
                });
            }
        }
        public RelayCommand StopStation3Command
        {
            get
            {
                return new RelayCommand(() => {
                    var station = WorkFlow.WorkFlowMgr.Instance.FindStationByIndex(2);
                    if (station != null)
                        station.Stop();
                });
            }
        }
        public RelayCommand StopStation4Command
        {
            get
            {
                return new RelayCommand(() => {
                    var station = WorkFlow.WorkFlowMgr.Instance.FindStationByIndex(3);
                    if (station != null)
                        station.Stop();
                });
            }
        }
        /// <summary>
        /// 切换中英文
        /// </summary>
        public RelayCommand<string> SwitchLangCommand
        {
            get
            {
                return new RelayCommand<string>(strLang => {
                    string langFileNew = null;
                    switch (strLang)
                    {
                        case "CH":
                            langFileNew = "Lang_CH";
                            break;
                        case "EN":
                            langFileNew = "Lang_EN";
                            break;
                        default:
                            break;
                    }
                    var MergedDic = Application.Current.Resources.MergedDictionaries;
                    if (!string.IsNullOrEmpty(langFileNew))
                    {
                        foreach (ResourceDictionary dictionary in MergedDic)
                        {
                            if (dictionary.Source.OriginalString.Contains(langFileNew))
                            {
                                MergedDic.Remove(dictionary);
                                MergedDic.Add(dictionary);
                                break;
                            }
                        }
                    }
                });
            }
        }
        /// <summary>
        /// 窗口Load
        /// </summary>
        public RelayCommand WindowLoadedCommand
        {
            get
            {
                return new RelayCommand(() => {

                    foreach (var err in ErrList)
                    {
                        ShowErrorinfo(err);
                    }
                });
            }
        }

        /// <summary>
        /// 界面主菜单Home按钮
        /// </summary>
        public RelayCommand BtnHomeCommand
        {
            get { return new RelayCommand(() => ViewIndex = 0); }
        }

        /// <summary>
        /// 界面主菜单设置按钮
        /// </summary>
        public RelayCommand BtnSettingCommand
        {
            get { return new RelayCommand(() => ViewIndex = 1); }
        }

        /// <summary>
        /// 状态栏错误显示按钮
        /// </summary>
        public RelayCommand ShowInfoListCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ShowErrorinfo("Error Info");
                    ViewIndex = 2;
                });
            }
        }
        
        /// <summary>
        /// 弹出菜单清除错误菜单
        /// </summary>
        public RelayCommand ClearMessageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SystemErrorMessageCollection.Clear();
                });
            }
        }
        #endregion
    }
}