using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synthesizer.CORE
{

    public class KeyLibrary
    {
        IList<PianoKey> _ListOfWhiteKeys=new List<PianoKey>();
        IList<PianoKey> _ListOfBlackKeys = new List<PianoKey>();
        public IList<PianoKey> ListOfBlackKeys
        {
            get { return _ListOfBlackKeys; }
            set { }
        }
        public IList<PianoKey> ListOfWhiteKeys
        {
            get { return _ListOfWhiteKeys; }
            set { }
        }

        public KeyLibrary()
        {
            KeyCreator factory;
            IList<PianoKey> ListOfKeys;
            for (int i = 0; i < DBO.SoundsDataBase.octavas * 12; i++)
            {
                bool isWhite = true;
                int number = (i % 12) + 1;
                foreach (int x in DBO.SoundsDataBase.blacks)
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
                ListOfKeys[i].AddSound(DBO.DataBaseServices.ConnectingKeys.Connect(i));
        
            }
        }
        int GetToneFromNumber(int number)
        {
            double tone= 130.81;
            tone *= Math.Pow(2, (number / 12));
            return Convert.ToInt32(tone);
        }
    }
}
