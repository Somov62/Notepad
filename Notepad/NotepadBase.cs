using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Notepad
{
    public class NotepadBase : INotifyPropertyChanged
    {
        private string _text;
        private string _fileName;
        private bool _isSaved;
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsSaved
        {
            get => _isSaved; 
            set
            {
                _isSaved = value;
                if (FileName is null) return;
                if (_isSaved)
                {
                    if (FileName.Contains("*"))
                    {
                        FileName = FileName.Remove(0, 1);
                    }
                    return;
                }
                if (FileName.Contains("*")) return;
                FileName = FileName.Insert(0, "*");
            }
        }
        public string PathToFile { get; set; }
        public string FileName 
        {
            get => _fileName; 
            set
            {
                _fileName = value;
                OnPropertyChanged("FileName");
            }
        }
        public string Text 
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged("Text");
                IsSaved = false;
            }
        }

        public NotepadBase()
        {
            FileName = "Безымянный";
        }
        
        public void SaveAs()
        {
            FilePath path = SaveOpenTool.GetPathToSave();
            if (path == null) return;

            PathToFile = path.Path;
            FileName = path.FileName.Remove(path.FileName.LastIndexOf("."));

            File.WriteAllText(PathToFile, Text);
            IsSaved = true;
        }

        public void Save()
        {
            if (PathToFile == default)
            {
                SaveAs();
                return;
            }
            File.WriteAllText(PathToFile, Text);
            IsSaved = true;
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
