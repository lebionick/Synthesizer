using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Media;
using System.Threading;

namespace Synthesizer
{ 
    public abstract class PianoKey
    {
        protected int _tone;
        public SoundPlayer Sound;
        public PianoKey(int Tone)
        {
            _tone = Tone;
        }
        public void AddSound(SoundPlayer sound)
        {
            Sound = sound;
        }
        public void Play()
        {
            if (Sound == null)
                Beep(_tone, 500);
            else
                Sound.Play();
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

