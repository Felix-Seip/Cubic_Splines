using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SplineInterpolationInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Point p1 = new Point(-4, 16);
            Point p2 = new Point(-2, 4);
            Point p3 = new Point(0, 0);
            Point p4 = new Point(2, 4);
            Point p5 = new Point(4, 16);
            //Point p6 = new Point(1, 2.5);
            //Point p7 = new Point(2, 1);
            //Point p8 = new Point(3, 0.5);
            //Point p9 = new Point(4, 0.2941);

            List<Point> pointList = new List<Point>();
            pointList.Add(p1);
            pointList.Add(p2);
            pointList.Add(p3);
            pointList.Add(p4);
            pointList.Add(p5);
            //pointList.Add(p6);
            //pointList.Add(p7);
            //pointList.Add(p8);
            //pointList.Add(p9);

            Spline spline = new Spline(pointList);
            pointList = spline.Plot();
            Draw(pointList);
            //myCanvas.Children.Add(new PointUI(new Point(pointList[0].X/5, pointList[0].Y / 5), 5, 5, 5, 5, 0, 0, 0));
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            Line line = new Line();
            line.X1 = myCanvas.RenderTransformOrigin.X;
            line.X2 = line.X1;
            line.Y1 = 0;
            line.Y2 = myCanvas.Height;
            line.Stroke = Brushes.Black;
            myCanvas.Children.Add(line);

            Line line1 = new Line();
            line1.X1 = 0 - myCanvas.Width;
            line1.X2 = myCanvas.Width ;
            line1.Y1 = 0;
            line1.Y2 = 0;
            line1.Stroke = Brushes.Black;
            myCanvas.Children.Add(line1);
        }

        private void Draw(List<Point> pointList)
        {
            for(int i = 1; i < pointList.Count; i++)
            {
                Line line = new Line();
                line.X2 = pointList[i-1].X;
                line.Y2 = pointList[i-1].Y;
                line.X1 = pointList[i].X;
                line.Y1 = pointList[i].Y;
                line.Stroke = Brushes.Red;

                myCanvas.Children.Add(line);

                myCanvas.Children.Add(new PointUI(pointList[i], 1, 1, 1, 5, 0, 0, 0));
            }
        }
    }
}
