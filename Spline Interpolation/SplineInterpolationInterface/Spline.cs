using Math_Collection.LGS;
using Math_Collection.LinearAlgebra.Matrices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SplineInterpolationInterface
{
    public class Spline
    {
        private List<Point> pointList;
        public Spline(List<Point> arrPointList)
        {
            pointList = arrPointList;
        }

        public List<Point> Plot()
        {
            List<double> hList = CalculateH();
            List<double> gList = CalculateG(hList);
            Matrix m = CreateSplineMatrix(hList);

            double[] bla1 = gList.ToArray();
            LGS lgs = new LGS(m, new Math_Collection.LinearAlgebra.Vectors.Vector(bla1));
            Math_Collection.LinearAlgebra.Vectors.Vector outcome = lgs.Solve(LGS.SolveAlgorithm.Gauß);

            List<double> bList = CalculateB(outcome, hList);
            List<double> dList = CalculateD(outcome, hList);
            return SolveSplineFunctions(CreateSplineFunctions(bList, outcome, dList));
        }

        private List<Point> SolveSplineFunctions(List<string> splineFunctionList)
        {
            List<Point> graphPoints = new List<Point>(); 
            int splineFunctionIndex = 0;

            for (int i = 0; i < pointList.Count-1; i++)
            {
                double x = pointList[i].X;
                while (x < pointList[i + 1].X)
                {
                    double y = FunctionParser.FunctionParser.Parse(splineFunctionList[splineFunctionIndex], x);
                    graphPoints.Add(new Point(x * 30, y * 30));
                    x+=0.25;
                }
                splineFunctionIndex++;
            }
            return graphPoints;
        }

        private List<string> CreateSplineFunctions(List<double> bList, Math_Collection.LinearAlgebra.Vectors.Vector c, List<double> dList)
        {
            List<string> splineFunctionList = new List<string>();
            for (int i = 1; i < pointList.Count; i++)
            {
                splineFunctionList.Add(pointList[i - 1].Y + "+" + bList[i - 1] + "*x+" + c[i - 1] + "*x^2+" + dList[i - 1] + "*x^3");
            }
            return splineFunctionList;
        }


        private Matrix CreateSplineMatrix(List<double> hList)
        {
            //Matrix m = new Matrix();
            double[,] splineMatrixValues = new double[pointList.Count, pointList.Count];
            for (int n = 0; n < pointList.Count; n++)
            {
                for (int j = 0; j < pointList.Count; j++)
                {
                    if (j == 0 && n == 0)
                    {
                        splineMatrixValues[n, j] = 1;
                    }
                    else if (n == pointList.Count - 1 && j == pointList.Count - 1)
                    {
                        splineMatrixValues[n, j] = 1;
                    }
                    else if ((n == 0 || j == 0) || (n == pointList.Count - 1 || j == pointList.Count - 1))
                    {
                        splineMatrixValues[n, j] = 0;
                    }
                    else if (j == n)
                    {
                        splineMatrixValues[n + 1, j] = hList[n];
                        splineMatrixValues[n, j + 1] = hList[n];
                        splineMatrixValues[n, j] = 2 * (hList[n - 1] + hList[n]);
                    }
                }
            }
            
            return new Matrix(splineMatrixValues);
        }

        private List<double> CalculateH()
        {
            List<double> h = new List<double>();
            for (int i = 1; i < pointList.Count; i++)
            {
                h.Add(pointList[i].X - pointList[i - 1].X);
            }
            return h;
        }

        private List<double> CalculateB(Math_Collection.LinearAlgebra.Vectors.Vector c, List<double> hList)
        {
            List<double> bList = new List<double>();
            for (int i = 1; i < pointList.Count; i++)
            {
                double b = ((pointList[i].Y - pointList[i - 1].Y) / hList[i - 1]) - ((hList[i - 1] / 3) * (2 * c[i - 1] + c[i]));
                bList.Add(b);
            }
            return bList;
        }

        private List<double> CalculateD(Math_Collection.LinearAlgebra.Vectors.Vector outcome, List<double> hList)
        {
            List<double> dList = new List<double>();
            for (int i = 0; i < outcome.Size - 1; i++)
            {
                dList.Add((outcome[i + 1] - outcome[i]) / (3 * hList[i]));
            }

            return dList;
        }

        private List<double> CalculateG(List<double> hList)
        {
            List<double> gList = new List<double>();
            gList.Add(0);
            for (int i = 2; i < pointList.Count; i++)
            {
                double bla = (pointList[i - 1].Y - pointList[i - 2].Y) / hList[i - 2];
                double bla1 = (pointList[i].Y - pointList[i - 1].Y) / hList[i - 1];
                gList.Add(3 * (bla1 - bla));
            }
            gList.Add(0);
            return gList;
        }
    }
}
