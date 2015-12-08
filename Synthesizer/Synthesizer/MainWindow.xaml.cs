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
        }

        private void SomeKeyIsPressed(object sender, KeyEventArgs e)
        {
            if ((Mouse.Captured is Button) && ((Mouse.Captured as Button).DataContext != null))
            {
                var currentPianoKey = (((Button)Mouse.Captured).DataContext as PianoKeyViewModel);
                var pressedKey = e.Key;
                Debug.WriteLine(e.Key.ToString());
                InputBindings.Add(new KeyBinding(currentPianoKey.Play, new KeyGesture(pressedKey,ModifierKeys.Control)));
                currentPianoKey.BindedKey = pressedKey.ToString();
            }
        }

        private void whiteKey_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            PianoKeyViewModel currentKey = (sender as Button).DataContext as PianoKeyViewModel;
            currentKey.StopSound();
            Mouse.Capture(null);
        }
        private void whiteKey_Click(object sender, RoutedEventArgs e)
        {
            Mouse.Capture(sender as Button);
            PianoKeyViewModel currentKey = (sender as Button).DataContext as PianoKeyViewModel;
            currentKey.PlaySound();
        }
    }

}
