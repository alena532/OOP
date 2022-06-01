using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Figures
{
    public class Polygon : Figure
    {
        private List<Point> points;

        public Polygon(int figureType,List<Point> points, Color lineColor, Color fillColor, double width) : base(figureType, new Point(0,0), new Point(0, 0), lineColor, fillColor, width)
        {
            this.points = points;
        }
        public override void Draw(Graphics g)
        {
            if (TempColor == Color.Black)
            {
                Pen.Color = Color;
            }
            else
            {
                Pen.Color = TempColor;
            }

            g.DrawPolygon(Pen, points.ToArray());

        }

    }
}
