
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
    public class WorkTemplate2 : WorkFlowBase
    {
        protected override bool UserInit()
        {
            return true;
        }
        public WorkTemplate2(WorkFlowConfig cfg) : base(cfg)
        {

        }
        protected override int WorkFlow()
        {
            int i = 0;
            UpdateUI("FinalResult", new Tuple<int, string, string, string, string, bool>(
                 4,
                 i.ToString(),
                 i.ToString(),
                 i.ToString(),
                 i.ToString(),
                 false
                 ));
            UpdateUI("Collection", new Tuple<int, Point, Point, bool>(4, new Point(i, i + 1), new Point(2 * i, 2 * i + 1), false));
            while (!cts.IsCancellationRequested)
            {
                Thread.Sleep(100);
                if (bPause)
                    continue;
                ShowInfo($"{i}{i}{i}{i++}");
                UpdateUI("Progress", i + (4 << 16));
                UpdateUI("Collection", new Tuple<int, Point, Point, bool>(4, new Point(i, i + 1), new Point(2 * i, 2 * i + 1), true));
                if (i >= 100)
                {
                    UpdateUI("FinalResult", new Tuple<int, string, string, string, string, bool>(
                 4,
                 i.ToString(),
                 i.ToString(),
                 i.ToString(),
                 i.ToString(),
                 false
                 ));
                    return 0;
                }
            }
            return 0;
        }

    }
}
