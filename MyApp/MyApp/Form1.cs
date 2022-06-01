using MyApp.Figures;

namespace MyApp
{
    public partial class Form1 : Form
    {
        public List<Figure> figures = new();
        List<Figure> tempFigures = new();
        List<Figure> deletedFigure = new();
        FigureCreator importCreator;
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
        public FigureCreator creator;

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
            creator= new LineCreator();
            figureType = 1;

            penWidth = 3.8;
            figureType = 1;
            //RefreshFigures(new EllipseCreator());
            
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
                // tempFigures.Add(FigureFabrik.Create(startPoint, e.Location, color, fillColor, penWidth, figureType));
                tempFigures.Add(creator.Create(startPoint, e.Location, color, fillColor, penWidth, figureType));
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
               
                
                prevColor = selectedItem.Color;
                if(FigureFabrik.Create(e.Location, endPonitFigure, Color.Aqua, selectedItem.FillColor, selectedItem.PenWidth, selectedItem.figureType)==null)
                {
                    tempFigures.Add(importCreator.Create(e.Location, endPonitFigure, Color.Aqua, selectedItem.FillColor, selectedItem.PenWidth, selectedItem.figureType));
                }
                else
                {
                    tempFigures.Add(FigureFabrik.Create(e.Location, endPonitFigure, Color.Aqua, selectedItem.FillColor, selectedItem.PenWidth, selectedItem.figureType));
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
            creator = new RectangleCreator();
        }

        private void Pencil_Click(object sender, EventArgs e)
        {
            figureType = 1;
            creator = new LineCreator();
            
        }

        private void Ellipse_Click(object sender, EventArgs e)
        {
            figureType = 3;
            creator = new EllipseCreator();
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog loadFiguresDialog = new()
            {
                Title = "Load drawing",
                Filter = "(*.json)|*.json",
            };
            if (loadFiguresDialog.ShowDialog() == DialogResult.OK)
            {
                FigureSerializer figureSerializer = new();
                if (figureSerializer.DeserializeFromJson(loadFiguresDialog.FileName) == Result.OK)
                {
                    figures.Clear();
                    tempFigures.Clear();
                    foreach (Figure figure in figureSerializer.DeserializedFigures)
                    {
                        figures.Add(figure);
                    }
                    //isCanvasEmpty = false;
                    updateScene();
                }
                else
                {
                    MessageBox.Show("Plugins are not found", "Load");
                }
            }
            else
            {
                MessageBox.Show("Error", "Load");
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFiguresDialog = new()
            {
                Title = "Save drawing",
                Filter = "(*.json)|*.json",
            };
            if (saveFiguresDialog.ShowDialog() == DialogResult.OK)
            {
                FigureSerializer.SerializeToJson(figures, saveFiguresDialog.FileName);
            }
            else
            {
                MessageBox.Show("Error", "Save");
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            OpenFileDialog importFiguresDialog = new()
            {
                Title = "Import figures",
                Filter = "(*.dll) | *.dll",
            };
            if (importFiguresDialog.ShowDialog() == DialogResult.OK)
            {
                CreatorImporter importer = new();
                if (importer.ImportFromDll(importFiguresDialog.FileName) == ImportResult.OK)
                {
                    foreach (var creator in importer.ImportedCreators)
                    {
                        this.importCreator = creator;
                        RefreshFigures(creator);
                    }
                    
                }
                else
                {
                    MessageBox.Show("Error!", "Import");
                }
            }
            

        }

        private void RefreshFigures(FigureCreator creator)
        {
           
           // figuresFlowLayoutPanel.Controls.Clear();
            //foreach (var creator in creators)
            {
                Button button = new()
                {
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.DarkGray,
                    Location = new Point
                    {
                         X = 0,
                         Y = 0
                     },
                     Size = new System.Drawing.Size(71, 68),
             
                    Height = 30,
                    //Margin = new(0, 9, 0, 5),
                    Text = "Trapezoid",
             
                };
                comboBox2.Items.Add(creator);

                // if (creator.FigureType == this.creator.FigureType)
                //{
               // panel1.Controls.Add(button);
               // this.Controls.Add(button);
                //button.BackColor = Color.LightGray;
                //}
               // button.Click += (sender, EventArgs) =>
               // {
                  
                //    this.creator = creator;
                //    figureType = 5;
               // };

                //panel1.Controls.Add(button);
            }
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            this.creator = (FigureCreator)comboBox2.SelectedItem;
            figureType = 5;

        }
    }
}