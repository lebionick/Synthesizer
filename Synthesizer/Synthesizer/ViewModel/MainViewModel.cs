using GalaSoft.MvvmLight;
using Synthesizer.CORE;
using Synthesizer.DBO;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Synthesizer.ViewModel
{

    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        SoundsDataBase DataBaseOfSounds;
        KeyLibrary LibraryofKeys;
        public ObservableCollection<PianoKeyViewModel> WhiteKeys
        {
            get
            {
                List<PianoKeyViewModel> result = new List<PianoKeyViewModel>();
                foreach (var itemKey in LibraryofKeys.ListOfWhiteKeys)
                {
                    result.Add(new PianoKeyViewModel(itemKey));
                }
                return new ObservableCollection<PianoKeyViewModel>(result);
            }
        }
        public ObservableCollection<PianoKeyViewModel> BlackKeys
        {
            get
            {
                List<PianoKeyViewModel> result = new List<PianoKeyViewModel>();
                foreach (var itemKey in LibraryofKeys.ListOfBlackKeys)
                {
                    result.Add(new PianoKeyViewModel(itemKey));
                }
                return new ObservableCollection<PianoKeyViewModel>(result);
            }
                
        }
        int _i = -1;
        int I
        {
            get
            {
                return _i;
            }
            set
            {
                _i = value % 5;
            }
        }
        public double WhiteKeyWidth=40;
        public double WhiteKeyHeight = 165;
        public double BlackKeyWidth
        {
            get
            {
                return (WhiteKeyWidth * 0.75);
            }
        }
        public double BlackKeyHeight
        {
            get
            {
                return (WhiteKeyHeight * 0.6);
            }
        }
        public Thickness GetMargin()
        {
            Thickness margin;
            if ((I == 1) || (I >= 3))
                margin= new Thickness(0.4 * WhiteKeyWidth,0,0,0);
            if ((I == 2) || (I == 0))
                margin= new Thickness(1.4 * WhiteKeyWidth,0,0,0);
            else
                margin= new Thickness(0.7 * WhiteKeyWidth,0,0,0);
            I++;
            return margin;
        }
        public MainViewModel()
        {
            DataBaseOfSounds = new SoundsDataBase(); //GOVNOKOD
            DataBaseOfSounds.SwitchSound("piano");
            LibraryofKeys = new KeyLibrary();
        }
    }
}