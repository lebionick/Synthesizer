using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
namespace Synthesizer.CORE.RecordOperations
{
    public static class WaveFormatsGiver
    {
        public static WaveFormat GetFormat(WaveFileFormats wff)
        {
            switch (wff)
            {
                case WaveFileFormats.pianoFormat: return new WaveFormat(44100, 24, 2);
                case WaveFileFormats.guitarFormat: return new WaveFormat(44100, 16, 2);
                default: return new WaveFormat();
            }
        }
        public static Double GetWeight(WaveFileFormats wff)
        {
            switch (wff)
            {
                case WaveFileFormats.pianoFormat: return (44100*24*2)/8;
                case WaveFileFormats.guitarFormat: return (44100*16*2)/8;
                default: return (44100*16*2)/8;
            }
        } 
    }
    public enum WaveFileFormats
    {
        pianoFormat,
        guitarFormat
    }
}
