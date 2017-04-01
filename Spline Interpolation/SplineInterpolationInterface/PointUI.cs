using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SplineInterpolationInterface
{
    public class PointUI : Shape
    {
        public Point PointLocation{ get; set; }
        public Rect PointRect { get; private set; }
        public Geometry ClipShape { get; private set; }
        public int PointIndex { get; private set; }


        private Random randomColor = new Random();
        protected override Geometry DefiningGeometry
        {
            get
            {
                return this.ClipShape;
            }
        }

        public PointUI(Point p, double width, double height, double radius, double phi, int r, int g, int b, int pointIndex)
        {
            ClipShape = new EllipseGeometry(new Point(p.X, p.Y), width, height);
            Stroke = new SolidColorBrush(Color.FromArgb(255, (byte)r, /*(byte)(radius/5 * 100)*/(byte)g, (byte)b));
            StrokeThickness = 2;

            PointIndex = pointIndex;
            PointLocation = p;
            PointRect = new Rect(p.X, p.Y, width, height);
        }

    }
}
