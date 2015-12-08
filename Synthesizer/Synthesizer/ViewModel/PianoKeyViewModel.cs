using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Synthesizer.ViewModel
{
    public class PianoKeyViewModel
    {
        string _bindedKey;
        public string BindedKey
        {
            get
            {
                return _bindedKey;
            }
            set
            {
                _bindedKey = value;
            }
        }
        private PianoKey _model;
        MediaPlayer Player=new MediaPlayer();
        public void PlaySound()
        {
            if (Player != null)
            {
                Player.Stop();
                Player.Play();
            }
        }
        public void StopSound()
        {
            if (Player != null)
            {
                Player.Stop();
            }
        }
        public PianoKeyViewModel(PianoKey model)
        {
            _model = model;
            Player.Open(new Uri(_model.Sound.SoundLocation));
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
