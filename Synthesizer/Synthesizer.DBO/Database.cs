using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Threading;

namespace Synthesizer.DBO
{
    public class SoundsDataBase
    {
        public static int octavas = 2;
        public static int[] blacks = new int[] { 2, 4, 7, 9, 11 };
        public static IList<SoundPlayer> ListOfSounds;
        List<SoundPlayer> ListOfPianoSounds;
        void LoadInThread()
        {
            ILoadSound loader = new LoadPiano();
            ListOfPianoSounds = loader.Load(octavas);
        }
        public SoundsDataBase()
        {
            Thread LoadingThread = new Thread(LoadInThread);
            LoadingThread.Start();
            LoadingThread.Join();
        }
        public void SwitchSound(string mode)
        {
            switch (mode)
            {
                case "piano": ListOfSounds = ListOfPianoSounds; break;
                default: break;
            }
        }
    }
}
