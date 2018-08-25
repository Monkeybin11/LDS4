using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GPAP.UserCtrl
{
    /// <summary>
    /// UC_TestUnit.xaml 的交互逻辑
    /// </summary>
    public partial class UC_TestUnit : UserControl
    {
        public UC_TestUnit()
        {
            InitializeComponent();
        }

        #region UI 当前进度  标准数据源， 测试数据源， 命令
        public const string ProgressValuePropertyName = "ProgressValue";
        public int ProgressValue
        {
            get
            {
                return (int)GetValue(ProgressValueProperty);
            }
            set
            {
                SetValue(ProgressValueProperty, value);
            }
        }
        public static readonly DependencyProperty ProgressValueProperty = DependencyProperty.Register(ProgressValuePropertyName, typeof(int), typeof(UC_TestUnit));


        public const string StandardDataSourcePropertyName = "StandardDataSource";
        public ObservableCollection<Point> StandardDataSource
        {
            get
            {
                return (ObservableCollection<Point>)GetValue(StandardDataSourceProperty);
            }
            set
            {
                SetValue(StandardDataSourceProperty, value);
            }
        }
        public static readonly DependencyProperty StandardDataSourceProperty = DependencyProperty.Register(StandardDataSourcePropertyName, typeof(ObservableCollection<Point>), typeof(UC_TestUnit));

        public const string DataSourcePropertyName = "DataSource";
        public ObservableCollection<Point> DataSource
        {
            get
            {
                return (ObservableCollection<Point>)GetValue(DataSourceProperty);
            }
            set
            {
                SetValue(DataSourceProperty, value);
            }
        }
        public static readonly DependencyProperty DataSourceProperty = DependencyProperty.Register(DataSourcePropertyName, typeof(ObservableCollection<Point>), typeof(UC_TestUnit));

        public const string StartCommandPropertyName = "StartCommand";
        public RelayCommand StartCommand
        {
            get
            {
                return (RelayCommand)GetValue(StartCommandProperty);
            }
            set
            {
                SetValue(StartCommandProperty, value);
            }
        }
        public static readonly DependencyProperty StartCommandProperty = DependencyProperty.Register(StartCommandPropertyName, typeof(RelayCommand), typeof(UC_TestUnit));

        public const string StopCommandPropertyName = "StopCommand";
        public RelayCommand StopCommand
        {
            get
            {
                return (RelayCommand)GetValue(StopCommandProperty);
            }
            set
            {
                SetValue(StopCommandProperty, value);
            }
        }
        public static readonly DependencyProperty StopCommandProperty = DependencyProperty.Register(StopCommandPropertyName, typeof(RelayCommand), typeof(UC_TestUnit));

        public const string HeaderPropertyName = "Header";
        public string Header
        {
            get
            {
                return (string)GetValue(HeaderProperty);
            }
            set
            {
                SetValue(HeaderProperty, value);
            }
        }
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(HeaderPropertyName, typeof(string), typeof(UC_TestUnit));
        #endregion

        #region Result 速度， 分数， 精度， 最终结果
        public const string ResultSpeedPropertyName = "ResultSpeed";
        public string ResultSpeed
        {
            get
            {
                return (string)GetValue(ResultSpeedProperty);
            }
            set
            {
                SetValue(ResultSpeedProperty, value);
            }
        }
        public static readonly DependencyProperty ResultSpeedProperty = DependencyProperty.Register(ResultSpeedPropertyName, typeof(string), typeof(UC_TestUnit));

        public const string ResultScorePropertyName = "ResultScore";
        public string ResultScore
        {
            get
            {
                return (string)GetValue(ResultScoreProperty);
            }
            set
            {
                SetValue(ResultScoreProperty, value);
            }
        }
        public static readonly DependencyProperty ResultScoreProperty = DependencyProperty.Register(ResultScorePropertyName, typeof(string), typeof(UC_TestUnit));

        public const string ResultAccuracyPropertyName = "ResultAccuracy";
        public string ResultAccuracy
        {
            get
            {
                return (string)GetValue(ResultAccuracyProperty);
            }
            set
            {
                SetValue(ResultAccuracyProperty, value);
            }
        }
        public static readonly DependencyProperty ResultAccuracyProperty = DependencyProperty.Register(ResultAccuracyPropertyName, typeof(string), typeof(UC_TestUnit));

        public const string ResultFinalStringPropertyName = "ResultFinalString";
        public string ResultFinalString
        {
            get
            {
                return (string)GetValue(ResultFinalStringProperty);
            }
            set
            {
                SetValue(ResultFinalStringProperty, value);
            }
        }
        public static readonly DependencyProperty ResultFinalStringProperty = DependencyProperty.Register(ResultFinalStringPropertyName, typeof(string), typeof(UC_TestUnit));
        #endregion

    }



}
