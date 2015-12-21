using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synthesizer.CORE.RecordOperations
{
    public class Timer
    {
        DateTime startTime;
        public Timer()
        {
            startTime = DateTime.Now;
        }
        public Double GetMiliSeconds
        {
            get
            {
                return (DateTime.Now - startTime).TotalMilliseconds;
            }
        }
    }
}
