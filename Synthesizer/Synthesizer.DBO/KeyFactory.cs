using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synthesizer.DBO
{
    /// <summary>
    /// Абстрактная фабрика для создания клавиш
    /// конкретных типов
    /// </summary>
    public abstract class KeyCreator
    {
        public abstract PianoKey GetKey(ISoundsDataBase dataBase, int tone);
    }
    /// <summary>
    /// Создает WhiteKey
    /// </summary>
    public class WhiteKeyCreator : KeyCreator
    {
        public override PianoKey GetKey(ISoundsDataBase dataBase, int tone)
        {
            return new WhiteKey(dataBase, tone);
        }
    }
    /// <summary>
    /// Создает BlackKey
    /// </summary>
    public class BlackKeyCreator : KeyCreator
    {
        public override PianoKey GetKey(ISoundsDataBase dataBase, int tone)
        {
            return new BlackKey(dataBase, tone);
        }
    }
}
