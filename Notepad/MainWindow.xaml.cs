using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using Microsoft.Win32;
using System.Windows.Forms;

namespace Notepad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotepadBase _model;

        public MainWindow()
        {
            InitializeComponent();
            _model = new NotepadBase();
            _model.FileName = "Безымянный";
            _model.IsSaved = true;
            this.DataContext = _model;

            if (!File.Exists(@"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"))
            {
                btnBing.Visibility = Visibility.Hidden;
            }
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            _model.SaveAs();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _model.Save();
        }

        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Environment.CurrentDirectory + "\\Notepad.exe");
        }

        private void CreateList_Click(object sender, RoutedEventArgs e)
        {
            if (SaveQuestion())
            {
                _model = new NotepadBase();
                this.DataContext = _model;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (SaveQuestion())
            {
                this.Close();
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (!SaveQuestion()) return;
            FilePath path = SaveOpenTool.GetPathToOpen();
            if (path == null) return;
            _model = new NotepadBase();
            _model.Text = File.ReadAllText(path.Path);
            _model.FileName = path.FileName.Remove(path.FileName.LastIndexOf("."));
            _model.IsSaved = true;
            _model.PathToFile = path.Path;
            this.DataContext = _model;
        }

        private bool SaveQuestion()
        {
            if (!_model.IsSaved)
            {
                ModalWindow window = new ModalWindow(_model.PathToFile);
                window.Owner = this;
                window.ShowDialog();
                if (window.SavingState is null) return false;
                if (window.SavingState == true)
                {
                    _model.Save();
                }
            }
            return true;
        }

        private void SettingsPage_Click(object sender, RoutedEventArgs e)
        {
            PageSetupDialog dialog = new PageSetupDialog();
            dialog.PageSettings = new System.Drawing.Printing.PageSettings();
            dialog.ShowDialog();
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.PrintDialog dialog = new();
            dialog.ShowDialog();
        }

        private void Font_Click(object sender, RoutedEventArgs e)
        {
            FontDialog dialog = new();
            dialog.ShowDialog();
        }

        private void Undo_Click(object sender, RoutedEventArgs e) => textbox.Undo();
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            int caretIndex = textbox.SelectionStart;
            textbox.Text = textbox.Text.Remove(textbox.SelectionStart, textbox.SelectionLength);
            textbox.CaretIndex = caretIndex;
        }

        private void Textbox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (textbox.SelectionLength == 0)
            {
                btnCopy.IsEnabled = false;
                btnBing.IsEnabled = false;
                btnCut.IsEnabled = false;
                btnDelete.IsEnabled = false;
                return;
            }
            btnCopy.IsEnabled = true;
            btnBing.IsEnabled = true;
            btnCut.IsEnabled = true;
            btnDelete.IsEnabled = true;
        }

        private void FindBing_Click(object sender, RoutedEventArgs e)
        {
            var request = textbox.SelectedText;
            Process.Start(@"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe", "https://www.bing.com/search?q=" + Uri.EscapeUriString(request));
        }

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            string text = _model.Text;
            int countRow = 0;

            var lines = text.Split("\r\n");
            countRow = lines.Length - 1;
            while (text.IndexOf('\n') == -1)
            {
                countRow++;
                text = text.Substring(text.IndexOf('\n') + 1);
            }
            GoWindow window = new GoWindow(countRow);
            window.Owner = this;
            window.ShowDialog();
            if (window.RowNumber == -1) return;
            int startIndex = 0;
            for (int i = 1; i < window.RowNumber; i++)
            {
                textbox.CaretIndex = _model.Text.IndexOf('\n', startIndex);
                startIndex = _model.Text.IndexOf('\n', startIndex) + 1;
            }
        }

        private void DeleteRow_Click(object sender, RoutedEventArgs e)
        {
            string text = _model.Text;
            int rowIndex = text.LastIndexOf('\n', textbox.CaretIndex) + 1;
            _model.Text = text.Remove(rowIndex, text.IndexOf('\n', rowIndex) - rowIndex + 1);
            textbox.CaretIndex = rowIndex;
        }
    }
}
