using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
namespace Synthesizer.CORE.RecordOperations
{
    /// <summary>
    ///  Вспомогательный класс
    ///  для получения форматов и битрейтов Wav
    /// </summary>
    public static class WaveFormatsGiver
    {
        /// <summary>
        /// Метод возвращающий формат для wav
        /// в формате WaveFormat библиотеки Naudio
        /// </summary>
        public static WaveFormat GetFormat(Modes soundMode)
        {
            switch (soundMode)
            {
                case Modes.piano: return new WaveFormat(44100, 24, 2);
                case Modes.guitar: return new WaveFormat(44100, 16, 2);
                default: return new WaveFormat();
            }
        }
        /// <summary>
        /// Метод возвращающий вес одной секунды в байтах
        /// </summary>
        public static Double GetWeight(Modes soundMode)
        {
            switch (soundMode)
            {
                case Modes.piano: return (44100*24*2)/8;
                case Modes.guitar: return (44100*16*2)/8;
                default: return (44100*16*2)/8;
            }
        } 
    }
}
