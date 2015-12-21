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
using Synthesizer.CORE.RecordOperations;
namespace Synthesizer.ViewModel
{

    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        ISoundsDataBase DataBaseOfSounds;
        RecordIncapsulated Recorder;
        public string SoundStatus
        {
            get
            {
                return DataBaseOfSounds.Status;
            }
        }
        string _folderForRecord = @"..\\..\\..\\Samples";
        double _currentWeight = 264500;
        public void StartRecord()
        {
            if(Recorder==null)
              Recorder = new RecordIncapsulated(_folderForRecord);
            Recorder.Initialize(GetName);
            if (startRecord != null)
                startRecord(this, null);
        }
        public void StopRecord()
        {
            Recorder.MemoryClear();
        }
        public event EventHandler startRecord;
        public void keyRecordEventHandler(object sender, MetaData ea)
        {
            if (Recorder != null)
            {
                Recorder.Start(ea.Duration, ea.SoundPath);
            }
        }
        public ObservableCollection<PianoKeyViewModel> WhiteKeys
        {
            get
            {
                List<PianoKeyViewModel> result = new List<PianoKeyViewModel>();
                foreach (var itemKey in DataBaseOfSounds.GetListOfWhiteKeys)
                {
                    var wrappedKey = new PianoKeyViewModel(itemKey);
                    startRecord += wrappedKey.recordingHandler;
                    wrappedKey.completeTracking += keyRecordEventHandler;
                    wrappedKey.GetWeightOf1Second = _currentWeight;
                    result.Add(wrappedKey);
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
                    var wrappedKey = new PianoKeyViewModel(itemKey);
                    startRecord += wrappedKey.recordingHandler;
                    wrappedKey.completeTracking += keyRecordEventHandler;
                    wrappedKey.GetWeightOf1Second = _currentWeight;
                    result.Add(wrappedKey);
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
        RelayCommand _startRecord, _stopRecord;
        void turnGuitar()
        {
            if (DataBaseOfSounds.CurrentMode != Modes.guitar)
            {
                DataBaseOfSounds.SwitchSound(Modes.guitar);
                _currentWeight = WaveFormatsGiver.GetWeight(WaveFileFormats.guitarFormat);
                refreshKeys();
                if (Recorder != null)
                Recorder.SetFormat = WaveFormatsGiver.GetFormat(WaveFileFormats.guitarFormat);
            }
        }
        void turnPiano()
        {
            if (DataBaseOfSounds.CurrentMode != Modes.piano)
            {
                DataBaseOfSounds.SwitchSound(Modes.piano);
                _currentWeight = WaveFormatsGiver.GetWeight(WaveFileFormats.pianoFormat);
                refreshKeys();
                if (Recorder != null)
                    Recorder.SetFormat = WaveFormatsGiver.GetFormat(WaveFileFormats.pianoFormat);
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

        public ICommand StartRecordCommand
        {
            get
            {
                if (_startRecord == null)
                    _startRecord = new RelayCommand(StartRecord);
                return _startRecord;
            }
        }
        public ICommand StopCommand
        {
            get
            {
                if (_stopRecord == null)
                    _stopRecord = new RelayCommand(StopRecord);
                return _stopRecord;
            }
        }

        int _count = 0;
        string GetName
        {
            get
            {
                _count++;
                return ("audioSample-" + Convert.ToString(_count) + ".wav");
            }
        }

        public MainViewModel()
        {
            DataBaseOfSounds = new SoundsDataBase();
        }
    }
}