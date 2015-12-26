
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
    using System.Threading;
    using System.Threading.Tasks;
    /// <summary>
    /// Класс инкапсулирующий средства для записи из файлов wav
    /// в единый файл частями заданной длины
    /// </summary>
    public class RecordIncapsulated
    { 
        //главный записывающий поток
         WaveFileWriter MainStream;
         WaveFormat waveFormat;

         string _folder;
        /// <summary>
        /// создает экземпляр класса
        /// </summary>
        /// <param name="folder">Папка в которую будет вестись запись</param>
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
        /// <summary>
        /// необходим для начала записи частей отдельных файлов
        /// в файл, заданный параметром name
        /// </summary>
        public void Initialize(string name)
            {
               if(MainStream!=null)
                     MainStream.Close();
               //создает папку folder, если она не была создана
               if (!Directory.Exists(_folder))
                  Directory.CreateDirectory(_folder);

              string path = Path.Combine(_folder, name);
              MainStream = new WaveFileWriter(path, waveFormat);
            }
        /// <summary>
        /// записывает файл
        /// </summary>
        /// <param name="PlayingSoundSize">Количество байт для записи из файла WavFileName</param>
        public void Start(int PlayingSoundSize, string WavFileName)
            {
                if (MainStream == null)
                    throw new Exception("класс записи не инициализирован");
            //открывает поток читающий переданный файл
            FileStream FileStreamWav = File.OpenRead(WavFileName);
            //буфер для записи
            byte[] test = new byte[PlayingSoundSize];
            //читает файл в буфер test
            FileStreamWav.Read(test, 0, PlayingSoundSize);
            //пишет в поток MainStream - библиотеки NAudio
            MainStream.Write(test, 0, test.Length);

            FileStreamWav.Close();
            }
        /// <summary>
        /// Закрывает пишущий поток
        /// </summary>
        public void MemoryClear()
            {
                try
                {
                    MainStream.Close();
                    GC.SuppressFinalize(MainStream);
                    GC.Collect();
                }
                catch (Exception)
                {
                    Debug.WriteLine("Не могу закрыть поток");
                }
            }

        }
    }
