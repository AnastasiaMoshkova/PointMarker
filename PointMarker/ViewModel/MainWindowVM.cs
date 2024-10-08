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
//using Prism.Commands;
using System.Windows.Data;
using System.Configuration;

//using IronPython.Hosting;//for DLHE
//using Microsoft.Scripting.Hosting;//provides scripting abilities comparable to batch files
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Python.Runtime;
using System.Collections;
using System.ComponentModel;
using System.Windows.Threading;
using System.ComponentModel;
using System.Threading;
using Microsoft.VisualBasic.FileIO;
using System.Configuration;



namespace PointMarker.ViewModel
{
    public class MainWindowVM : ViewModelBase
    {


        public double _name;
        public MainWindowVM()
        {
            string PythonPath = Path.Combine(ConfigurationManager.AppSettings["PathToPython"].ToString());
            string dllpath = Path.Combine(PythonPath, ConfigurationManager.AppSettings["PathToPythonDLL"].ToString());
            Runtime.PythonDLL = @dllpath;
            PythonEngine.PythonHome = @PythonPath;
            PythonEngine.Initialize();

            worker.DoWork += DoWork;
            worker.ProgressChanged += ProgressChanged;
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RunWorkerCompleted);
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            CurrentProgress = 0;
            _isRunning = true;

            ModeHand = false;
            ModeFace2D = false;

            MAX = true;
            LeftHand = true;
            Task1 = true;
            TimeMean = true;


            PointXY = new ObservableCollection<Point>();
            LineXY = new ObservableCollection<Line>();

            CheckP = new ObservableCollection<CheckPoint>();

            CompositeCollection coll = new CompositeCollection();
            coll.Add(new CollectionContainer() { Collection = CheckP });
            coll.Add(new CollectionContainer() { Collection = LineXY });
            Data = coll;

            Groups2D = new ObservableCollection<GroupAU>();
            GroupsTask = new ObservableCollection<GroupsTask>
            {
                new GroupsTask("Двигательная активность рук",Brushes.White),
                new GroupsTask("Мимика",Brushes.White),
            };
            
        }

        private List<double> AU01 = new List<double>();
        private List<double> AU02 = new List<double>();
        private List<double> AU04 = new List<double>();
        private List<double> AU05 = new List<double>();
        private List<double> AU06 = new List<double>();
        private List<double> AU07 = new List<double>();
        private List<double> AU09 = new List<double>();
        private List<double> AU10 = new List<double>();
        private List<double> AU12 = new List<double>();
        private List<double> AU14 = new List<double>();
        private List<double> AU15 = new List<double>();
        private List<double> AU17 = new List<double>();
        private List<double> AU20 = new List<double>();
        private List<double> AU23 = new List<double>();
        private List<double> AU25 = new List<double>();
        private List<double> AU26 = new List<double>();
        private List<double> AU28 = new List<double>();
        private List<double> AU45 = new List<double>();
        private List<double> All2D = new List<double>();



        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentProgress = e.ProgressPercentage;
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            if (_isRunning)
            {
                worker.CancelAsync();
            }

