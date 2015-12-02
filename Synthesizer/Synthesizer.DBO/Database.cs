using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Threading;
using Synthesizer.CORE;
using Synthesizer.DBO.DataBaseServices;

namespace Synthesizer.DBO
{
    public interface ISoundsDataBase
    {
        IList<PianoKey> GetListOfWhiteKeys
        {
            get;
            
        }
        IList<PianoKey> GetListOfBlackKeys
        {
            get;
            
        }
    }
    public class SoundsDataBase:ISoundsDataBase
    {
        public static int _octavas = 2;
        readonly int[] _blacks = new int[] { 2, 4, 7, 9, 11 };

        public static IList<SoundPlayer> ListOfSounds;

        List<SoundPlayer> _ListOfPianoSounds;

        IList<PianoKey> _ListOfWhiteKeys = new List<PianoKey>();
        IList<PianoKey> _ListOfBlackKeys = new List<PianoKey>();
        public IList<PianoKey> GetListOfWhiteKeys
        {
            get { return _ListOfWhiteKeys; }
        }
        public IList<PianoKey> GetListOfBlackKeys
        {
            get { return _ListOfBlackKeys; }
        }
        void LoadPianoKeys()
        {
            KeyCreator factory;
            IList<PianoKey> ListOfKeys;
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
                    ListOfKeys = _ListOfWhiteKeys;
                }
                else
                {
                    factory = new BlackKeyCreator();
                    ListOfKeys = _ListOfBlackKeys;
                }

                ListOfKeys.Add(factory.GetKey(GetToneFromNumber(i)));
                ListOfKeys[ListOfKeys.Count - 1].AddSound(ConnectingKeys.Connect(i));
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
            ILoadSound loader = new LoadPiano();
            _ListOfPianoSounds = loader.Load(_octavas);
            SwitchSound("piano");

            LoadPianoKeys();

        }
        public void SwitchSound(string mode) //GOVNOKOD
        {
            switch (mode)
            {
                case "piano": ListOfSounds = _ListOfPianoSounds; break;
                default: break;
            }
        }
    }
}
