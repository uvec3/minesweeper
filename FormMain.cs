﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class FormMain : Form
    {
        GameField mf;
        FormGameMode formGameMode = new FormGameMode();

        public FormMain()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            newGame();
        }

        public void newGame()
        {
            if (mf != null)
                mf.Dispose();
            mf = new GameField(formGameMode.width, formGameMode.height, formGameMode.mines);
            prepareGame();
        }

        public void loadGame(GameField gf)
        {
            if (mf != null)
                mf.Dispose();
            mf = gf;
            prepareGame();
            gameStart();
        }

        public void prepareGame()
        {
            mf.Location = new Point(100, 100);
            mf.gameEndEvent += gameEnd;
            mf.fieldChangedEvent += fieldChanged;
            mf.gameStartEvent += gameStart;
            mf.ShowProbability = mShowProbability.Checked;
            mf.ShowPercentage = mShowPercentage.Checked;
            lbGameRes.Text = null;
            lbTime.Text = "0000,00";
            fieldChanged(mf.getInfo());
            Form_Resize(null, null);
            mf.Parent = this;
            mf.allowDraw = true;
        }


        private void fieldChanged(int[,] info)
        {

            lbCounter.Text = mf.openedCount.ToString() + "/" + (mf.width * mf.height - mf.minesCount);
            lbMines.Text = (mf.minesCount - mf.countMarkedCells()).ToString();

            if (!mf.gameOver && mf.ShowProbability)
                calculateAndSetProbabilities();
        }

        private void calculateAndSetProbabilities()
        {
            if (mf.gameOver)
                return;
            float[,] probability = GameAnalysis.calculateProbabilityOfSuccess(mf.getInfo(), mf.minesCount);
            {
                for (int i = 0; i < probability.GetLength(0); ++i)
                    for (int j = 0; j < probability.GetLength(1); ++j)
                        mf.setCellProbability(i, j, probability[i, j]);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (mf.gameStarted)
            {
                if (!mf.gameOver)
                    lbTime.Text = $"{mf.getTimeFromStart():0000.00}";
            }

        }

        private void gameStart()
        {
            timer.Enabled = true;
        }

        private void gameEnd(bool win)
        {
            timer.Enabled = false;

            if (win)
                lbGameRes.Text = "You win!";
            else
                lbGameRes.Text = "Try again.";
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            if (Size.Width >= MinimumSize.Width)
                mf.Size = new Size(Width - 200, Height - 200);
        }

        private void btRest_Click(object sender, EventArgs e)
        {
            newGame();
        }


        private void mShowProbablity_Click(object sender, EventArgs e)
        {
            mShowPercentage.Enabled = mShowProbability.Checked = !mShowProbability.Checked;
            if (mShowProbability.Checked)
                calculateAndSetProbabilities();
            mf.ShowProbability = mShowProbability.Checked;
        }

        private void mShowPercentage_Click(object sender, EventArgs e)
        {
            mShowPercentage.Checked = !mShowPercentage.Checked;
            mf.ShowPercentage = mShowPercentage.Checked;
        }

        private void mMode_Click(object sender, EventArgs e)
        {
            if (formGameMode.ShowDialog() == DialogResult.OK)
                newGame();
        }

        private void mExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mf.gameStarted && !mf.gameOver)
            {
                FileStream fs = new FileStream(".last_game", FileMode.OpenOrCreate);
                mf.writeToFile(fs);
                fs.Close();
            }
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            if (File.Exists(".last_game"))
            {
                if (MessageBox.Show("The last game was not finished. Do you want to restore it?", "Restore", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    FileStream fs = new FileStream(".last_game", FileMode.Open);
                    loadGame(GameField.readFromFile(fs));
                    fs.Close();
                }
                File.Delete(".last_game");
            }

        }

        private void mSaveState_Click(object sender, EventArgs e)
        {
            if (!mf.gameStarted)
                MessageBox.Show("Cannot save a game that has not started!\r\n", "Save",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                FileStream fs = new FileStream(".saved_games", FileMode.Append);
                fs.Write(BitConverter.GetBytes(DateTime.Now.ToBinary()), 0, sizeof(long));
                mf.writeToFile(fs);
                fs.Close();
                MessageBox.Show("Game state saved successfully!\r\n" +
                    "All game states are saved to the \".saved_games\" file. If you delete this file, this saved field will also be lost!", "Save",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void mLoadState_Click(object sender, EventArgs e)
        {
            FormSync formSync = new FormSync();
            if (formSync.ShowDialog() == DialogResult.OK)
                loadGame(formSync.loadedGame);
        }
    }
}
