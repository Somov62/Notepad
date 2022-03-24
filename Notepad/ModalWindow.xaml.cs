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
using System.Windows.Shapes;

namespace Notepad
{
    /// <summary>
    /// Логика взаимодействия для ModalWindow.xaml
    /// </summary>
    public partial class ModalWindow : Window
    {
        public ModalWindow(string path)
        {
            InitializeComponent();
            if (path is null) path = "Безымянный";
            text.Text = text.Text.Insert(text.Text.Length - 1, $"\"{path}\"");
        }

        public bool? SavingState { get; set; }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SavingState = true;
            this.Close();
        }

        private void NoSave_Click(object sender, RoutedEventArgs e)
        {
            SavingState = false;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            SavingState = null;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(btnSave);
            btnSave.Focus();
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
