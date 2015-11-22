using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Threading;

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
                try
                {
                    player.SoundLocation = (@"C:\Sounds\pianotones\pianotone-" + i.ToString() + ".wav");
                    player.Load();
                }
                catch (NullReferenceException)
                {
                    continue;
                }
                _soundList.Add(player);
            }
            return _soundList;
        }
    }
}
