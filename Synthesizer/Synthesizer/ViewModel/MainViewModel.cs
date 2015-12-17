using GalaSoft.MvvmLight;
using Synthesizer.DBO;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace Synthesizer.ViewModel
{

    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        ISoundsDataBase DataBaseOfSounds;
        public string SoundStatus
        {
            get
            {
                return DataBaseOfSounds.Status;
            }
        }
        public ObservableCollection<PianoKeyViewModel> WhiteKeys
        {
            get
            {
                List<PianoKeyViewModel> result = new List<PianoKeyViewModel>();
                foreach (var itemKey in DataBaseOfSounds.GetListOfWhiteKeys)
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
                foreach (var itemKey in DataBaseOfSounds.GetListOfBlackKeys)
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
        public Thickness GetMargin
        {
            get
            {
                Thickness margin;
                if ((I == 1) || (I >= 3))
                    margin = new Thickness(WhiteKeyWidth - BlackKeyWidth, 0, 0, 0);
                else if ((I == 2) || (I == 0))
                    margin = new Thickness( 2*WhiteKeyWidth-BlackKeyWidth, 0, 0, 0);
                else
                {
                    margin = new Thickness(WhiteKeyWidth - 0.5*BlackKeyWidth, 0, 0, 0);
                    I++;
                }
                I++;
                return margin;
            }
        }
        public double WhiteKeyWidth
        {
            get
            {
                return 40;
            }
        }
        public double WhiteKeyHeight
        {
            get
            {
                return 165;
            }
        }
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
        RelayCommand _turnGuitar, _turnPiano;
        void turnGuitar()
        {
            if (DataBaseOfSounds.CurrentMode != Modes.guitar)
            {
                DataBaseOfSounds.SwitchSound(Modes.guitar);
                refreshKeys();
            }
        }
        void turnPiano()
        {
            if (DataBaseOfSounds.CurrentMode != Modes.piano)
            {
                DataBaseOfSounds.SwitchSound(Modes.piano);
                refreshKeys();
            }
        }

        void refreshKeys()
        {
            _i = -1;
            RaisePropertyChanged("WhiteKeys");
            RaisePropertyChanged("BlackKeys");
        }
        public ICommand SwitchGuitar
        {
            get
            {
                if (_turnGuitar == null)
                    _turnGuitar = new RelayCommand(turnGuitar);
                return _turnGuitar;
            }
        }

        public ICommand SwitchPiano
        {
            get
            {
                if (_turnPiano == null)
                    _turnPiano = new RelayCommand(turnPiano);
                return _turnPiano;
            }
        }

        public MainViewModel()
        {
            DataBaseOfSounds = new SoundsDataBase();
        }
    }
}