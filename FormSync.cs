﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class FormSync : Form
    {
        public GameField loadedGame = null;
        List<GameField> gameFields;
        List<DateTime> times;

        public FormSync()
        {
            InitializeComponent();
        }

        private void FormSync_Load(object sender, EventArgs e)
        {
            gameFields= new List<GameField>();
            times =new List<DateTime>();
            if (File.Exists(".saved_games"))
            {
                FileStream fs = new FileStream(".saved_games", FileMode.Open);
                byte[] dateSave = new byte[sizeof(long)];
                while (fs.Read(dateSave, 0, sizeof(long)) > 0)
                {
                    times.Add(DateTime.FromBinary(BitConverter.ToInt64(dateSave, 0)));
                    gameFields.Add( GameField.readFromFile(fs));
                }
                fs.Close();

                table.RowStyles.Clear();
                table.RowCount = gameFields.Count / 2 + 1;
                for (int i = 0; i < table.RowCount; ++i)
                    table.RowStyles.Add(new RowStyle(SizeType.Absolute, 500));

                for (int i=0;i<gameFields.Count;++i)
                {
                    gameFields[i].Dock = DockStyle.Top;
                    gameFields[i].Enabled = false;
                    gameFields[i].Height = 400;
                    gameFields[i].BackColor = this.BackColor;
                    (new Panel()).Controls.Add(gameFields[i]);
                    gameFields[i].Parent.Dock = DockStyle.Fill;
                    table.Controls.Add(gameFields[i].Parent);
                    gameFields[i].allowDraw = true;

                    Button bt = new Button();
                    bt.Text = "Load";
                    bt.Parent = gameFields[i].Parent;
                    bt.Left = 130;
                    bt.Top = bt.Parent.Height-50;
                    bt.Width = 70;
                    bt.Height = 30;
                    bt.Anchor = AnchorStyles.Bottom;
                    bt.Tag = gameFields[i];
                    bt.DialogResult = DialogResult.OK;
                    bt.Click += btLoadClick;



                    bt = new Button();
                    bt.Text = "Delete";
                    bt.Parent = gameFields[i].Parent;
                    bt.Left = 200;
                    bt.Top = bt.Parent.Height - 50;
                    bt.Width = 70;
                    bt.Height = 30;
                    bt.Anchor = AnchorStyles.Bottom;
                    bt.Tag = gameFields[i];
                    bt.Click += btRemoveClick;

                    Label lb = new Label();
                    lb.Text = times[i].ToString();
                    lb.Parent = gameFields[i].Parent;
                    lb.Left = 0;
                    lb.Top = lb.Parent.Height -90;
                    lb.Width = 70;
                    lb.AutoSize = true;
                    lb.Font = new Font(lb.Font.FontFamily,12.0f);

                }

            }
            if (gameFields.Count == 0)
            {
                table.Visible = false;
                lbMessage.Visible = true;
                return;
            }
        }

        private void btLoadClick(object sender, EventArgs e)
        {
            GameField result = (GameField)(((Control)sender).Tag);
            result.allowDraw = false;
            result.Dock = DockStyle.None;
            result.BackColor = Color.Aqua;
            result.Enabled = true;
            loadedGame = result;
        }

        private void btRemoveClick(object sender, EventArgs e)
        {
            int index = gameFields.IndexOf((GameField)(((Control)sender).Tag));
            gameFields.RemoveAt(index);
            times.RemoveAt(index);
            table.Controls.RemoveAt(index);

            FileStream fs = new FileStream(".saved_games", FileMode.Create);
            for (int i=0; i<gameFields.Count;++i)
            {
                fs.Write(BitConverter.GetBytes(times[i].ToBinary()), 0, sizeof(long));
                gameFields[i].writeToFile(fs);
            }
            fs.Close();

            if (gameFields.Count == 0)
            {
                table.Visible = false;
                lbMessage.Visible = true;
                this.Focus();
            }

        }

        private void FormSync_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}
