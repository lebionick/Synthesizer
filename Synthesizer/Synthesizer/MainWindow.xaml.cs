using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Synthesizer.DBO;
using MahApps.Metro.Controls;
using Synthesizer.ViewModel;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using System.Speech.Synthesis;

namespace Synthesizer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            KeyDown += SomeKeyIsPressed;
            KeyUp += SomeKeyIsUp;
        }

        private void SomeKeyIsUp(object sender, KeyEventArgs e)
        {
            foreach (Key bindedKey in keyBindingDictionary.Keys)
            {
                if (bindedKey == e.Key)
                {
                    keyBindingDictionary[bindedKey].StopSound();
                }
            }
        }

        Dictionary<Key, PianoKeyViewModel> keyBindingDictionary = new Dictionary<Key, PianoKeyViewModel>();

        private void SomeKeyIsPressed(object sender, KeyEventArgs e)
        {
            if (!e.IsRepeat)
            {
                foreach (Key bindedKey in keyBindingDictionary.Keys)
                {
                    if (bindedKey == e.Key)
                    {
                        keyBindingDictionary[bindedKey].PlaySound();
                    }
                }
                if ((Mouse.Captured is Button) && ((Mouse.Captured as Button).DataContext != null)&&
                    ((((Button)Mouse.Captured).Name == "whiteKey")||(((Button)Mouse.Captured).Name == "blackKey")))
                {
                    
                    var currentPianoKey = (((Button)Mouse.Captured).DataContext as PianoKeyViewModel);
                    var pressedKey = e.Key;
                    if (!keyBindingDictionary.ContainsKey(pressedKey))
                    {
                        foreach(Key keyVar in keyBindingDictionary.Keys)
                        {
                            if (keyBindingDictionary[keyVar].Equals(currentPianoKey))
                            {
                                keyBindingDictionary[keyVar].BindedKey = "";
                                keyBindingDictionary.Remove(keyVar);
                                break;
                            }
                        }
                        keyBindingDictionary.Add(pressedKey, currentPianoKey);
                        currentPianoKey.BindedKey = pressedKey.ToString();
                    }
                    else
                    {
                        currentPianoKey.BindedKey = pressedKey.ToString();
                        keyBindingDictionary[pressedKey].BindedKey = "";
                        keyBindingDictionary[pressedKey] = currentPianoKey;
                    }
                }
            }
            
        }

        private void PianoKey_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            PianoKeyViewModel currentKey = (sender as Button).DataContext as PianoKeyViewModel;
            currentKey.StopSound();
            Mouse.Capture(null);
        }
        private void PianoKey_Click(object sender, RoutedEventArgs e)
        {
            Mouse.Capture(sender as Button);
            PianoKeyViewModel currentKey = (sender as Button).DataContext as PianoKeyViewModel;
            currentKey.PlaySound();
        }

        private void ChooseFolder(object sender, RoutedEventArgs e)
        {
            SaveFileDialog MyDialog = new SaveFileDialog();
           // MyDialog.Filter= "|*.wav";
            if (MyDialog.ShowDialog() == true)
            {
                string folder = MyDialog.FileName;
                FileInfo info = new FileInfo(folder);
                folder = info.DirectoryName;
                (metroWindow.DataContext as MainViewModel).FolderForRecord = folder;
                return;
            }

        }

        private void PlayFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog MyDialog = new OpenFileDialog();
            MyDialog.Filter= "WAV (*.wav)|*.wav|MP3 (*.mp3)|*.mp3";

            if (MyDialog.ShowDialog()==true)
            {
                string file = MyDialog.FileName;
                MediaPlayer player = new MediaPlayer();
                player.Open(new Uri(file));
                player.Play();
            }
        }
        void Open_PopUp(object sender, object ea)
        {
            popUp.IsOpen = true;
        }
    }

}
