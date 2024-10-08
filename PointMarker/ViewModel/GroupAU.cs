using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace PointMarker.ViewModel
{
    public class GroupAU : ViewModelBase
    {
        public string Group { get; }

        public Brush Brush { get; }

        public List<double> PointFaceAU { get; }

        public bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        private List<CheckPoint> _pointAU;
        public List<CheckPoint> PointAU
        {
            get { return _pointAU; }
            set
            {
                _pointAU = value;
                OnPropertyChanged();
            }
        }


        public GroupAU(string group, Brush brush, List<double> pointFaceAU)
        {
            Group = group;
            Brush = brush;
            PointFaceAU = pointFaceAU;
            IsSelected = false;
            PointAU = new List<CheckPoint>();
        }
    }


    public class GroupsTask : ViewModelBase
    {
        public string GroupTask { get; }

        public Brush BrushTask { get; }
   
        public bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        

        public GroupsTask(string group, Brush brush)
        {
            GroupTask = group;
            BrushTask = brush;
            IsSelected = false;
            
        }
    }
}
