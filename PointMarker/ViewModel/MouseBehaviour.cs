using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
//using System.Windows.Interactivity;
using Microsoft.Xaml.Behaviors;


namespace PointMarker.ViewModel
{
    /// <summary>
    /// Класс для работы с координатами курсора мышки
    /// </summary>
    /// <param name="MouseX">Координата мышки по оси Х</param>
    /// <param name="MouseY">Координата мышки по оси Y</param>

    public class MouseBehaviour : Behavior<Panel>
    {
        public static readonly DependencyProperty MouseYProperty = DependencyProperty.Register(
           "MouseY", typeof(double), typeof(MouseBehaviour), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty MouseXProperty = DependencyProperty.Register(
           "MouseX", typeof(double), typeof(MouseBehaviour), new PropertyMetadata(default(double)));


        public static readonly DependencyProperty WhellProperty = DependencyProperty.Register(
           "Whell", typeof(int), typeof(MouseBehaviour), new PropertyMetadata(default(int)));



        public double MouseY
        {
            get { return (double)GetValue(MouseYProperty); }
            set { SetValue(MouseYProperty, value); }
        }

        public double MouseX
        {
            get { return (double)GetValue(MouseXProperty); }
            set { SetValue(MouseXProperty, value); }
        }

        public int Whell
        {
            get { return (int)GetValue(WhellProperty); }
            set { SetValue(WhellProperty, value); }
        }



        protected override void OnAttached()
        {
            AssociatedObject.MouseMove += AssociatedObjectOnMouseMove;
            this.AssociatedObject.PreviewMouseWheel += PreviewMouseWheel;
        }


        private void AssociatedObjectOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            var pos = mouseEventArgs.GetPosition(AssociatedObject);
            MouseX = pos.X;
            MouseY = pos.Y;
        }

        private void PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Whell = e.Delta;
        }




        protected override void OnDetaching()
        {
            AssociatedObject.MouseMove -= AssociatedObjectOnMouseMove;
            this.AssociatedObject.PreviewMouseWheel -= PreviewMouseWheel;

        }
    }
}
