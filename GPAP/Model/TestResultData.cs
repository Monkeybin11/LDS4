using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPAP.Model
{
    public class TestResultData : INotifyPropertyChanged
    {
        private string speed = "";
        private string score = "";
        private string accuracy = "";
        private string finalresultstring = "";
        public string SpeedResult
        {
            get { return speed; }
            set
            {
                if (speed != value)
                {
                    speed = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("SpeedResult"));
                }
            }
        }
        public string ScoreResult
        {
            get { return score; }
            set
            {
                if (score != value)
                {
                    score = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ScoreResult"));
                }
            }
        }
        public string AccuracyResult
        {
            get { return accuracy; }
            set
            {
                if (accuracy != value)
                {
                    accuracy = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AccuracyResult"));
                }
            }
        }
        public string FinalResultString
        {
            get { return finalresultstring; }
            set
            {
                if (finalresultstring != value)
                {
                    finalresultstring = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FinalResultString"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
