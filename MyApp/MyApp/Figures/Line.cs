using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    class Line : Figure
    {
        public Line(Point point1, Point point2, Color lineColor, Color fillColor, double width) : base(point1,point2,lineColor,fillColor,width)
        {

        }
        public override void Draw(Graphics g)
        {
           
            Pen.Width = (float)PenWidth;
            if(TempColor == Color.Black)
            {
                Pen.Color = Color;
            }
            else
            {
                Pen.Color = TempColor;
            }
            g.DrawLine(Pen, StartPoint, EndPoint);
        }
    }
}
