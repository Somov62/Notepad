using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad
{
    public static class SaveOpenTool
    {
        public static FilePath GetPathToSave()
        {
            SaveFileDialog save = new SaveFileDialog();

            save.Filter = "Текстовый документ (*.txt) |*.txt| rtf |*.rtf";
            save.Title = "Сохранение";
            save.FileName = "Новый текстовый документ";
            save.AddExtension = true;
            if (save.ShowDialog() != true) return null;
            return new FilePath(save.FileName, save.SafeFileName);
        }

        public static FilePath GetPathToOpen()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Текстовый документ (*.txt) |*.txt| rtf |*.rtf";
            open.Title = "Открытие";
            open.AddExtension = true;
            if (open.ShowDialog() != true) return null;
            return new FilePath(open.FileName, open.SafeFileName);
        }
    }
    public class FilePath
    {
        public FilePath(string path, string fileName)
        {
            Path = path;
            FileName = fileName;
        }

        public string Path { get; set; }
        public string FileName { get; set; }
    }
}
