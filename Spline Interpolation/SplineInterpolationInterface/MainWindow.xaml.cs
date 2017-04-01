using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SplineInterpolationInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<Point> m_lstUserPoints;
        private bool m_bIsDraggingPoint;
        private bool m_bButtonPressed;
        private PointUI m_CurrentDragPoint;
        private int m_iPointIndex;

        private const int PointWidth = 2;
        private const int PointHeight = 2;
        public MainWindow()
        {
            InitializeComponent();
            m_lstUserPoints = new List<Point>();
            m_bIsDraggingPoint = false;
            m_bButtonPressed = false;
            m_iPointIndex = 0;
        }

        private void btnCreateSplineFunction_Click(object sender, RoutedEventArgs e)
        {
            if (!m_bButtonPressed)
            {
                CreateFunction();
                m_bButtonPressed = true;
            }
        }

        private void OnAddPointToCanvas(object sender, MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(myCanvas);
            CheckDrag(p);

            if (!m_bIsDraggingPoint && !m_bButtonPressed)
            {
                m_lstUserPoints.Add(p);
                myCanvas.Children.Add(new PointUI(p, PointWidth, PointHeight, 1, 5, 255, 0, 0, m_iPointIndex));
                m_iPointIndex++;
            }
        }

        private void OnReleasePoint(object sender, MouseButtonEventArgs e)
        {
            if(m_bIsDraggingPoint)
            {
                CreateFunction();
            }
            m_bIsDraggingPoint = false;
        }

        private void CheckDrag(Point currentMousePosition)
        {
            for (int i = 0; i < myCanvas.Children.Count; i++)
            {
                if (myCanvas.Children[i].GetType() == typeof(PointUI))
                {
                    if (((PointUI)myCanvas.Children[i]).PointRect.IntersectsWith(new Rect(currentMousePosition.X, currentMousePosition.Y, 5, 5)))
                    {
                        m_bIsDraggingPoint = true;
                        m_CurrentDragPoint = (PointUI)myCanvas.Children[i];
                    }
                }
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if(m_bIsDraggingPoint)
            {
                PlayMovePointAnimation();
            }
        }

        private void CreateFunction()
        {
            ResetCanvas();
            Spline spline = new Spline(m_lstUserPoints);
            Draw(spline);
            m_lstUserPoints.Clear();
        }

        private void PlayMovePointAnimation()
        {
            myCanvas.Children.Remove(m_CurrentDragPoint);

            Point currentPos = Mouse.GetPosition(myCanvas);
            PointUI p = new PointUI(currentPos, PointWidth, PointHeight, 1, 5, 255, 0, 0, m_CurrentDragPoint.PointIndex);

            myCanvas.Children.Insert(m_CurrentDragPoint.PointIndex, p);

            m_CurrentDragPoint = p;
            UpdateData();
        }

        private void Draw(Spline spline)
        {
            myCanvas.Children.Add(spline);
        }

        private void UpdateData()
        {
            m_lstUserPoints.Clear();
            for (int i = 0; i < myCanvas.Children.Count; i++)
            {
                if (myCanvas.Children[i].GetType() == typeof(PointUI))
                {
                    m_lstUserPoints.Add(((PointUI)myCanvas.Children[i]).PointLocation);
                }
            }
        }

        private void ResetCanvas()
        {
            myCanvas.Children.Clear();

            m_iPointIndex = 0;
            for(int i = 0; i < m_lstUserPoints.Count; i++)
            {
                PointUI p = new PointUI(m_lstUserPoints[i], PointWidth, PointHeight, 1, 5, 255, 0, 0, m_iPointIndex);
                myCanvas.Children.Add(p);
                m_iPointIndex++;
            }
        }
    }
}
