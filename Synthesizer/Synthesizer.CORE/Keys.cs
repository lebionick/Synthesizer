using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Media;
using System.Threading;
using System.IO;

namespace Synthesizer
{ 
    public abstract class PianoKey
    {

        protected int _tone;
        public string Sound;

        public PianoKey(int tone)
        {
            _tone = tone;
        }
        public void AddSound(string sound)
        {   
            Sound = sound;
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
        public BlackKey(int Tone) : base(Tone)
        {

        }
    }
         
    public class WhiteKey : PianoKey
    {

        public WhiteKey(int Tone) : base(Tone)
        {

        }
    }
}

