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
using Synthesizer.CORE;
using System.Speech.Synthesis;

namespace Synthesizer.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        //���� ������
        ISoundsDataBase DataBaseOfSounds;
        //����� ��� ������ � wav
        RecordIncapsulated Recorder;
        //������ ��������� �������� ���� ������
        public string SoundStatus
        {
            get
            {
                return DataBaseOfSounds.Status;
            }
        }
        //������ ��������� ������
        public string RecordStatus
        {
            get
            {
                return _recordStatus;
            }
            set
            {
                _recordStatus = value;
            }
        }
        string _recordStatus = "";
        //����� ��� ������(�� ��������� Samples)
        string _folderForRecord = @"..\\..\\..\\Samples";
        public string FolderForRecord
        {
            get
            {
                return _folderForRecord;
            }
            set
            {
                _folderForRecord = value;
            }
        }
        /// <summary>
        /// ����� ���������� ���� ������ � ���������� ������
        /// </summary>
        public void StartRecord()
        {
            //����� ���������
            if(Recorder==null)
              Recorder = new RecordIncapsulated(_folderForRecord);
            Recorder.Initialize(GetName);
            if (startRecord != null)
                startRecord(this, null);

            _recordStatus = "Recording...";
            RaisePropertyChanged("RecordStatus");
            synth.Speak("RECORD");
        }
        /// <summary>
        /// ����� �������������� ���� ������ �� ������
        /// </summary>
        public void StopRecord()
        {
            if (stopRecord != null)
                stopRecord(this, null);
            Recorder.MemoryClear();
            _recordStatus = "Stopped";
            RaisePropertyChanged("RecordStatus");
            synth.Speak("STOP");
        }
        //������� ������/���������
        public event EventHandler startRecord;
        public event EventHandler stopRecord;
        //������� ����
        SpeechSynthesizer synth = new SpeechSynthesizer();
        
        /// <summary>
        /// ���������� ������� ���������� �������� ������� ������
        /// </summary>
        public void keyRecordEventHandler(object sender, MetaData ea)
        {
            if (Recorder != null)
            {
                Recorder.Start(ea.Duration, ea.SoundPath);
            }
        }
        
        /// <summary>
        /// ��������� ����� ������
        /// </summary>
        public ObservableCollection<PianoKeyViewModel> WhiteKeys
        {
            get
            {
                List<PianoKeyViewModel> result = new List<PianoKeyViewModel>();
                foreach (var itemKey in DataBaseOfSounds.GetListOfWhiteKeys)
                {
                    var wrappedKey = new PianoKeyViewModel(itemKey);
                    startRecord += wrappedKey.recordingHandler;
                    stopRecord += wrappedKey.stopHandler;
                    wrappedKey.completeTracking += keyRecordEventHandler;
                    result.Add(wrappedKey);
                }
                return new ObservableCollection<PianoKeyViewModel>(result);
            }
        }
        /// <summary>
        /// ��������� ������ ������
        /// </summary>
        public ObservableCollection<PianoKeyViewModel> BlackKeys
        {
            get
            {
                List<PianoKeyViewModel> result = new List<PianoKeyViewModel>();
                foreach (var itemKey in DataBaseOfSounds.GetListOfBlackKeys)
                {
                    var wrappedKey = new PianoKeyViewModel(itemKey);
                    startRecord += wrappedKey.recordingHandler;
                    stopRecord += wrappedKey.stopHandler;
                    wrappedKey.completeTracking += keyRecordEventHandler;
                    result.Add(wrappedKey);
                }
                return new ObservableCollection<PianoKeyViewModel>(result);
            }
                
        }
        //������ ��� ����������� ������ ������ � ��������
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
        /// <summary>
        /// ���������� ������ ��� ������ ������
        /// </summary>
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
        //�������� � ������ ��� ������������ �������� �������
        RelayCommand _turnGuitar, _turnPiano;
        RelayCommand _startRecord, _stopRecord;
        void turnGuitar()
        {
            if (DataBaseOfSounds.CurrentMode != Modes.guitar)
            {
                DataBaseOfSounds.SwitchSound(Modes.guitar);
                refreshKeys();
                if (Recorder == null)
                    Recorder = new RecordIncapsulated(GetName);
                Recorder.SetFormat = WaveFormatsGiver.GetFormat(Modes.guitar);
                synth.Speak("GUITAR");
            }
        }
        void turnPiano()
        {
            if (DataBaseOfSounds.CurrentMode != Modes.piano)
            {
                DataBaseOfSounds.SwitchSound(Modes.piano);
                refreshKeys();
                if (Recorder == null)
                    Recorder = new RecordIncapsulated(GetName);
                Recorder.SetFormat = WaveFormatsGiver.GetFormat(Modes.piano);
                synth.Speak("PIANO");
            }
        }
        //��������� ViewModel
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
        //������������ ���� ��� ������ ������
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