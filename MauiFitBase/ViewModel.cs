using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace MauiFitBase
{
    public class ViewModel
    {

        //static 

        public ISeries[] Series { get; set; }


        public void ChangeData(double[] Weight)
        {
            Series = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = Weight
                }
            };
        }
}
}
