using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace SplineInterpolationInterface
{
    /// <summary>
    /// Interaction logic for MyCanvas.xaml
    /// </summary>
    public partial class MyCanvas : Canvas
    {
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            Point middle = new Point(arrangeSize.Width / 2, arrangeSize.Height / 2);

            double x = 0.0, y = 0.0;
            foreach (UIElement element in base.InternalChildren)
            {
                if (element == null)
                    continue;

                element.RenderTransform = new MatrixTransform(1, 0, 0, -1, 0, 0);
                element.Arrange(new Rect(new Point(middle.X + x, middle.Y + y), element.DesiredSize));
            }
            return arrangeSize;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
            foreach (UIElement element in base.InternalChildren)
            {
                if (element != null)
                {
                    element.Measure(availableSize);
                }
            }
            return new Size();
        }

        public MyCanvas()
        {
            InitializeComponent();
        }
    }
}
