using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synthesizer.CORE.RecordOperations
{
    /// <summary>
    ///  При инициализации
    ///  начинает отсчет времени
    /// </summary>
    public class Timer
    {
        DateTime startTime;
        public Timer()
        {
            startTime = DateTime.Now;
        }
        /// <summary>
        ///  Возвращает количество милисекунд от времени создания
        /// </summary>
        public Double GetMiliSeconds
        {
            get
            {
                return (DateTime.Now - startTime).TotalMilliseconds;
            }
        }
    }
}
