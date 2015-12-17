using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Media;
using System.Threading;
using System.IO;

namespace Synthesizer.DBO
{ 
    public abstract class PianoKey
    {
        
        protected int _tone;
        ISoundsDataBase _soundsDataBase;
        public string Sound
        {
            get
            {
                if (_soundsDataBase.ListOfSounds != null)
                    return _soundsDataBase.ListOfSounds[this];
                else
                    return null;
            }
        }
        public PianoKey(ISoundsDataBase dataBase, int tone)
        {
            _tone = tone;
            _soundsDataBase = dataBase;
        }
        public void Play()
        {
                Beep(_tone, 700);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Beep(int frequency, int duration);

    }
    public class BlackKey : PianoKey
    {
        public BlackKey(ISoundsDataBase soundBase, int Tone) : base(soundBase, Tone)
        {

        }
    }
         
    public class WhiteKey : PianoKey
    {

        public WhiteKey(ISoundsDataBase soundBase, int Tone) : base(soundBase, Tone)
        {

        }
    }
}

