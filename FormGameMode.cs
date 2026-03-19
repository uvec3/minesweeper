﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class FormGameMode : Form
    {
        public int width;
        public int height;
        public int mines;

        private RadioButton activeMmode;

        public FormGameMode()
        {
            InitializeComponent();
            rbMiddle_CheckedChanged(null, null);
            setParams();
        }

        private void numWidthHeightChanged(object sender, EventArgs e)
        {
            numericMines.Maximum = numericHeight.Value * numericWidth.Value - 1;
        }

        private void rbEasy_CheckedChanged(object sender, EventArgs e)
        {
            numericWidth.Value = 7;
            numericHeight.Value = 7;
            numericMines.Value = 5;
        }

        private void rbMiddle_CheckedChanged(object sender, EventArgs e)
        {
            numericWidth.Value = 15;
            numericHeight.Value = 15;
            numericMines.Value = 30;
        }

        private void rbHard_CheckedChanged(object sender, EventArgs e)
        {
            numericWidth.Value = 20;
            numericHeight.Value = 20;
            numericMines.Value = 100;
        }



        private void rbOther_CheckedChanged(object sender, EventArgs e)
        {
            numericHeight.Enabled = numericWidth.Enabled = numericMines.Enabled = rbOther.Checked;
        }

        private void setParams()
        {
            width = (int)numericWidth.Value;
            height = (int)numericHeight.Value;
            mines = (int)numericMines.Value;
            foreach (Control c in Controls)
                if (c is RadioButton && ((RadioButton)c).Checked)
                    activeMmode =(RadioButton)c;
        }

        private void setNumberic()
        {
            numericWidth.Value=width;
            numericHeight.Value= height;
            numericMines.Value= mines;
            activeMmode.Checked = true;
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            setParams();
        }

        private void FormGameMode_Load(object sender, EventArgs e)
        {
            setNumberic();
        }
    }
}
