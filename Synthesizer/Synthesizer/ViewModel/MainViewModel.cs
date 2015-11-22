using GalaSoft.MvvmLight;
using Synthesizer.CORE;
using Synthesizer.DBO;
using System.Collections.ObjectModel;

namespace Synthesizer.ViewModel
{

    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        SoundsDataBase DataBaseOfSounds;
        KeyLibrary LibraryofKeys;
        public ObservableCollection<PianoKey> BlackKeys
        {
            get;
        }
        public ObservableCollection<PianoKey> WhiteKeys
        {
            get;
        }
        public MainViewModel()
        {
            DataBaseOfSounds = new SoundsDataBase();
            DataBaseOfSounds.SwitchSound("piano");
            LibraryofKeys = new KeyLibrary();
            BlackKeys = new ObservableCollection<PianoKey>(LibraryofKeys.ListOfBlackKeys);
            WhiteKeys = new ObservableCollection<PianoKey>(LibraryofKeys.ListOfWhiteKeys);
        }
    }
}