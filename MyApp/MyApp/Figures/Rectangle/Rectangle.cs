using MyApp.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
     class Rectangles:Figure
     {
       
        public Rectangles(int figureType, Point point1, Point point2, Color lineColor, Color fillColor, double width) : base(figureType, point1, point2, lineColor, fillColor, width)
        {

        }

        public override void Draw(Graphics g)
        {
            Pen.Color = Color;
            if (TempColor == Color.Black)
            {
                Pen.Color = Color;
            }
            else
            {
                Pen.Color = TempColor;
            }

            if (FillColor != Color.Black)
            {
                Brush br = new SolidBrush(FillColor);
                g.FillRectangle(br, StartPoint.X, StartPoint.Y, EndPoint.X - StartPoint.X, EndPoint.Y - StartPoint.Y);
            }
            g.DrawRectangle(Pen, StartPoint.X, StartPoint.Y, EndPoint.X - StartPoint.X, EndPoint.Y - StartPoint.Y);

        }
    }

    public class RectangleCreator : FigureCreator
    {
        public override Figure Create(Point point1, Point point2, Color lineColor, Color fillColor, double width, int figureType)
        {
            return new Rectangles(figureType, point1, point2, lineColor, fillColor, width);
        }
    }
}
