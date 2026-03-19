using System;
using System.IO;
using System.Windows.Forms;

namespace Minesweeper
{
    public delegate void GameStartEvent();
    public delegate void GameEndEvent(bool win);
    public delegate void FieldChanged(int[,] info);


    public class GameField : BaseField
    {
        private DateTime startTime;

        public bool gameStarted { get; private set; } = false;
        public bool gameOver { get; private set; } = false;
        public int openedCount { get; private set; } = 0;


        public event GameStartEvent gameStartEvent;
        public event GameEndEvent gameEndEvent;
        public event FieldChanged fieldChangedEvent;

        public GameField(int width, int height, int mineCount) : base(width, height, mineCount) { }

        public static GameField readFromFile(FileStream fs)
        {
            byte[] bytes = new byte[sizeof(int) * 3];

            fs.Read(bytes, 0, sizeof(int) * 3);

            int width = BitConverter.ToInt32(bytes, 0);
            int height = BitConverter.ToInt32(bytes, sizeof(int)); ;
            int minesCount = BitConverter.ToInt32(bytes, sizeof(int) * 2);

            bytes = new byte[sizeof(double)];
            fs.Read(bytes, 0, sizeof(double));
            double duration = BitConverter.ToDouble(bytes, 0);

            GameField gf = new GameField(width, height, minesCount);

            gf.gameOver = fs.ReadByte() == 1;

            int openedCount = 0;

            for (int i = 0; i < height; ++i)
                for (int j = 0; j < width; ++j)
                {
                    Cell cell = new Cell(fs);
                    gf.field[i, j].opened = cell.opened;
                    gf.field[i, j].isMine = cell.isMine;
                    gf.field[i, j].marked = cell.marked;
                    gf.field[i, j].minesNear = cell.minesNear;
                    if (cell.opened)
                        ++openedCount;
                }

            gf.openedCount = openedCount;
            gf.startTime = DateTime.Now.AddSeconds(-duration);
            gf.gameStarted = true;
            return gf;
        }


        public void writeToFile(FileStream fs)
        {
            fs.Write(BitConverter.GetBytes(width), 0, sizeof(int));
            fs.Write(BitConverter.GetBytes(height), 0, sizeof(int));
            fs.Write(BitConverter.GetBytes(minesCount), 0, sizeof(int));
            fs.Write(BitConverter.GetBytes(getTimeFromStart()), 0, sizeof(double));
            fs.WriteByte((byte)(gameOver ? 1 : 0));

            for (int i = 0; i < height; ++i)
                for (int j = 0; j < width; ++j)
                    field[i, j].writeToFile(fs);
        }


        private void m_open(int i, int j)
        {
            if (!gameStarted)
                init(i, j);

            if (!field[i, j].opened)
            {
                field[i, j].opened = true;
                ++openedCount;


                if (field[i, j].isMine)
                {
                    gameOver = true;
                    openAllMines();
                    if (gameEndEvent != null)
                        gameEndEvent(false);
                }
                else
                {
                    if (field[i, j].minesNear == 0)
                        for (int ii = i - 1; ii <= i + 1; ++ii)
                            for (int jj = j - 1; jj <= j + 1; ++jj)
                                if ((ii >= 0) && (jj >= 0) && (ii < height) && (jj < width) && !(ii == i && jj == j) && !field[ii, jj].opened)
                                    m_open(ii, jj);
                    if (openedCount == width * height - minesCount)
                    {
                        gameOver = true;
                        if (gameEndEvent != null)
                            gameEndEvent(true);
                    }
                }
            }

        }

        private void openAllMines()
        {
            for (int ii = 0; ii < height; ++ii)
                for (int jj = 0; jj < width; ++jj)
                    if (field[ii, jj].isMine)
                        field[ii, jj].opened = true;
        }

        private void init(int starti, int startj)
        {
            Random random = new Random();
            int column, line;
            for (int i = 0; i < minesCount; ++i)
            {
                while (field[line = random.Next(height), column = random.Next(width)].isMine || (line == starti && column == startj));
                field[line, column].isMine = true;
            }


            for (int i = 0; i < height; ++i)
                for (int j = 0; j < width; ++j)
                {
                    int counter = 0;
                    if (!field[i, j].isMine)
                        for (int ii = i - 1; ii <= i + 1; ++ii)
                            for (int jj = j - 1; jj <= j + 1; ++jj)
                                if ((ii >= 0) && (jj >= 0) && (ii < height) && (jj < width) && !(ii == i && jj == j) && field[ii, jj].isMine)
                                    ++counter;
                    field[i, j].minesNear = counter;

                }
            startTime = DateTime.Now;
            gameStarted = true;

            if (gameStartEvent != null)
                gameStartEvent();
        }


        protected override void cellClick(int i, int j, MouseButtons button)
        {
            base.cellClick(i, j, button);
            if (!gameOver)
            {
                if (button == MouseButtons.Left)
                {
                    if (!field[i, j].opened && !field[i, j].marked)
                    {

                        m_open(i, j);
                        if (fieldChangedEvent != null)
                            fieldChangedEvent(getInfo());
                    }

                    else if (field[i, j].minesNear > 0)
                    {
                        int closedCount = 0;
                        for (int ii = i - 1; ii <= i + 1; ++ii)
                            for (int jj = j - 1; jj <= j + 1; ++jj)
                                if ((ii >= 0) && (jj >= 0) && (ii < height) && (jj < width) && !field[ii, jj].opened)
                                    ++closedCount;

                        if (closedCount == field[i, j].minesNear)
                        {
                            for (int ii = i - 1; ii <= i + 1; ++ii)
                                for (int jj = j - 1; jj <= j + 1; ++jj)
                                    if ((ii >= 0) && (jj >= 0) && (ii < height) && (jj < width) && !field[ii, jj].opened)
                                        markMine(ii, jj, true);
                            if (fieldChangedEvent != null)
                                fieldChangedEvent(getInfo());
                        }
                    }
                }
                else if (button == MouseButtons.Right)
                {
                    if (!field[i, j].opened)
                        markMine(i, j, !field[i, j].marked);
                    if (fieldChangedEvent != null)
                        fieldChangedEvent(getInfo());
                }
            }
        }

        public void markMine(int i, int j, bool flag)
        {
            field[i, j].marked = flag;
            if (fieldChangedEvent != null)
                fieldChangedEvent(getInfo());

            if (gameStarted)
            {
                bool allMinesMarked = true;
                for (int ii = 0; ii < height; ++ii)
                    for (int jj = 0; jj < width; ++jj)
                        if (!field[ii, jj].opened && field[ii, jj].isMine != field[ii, jj].marked)
                        {
                            allMinesMarked = false;
                            break;
                        }
                if (allMinesMarked)
                {
                    for (int ii = 0; ii < height; ++ii)
                        for (int jj = 0; jj < width; ++jj)
                            if (!field[ii, jj].opened && !field[ii, jj].isMine)
                                m_open(ii, jj);
                }
            }
        }

        public double getTimeFromStart()
        {
            if (gameStarted)
                return (DateTime.Now - startTime).TotalSeconds;
            return 0.0;
        }

        public void setCellProbability(int i, int j, float prbability)
        {
            if (i >= 0 && i < height && j >= 0 && j < width)
                field[i, j].probability = prbability;
        }

        public int countMarkedCells()
        {
            int count = 0;
            for (int i = 0; i < height; ++i)
                for (int j = 0; j < width; ++j)
                    if (!field[i, j].opened && field[i, j].marked)
                        ++count;
            return count;
        }

    }
}
