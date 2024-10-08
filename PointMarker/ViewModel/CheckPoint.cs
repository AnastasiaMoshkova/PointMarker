using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;


namespace PointMarker.ViewModel
{
    public class CheckPoint : ViewModelBase
    {
        private double _x { get; set; }
        private double _y { get; set; }
        private double _statsicX { get; set; }
        private double _statsicY { get; set; }
        public int Type;

        private double _scale;
        private Brush _brush;


        public double Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
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

        public double X
        {
            get { return _x; }
            set
            {
                _x = value;
                //_statsicX = X;
                OnPropertyChanged();
            }
        }

        public double Y
        {
            get { return _y; }
            set
            {
                _y = value;
                // _statsicY = Y;
                OnPropertyChanged();
            }
        }
    }
}
