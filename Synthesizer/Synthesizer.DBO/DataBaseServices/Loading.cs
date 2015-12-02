using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Threading;
using System.IO;

namespace Synthesizer.DBO
{
    interface ILoadSound
    {
        List<SoundPlayer> Load(int octavas); 
    }
    class LoadPiano : ILoadSound
    {
        public List<SoundPlayer> Load(int octavas)
        {
            List<SoundPlayer> _soundList = new List<SoundPlayer>();

            for (int i = 0; i < octavas * 12; i++)
            {
                SoundPlayer player = new SoundPlayer();
                string path = ("C:\\SoundBanks\\pianotones\\pianotone-" + Convert.ToString(i + 1) + ".wav");
                try
                {
                    player.SoundLocation = (@path);
                    player.LoadAsync();
                }
                catch (Exception)
                {
                    player = null;
                }
                _soundList.Add(player);
            }
            return _soundList;
        }
    }
}
