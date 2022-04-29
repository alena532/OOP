using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    public class FigureFabrik
    {
        public static Figure? Create(Point point1, Point point2, Color lineColor, Color fillColor, double width, int figureType)
        {
            switch (figureType)
            {
                case 1:
                    return new Line(point1, point2, lineColor, fillColor, width);
                    break;
                case 2:
                   return new Rectangles(point1, point2, lineColor, fillColor, width);
                   break;
                case 3:
                   return new Ellipse(point1, point2, lineColor, fillColor, width);
                   break;
                    
            }
            return null;
        }
    }
}
