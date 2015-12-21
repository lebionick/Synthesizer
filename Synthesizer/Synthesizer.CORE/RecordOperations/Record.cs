
namespace Synthesizer.CORE.RecordOperations
{
    using NAudio.Wave;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Media;
    using System.Text;
    using System.Threading.Tasks;

        public class RecordIncapsulated
        {
         WaveFileWriter MainStream;
         WaveFormat waveFormat;

            string _folder;
            public RecordIncapsulated(string folder)
            {
                waveFormat = new WaveFormat(44100, 24, 2);
               _folder = folder;
            }
            public WaveFormat SetFormat
        {
            set
            {
                waveFormat = value;
            }
        }
            public void Initialize(string name)
            {
               if(MainStream!=null)
                     MainStream.Close();
            string path = Path.Combine(_folder, name);
              MainStream = new WaveFileWriter(name, waveFormat);
            }
            public void Start(int PlayingSoundSize, string WavFileName)
            {
                if (MainStream == null)
                    throw new Exception("класс записи не инициализирован");
                 File.Copy (WavFileName, "temp.wav");
                FileStream FileStreamWav = File.Open("temp.wav", FileMode.Open);
                byte[] test = new byte[PlayingSoundSize];
                FileStreamWav.Position = 0;
                FileStreamWav.Read(test, 0, PlayingSoundSize);

                MainStream.Write(test, 0, test.Length);

                FileStreamWav.Close();
                File.Delete("temp.wav");
        }
            public void MemoryClear()
            {
                try
                {
                    MainStream.Close();
                    GC.SuppressFinalize(MainStream);
                    GC.Collect();
                    long check3 = GC.GetTotalMemory(true);
                }
                catch (Exception)
                {
                    Debug.WriteLine("Can't vanish stream");
                }
            }

        }
    }
