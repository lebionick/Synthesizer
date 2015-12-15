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
using Synthesizer.CORE;
using MahApps.Metro.Controls;
using Synthesizer.ViewModel;
using System.Diagnostics;


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
                if ((Mouse.Captured is Button) && ((Mouse.Captured as Button).DataContext != null))
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

    }

}
