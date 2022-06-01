using MyApp.Figures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyApp
{
    public partial class FormForPolygon : Form
    {
        Form1 Form1 { get; set; }
        public FormForPolygon(Form1 from)
        {
            InitializeComponent();
            Form1 = from;
            Form1.figureType = 5;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string points = textBox1.Text;
            string[] points2 = points.Split(" ");
            List<Point> poligonPoints = new();
            for (int i = 0; i < points2.Length; i++)
            {

                poligonPoints.Add(new Point(Convert.ToInt16(points2[i]), Convert.ToInt16(points2[++i])));
            }
            Form1.PoligonPoints = poligonPoints;
            Form1.figures.Add(new Polygon(4,Form1.PoligonPoints, Form1.color, Form1.fillColor, Form1.penWidth));
            Form1.comboBox1.Items.Add(Form1.figures.ElementAt(Form1.figures.Count - 1));
            Form1.updateScene();
            this.Close();
        }
    }
}
    

