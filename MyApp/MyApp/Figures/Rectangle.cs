using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
     class Rectangles:Figure
     {
       
        public Rectangles(Point point1, Point point2, Color lineColor, Color fillColor, double width) : base(point1, point2, lineColor, fillColor, width)
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
}
