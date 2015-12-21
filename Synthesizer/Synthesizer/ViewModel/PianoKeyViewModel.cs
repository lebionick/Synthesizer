using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Synthesizer.CORE.RecordOperations;
using Synthesizer.DBO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Synthesizer.ViewModel
{
    public class MetaData : EventArgs
    {
        public int Duration
        {
            get;
            set;
        }
        public string SoundPath
        {
            get;
            set;
        }
    }
    public class PianoKeyViewModel:ViewModelBase
    {
        public event EventHandler<MetaData> completeTracking;
        string _bindedKey;
        bool _isRecorded = false;
        public void recordingHandler(object sender, EventArgs ea)
        {
            _isRecorded = true;
        }
        public void stopHandler(object sender, EventArgs ea)
        {
            _isRecorded = false;
        }
        Double _getWeightOf1Second = 264500.0;  //44100Гц*24бит*2канала/8
        public Double GetWeightOf1Second
        {
            get
            {
                return _getWeightOf1Second; //зависит от разрялности и дискретизации пишущего потока в RecordIncapsulated
            }
            set
            {
                _getWeightOf1Second = value;
            }
        }
        Timer _timer;
        public string BindedKey //клавиша, на которую подвязано воспроизведение этой клавиши
        {
            get
            {
                return _bindedKey;
            }
            set
            {
                _bindedKey = value;
                RaisePropertyChanged("BindedKey");
            }
        }
        private PianoKey _model; //модель клавиши, которую оборачиваем
        MediaPlayer Player=new MediaPlayer(); //класс для воспроизведения звуков
        public void PlaySound()
        {
            if (Player != null)
            {
                Player.Play();

                if (_isRecorded)
                {
                    _timer = new Timer();
                }
            }
            else
            {
                _model.Play();
            }
        }
        public void StopSound()
        {
            if (Player != null)
            {
                Player.Stop();
                performTracking();
            }
        }
        void performTracking()
        { 
            if (_isRecorded)
            {
                if ((_timer != null) && (completeTracking != null))
                {
                    MetaData metaData = new MetaData();
                    FileInfo info = new FileInfo(_model.Sound);
                    int testDuration = Convert.ToInt32((GetWeightOf1Second * _timer.GetMiliSeconds) / 1000.0);
                    
                    Debug.WriteLine(testDuration);
                    Debug.WriteLine(_timer.GetMiliSeconds);
                    metaData.Duration = (testDuration < info.Length) ? testDuration : 600000;
                    Debug.WriteLine(metaData.Duration);
                    metaData.SoundPath = _model.Sound;
                    completeTracking(this, metaData);
                }
            }
        }
        public PianoKeyViewModel(PianoKey model)
        {
            _model = model;
            if (model.Sound != null)
                Player.Open(new Uri(Path.GetFullPath(_model.Sound)));
            else
                Player = null;
        }
        
        private RelayCommand _play;
        public ICommand Play
        {
            get
            {
                Player.Stop();
                if (_play == null) _play = new RelayCommand(PlaySound);
                return _play;
            }
        }
    }
}
