using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ZADANIE_11
{
    public partial class Form1 : Form
    {
        private int[,] sudoku;
        private int m, s;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateSudoku();
            timer1.Enabled = true;
            DisplaySudoku();
            button1.Enabled = false;
            button2.Enabled = true; 
            button3.Enabled = true;
            button4.Enabled = true;
        }
        private void GenerateSudoku()
        {
            sudoku = new int[9, 9];
            Random rand = new Random();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                    sudoku[i, j] = (i * 3 + i / 3 + j) % 9 + 1;
            }

            for (int i = 0; i < 3; i++)
            {
                int rowGroup = rand.Next(0, 3) * 3;
                int row1 = rowGroup + rand.Next(0, 3);
                int row2 = rowGroup + rand.Next(0, 3);

                if (row1 != row2)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        int temp = sudoku[row1, j];
                        sudoku[row1, j] = sudoku[row2, j];
                        sudoku[row2, j] = temp;
                    }
                }
            }

            int cells = rand.Next(30, 45);
            while (cells > 0)
            {
                int row = rand.Next(0, 9);
                int col = rand.Next(0, 9);
                if (sudoku[row, col] != 0)
                {
                    sudoku[row, col] = 0;
                    cells--;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.RowCount = 10;
            dataGridView1.ColumnCount = 10;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.Columns[9].Visible = false;
            dataGridView1.EditingControlShowing += 
                dataGridView1_EditingControlShowing;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;

            int cellSize = 30;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.Width = cellSize;
                column.DefaultCellStyle.Alignment = 
                    DataGridViewContentAlignment.MiddleCenter;            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
                row.Height = cellSize;
        }

        private void DisplaySudoku()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (sudoku[i, j] != 0)
                        dataGridView1.Rows[i].Cells[j].Value =
                            sudoku[i, j].ToString();
                }
            }
        }

        int dataRow;
        int dataCol;
        int tryy = 3;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    dataGridView1.Rows[i].Cells[j].Style.BackColor =
                        Color.White;

            dataRow = e.RowIndex;
            dataCol = e.ColumnIndex;
            int squareRow = dataRow / 3;
            int squareCol = dataCol / 3;

            for (int i = squareRow * 3; i < (squareRow + 1) * 3; i++)
            {
                for (int j = squareCol * 3; j < (squareCol + 1) * 3; j++)
                    dataGridView1.Rows[i].Cells[j].Style.BackColor =
                        Color.LightBlue;
            }

            for (int i = 0; i < 9; i++)
            {
                dataGridView1.Rows[i].Cells[e.ColumnIndex].Style.BackColor =
                    Color.LightBlue;
                dataGridView1.Rows[e.RowIndex].Cells[i].Style.BackColor =
                    Color.LightBlue;
            }

        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox text = e.Control as TextBox;
            if (text != null)
            {
                text.KeyPress -= TextBox_KeyPress;
                text.KeyPress += TextBox_KeyPress;
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e) {
            DataGridViewCell cell = dataGridView1.CurrentCell;
            if (cell != null && cell.Value != null && cell.Value.ToString()
                != "")
                e.Handled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tryy != 0)
            {
                dataGridView1.Rows[dataRow]
                    .Cells[dataCol].Value = "";
                tryy--;
                label1.Text = tryy.ToString();
            }
            else
                button2.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button1.Enabled == false)
            {
                timer1.Enabled = false;
                MessageBox.Show($"Ваше время: {textBox1.Text}", "Вы сдались!");
                Environment.Exit(0);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            s++;
            m = s / 60;
            textBox1.Text = m + ":" + s % 60;
        }

        private bool IsValidRow(int[,] sudoku, int row)
        {
            HashSet<int> set = new HashSet<int>();
            for (int i = 0; i < 9; i++)
                if (sudoku[row, i] != 0)
                {
                    if (set.Contains(sudoku[row, i]))
                        return false;
                    set.Add(sudoku[row, i]);
                }
            return true;
        }

        private bool IsValidColum(int[,] sudoku, int colum)
        {
            HashSet<int> set = new HashSet<int>();
            for (int i = 0; i < 9; i++)
                if (sudoku[i, colum] != 0)
                {
                    if (set.Contains(sudoku[i, colum]))
                        return false;
                    set.Add(sudoku[i, colum]);
                }
            return true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    sudoku[i, j] = Convert.ToInt32(dataGridView1.Rows[i].
                        Cells[j].Value);
            if (IsSudokuTrue(sudoku))
            {
                MessageBox.Show("Судоку решена правильно!");
                Environment.Exit(1);
            }
            else
            {
                MessageBox.Show("Судоку заполнена не верно!");
                Environment.Exit(1);
            }
        }

        private bool IsSudokuTrue(int[,] sudoku)
        {
            for (int i = 0; i < 9; i++)
                if (!IsValidRow(sudoku, i) || !IsValidColum(sudoku, i))
                    return false;

            return true;
        }
    }
}