            if (!worker.CancellationPending)
            {
                // code I do not want to execute
                if (CurrentProgress >= 100)
                {
                    CurrentProgress = 0;
                    //worker.ReportProgress(CurrentProgress);
                }
                while (CurrentProgress < 100 && !_isRunning)
                {
                    worker.ReportProgress(CurrentProgress);
                    Thread.Sleep(200);
                    CurrentProgress++;
                }

                _isRunning = true;
            }
            else
            {
                e.Cancel = true;
                return;
            }

        }

        public ICommand Command
        {
            get
            {
                return _command ?? (_command = new RelayCommand(x =>
                {
                    _isRunning = !_isRunning;

                    if (!_isRunning)
                    {
                        //ExecuteOpenFileDialog();
                        //DoStuff();
                    }
                    else
                    {
                        ButtonLabel = "PAUSED";
                    }
                }));
            }
        }

        public int CurrentProgress
        {
            get { return currentProgress; }
            private set
            {
                if (currentProgress != value)
                {
                    currentProgress = value;
                    OnPropertyChanged("CurrentProgress");
                }
            }
        }

        public string ButtonLabel
        {
            get { return _buttonLabel; }
            private set
            {
                if (_buttonLabel != value)
                {
                    _buttonLabel = value;
                    OnPropertyChanged("ButtonLabel");
                }
            }
        }

        private void DoStuff()
        {
            ButtonLabel = "GO";
            worker.RunWorkerAsync();

        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // I want to jump here from the cancel point
        }

        private static bool _isRunning;
        private string _buttonLabel;
        private int currentProgress;
        private ICommand _command;
        private BackgroundWorker worker = new BackgroundWorker();


        public double scaleParams;
        private CompositeCollection _data;
        public CompositeCollection Data
        {
            get { return _data; }
            set
            {
                if (_data == value) return;
                _data = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Координаты курсора мышки
        /// </summary>
        private double _mouseY;
        private double _mouseX;

        private int _whell;

        /// <summary>
        /// Размеры окна прложения
        /// </summary>
        private double _previousSizeH;
        private double _previousSizeW;

        private double _newSizeH;
        private double _newSizeW;


        public double _heigth;
        public double _width;

        public Brush _brush;
        public bool _max;
        public bool _min;
        public bool _righthand;
        public bool _lefthand;

        public bool _task1;
        public bool _task2;
        public bool _task3;

        private double _param1;
        private double _param2;
        private double _param3;
        private double _param4;
        private double _param5;

        private double _calc1;
        private double _calc2;
        private double _calc3;
        private double _calc4;
        private double _calc5;
        private double _calc61;
        private double _calc62;
        private double _calc71;
        private double _calc72;
        private double _calc81;
        private double _calc82;

        private bool _timeMean;
        private bool _time1;
        private bool _time2;
        private bool _time3;
        private bool _time4;

        private int typeTime;
        private bool _bothHand;
        private bool _status;

        private double _previousW;
        public int taskType;
        private GroupAU _selectedGroup;
        private GroupsTask _selectedTask;



        public double Calc1
        {
            get { return _calc1; }
            set
            {
                _calc1 = value;
                OnPropertyChanged();
            }
        }

        public double Calc2
        {
            get { return _calc2; }
            set
            {
                _calc2 = value;
                OnPropertyChanged();
            }
        }

        public double Calc3
        {
            get { return _calc3; }
            set
            {
                _calc3 = value;
                OnPropertyChanged();
            }
        }

        public double Calc4
        {
            get { return _calc4; }
            set
            {
                _calc4 = value;
                OnPropertyChanged();
            }
        }

        public double Calc5
        {
            get { return _calc5; }
            set
            {
                _calc5 = value;
                OnPropertyChanged();
            }
        }

        public double Calc61
        {
            get { return _calc61; }
            set
            {
                _calc61 = value;
                OnPropertyChanged();
            }
        }

        public double Calc62
        {
            get { return _calc62; }
            set
            {
                _calc62 = value;
                OnPropertyChanged();
            }
        }

        public double Calc71
        {
            get { return _calc71; }
            set
            {
                _calc71 = value;
                OnPropertyChanged();
            }
        }

        public double Calc72
        {
            get { return _calc72; }
            set
            {
                _calc72 = value;
                OnPropertyChanged();
            }
        }

        public double Calc81
        {
            get { return _calc81; }
            set
            {
                _calc81 = value;
                OnPropertyChanged();
            }
        }

        public double Calc82
        {
            get { return _calc82; }
            set
            {
                _calc82 = value;
                OnPropertyChanged();
            }
        }

        public double Param1
        {
            get { return _param1; }
            set
            {
                _param1 = value;
                OnPropertyChanged();
            }
        }

        public double Param2
        {
            get { return _param2; }
            set
            {
                _param2 = value;
                OnPropertyChanged();
            }
        }

        public double Param3
        {
            get { return _param3; }
            set
            {
                _param3 = value;
                OnPropertyChanged();
            }
        }

        public double Param4
        {
            get { return _param4; }
            set
            {
                _param4 = value;
                OnPropertyChanged();
            }
        }

        public double Param5
        {
            get { return _param5; }
            set
            {
                _param5 = value;
                OnPropertyChanged();
            }
        }

        public double NewSizeH
        {
            get { return _newSizeH; }
            set
            {
                if (_newSizeH == value) return;
                PreviousSizeH = _newSizeH;
                _newSizeH = value;
                OnPropertyChanged();
            }
        }


        public double NewSizeW
        {
            get { return _newSizeW; }
            set
            {
                if (_newSizeW == value) return;
                PreviousSizeW = _newSizeW;
                _newSizeW = value;
                OnPropertyChanged();
            }
        }

        public double PreviousSizeH
        {
            get { return _previousSizeH; }
            set
            {
                _previousSizeH = value;
                OnPropertyChanged();
            }
        }

        public double PreviousSizeW
        {
            get { return _previousSizeW; }
            set
            {
                _previousSizeW = value;
                OnPropertyChanged();
            }
        }


        public double PrevioussWidth
        {
            get { return _previousW; }
            set
            {
                _previousW = value;
                OnPropertyChanged();
            }
        }

        public double MouseY
        {
            get { return _mouseY; }
            set
            {
                _mouseY = value;
                OnPropertyChanged();
            }
        }


        public double MouseX
        {
            get { return _mouseX; }
            set
            {
                _mouseX = value;
                OnPropertyChanged();
            }
        }

        public int Whell
        {
            get { return _whell; }
            set
            {
                _whell = value;
                OnPropertyChanged();
            }
        }


        public double Heigth
        {
            get { return _heigth; }
            set
            {
                _heigth = value;
                OnPropertyChanged();
            }
        }

        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }

        public Brush Brush
        {
            get { return _brush; }
            set
            {
                _brush = value;
                OnPropertyChanged();
            }
        }

        public bool MAX
        {
            get { return _max; }
            set
            {
                if (MIN)
                    MIN = false;
                _max = value;
                OnPropertyChanged();
            }
        }

        public bool MIN
        {
            get { return _min; }
            set
            {
                if (MAX)
                    MAX = false;
                _min = value;
                OnPropertyChanged();
            }
        }

        public bool RightHand
        {
            get { return _righthand; }
            set
            {
                if (LeftHand)
                    LeftHand = false;
                _righthand = value;
                OnPropertyChanged();
            }
        }

        public bool LeftHand
        {
            get { return _lefthand; }
            set
            {
                if (RightHand)
                    RightHand = false;
                _lefthand = value;
                OnPropertyChanged();
            }
        }

        public bool Task1
        {
            get { return _task1; }
            set
            {
                if (_task2)
                    Task2 = false;
                if (_task3)
                    Task3 = false;
                _task1 = value;

                if (_task1)
                {
                    taskType = 1;
                    Param1 = 10;
                    Param2 = 1;
                    Param3 = 0.02;
                    Param4 = 10;
                    Param5 = 5;
                }

                OnPropertyChanged();
            }
        }

        public bool Task2
        {
            get { return _task2; }
            set
            {
                if (_task1)
                    Task1 = false;
                if (_task3)
                    Task3 = false;
                _task2 = value;

                if (_task2)
                {
                    taskType = 2;
                    Param1 = 10;
                    Param2 = 1;
                    Param3 = 0.02;
                    Param4 = 10;
                    Param5 = 5;
                }
                OnPropertyChanged();
            }
        }

        public bool Task3
        {
            get { return _task3; }
            set
            {
                if (_task1)
                    Task1 = false;
                if (_task2)
                    Task2 = false;
                _task3 = value;
                if (_task3)
                {
                    taskType = 3;
                    Param1 = 0.1;
                    Param2 = 1;
                    Param3 = 0.02;
                    Param4 = 10;
                    Param5 = 5;
                }
                OnPropertyChanged();
            }
        }

        public bool TimeMean
        {
            get { return _timeMean; }
            set
            {
                if (Time1)
                    Time1 = false;
                if (Time2)
                    Time2 = false;
                if (Time3)
                    Time3 = false;
                if (Time4)
                    Time4 = false;
                _timeMean = value;
                typeTime = 0;
                OnPropertyChanged();
            }
        }

        public bool Time1
        {
            get { return _time1; }
            set
            {
                if (TimeMean)
                    TimeMean = false;
                if (Time2)
                    Time2 = false;
                if (Time3)
                    Time3 = false;
                if (Time4)
                    Time4 = false;
                _time1 = value;
                typeTime = 1;
                OnPropertyChanged();
            }
        }

        public bool Time2
        {
            get { return _time2; }
            set
            {
                if (TimeMean)
                    TimeMean = false;
                if (Time1)
                    Time1 = false;
                if (Time3)
                    Time3 = false;
                if (Time4)
                    Time4 = false;
                _time2 = value;
                typeTime = 2;
                OnPropertyChanged();
            }
        }

        public bool Time3
        {
            get { return _time3; }
            set
            {
                if (TimeMean)
                    TimeMean = false;
                if (Time2)
                    Time2 = false;
                if (Time1)
                    Time1 = false;
                if (Time4)
                    Time4 = false;
                _time3 = value;
                typeTime = 3;
                OnPropertyChanged();
            }
        }

        public bool Time4
        {
            get { return _time4; }
            set
            {
                if (TimeMean)
                    TimeMean = false;
                if (Time2)
                    Time2 = false;
                if (Time3)
                    Time3 = false;
                if (Time1)
                    Time1 = false;
                _time4 = value;
                typeTime = 4;
                OnPropertyChanged();
            }
        }

        public bool BothHand
        {
            get { return _bothHand; }
            set
            {
                _bothHand = value;
                OnPropertyChanged();
            }
        }

        public bool Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Line> _linexy;
        public ObservableCollection<Line> LineXY
        {
            get { return _linexy; }
            set
            {
                if (_linexy == value) return;
                _linexy = value;
                OnPropertyChanged();
            }
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

        private ObservableCollection<CheckPoint> _checkp;
        public ObservableCollection<CheckPoint> CheckP
        {
            get { return _checkp; }
            set
            {
                if (_checkp == value) return;
                _checkp = value;
                OnPropertyChanged();
            }
        }

     
        public List<CheckPoint> PointAU01 = new List<CheckPoint>();
        public List<CheckPoint> PointAU02 = new List<CheckPoint>();
        public List<CheckPoint> PointAU04 = new List<CheckPoint>();
        public List<CheckPoint> PointAU05 = new List<CheckPoint>();
        public List<CheckPoint> PointAU06 = new List<CheckPoint>();
        public List<CheckPoint> PointAU07 = new List<CheckPoint>();
        public List<CheckPoint> PointAU09 = new List<CheckPoint>();
        public List<CheckPoint> PointAU10 = new List<CheckPoint>();
        public List<CheckPoint> PointAU12 = new List<CheckPoint>();
        public List<CheckPoint> PointAU14 = new List<CheckPoint>();
        public List<CheckPoint> PointAU15 = new List<CheckPoint>();
        public List<CheckPoint> PointAU17 = new List<CheckPoint>();
        public List<CheckPoint> PointAU20 = new List<CheckPoint>();
        public List<CheckPoint> PointAU23 = new List<CheckPoint>();
        public List<CheckPoint> PointAU25 = new List<CheckPoint>();
        public List<CheckPoint> PointAU26 = new List<CheckPoint>();
        public List<CheckPoint> PointAU28 = new List<CheckPoint>();
        public List<CheckPoint> PointAU45 = new List<CheckPoint>();

        private string _fileSaveName;

              public string FileSaveName
        {
            get { return _fileSaveName; }
            set
            {
                if (_fileSaveName == value) return;
                _fileSaveName = value;
                OnPropertyChanged();
            }
        }


        readonly System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog()
        {
            Multiselect = true,
            Filter = "Text files (*.JSON, *.csv)|*.json;*.csv;"

        };

        public string FileName = "";
        public string FileTitle = "";

        public GroupAU SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {

                scaleCanvas = 1;
                PointXY.Clear();
                LineXY.Clear();
                CheckP.Clear();
                _selectedGroup = value;

                if (Groups2D.Count() != 0)
                {
                    DrawFaceGraf();

                    if (SelectedGroup.PointAU.Count() != 0)
                    {
                        foreach (CheckPoint cp in SelectedGroup.PointAU)
                        {
                            CheckP.Add(new CheckPoint { X = ((cp.X+4)/cp.Scale)-4, Y = cp.Y, Brush = cp.Brush, Type = cp.Type ,Scale= scaleCanvas });
                        }

                    }
                }
                OnPropertyChanged();
                OnRelatedPropertyChanged(nameof(CheckP));
            }
        }

        public bool _modeFace2D;

        public bool ModeFace2D
        {
            get { return _modeFace2D; }
            set
            {
                _modeFace2D = value;
                OnPropertyChanged();
            }
        }

        public bool _modeHand;

        public bool ModeHand
        {
            get { return _modeHand; }
            set
            {
                _modeHand = value;
                OnPropertyChanged();
            }
        }

        private bool Mode2d = false;

        public GroupsTask SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                _selectedTask = value;
                Groups2D.Clear();


                if (SelectedTask.GroupTask == "Двигательная активность рук")
                {
                    ModeFace2D = false;
                    ModeHand = true;
                    Mode2d = false;
                }
                if (SelectedTask.GroupTask == "Мимика")
                {
                    ModeHand = false;
                    ModeFace2D = true;
                    Mode2d = true;
                }

                OnPropertyChanged();
            }
        }

        public ObservableCollection<GroupAU> Groups2D { get; }
        public ObservableCollection<GroupsTask> GroupsTask { get; }


        /// <summary>
        /// The command object.
        /// </summary>
        private AsyncCommand asyncCommand1;

        /// <summary>
        /// Gets the command.
        /// </summary>
        public AsyncCommand AsyncCommand1
        {
            get { return asyncCommand1; }
        }

        // private readonly BackgroundWorker worker = new BackgroundWorker();

        private int _progress;


        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }


        private void DrawFaceGraf()
        {
            CheckP.Clear();
            PointXY.Clear();
            LineXY.Clear();
            int j = 0;
            if (Mode2d)
            {
                nScale = 50;
                foreach (double pp in SelectedGroup.PointFaceAU)
                {
                    Point p = new Point { X = j++, Y = pp };
                    PointXY.Add(p);
                }
                Heigth = SelectedGroup.PointFaceAU.Max() * 50;
            }




            Width = PointXY.Count();
            step = 1;

            for (int i = 0; i < PointXY.Count() - 1; i++)
            {
                LineXY.Add(new Line { From = new Point(PointXY[i].X, (PointXY[i].Y) * nScale), To = new Point(PointXY[i + 1].X, (PointXY[i + 1].Y) * nScale) });
            }


        }


        private void ExecuteFace2D()
        {
            ClearAll();
          
            using (TextFieldParser parser = new TextFieldParser(FileName))
            {
                int linecounter = 0;
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                string[] fieldsStart = parser.ReadFields();
                while (!parser.EndOfData)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();

                    if ((Int32.Parse(fields[4]) == 0))
                    {
                        AU01.Add(0);
                        AU02.Add(0);
                        AU04.Add(0);
                        AU05.Add(0);
                        AU06.Add(0);
                        AU07.Add(0);
                        AU09.Add(0);
                        AU10.Add(0);
                        AU12.Add(0);
                        AU14.Add(0);
                        AU15.Add(0);
                        AU17.Add(0);
                        AU20.Add(0);
                        AU23.Add(0);
                        AU25.Add(0);
                        AU26.Add(0);
                        AU28.Add(0);
                        AU45.Add(0);
                    }
                    else
                    {

                        AU01.Add(convToDouble(fields[5]) * convToDouble(fields[22]));
                        AU02.Add(convToDouble(fields[6]) * convToDouble(fields[23]));
                        AU04.Add(convToDouble(fields[7]) * convToDouble(fields[24]));
                        AU05.Add(convToDouble(fields[8]) * convToDouble(fields[25]));
                        AU06.Add(convToDouble(fields[9]) * convToDouble(fields[26]));
                        AU07.Add(convToDouble(fields[10]) * convToDouble(fields[27]));
                        AU09.Add(convToDouble(fields[11]) * convToDouble(fields[28]));
                        AU10.Add(convToDouble(fields[12]) * convToDouble(fields[29]));
                        AU12.Add(convToDouble(fields[13]) * convToDouble(fields[30]));
                        AU14.Add(convToDouble(fields[14]) * convToDouble(fields[31]));
                        AU15.Add(convToDouble(fields[15]) * convToDouble(fields[32]));
                        AU17.Add(convToDouble(fields[16]) * convToDouble(fields[33]));
                        AU20.Add(convToDouble(fields[17]) * convToDouble(fields[34]));
                        AU23.Add(convToDouble(fields[18]) * convToDouble(fields[35]));
                        AU25.Add(convToDouble(fields[19]) * convToDouble(fields[36]));
                        AU26.Add(convToDouble(fields[20]) * convToDouble(fields[37]));
                        AU28.Add(convToDouble(fields[38]));
                        AU45.Add(convToDouble(fields[21]) * convToDouble(fields[39]));
                    }
   
                    linecounter++;
                }


                Application.Current.Dispatcher.Invoke((Action)(() => {

                    if (AU01.Sum() != 0)
                        Groups2D.Add(new GroupAU("AU1 подниматель внутренней части брови", Brushes.Aqua, AU01));
                    if (AU02.Sum() != 0)
                        Groups2D.Add(new GroupAU("AU2 Подниматель внешней части брови", Brushes.Green, AU02));
                    if (AU04.Sum() != 0)
                        Groups2D.Add(new GroupAU("AU4 Опускатель брови", Brushes.SeaGreen, AU04));
                    if (AU05.Sum() != 0)
                        Groups2D.Add(new GroupAU("AU5 Подниматель верхнего века", Brushes.Yellow, AU05));
                    if (AU06.Sum() != 0)
                        Groups2D.Add(new GroupAU("AU6 Подниматель щеки", Brushes.Violet, AU06));
                    if (AU07.Sum() != 0)
                        Groups2D.Add(new GroupAU("AU7 Натягиватель века", Brushes.Brown, AU07));
                    if (AU09.Sum() != 0)
                        Groups2D.Add(new GroupAU("AU9 Сморщиватель носа", Brushes.CadetBlue, AU09));
                    if (AU10.Sum() != 0)
                        Groups2D.Add(new GroupAU("AU10 Подниматель верхней губы", Brushes.Chartreuse, AU10));
                    if (AU12.Sum() != 0)
                        Groups2D.Add(new GroupAU("AU12 Подниматель уголка губы", Brushes.Coral, AU12));
                    if (AU14.Sum() != 0)
                        Groups2D.Add(new GroupAU("AU14 Ямочка", Brushes.Crimson, AU14));
                    if (AU15.Sum() != 0)
                        Groups2D.Add(new GroupAU("AU15 Опускатель уголка губы", Brushes.DarkCyan, AU15));
                    if (AU17.Sum() != 0)
                        Groups2D.Add(new GroupAU("AU17 Подниматель подбородка", Brushes.DarkGreen, AU17));
                    if (AU20.Sum() != 0)
                        Groups2D.Add(new GroupAU("AU20 Растягиватель губ", Brushes.DarkKhaki, AU20));
                    if (AU23.Sum() != 0)
                        Groups2D.Add(new GroupAU("AU23 Натягиватель губ", Brushes.DarkMagenta, AU23));
                    if (AU25.Sum() != 0)
                        Groups2D.Add(new GroupAU("AU25 Губы разведены", Brushes.DarkOrange, AU25));
                    if (AU26.Sum() != 0)
                        Groups2D.Add(new GroupAU("AU26 Челюсть опущена", Brushes.DarkRed, AU26));
                    if (AU28.Sum() != 0)
                        Groups2D.Add(new GroupAU("AU28", Brushes.DarkSeaGreen, AU28));
                    if (AU45.Sum() != 0)
                        Groups2D.Add(new GroupAU("AU45 Моргание", Brushes.DarkTurquoise, AU45));
                }));

                var a = 0;
                worker.CancelAsync();
                _isRunning = true;
                Thread.Sleep(400);
                CurrentProgress = 100;
                Thread.Sleep(900);
                CurrentProgress = 0;
            }
        }

        private double convToDouble(string s)
        {
            return Convert.ToDouble(s, System.Globalization.CultureInfo.InvariantCulture);
        }

        private void ClearAll()
        {
            AU01.Clear();
            AU02.Clear();
            AU04.Clear();
            AU05.Clear();
            AU06.Clear();
            AU07.Clear();
            AU09.Clear();
            AU10.Clear();
            AU12.Clear();
            AU14.Clear();
            AU15.Clear();
            AU17.Clear();
            AU20.Clear();
            AU23.Clear();
            AU25.Clear();
            AU26.Clear();
            AU28.Clear();
            AU45.Clear();

        }
       
        private void ExecuteHand()
        {
            var text = File.ReadAllText(FileName);
            var mydictionary = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(text);
            double firsrFrame = 0;
            string HandTo = "";
            string HandFrom = "";
            string XX = "", YY = "", ZZ = "";
            bool flagcounter = false;
            string HandType = LeftHand ? "left hand" : "right hand";
            int k = 0;
            foreach (Dictionary<string, object> obg in mydictionary)
            {
                if (obg.ContainsKey(HandType))
                {
                    double res = 0.0;
                    var obj = obg[HandType];
                    var json = JsonConvert.SerializeObject(obj);
                    var dictionary = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, double>>>(json);
                    if (Task1 || Task2)
                    {
                        if (Task1)
                        {
                            HandTo = "FORE_TIP";
                            HandFrom = "THUMB_TIP";
                            XX = "X1";
                            YY = "Y1";
                            ZZ = "Z1";
                        }

                        if (Task2)
                        {
                            HandTo = "MIDDLE_TIP";
                            HandFrom = "CENTRE";
                            XX = "X";
                            YY = "Y";
                            ZZ = "Z";
                        }

                        var X1 = dictionary[HandTo]["X1"];
                        var Y1 = dictionary[HandTo]["Y1"];
                        var Z1 = dictionary[HandTo]["Z1"];

                        var X2 = dictionary[HandFrom][XX];
                        var Y2 = dictionary[HandFrom][YY];
                        var Z2 = dictionary[HandFrom][ZZ];

                        double X = X1 - X2;
                        double Y = Y1 - Y2;
                        double Z = Z1 - Z2;
                        double result = Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2);
                        res = Math.Sqrt(result);

                       
                    }
                    if (Task3)
                    {
                        res = dictionary["CENTRE"]["Angle"];
                    }

                    /*
                     * // if using timestame (need debug)
                    var frameobj = obg[HandType];
                    var jsonframe = JsonConvert.SerializeObject(frameobj);
                    var dictionaryinfo = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
                    double dictionaryframe = Convert.ToInt64(dictionaryinfo["info"]["timestamp"]) / 10000;
                    */
                    
                    var frameobj = obg["frame"];
                    var jsonframe = JsonConvert.SerializeObject(frameobj);
                    var dictionaryframe = JsonConvert.DeserializeObject<int>(jsonframe);
                   


                    if (!flagcounter)
                    {
                        firsrFrame = dictionaryframe;
                        flagcounter = true;
                    }

                    Point p = new Point { X = dictionaryframe - firsrFrame, Y = res };
                    PointXY.Add(p);
                }


            }


            Width = PointXY.Count();
            if (Width == 0)
            {
                MessageBox.Show(HandType + " None");
            }

            OnPropertyChanged("PointXY");

            Application.Current.Dispatcher.Invoke((Action)(() => DraWGraf()));
            worker.CancelAsync();
            _isRunning = true;
            Thread.Sleep(400);
            CurrentProgress = 100;
            Thread.Sleep(900);
            CurrentProgress = 0;

        }


        private void ExecuteOpenFileDialog()
        {

            Status = true;
            minPointTask3 = 0;
            nScale = 1;
            PointXY.Clear();
            LineXY.Clear();
            CheckP.Clear();
            if ((openFileDialog.ShowDialog()) == System.Windows.Forms.DialogResult.OK)
            {


                List<DataPointFrame> datajson = new List<DataPointFrame>();
                FileName = openFileDialog.SafeFileName;
                FileName = openFileDialog.FileName;

                FileTitle = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);

                if (!Task1 && !Task2 && !Task3)
                {
                    MessageBox.Show("TaskType is None");

                }
                else
                {
                    _isRunning = !_isRunning;

                    if (!_isRunning)
                    {

                        worker.RunWorkerAsync();
                    }
                    else
                    {
                        ButtonLabel = "PAUSED";
                    }

                    if (ModeHand)
                    {
                        th1 = new Thread(ExecuteHand);
                        th1.Start();
                    }
                    if (Mode2d)
                    {
                        Groups2D.Clear();
                        th1 = new Thread(ExecuteFace2D);
                        th1.Start();
                    }

                }
            }
            Status = false;

        }

        private double minPointTask3 = 0;
        private int nScale = 1;
        private void MakePointVector()
        {

        }
        // Действие при нажатии на кнопку "Open File Dialog"

        public void DraWGraf()
        {
            if (Task1| Task2)
            {
                minPointTask3 = 0;
            }
            if (Task3)
            {
                List<double> minPoint = new List<double>();
                foreach (Point p in PointXY)
                {
                    minPoint.Add(p.Y);
                }
                minPointTask3 = minPoint.Min();
            }

            Width = PointXY.Count();
            step = 1;
            for (int i = 0; i < PointXY.Count() - 1; i++)
            {
                LineXY.Add(new Line { From = new Point(PointXY[i].X, (PointXY[i].Y - minPointTask3) * nScale), To = new Point(PointXY[i + 1].X, (PointXY[i + 1].Y - minPointTask3) * nScale) });
            }
            OnPropertyChanged("LineXY");


        }
        public ICommand OpenFileDialogCommand => new RelayCommand(e => ExecuteOpenFileDialog());



        /// <summary>
        /// Нажатие левой клавиши мыши для базовой точки прямоугольника
        /// </summary>
        public ICommand MouseLeftDownCommand => new RelayCommand(e =>
        {
            if (ModeHand)
            {
                CheckP.Add(new CheckPoint { X = MouseX - 4, Y = MouseY - 4, Brush = MAX ? Brushes.Red : Brushes.Blue, Type = MAX ? 1 : 0 , Scale= scaleCanvas });
            }
            if (ModeFace2D)
            {
                CheckP.Add(new CheckPoint { X = MouseX - 4, Y = MouseY - 4, Brush = MAX ? Brushes.Red : Brushes.Blue, Type = MAX ? 1 : 0, Scale = scaleCanvas });
               SelectedGroup.PointAU= CheckP.ToList();
                
            }
        });

        /// <summary>
        /// Проверка размеров прямоугольник апо порогу
        /// </summary>
        public ICommand MouseLeftUpCommand => new RelayCommand(e =>
        {

        });

        /// <summary>
        /// Нажатие правой клавиши мыши для закрепления прямоугольника на форме
        /// </summary>
        public ICommand MouseRightClickCommand => new RelayCommand(e =>
        {
            
            var mousePos = new Point(MouseX, MouseY);
            if (ModeHand)
            {
                CheckPoint _currentPoint = CheckP.FirstOrDefault(f => new Rect(f.X, f.Y, 8, 8).Contains(mousePos));
                CheckP.Remove(_currentPoint);
            }

            if (ModeFace2D)
            {
                CheckPoint _currentPoint = CheckP.FirstOrDefault(f => new Rect(f.X, f.Y, 8, 8).Contains(mousePos));
                CheckP.Remove(_currentPoint);
                SelectedGroup.PointAU= CheckP.ToList();
            }

        });


        /// <summary>
        /// Проверка выхода курсора мыши за форму
        /// </summary>
        public ICommand MouseLeaveCommand => new RelayCommand(e =>
        {

        });

        /// <summary>
        /// Проверка выхода курсора мыши за форму
        /// </summary>
        /// 
        public ICommand MouseWheelCommand => new RelayCommand(e => MouseWheelExecute());
        public int step = 1;
        private double scaleCanvas=1;

        private void MouseWheelExecute()
        {

            int a = Whell;

            if (a > 0)
            {
                PrevioussWidth = Width;
                Width = Width + PointXY.Count();

                var scale = Width / PrevioussWidth;
                step = step + 1;

                scaleCanvas = step;

                foreach (CheckPoint cp in CheckP)
                {
                    cp.X = (cp.X + 4) * scale - 4;
                    cp.Scale = scaleCanvas;
                    //cp.X = cp.X+ cp.X / step;

                }

            }

            if (a < 0)
            {
                if (step != 1)
                {
                    PrevioussWidth = Width;
                    Width = Width - PointXY.Count();
                    var scale = Width / PrevioussWidth;
                    step = step - 1;

                    scaleCanvas = step;

                    foreach (CheckPoint cp in CheckP)
                    {
                        //cp.X = cp.X * scale;
                        cp.X = (cp.X + 4) * scale - 4;
                        cp.Scale = scaleCanvas;
                        //cp.X = cp.X - cp.X / step;
                    }

                }
                else
                {
                    step = 1;
                }
            }

            LineXY.Clear();
            for (int i = 0; i < PointXY.Count() - 1; i++)
            {
                LineXY.Add(new Line { From = new Point(step * PointXY[i].X, (PointXY[i].Y - minPointTask3) * nScale), To = new Point(step * PointXY[i].X + step, (PointXY[i + 1].Y - minPointTask3) * nScale) });

            }

            OnPropertyChanged("CheckP");
            OnPropertyChanged("LineXY");
        }
       

        /// <summary>
        /// Изменение размеров прямоугольников на форме
        /// </summary>
        private void SizeChanged()
        {
            //Heigth = 100;
            /* int a = Whell;
             double xChange = 1, yChange = 1;
             if (PreviousSizeW != 0 && PreviousSizeH != 0)
             {
                 xChange = NewSizeW / PreviousSizeW;
                 yChange = NewSizeH / PreviousSizeH;

                 Width = Width* xChange;
                 Heigth = Heigth * yChange;

                 LineXY.Clear();

                 for (int i = 0; i < PointXY.Count() - 1; i++)
                 {


                     LineXY.Add(new Line { From = new Point(PointXY[i].X * xChange, PointXY[i].Y * yChange), To = new Point(PointXY[i].X * xChange, PointXY[i + 1].Y * yChange) });

                 }

                 foreach(CheckPoint cp in CheckP)
                 {
                     cp.X = cp.X * xChange;
                     cp.Y = cp.Y * yChange;
                 }
             }

             if (NewSizeW == 0 || NewSizeH == 0)
             {
                 LineXY.Clear();
                 CheckP.Clear();
             }
             */

        }

        public ICommand SizeChangedCommand => new RelayCommand(e => SizeChanged());


        public List<int> listznachMIN = new List<int>();
        public List<int> listznachMAX = new List<int>();

        public void DrawPoint(dynamic listznach)
        {
            int i = 0;
            Brush colour;
            int type;
            foreach (dynamic n in listznach)
            {
                foreach (int p in n)
                {
                    if (i == 0)
                    {
                        colour = Brushes.Blue;
                        type = 0;
                    }
                    else
                    {
                        colour = Brushes.Red;
                        type = 1;
                    }

                    CheckP.Add(new CheckPoint { X = PointXY[p].X - 4, Y = ((PointXY[p].Y - minPointTask3) * nScale) - 4, Brush = colour, Type = type, Scale = scaleCanvas });
                    //i++;
                }
                i++;
            }
            OnPropertyChanged("CheckP");
        }

        public void DrawPointHand(List<int> listznachMIN, List<int> listznachMAX)
        {
            int i = 0;
            Brush colour;
            int type;
            foreach (int p in listznachMIN)
            {
                colour = Brushes.Blue;
                type = 0;
                CheckP.Add(new CheckPoint { X = PointXY[p].X - 4, Y = ((PointXY[p].Y - minPointTask3) * nScale) - 4, Brush = colour, Type = type, Scale = scaleCanvas });
                //i++;
            }

            foreach (int p in listznachMAX)
            {
                colour = Brushes.Red;
                type = 1; ;
                CheckP.Add(new CheckPoint { X = PointXY[p].X - 4, Y = ((PointXY[p].Y - minPointTask3) * nScale) - 4, Brush = colour, Type = type, Scale = scaleCanvas });
                //i++;
            }

            OnPropertyChanged("CheckP");
            listznachMIN.Clear();
            listznachMAX.Clear();
        }


        public void DrawPointFace(List<int> listznachMIN, List<int> listznachMAX)
        {
            int i = 0;
            Brush colour;
            int type;
            foreach (int p in listznachMIN)
            {
                colour = Brushes.Blue;
                type = 0;
                CheckP.Add(new CheckPoint { X = PointXY[p].X - 4, Y = ((PointXY[p].Y - minPointTask3) * nScale) - 4, Brush = colour, Type = type, Scale = scaleCanvas });
               
                //i++;
            }

            foreach (int p in listznachMAX)
            {
                colour = Brushes.Red;
                type = 1; ;
                CheckP.Add(new CheckPoint { X = PointXY[p].X - 4, Y = ((PointXY[p].Y - minPointTask3) * nScale) - 4, Brush = colour, Type = type, Scale = scaleCanvas });
                
                //i++;
            }

            SelectedGroup.PointAU = CheckP.ToList();

            OnPropertyChanged("CheckP");
            listznachMIN.Clear();
            listznachMAX.Clear();
        }

        public Thread th1;

        private void doPython()
        {
            scaleCanvas = 1;
            _isRunning = false;
            if (worker.IsBusy)
            {
                worker.CancelAsync();
                Thread.Sleep(400);
            }
            CheckP.Clear();
            LineXY.Clear();
            DraWGraf();

            _isRunning = true;
            _isRunning = !_isRunning;

            if (!_isRunning && !worker.IsBusy)
            {
                DoStuff();
            }

            try
            {


                using (Py.GIL())
                {
                    Thread th2 = new Thread(doPythonTread);
                    th2.Start();
                }
                
            }
            finally
            {
                //PythonEngine.EndAllowThreads(ts);
            }


        }

        public void doPythonTread()
        {
    
            var ts = PythonEngine.BeginAllowThreads();

            if (ModeHand)
            {
                using (Py.GIL())
                {
                    try
                    {
                        dynamic hand_processing = Py.Import("hand_processing");
                        dynamic process = hand_processing.processing;

                        int typeHand = LeftHand ? 0 : 1;

                        var listznach = process(FileName, (int)Param1, typeHand, taskType, (int)Param2, (int)Param5, (int)Param4, (double)Param3);

                        int i = 0;

                        foreach (dynamic n in listznach)
                        {
                            foreach (int p in n)
                            {
                                if (i == 0)
                                {
                                    listznachMIN.Add(p);
                                }
                                else
                                {
                                    listznachMAX.Add(p);
                                }
                            }
                            i++;
                        }

                        Application.Current.Dispatcher.Invoke((Action)(() => DrawPointHand(listznachMIN, listznachMAX)));

                   

                    }
                    catch (Exception ex)
                    {
                        // Handled Exception
                    }


                }
                PythonEngine.EndAllowThreads(ts);
                Status = false;
                _isRunning = false;
                worker.CancelAsync();
                CurrentProgress = 100;
                Thread.Sleep(400);
                CurrentProgress = 0;
            }


            if (Mode2d)
            {
                using (Py.GIL())
                {
                    try
                    {

                        dynamic face_processing = Py.Import("face_processing");
                        dynamic process = face_processing.process;
                        var listznach = process(SelectedGroup.PointFaceAU, (double)Param3,(int)Param4,(int)Param5);

                        int i = 0;

                        foreach (dynamic n in listznach)
                        {
                            foreach (int p in n)
                            {
                                if (i == 0)
                                {
                                    listznachMIN.Add(p);
                                }
                                else
                                {
                                    listznachMAX.Add(p);
                                }
                            }
                            i++;
                        }


                        Application.Current.Dispatcher.Invoke((Action)(() => DrawPointFace(listznachMIN, listznachMAX)));
                        

                    }
                    catch (Exception ex)
                    {

                    }


                }
                PythonEngine.EndAllowThreads(ts);
                Status = false;
                _isRunning = false;
                worker.CancelAsync();
                CurrentProgress = 100;
                Thread.Sleep(400);
                CurrentProgress = 0;
            }
        }

        public ICommand TestCommand => new RelayCommand(e => doPython());


        private bool isChecked;
        private ICommand checkCommand;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        public ICommand CheckCommand
        {
            get
            {
                if (checkCommand == null)
                    checkCommand = new RelayCommand(i => Checkprocess(i), null);
                return checkCommand;
            }
            set
            {
                checkCommand = value;
                OnPropertyChanged("CheckCommand");
            }
        }
        public void Checkprocess(object sender)
        {
            //this DOES react when the checkbox is checked or unchecked
        }

        
        private bool CheckMIN()
        {
            var checkvector = CheckP.OrderBy(p => p.X);
            if (checkvector.First().Type==1)
            {
                checkvector.First().Brush= Brushes.Green;
                MessageBox.Show("начните с минимума");
                return false;
            }
            return true;
         
        }
        private bool CheckMAXMIN()
        {
            bool counter = false;
            var oldvalue = 1;
            var checkvector = CheckP.OrderBy(p => p.X);
             foreach (CheckPoint obg in checkvector)
            {
                
             if ((obg.Type+ oldvalue)!= 1)
             {
                 oldvalue = obg.Type;
                 obg.Brush = Brushes.Green;
                 counter = true;
                }
                oldvalue = obg.Type;

            }
             if (counter)
            {
                MessageBox.Show("точки не чередуются");
                return false;
            }
            else {
                return true;
            }
         
            
        }
        private void SaveJson()
        {
            if (CheckMIN())
            {
               
                    if (ModeHand)
                    {
                    if (CheckMAXMIN())
                    {
                        var pointjson = CheckP.Select(p => new CheckPoint() { X = (int)((p.X + 4) * PointXY.Count() / Width), Y = (p.Y + 4) / nScale + minPointTask3, Brush = p.Brush, Type = p.Type, Scale = p.Scale });
                        string json = JsonConvert.SerializeObject(pointjson, Formatting.None,
                                    new JsonSerializerSettings()
                                    {
                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                    });

                        string PathResult = Path.Combine(Directory.GetCurrentDirectory(), "Result");
                        string FileName = Path.Combine(PathResult, FileTitle + "_point_" + Param3.ToString("0.0000") + "_" + Param4.ToString("0") + "_" + Param5.ToString("0") + ".json");
                        if (!Directory.Exists(PathResult))
                        {
                            Directory.CreateDirectory(PathResult);
                        }
                        System.IO.File.WriteAllText(FileName, json);

                    }
                    }
               

                    if (ModeFace2D)
                    {
                        Dictionary<string, List<CheckPoint>> pointjson = new Dictionary<string, List<CheckPoint>>();
                        foreach (GroupAU AU in Groups2D)
                        {
                            var point = AU.PointAU.Select(p => new CheckPoint() { X = (int)((p.X + 4) / p.Scale), Y = (p.Y + 4) / nScale, Brush = p.Brush, Type = p.Type, Scale = p.Scale });
                            pointjson.Add(AU.Group, point.ToList());
                        }

                        string json = JsonConvert.SerializeObject(pointjson, Formatting.None,
                                    new JsonSerializerSettings()
                                    {
                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                    });
                        string PathResult = Path.Combine(Directory.GetCurrentDirectory(), "Result");
                        string FileName = Path.Combine(PathResult, FileTitle + ".json");
                        if (!Directory.Exists(PathResult))
                        {
                            Directory.CreateDirectory(PathResult);
                        }
                        System.IO.File.WriteAllText(FileName, json);

                    }
                
            }
        }

        private void SaveExecute()
        {
            SaveJson();
        }

        public ICommand SaveCommand => new RelayCommand(e => SaveExecute());

        private void CalculateExecute()
        {
            List<int> MaxPointX=new List<int>();
            List<int> MinPointX=new List<int>();
            List<double> MaxPointY = new List<double>();
            List<double> MinPointY = new List<double>();
            List<double> speedSupination = new List<double>();
            List<double> speedPronation = new List<double>();
            List<double> friquency = new List<double>();
            List<double> amplitude = new List<double>();
            List<double>  maxminSpeedPronation = new List<double>();
            List<double>  maxminSpeedSupination = new List<double>();

            if (TimeMean)
            {
                foreach (CheckPoint p in CheckP)
                {
                    if (p.Type == 0)
                    {
                        MinPointX.Add((int)p.X + 4);
                        MinPointY.Add((p.Y + 4) / nScale + minPointTask3);
                    }
                    else
                    {
                        MaxPointX.Add((int)p.X + 4);
                        MaxPointY.Add((p.Y + 4) / nScale + minPointTask3);
                    }
                }
            }

            if(Time1||Time2||Time3||Time4)
            {

                int numberpoint =PointXY.Count()/4;
                int startX = numberpoint * (typeTime - 1);
                int stopX= numberpoint * (typeTime) - 1;

                var start = PointXY[startX].X;
                var stop = PointXY[stopX].X;


                foreach (CheckPoint p in CheckP)
                {
                    if ((p.Type == 0)&&(((int)p.X + 4)> start)&& (((int)p.X + 4) < stop))
                    {
                        MinPointX.Add((int)p.X + 4);
                        MinPointY.Add((p.Y + 4) / nScale + minPointTask3);
                    }
                    if ((p.Type == 1) && (((int)p.X + 4) > start) && (((int)p.X + 4) < stop))
                    {
                        MaxPointX.Add((int)p.X + 4);
                        MaxPointY.Add((p.Y + 4) / nScale + minPointTask3);
                    }
                }

            }

            if (MaxPointX.Count() != 0 && MinPointX.Count() != 0)
            {
                Calc1 = MaxPointY.Count();

                var count = Math.Min(MaxPointX.Count(), MinPointY.Count());

                for (int i = 0; i < count - 1; i++)
                {
                    speedSupination.Add(Math.Abs(MaxPointY[i] - MinPointY[i + 1]) / Math.Abs(MaxPointX[i] - MinPointX[i]));
                }

                Calc2 = speedSupination.Average();


                for (int i = 0; i < count; i++)
                {
                    speedPronation.Add(Math.Abs(MinPointY[i] - MaxPointY[i]) / Math.Abs(MinPointX[i] - MaxPointX[i]));
                }
                Calc3 = speedPronation.Average();


                for (int i = 0; i < count - 1; i++)
                {

                    friquency.Add(1.0 / (Math.Abs(MinPointX[i] - MinPointX[i + 1])));
                }
                Calc4 = getStandardDeviation(friquency);

                for (int i = 0; i < count - 1; i++)
                {
                    amplitude.Add(Math.Abs(MinPointY[i] - MaxPointY[i + 1]));
                }
                Calc5 = getStandardDeviation(amplitude);

                Calc61 = MaxPointY.Max();
                Calc62 = MaxPointY.Min();

                for (int i = 0; i < count; i++)
                {
                    maxminSpeedSupination.Add(Math.Abs(MinPointY[i] - MaxPointY[i]) / Math.Abs(MinPointX[i] - MaxPointX[i]));
                }



                Calc71 = maxminSpeedSupination.Max();
                Calc72 = maxminSpeedSupination.Min();

                for (int i = 0; i < count - 1; i++)
                {
                    maxminSpeedPronation.Add(Math.Abs(MaxPointY[i] - MinPointY[i + 1]) / Math.Abs(MaxPointX[i] - MinPointX[i]));
                }

                Calc81 = maxminSpeedPronation.Max();
                Calc82 = maxminSpeedPronation.Min();
            }
            else
            {
                MessageBox.Show("Ключевые точки за выбранный период отсутствуют");
            }

        }

        public ICommand CalculateCommand => new RelayCommand(e => CalculateExecute());

        private double getStandardDeviation(List<double> doubleList)
        {
            double average = doubleList.Average();
            double sumOfDerivation = 0;
            foreach (double value in doubleList)
            {
                sumOfDerivation += (value) * (value);
            }
            double sumOfDerivationAverage = sumOfDerivation / (doubleList.Count - 1);
            return Math.Sqrt(sumOfDerivationAverage - (average * average));
        }
    }







}

