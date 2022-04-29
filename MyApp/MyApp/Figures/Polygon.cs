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

        public Polygon(List<Point> points, Color lineColor, Color fillColor, double width) : base(new Point(0,0), new Point(0, 0), lineColor, fillColor, width)
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
