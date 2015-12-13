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
        List<string> Load(int octavas, ref string status); 
    }
    class LoadPiano : ILoadSound
    {
        public List<string> Load(int octavas, ref string status)
        {
            List<string> _soundList = new List<string>();
            status = "Все звуки успешно загружены";
            
            for (int i = 0; i < octavas * 12; i++)
            {
                string path = ("C:\\SoundBanks\\pianotones\\pianotone-" + Convert.ToString(i + 1) + ".wav");
                if (File.Exists(@path))
                    _soundList.Add(path);
                else
                {
                    _soundList.Add(null);
                    status = "Не все звуки были загружены!";
                }
            }
            return _soundList;
        }
    }
}
