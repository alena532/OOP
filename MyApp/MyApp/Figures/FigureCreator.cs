using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Figures
{
    public abstract class FigureCreator
    {
        public abstract Figure Create(Point point1, Point point2, Color lineColor, Color fillColor, double width, int figureType);
    }
}
