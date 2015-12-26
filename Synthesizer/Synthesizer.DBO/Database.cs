using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Threading;
using Synthesizer.DBO;
using System.IO;
using Synthesizer.CORE;
using Synthesizer.CORE.RecordOperations;

namespace Synthesizer.DBO
{
    /// <summary>
    /// Интерфейс работы с SoundsDataBase
    /// </summary>
    public interface ISoundsDataBase
    {
        double WeightOf1Sec
        {
            get;
        }
        void SwitchSound(Modes mode);

        IList<PianoKey> GetListOfWhiteKeys
        {
            get;
        }
        IList<PianoKey> GetListOfBlackKeys
        {
            get;
        }
        string Status
        {
            get;
        }
        IDictionary<PianoKey, string> ListOfSounds
        {
            get;
        }
        Modes CurrentMode
        {
            get;
        }
    }
    /// <summary>
    /// Создает список клавиш
    /// и хранит пути звуковых файлов
    /// </summary>
    public class SoundsDataBase:ISoundsDataBase
    {
        //количество октав, в текущей версии достпуно
        //две октвавы, но количество звуков модет быть расширено
        int _octavas = 2;

        //вспомогательный массив - черные клавиши по счету:
        readonly int[] _blacks = new int[] { 2, 4, 7, 9, 11 };

        //коллекция звуков по ключу - конкретной клавише
        //реализует "стратегию"
        Dictionary<PianoKey, string> _ListOfSounds;
        public IDictionary<PianoKey,string> ListOfSounds
        {
            get
            {
                return _ListOfSounds;
            }
        }

        //список всех клавиш
        IList<PianoKey> _ListOfKeys;

        //конкретные словари клавиша-путь к звуку
        Dictionary<PianoKey, string> _ListOfPianoSounds = new Dictionary<PianoKey, string>();
        Dictionary<PianoKey, string> _ListOfGuitarSounds = new Dictionary<PianoKey, string>();

        //списки конкретных клавиш
        public IList<PianoKey> GetListOfWhiteKeys
        {
            get
            {
                List<PianoKey> listOfWhiteKeys = new List<PianoKey>();
                foreach (PianoKey varKey in _ListOfKeys)
                    if(varKey is WhiteKey)
                    listOfWhiteKeys.Add(varKey);
                return listOfWhiteKeys;
            }
        }
        public IList<PianoKey> GetListOfBlackKeys
        {
            get
            {
                List<PianoKey> listOfBlackKeys = new List<PianoKey>();
                foreach (PianoKey varKey in _ListOfKeys)
                    if (varKey is BlackKey)
                        listOfBlackKeys.Add(varKey);
                return listOfBlackKeys;
            }
        }

        //статус загрузки
        string _status = "Все звуки успешно загружены";
        public string Status
        {
            get
            {
                return _status;
            }
        }

        //режим звуков
        Modes _currentMode;
        public Modes CurrentMode
        {
            get
            {
                return _currentMode;
            }
        }
        //вес одной секнуды при текущем формате wav
        public double WeightOf1Sec
        {
            get
            {
                return WaveFormatsGiver.GetWeight(CurrentMode);
            }
        } 

        //заполняет ListOfKeys конкретными клавишами
        void LoadPianoKeys()
        {
            KeyCreator factory;
            _ListOfKeys = new List<PianoKey>();
            for (int i = 0; i < _octavas * 12; i++)
            {
                bool isWhite = true;
                int number = (i % 12) + 1;
                foreach (int x in _blacks)
                {
                    if (number == x)
                    {
                        isWhite = false;
                        break;
                    }
                }
                if (isWhite)
                {
                    factory = new WhiteKeyCreator();
                }
                else
                {
                    factory = new BlackKeyCreator();
                }
                _ListOfKeys.Add(factory.GetKey(this, GetToneFromNumber(i)));
            }
        }

        //при создании клавиш дает им соотвествующие высоты звука
        int GetToneFromNumber(int number)
        {
            double tone = 130.81;
            tone *= Math.Pow(2, ((double)number / (double)12));
            return Convert.ToInt32(tone);
        }
        /// <summary>
        /// При создании заполняет список клавиш
        /// и словари с путями звуков
        /// </summary>
        public SoundsDataBase()
        {
            LoadPianoKeys();

            LoadPiano();
            LoadGuitar();
            //по умолчанию звук пианино
            SwitchSound(Modes.piano);

        }
        //Load методы заполняют соответсвующие словари ключ-путь
        void LoadPiano()
        {
            int i = 0;
            foreach (PianoKey varKey in _ListOfKeys)
            {
                i++;
                string path = ("..\\..\\..\\SoundBanks\\pianotones\\pianotone-" + Convert.ToString(i) + ".wav");
                if (File.Exists(@path))
                    _ListOfPianoSounds.Add(varKey, path);
                else
                {
                    _ListOfPianoSounds.Add(varKey,null);
                    _status = "Не все звуки были загружены!";
                }
            }
        }
        void LoadGuitar()
        {
            int i = 0;
            foreach (PianoKey varKey in _ListOfKeys)
            {
                i++;
                string path = ("..\\..\\..\\SoundBanks\\guitartones\\guitartone-" + Convert.ToString(i) + ".wav");
                if (File.Exists(@path))
                    _ListOfGuitarSounds.Add(varKey, path);
                else
                {
                    _ListOfGuitarSounds.Add(varKey, null);
                    _status = "Не все звуки были загружены!";
                }
            }
        }
        /// <summary>
        /// Переключает звуки
        /// </summary>
        /// <param name="mode">режим базы звуков</param>
        public void SwitchSound(Modes mode)
        {
                switch (mode)
                {
                    case Modes.piano:
                        {
                            _ListOfSounds = _ListOfPianoSounds;
                            break;
                        }
                    case Modes.guitar:
                        {
                            _ListOfSounds = _ListOfGuitarSounds;
                            break;
                        }
                    default: break;
                }
            _currentMode = mode;
         }
    }
}
