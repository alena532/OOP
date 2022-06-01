using MyApp;
using MyApp.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trapezoid
{
    class Trapezoid2 : Figure
    {
        public Trapezoid2(int figureType, Point point1, Point point2, Color lineColor, Color fillColor, double width) : base(figureType, point1, point2, lineColor, fillColor, width)
        {

        }
        public override void Draw(Graphics g)
        {

            Pen.Width = (float)PenWidth;
            if (TempColor == Color.Black)
            {
                Pen.Color = Color;
            }
            else
            {
                Pen.Color = TempColor;
            }
            Point p3 = new Point(EndPoint.X - 50, StartPoint.Y);
            Point p4 = new Point(StartPoint.X - 50, EndPoint.Y);
            g.DrawLine(Pen, StartPoint, p3);
            g.DrawLine(Pen, p3, EndPoint);
            g.DrawLine(Pen, EndPoint, p4);
            g.DrawLine(Pen, StartPoint, p4);
            // Rectangle rect = new(){X=StartPoint.X, Y=StartPoint.Y,Width= EndPoint.X - StartPoint.X, Height=EndPoint.Y - StartPoint.Y};
            // StartPoint.X, StartPoint.Y, EndPoint.X - StartPoint.X, EndPoint.Y - StartPoint.Y
            // g.DrawLine(Pen, StartPoint, EndPoint);
            //g.DrawArc(Pen, rect,10,20);
            // g.DrawRectangle(Pen, StartPoint.X, StartPoint.Y, EndPoint.X - StartPoint.X, EndPoint.Y - StartPoint.Y);

            //Rectangle rect = new(StartPoint.X, StartPoint.Y, EndPoint.X - StartPoint.X, EndPoint.Y - StartPoint.Y);
            // g.DrawArc(Pen, (int)StartPoint.X, (int)StartPoint.Y, (int)(EndPoint.X - StartPoint.X), (int)(EndPoint.Y - StartPoint.Y), 55, 66);
        }


    }

    public class ArcCreator2 : FigureCreator
    {
        public override Figure Create(Point point1, Point point2, Color lineColor, Color fillColor, double width, int figureType)
        {
            return new Trapezoid2(figureType, point1, point2, lineColor, fillColor, width);
        }
    }
}
