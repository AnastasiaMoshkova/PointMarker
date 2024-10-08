using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;

namespace PointMarker.ViewModel
{
    class Content : ViewModelBase
    {
        public Content()
        {

            // Инициализация команды
            openFileDialogCommand = new Command(ExecuteOpenFileDialog);
            // Инициализация OpenFileDialog
            openFileDialog = new System.Windows.Forms.OpenFileDialog()
            {
                Multiselect = true,
                Filter = "Text files (*.JSON, *.TXT)|*.json;*.txt;"
            };

        }

        private ObservableCollection<Point> _pointxy;
        public ObservableCollection<Point> PointXY
        {
            get { return _pointxy; }
            set
            {
                if (_pointxy == value) return;
                _pointxy = value;
                OnPropertyChanged();
            }
        }

        readonly System.Windows.Forms.OpenFileDialog openFileDialog;

        public string FileName = "";


        readonly ICommand openFileDialogCommand;
        public ICommand OpenFileDialogCommand { get { return openFileDialogCommand; } }


        // Действие при нажатии на кнопку "Open File Dialog"
        void ExecuteOpenFileDialog()
        {
            if ((openFileDialog.ShowDialog()) == System.Windows.Forms.DialogResult.OK)
            {
                using (var stream = new FileStream(openFileDialog.FileName, FileMode.Open))
                {

                    //FileName = openFileDialog.SafeFileName;
                    //FileName = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);

                    JsonSerializer se = new JsonSerializer();

                    StreamReader re = new StreamReader(stream);
                    JsonTextReader reader = new JsonTextReader(re);
                    var DeserializedObject = se.Deserialize(reader);


                    //RaisePropertyChanged("Image");
                    OnPropertyChanged("PointXY");
                }
            }
        }

        // Реализация интерфейса INotifyPropertyChanged
        /*public event PropertyChangedEventHandler PropertyChanged;
        void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        */
    }
}
