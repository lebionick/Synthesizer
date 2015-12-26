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
    /// <summary>
    /// Передает с событием данные
    /// о записываемом файле
    /// </summary>
    public class MetaData : EventArgs
    {
        /// <summary>
        /// Длина для записи в байтах
        /// </summary>
        public int Duration
        {
            get;
            set;
        }
        /// <summary>
        /// Файл для записи
        /// </summary>
        public string SoundPath
        {
            get;
            set;
        }
    }
    /// <summary>
    /// Обертка для класса Key
    /// </summary>
    public class PianoKeyViewModel:ViewModelBase
    {
        //событие по завершению нажатия клавиши
        public event EventHandler<MetaData> completeTracking;
        //кнопка клавиатуры привязанная к этой клавише
        string _bindedKey;
        //флаг о производимой записи
        bool _isRecorded = false;
        /// <summary>
        /// Метод вызываемый при старте записи
        /// </summary>
        public void recordingHandler(object sender, EventArgs ea)
        {
            _isRecorded = true;
        }
        /// <summary>
        /// Метод вызываемый при остановке записи
        /// </summary>
        public void stopHandler(object sender, EventArgs ea)
        {
            _isRecorded = false;
        }
        //вес одной секунды в байтах
        public Double WeightOf1Second
        {
            get
            {
                return _model.Weight;
            }
        }
        Timer _timer;
        /// <summary>
        /// Кнопка к которой привязана клавиша
        /// </summary>
        public string BindedKey
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

        //модель клавиши, которую оборачиваем
        private PianoKey _model; 

        //класс для воспроизведения звуков
        MediaPlayer Player=new MediaPlayer();
        /// <summary>
        /// Проигрывает звук клавиши
        /// либо стандартный виндоус звук
        /// </summary>
        public void PlaySound()
        {
            if (Player != null)
            {
                Player.Play();

                if (_isRecorded)
                {
                    //запускает таймер(сколько нажата кнопка)
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
                //отправляет данные о нажатии в главный класс
                performTracking();
            }
        }
        //метод для отправки количества байт для записи и 
        //пути до файла в главный класс
        void performTracking()
        { 
            if (_isRecorded)
            {
                if ((_timer != null) && (completeTracking != null))
                {
                    //заполнение EventArgs класса
                    MetaData metaData = new MetaData();

                    FileInfo info = new FileInfo(_model.Sound);

                    //расчет длины файла для записи в байтах
                    int testDuration = Convert.ToInt32((WeightOf1Second * _timer.GetMiliSeconds) / 1000.0);
                    
                    Debug.WriteLine(testDuration);
                    Debug.WriteLine(_timer.GetMiliSeconds);

                    //если привышена длина файла, то записать весь файл
                    metaData.Duration = (testDuration < info.Length) ? testDuration : Convert.ToInt32(info.Length);

                    //ВНИМАНИЕ, если убрать следующую строчку, то будет возможна запись 
                    //нефиксированных отрезков времени, но иногда будет возникать шипение вместо звуков
                    //причина на данный момент не установлена
                    //закомментируйте и проверьте
                    metaData.Duration = 600000; 

                    metaData.SoundPath = _model.Sound;
                    completeTracking(this, metaData);
                }
            }
        }
        /// <summary>
        /// создает обертку ViewModel для PianoKey
        /// </summary>
        /// <param name="model">экземпляр для обертки</param>
        public PianoKeyViewModel(PianoKey model)
        {
            _model = model;
            if (model.Sound != null)
                Player.Open(new Uri(Path.GetFullPath(_model.Sound)));
            else
                Player = null;
        }
    }
}
