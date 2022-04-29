using MyApp.Figures;

namespace MyApp
{
    public partial class Form1 : Form
    {
        public List<Figure> figures = new();
        List<Figure> tempFigures = new();
        List<Figure> deletedFigure = new();

        Color prevColor;

        bool Clicked = false;
        Pen pen = new Pen(Color.Black);
        Bitmap bmp = new Bitmap(100, 100);
        Graphics graphics;
        Figure figure;
        bool copy = false;

        private Point startPoint;
        private Point endPoint;

        public List<Point> PoligonPoints = new();
        public  Color color;
        public Color fillColor;

        public double penWidth;

        public int figureType;

        public Form1()
        {
            InitializeComponent();
            SetSize();
        }

        private void SetSize()
        {
            Rectangle rec=Screen.PrimaryScreen.Bounds;
            bmp = new Bitmap(rec.Width, rec.Height);
            graphics = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            color = Color.Black;
            fillColor = Color.White;

            penWidth = 3.8;
            figureType = 1;

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            
            if (figureType == 0) return;
            if (startPoint == e.Location )
            {
                Clicked = false;
                return;
            }
            var fig = tempFigures.ElementAt(tempFigures.Count - 1);
            
            figures.Add(tempFigures.ElementAt(tempFigures.Count - 1));

            comboBox1.Items.Add(tempFigures.ElementAt(tempFigures.Count - 1));
           
            tempFigures.Clear();
            
            Clicked = false;  

            if (copy)
            {
                copy = false;

                int idx = 0;
                fig.Color = prevColor;
                fig.TempColor = Color.Black;
                foreach (Figure item in figures)
                {
                    if (item.TempColor == Color.Aqua)
                    {
                        idx = figures.IndexOf(item);
                        break;
                    }
                }
                Figure selectedItem = figures.ElementAt(idx);
                selectedItem.TempColor = Color.Black;
            }
           
            updateScene();

            if (!(deletedFigure.Count==0))
            {
                deletedFigure.Clear();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Clicked) return;

            if (!copy)
            {
                tempFigures.Clear();
                tempFigures.Add(FigureFabrik.Create(startPoint, e.Location, color, fillColor, penWidth, figureType));
                updateScene();
            }
           
            if (copy)
            {
                tempFigures.Clear();
                int idx = 0;
                
                foreach(Figure item in figures)
                {
                    if (item.TempColor == Color.Aqua)
                    {
                        idx = figures.IndexOf(item);
                        break;
                    }
                }
                Figure selectedItem = figures.ElementAt(idx);
                var tempX = selectedItem.EndPoint.X - selectedItem.StartPoint.X;
                var tempY = selectedItem.EndPoint.Y - selectedItem.StartPoint.Y;

                Point endPonitFigure=new Point(e.X + tempX, e.Y+tempY);
               
                if (selectedItem is Ellipse)
                {
                    prevColor = selectedItem.Color;
                    tempFigures.Add(FigureFabrik.Create(e.Location, endPonitFigure, Color.Aqua, selectedItem.FillColor, selectedItem.PenWidth, 3));
                }

                else if(selectedItem is Rectangles)
                {
                    prevColor = selectedItem.Color;
                    tempFigures.Add(FigureFabrik.Create(e.Location, endPonitFigure, Color.Aqua, selectedItem.FillColor, selectedItem.PenWidth, 2));
                }
               
                else if (selectedItem is Line)
                {
                    prevColor = selectedItem.Color;
                    tempFigures.Add(FigureFabrik.Create(e.Location, endPonitFigure, Color.Aqua, selectedItem.FillColor, selectedItem.PenWidth, 1));
                }

                updateScene();
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
                if (figureType == 0) return;
            
                if (!Clicked)
                {
                    Clicked = true;
                    startPoint = e.Location;
                }
        }

        public void updateScene()
        {
            graphics.Clear(Color.White);
            foreach (var el in figures)
            {
                el.Draw(graphics);
            }
            foreach (var el in tempFigures)
            {
                el.Draw(graphics);
            }
            pictureBox1.Refresh();
        }


        private void button4_Click(object sender, EventArgs e)//Rectangle
        {
            figureType = 2;
        }

        private void Pencil_Click(object sender, EventArgs e)
        {
            figureType = 1;
        }

        private void Ellipse_Click(object sender, EventArgs e)
        {
            figureType = 3;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

       
        private void RedButton_Click(object sender, EventArgs e)
        {
            color = Color.Red;
        }

        private void YellowButton_Click(object sender, EventArgs e)
        {
            color = Color.Yellow;
        }

        private void GreenButton_Click(object sender, EventArgs e)
        {
            color = Color.Green;
        }

        private void BlueButton_Click(object sender, EventArgs e)
        {
            color = Color.Blue;
        }

        private void button11_Click(object sender, EventArgs e)
        {
           
        }

        private void Purplebutton_Click(object sender, EventArgs e)
        {
            color = Color.Purple;

        }

        private void BlackButton_Click(object sender, EventArgs e)
        {
            color = Color.Black;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            figures.Clear();
            tempFigures.Clear();
            comboBox1.Items.Clear();
            deletedFigure.Clear();
            updateScene();

            
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            //figure?.SetWidth(trackBar1.Value);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            penWidth = trackBar1.Value;
        }

        private void Undo_Click(object sender, EventArgs e)
        {
            if (figures.Count != 0)
            {
                deletedFigure.Add(figures.ElementAt(figures.Count - 1));
                comboBox1.Items.Remove(figures.ElementAt(figures.Count - 1));
                figures.Remove(figures.ElementAt(figures.Count - 1));
                updateScene();

            }
        }

        private void Redo_Click(object sender, EventArgs e)
        {
            if(deletedFigure.Count != 0)
            {
                figures.Add(deletedFigure.ElementAt(deletedFigure.Count - 1));
                comboBox1.Items.Add(deletedFigure.ElementAt(deletedFigure.Count - 1));
                deletedFigure.Remove(deletedFigure.ElementAt(deletedFigure.Count - 1));
                updateScene();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void RedFillColor_Click(object sender, EventArgs e)
        {
            fillColor = Color.Red;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach(var fig in figures)
            {
                if(fig.TempColor == Color.Aqua)
                {
                    fig.TempColor = Color.Black;
                }
            }

            Figure figure = (Figure)comboBox1.SelectedItem;
            figure.TempColor = Color.Aqua;
            updateScene();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int idx = -1;
            foreach (var element in figures)
            {
                if (element.TempColor == Color.Aqua)
                {
                    idx = figures.IndexOf(element);
                }
            }
            if (idx == -1) return;
            else copy = true;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int idx = -1;
            foreach(var element in figures)
            {
                if (element.TempColor == Color.Aqua)
                {
                    element.TempColor = Color.Black;
                    deletedFigure.Add(element);
                    comboBox1.Items.Remove(element);
                    idx = figures.IndexOf(element);
                }
            }
            if (idx == -1) return;
            figures.RemoveAt(idx);
            updateScene();

        }

        private void button9_Click(object sender, EventArgs e)
        {
            fillColor = Color.Yellow;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            fillColor = Color.Green;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            fillColor = Color.Blue;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            fillColor = Color.Purple;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            fillColor = Color.Black;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            fillColor = Color.Gray;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FormForPolygon polygon = new(this);
            polygon.Show();
        }
    }
}