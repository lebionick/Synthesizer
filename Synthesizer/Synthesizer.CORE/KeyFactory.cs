using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synthesizer.CORE
{
    public abstract class KeyCreator
    {
        public abstract PianoKey GetKey(int tone);
    }
    public class WhiteKeyCreator : KeyCreator
    {
        public override PianoKey GetKey(int tone)
        {
            return new WhiteKey(tone);
        }
    }
    public class BlackKeyCreator : KeyCreator
    {
        public override PianoKey GetKey(int tone)
        {
            return new BlackKey(tone);
        }
    }
}
