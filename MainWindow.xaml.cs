using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
using static RenameFilesFromList.MainWindow;

namespace RenameFilesFromList
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<Fichier> fichiers
        {
            get => _fichiers;
            set
            {
                _fichiers = value;
                OnPropertyChanged();
            }
        }
        ObservableCollection<Fichier> _fichiers;

        public string directory { get => _directory?.FullName; }
        DirectoryInfo _directory;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        void Folder_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var droppedData = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                if (droppedData != null && droppedData.Any())
                {
                    // On ne traite que le premier élément déposé
                    string directoryPath = droppedData.First();
                    ReadFolder(directoryPath);
                }
            }
        }

        void ReadFolder(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                _directory = new DirectoryInfo(directoryPath);
                ReadFolder();
            }
        }

        void ReadFolder()
        {
            OnPropertyChanged("directory");
            FileInfo[] files = _directory.GetFiles();
            fichiers = new ObservableCollection<Fichier>();
            foreach (FileInfo file in files)
                fichiers.Add(new Fichier(file));
        }

        void PasteNames_Click(object sender, MouseButtonEventArgs e)
        {
            string txt = Clipboard.GetText();
            string[] newfilenames = txt.Split("\r\n");

            for (int i = 0; i < fichiers.Count; i++)            
                if (newfilenames.Length > i)                
                    fichiers[i].After = newfilenames[i];                
            
            OnPropertyChanged("fichiers");
        }

        void SetNames_Click(object sender, MouseButtonEventArgs e)
        {
            foreach (Fichier fichier in fichiers)
                fichier.SetNewName();

            ReadFolder();
            MessageBox.Show("Done");
        }
    }
}
