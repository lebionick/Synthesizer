using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Synthesizer.ViewModel
{
    public class PianoKeyViewModel
    {
        private PianoKey _model;

        public PianoKeyViewModel(PianoKey model)
        {
            _model = model;
        }

        private RelayCommand _play;
        public ICommand Play
        {
            get
            {
                if (_play == null) _play = new RelayCommand(_model.Play);
                return _play;
            }
        }
    }
}
