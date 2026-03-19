using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Minesweeper
{
    public class BaseField : System.Windows.Forms.Panel
    {
        public readonly int width, height, minesCount;

        private const float lineWidth = 0.03f;

        protected Cell[,] field;
        protected Panel panelForCells = new Panel();

        private bool showProbability = false;
        private bool showPercentage = false;
        private Font mainFont;
        private Font percentageFont;
        private Graphics graphics;
        private float cellWidth;
        private float cellHeight;
        private bool allowMove = false;
        private Point lastPos;


        public bool ShowProbability
        {
            get { return showProbability; }
            set
            {
                showProbability = value;
                updateAllCells();
            }
        }
        public bool ShowPercentage
        {
            get
            { return showPercentage; }

            set
            {
                showPercentage = value;
                if (showProbability)
                    updateAllCells();
            }
        }

        public bool allowDraw { get; set; } = false;




        public BaseField(int width, int height, int mineCount)
        {
            if (mineCount >= width * height)
                throw new Exception("Can't create a field with more mines than the number of cells!");

            this.width = width;
            this.height = height;
            this.minesCount = mineCount;
            this.MouseDown += mouseDown;
            this.MouseMove += mouseMove;
            this.MouseUp += mouseUp;
            this.MouseWheel += mouseWheel;
            this.BackColor = Color.Aqua;

            panelForCells.Parent = this;
            panelForCells.SizeChanged += panelForCellsResize;
            panelForCells.Paint += PanelForCells_Paint;
            panelForCells.Enabled = false;


            field = new Cell[height, width];
            for (int i = 0; i < height; ++i)
                for (int j = 0; j < width; ++j)
                {
                    field[i, j] = new Cell();
                    field[i, j].parentField = this;
                    field[i, j].i = i;
                    field[i, j].j = j;
                }

            panelForCellsResize(null, null);
        }


        protected void updateAllCells()
        {
            if (allowDraw)
            {
                graphics.Clear(this.BackColor);
                for (int i = 0; i < height; ++i)
                    for (int j = 0; j < width; ++j)
                        field[i, j].draw();
            }
        }


        public int[,] getInfo()
        {
            int[,] res = new int[height, width];
            for (int i = 0; i < height; ++i)
                for (int j = 0; j < width; ++j)
                    if (field[i, j].opened)
                        res[i, j] = field[i, j].minesNear;
                    else
                        res[i, j] = -1;
            return res;
        }

        protected virtual void cellClick(int i, int j, MouseButtons button)
        { }

        private void mouseDown(object sender, MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) != 0 && e.Button == MouseButtons.Left)
            {
                lastPos = e.Location;
                allowMove = true;
            }
            else if (e.X >= panelForCells.Left && e.X < panelForCells.Right && e.Y >= panelForCells.Top && e.Y < panelForCells.Bottom)
                cellClick((int)((e.Y - panelForCells.Top) / cellHeight), (int)((e.X - panelForCells.Left) / cellWidth), e.Button);
        }


        private void mouseMove(object sender, MouseEventArgs e)
        {
            if (allowMove && (Control.ModifierKeys & Keys.Control) != 0)
            {
                Point newLocation = new Point(panelForCells.Location.X + e.X - lastPos.X, panelForCells.Location.Y + e.Y - lastPos.Y);

                if (panelForCells.Width <= Width && panelForCells.Height <= Height)
                {
                    if (newLocation.X < 0)
                        newLocation.X = 0;
                    if (newLocation.Y < 0)
                        newLocation.Y = 0;
                    if (newLocation.X + panelForCells.Width >= Width)
                        newLocation.X = Width - panelForCells.Width;
                    if (newLocation.Y + panelForCells.Height >= Height)
                        newLocation.Y = Height - panelForCells.Height;
                }

                panelForCells.Location = newLocation;
                lastPos = e.Location;
            }
        }

        private void mouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                allowMove = false;
        }

        private void mouseWheel(object sender, MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) != 0)
            {
                
                float scale = (float)Math.Pow(1.0005, e.Delta);
                if (scale * panelForCells.Width >= 300.0f && scale * panelForCells.Height >= 300.0f)
                {
                    allowDraw = false;
                    panelForCells.Size = new Size((int)(panelForCells.Size.Width * scale), (int)(panelForCells.Height * scale));

                    int dx, dy;
                    if (panelForCells.Width > Width || panelForCells.Height > Height)
                    {
                        dx = e.X - panelForCells.Left;
                        dy = e.Y - panelForCells.Top;
                    }
                    else
                    {
                        dx = Width / 2 - panelForCells.Left;
                        dy = Height / 2 - panelForCells.Top;
                    }
                    dx = (int)(dx * scale - dx);
                    dy = (int)(dy * scale - dy);
                    panelForCells.Location = new Point(panelForCells.Location.X - dx, panelForCells.Location.Y - dy);
                    allowDraw = true;
                    panelForCells.Invalidate();
                }

            }
        }

        private void PanelForCells_Paint(object sender, PaintEventArgs e)
        {
            updateAllCells();
        }

        protected override void OnResize(EventArgs eventargs)
        {

            base.OnResize(eventargs);
            float k = ((float)width) / height;
            if (Width / k < Height)
                panelForCells.Size = new Size(Width, (int)(Width / k));
            else
                panelForCells.Size = new Size((int)(Height * k), Height);

            panelForCells.Location = new Point((Width - panelForCells.Width) / 2, (Height - panelForCells.Height) / 2);
        }

        void panelForCellsResize(object sender, EventArgs e)
        {
            graphics = panelForCells.CreateGraphics();
            cellWidth = panelForCells.Width / (float)width;
            cellHeight = panelForCells.Height / (float)height;
            if (cellHeight > 0 && cellWidth > 0)
            {
                mainFont = new System.Drawing.Font(Font.FontFamily, cellHeight * 0.5f);
                percentageFont = new System.Drawing.Font(Font.FontFamily, cellHeight * 0.25f);

                float widthLineX = lineWidth * cellWidth;
                float widthLineY = lineWidth * cellHeight;

                for (int i = 0; i < height; ++i)
                    for (int j = 0; j < width; ++j)
                    {
                        field[i, j].x = j * cellWidth + widthLineX;
                        field[i, j].y = i * cellHeight + widthLineY;
                        field[i, j].width = cellWidth - 2 * widthLineX;
                        field[i, j].height = cellHeight - 2 * widthLineY;
                    }
            }

            panelForCells.Invalidate();
        }

        public class Cell
        {
            private static Image imageMine = Properties.Resources.mine;
            private static Image imageFlag = Properties.Resources.flag;
            public static Brush brushClosed = Brushes.Gray;
            public static Brush brushOpened = Brushes.White;
            public static StringFormat stringFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            public float x { get; set; }
            public float y { get; set; }
            public float width { get; set; }
            public float height { get; set; }

            public BaseField parentField{ get; set; }

            private bool m_opened = false;
            public bool opened
            {
                get { return m_opened; }
                set
                {
                    m_opened = value;
                    draw();
                }
            }
            public bool isMine { get; set; } = false;
            public int minesNear { get; set; }
            public int i { get; set; }
            public int j { get; set; }




            private bool m_marked = false;
            public bool marked
            {
                get { return m_marked; }
                set
                {
                    m_marked = value;
                    draw();
                }
            }

            private float m_probability = -1;
            public float probability
            {
                get { return m_probability; }
                set
                {
                    m_probability = value;
                    draw();
                }
            }



            public Cell() { }

            public Cell(FileStream fs)
            {
                isMine = fs.ReadByte() == 1;
                m_opened = fs.ReadByte() == 1;
                m_marked = fs.ReadByte() == 1;
                minesNear = fs.ReadByte();
            }

            public void writeToFile(FileStream fs)
            {
                fs.WriteByte((byte)(isMine ? 1 : 0));
                fs.WriteByte((byte)(m_opened ? 1 : 0));
                fs.WriteByte((byte)(m_marked ? 1 : 0));
                fs.WriteByte((byte)(minesNear));
            }

            public void draw()
            {
                if (opened)
                {
                    parentField.graphics.FillRectangle(brushOpened, x, y, width, height);

                    if (isMine)
                        parentField.graphics.DrawImage(imageMine, x, y, width, height);
                    else if (minesNear > 0)
                        parentField.graphics.DrawString(minesNear.ToString(), parentField.mainFont, Brushes.Black, new RectangleF(x, y, width, height), stringFormat);
                }
                else
                {
                    if (parentField.ShowProbability && probability >= 0)
                        parentField.graphics.FillRectangle(new SolidBrush(
                            Color.FromArgb((int)(255 * (1 - m_probability * m_probability * m_probability)), (int)(255 * m_probability * m_probability * m_probability), 0)
                            ), x, y, width, height);
                    else
                        parentField.graphics.FillRectangle(brushClosed, x, y, width, height);

                    if (m_marked)
                        parentField.graphics.DrawImage(imageFlag, x, y, width, height);

                    if (parentField.ShowProbability && parentField.ShowPercentage && probability >= 0)
                        parentField.graphics.DrawString(((int)(m_probability * 100.0f)).ToString() + '%', parentField.percentageFont, Brushes.Black, new RectangleF(x, y, width, height), stringFormat);
                }

            }

        }
    }

}

