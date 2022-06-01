using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    public abstract class Figure
    {
        public Point StartPoint { get; set; }
        public Point EndPoint {  get; set; }
        public Color TempColor { get; set; } = Color.Black;
        public int figureType { get; set; }
        protected Pen Pen;
        public Color Color { get; set; }
        public Color FillColor { get; set; }
        public double PenWidth { get; set; }
        public Figure() { }
        public Figure(int figureType,Point point1, Point point2, Color lineColor, Color fillColor, double width) 
        {
            StartPoint = point1;
            EndPoint = point2;
            Color = lineColor;
            FillColor = fillColor;
            PenWidth= width;
            Pen = new Pen(Color, (float)PenWidth);
            this.figureType = figureType;
        }
       
       public abstract void Draw(Graphics g);
       
    }
}
