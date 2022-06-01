using MyApp.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    class Line : Figure
    {
        public Line(int figureType, Point point1, Point point2, Color lineColor, Color fillColor, double width) : base(figureType, point1,point2,lineColor,fillColor,width)
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

    public class LineCreator : FigureCreator
    {
        public override Figure Create(Point point1, Point point2, Color lineColor, Color fillColor, double width, int figureType)
        {
            return new Line(figureType, point1, point2, lineColor, fillColor, width);
        }
    }
}
