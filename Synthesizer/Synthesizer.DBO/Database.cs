using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Threading;
using Synthesizer.DBO;
using System.IO;

namespace Synthesizer.DBO
{
    public interface ISoundsDataBase
    {
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
    public class SoundsDataBase:ISoundsDataBase
    {
        int _octavas = 2;
        Modes _currentMode;
        string _status = "Все звуки успешно загружены";
        readonly int[] _blacks = new int[] { 2, 4, 7, 9, 11 };
        Dictionary<PianoKey, string> _ListOfSounds;
        public IDictionary<PianoKey,string> ListOfSounds
        {
            get
            {
                return _ListOfSounds;
            }
        }

        IList<PianoKey> _ListOfKeys;

        Dictionary<PianoKey, string> _ListOfPianoSounds = new Dictionary<PianoKey, string>();
        Dictionary<PianoKey, string> _ListOfGuitarSounds = new Dictionary<PianoKey, string>();

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

        public string Status
        {
            get
            {
                return _status;
            }
        }
        public Modes CurrentMode
        {
            get
            {
                return _currentMode;
            }
        }

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
        int GetToneFromNumber(int number)
        {
            double tone = 130.81;
            tone *= Math.Pow(2, ((double)number / (double)12));
            return Convert.ToInt32(tone);
        }
        public SoundsDataBase()
        {
            LoadPianoKeys();

            LoadPiano();
            LoadGuitar();

            SwitchSound(Modes.piano);

        }
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
    public enum Modes
    {
        guitar,
        piano
    }
}
