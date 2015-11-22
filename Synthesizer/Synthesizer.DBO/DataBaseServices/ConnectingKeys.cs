using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Synthesizer.DBO.DataBaseServices
{
    public class ConnectingKeys
    {
        public static SoundPlayer Connect(int number)
        {
            if (SoundsDataBase.ListOfSounds != null)
                return SoundsDataBase.ListOfSounds[number];
            else return null;
        }
    }
}
