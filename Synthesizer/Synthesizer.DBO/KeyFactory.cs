using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synthesizer.DBO
{
    public abstract class KeyCreator
    {
        public abstract PianoKey GetKey(ISoundsDataBase dataBase, int tone);
    }
    public class WhiteKeyCreator : KeyCreator
    {
        public override PianoKey GetKey(ISoundsDataBase dataBase, int tone)
        {
            return new WhiteKey(dataBase, tone);
        }
    }
    public class BlackKeyCreator : KeyCreator
    {
        public override PianoKey GetKey(ISoundsDataBase dataBase, int tone)
        {
            return new BlackKey(dataBase, tone);
        }
    }
}
