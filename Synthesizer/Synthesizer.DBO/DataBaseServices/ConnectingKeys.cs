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
        public static void Connect(int numberOfSoundInBase, PianoKey key)
        {
            if ((SoundsDataBase.ListOfSounds != null)&&(SoundsDataBase.ListOfSounds.Count>numberOfSoundInBase))
                key.Sound=SoundsDataBase.ListOfSounds[numberOfSoundInBase];
            else key.Sound=null;
        }
    }
}
