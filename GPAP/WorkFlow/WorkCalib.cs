
using GPAP.Config.SoftwareManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GPAP.WorkFlow
{
    public class WorkCalib : WorkFlowBase
    {
        protected override bool UserInit()
        {
            return true;
        }
        public WorkCalib(WorkFlowConfig cfg) : base(cfg)
        {

        }
        protected override int WorkFlow()
        {
            int i = 0;
            UpdateUI("FinalResult", new Tuple<int, string, string, string, string, bool>(
                      1,
                      i.ToString(),
                      i.ToString(),
                      i.ToString(),
                      i.ToString(),
                      false
                      ));
            UpdateUI("Collection", new Tuple<int, Point, Point, bool>(1, new Point(StationIndex, i + 1), new Point(2 * i, 2 * i + 1), false));
            while (!cts.IsCancellationRequested)
            {
               
                Thread.Sleep(100);
                if (bPause)
                    continue;
                ShowInfo($"ABCDEFG{i}{i++}");
                UpdateUI("Progress", i+(1<<16));
                UpdateUI("Collection", new Tuple<int,Point,Point,bool>(1,new Point(i,i+1),new Point(2*i,2*i+1),true));
                if (i >= 100)
                {
                    UpdateUI("FinalResult", new Tuple<int, string, string, string, string, bool>(
                        1,
                        i.ToString(),
                        i.ToString(),
                        i.ToString(),
                        i.ToString(),
                        true
                        ));
                    return 0;
                }
            }
            return 0;
        }

    }
}
