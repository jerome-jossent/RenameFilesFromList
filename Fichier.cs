using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RenameFilesFromList
{
    public class Fichier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public string Before { get; set; }
        public string After
        {
            get => _After;
            set
            {
                _After = value;
                OnPropertyChanged();
                OnPropertyChanged("AfterFull");
            }
        }
        string _After="";

        public string AfterFull
        {
            get
            {
                if (_After != "") 
                    return fileinfo.Directory + "\\" + After + fileinfo.Extension;
                return "";
            }
        }

        FileInfo fileinfo;

        public Fichier(FileInfo fileinfo)
        {
            this.fileinfo = fileinfo;
            Before = System.IO.Path.GetFileNameWithoutExtension(fileinfo.FullName);
        }

        internal void SetNewName()
        {
            fileinfo.MoveTo(AfterFull);
        }
    }
}
