using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Media;
using System.Threading;
using System.IO;
using Synthesizer.CORE.RecordOperations;

namespace Synthesizer.DBO
{
    /// <summary>
    /// Класс - модель клавиши
    /// </summary>
    public abstract class PianoKey
    {
        //высота ноты, соответсвующей звуку клавиши
        protected int _tone;
        //в теории - ссылка на базу с клавишами(возможно стоит
        //сделать обращение к статичному свойству)
        ISoundsDataBase _soundsDataBase;
        //Вес одной секунды аудио в байтах
        public double Weight
        {
            get
            {
                return _soundsDataBase.WeightOf1Sec;
            }
        }
        //путь к звуку клавиши
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
        //стандартный метод для проирывания звуков видоус, если 
        //при загрузке звука была ошибка
        public void Play()
        {
                Beep(_tone, 700);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Beep(int frequency, int duration);

    }
    //класс отвечающий за черные клавиши
    public class BlackKey : PianoKey
    {
        public BlackKey(ISoundsDataBase soundBase, int Tone) : base(soundBase, Tone)
        {

        }
    }
    //класс отвечающий за белые клавиши
    public class WhiteKey : PianoKey
    {

        public WhiteKey(ISoundsDataBase soundBase, int Tone) : base(soundBase, Tone)
        {

        }
    }
}

