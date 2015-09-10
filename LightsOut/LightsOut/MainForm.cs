using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightsOut
{
    public partial class MainForm : Form
    {
        private int GRID_OFFSET;   //Distance from upper-left side of window
        private int GRID_LENGTH;    //Size in pixels of grid
        private int NUM_CELLS;      //Number of cells in grid
        private int CELL_LENGTH;

        private bool[,] grid;       //Stores on/off state of cells in grid
        private Random rand;        //Used to generate random numbers

        public MainForm()
        {
            InitializeComponent();

            //These cannot be constants because of the size change options available
            GRID_OFFSET = 25;
            GRID_LENGTH = 200;
            NUM_CELLS = 3;
            CELL_LENGTH = GRID_LENGTH / NUM_CELLS;

            rand = new Random();    //Initializes rnadom number generator

            grid = new bool[NUM_CELLS, NUM_CELLS];

            //Turn entire grid on
            for (int r = 0; r < NUM_CELLS; r++)
                for (int c = 0; c < NUM_CELLS; c++)
                    grid[r, c] = true;
        }

        //parameter accepted: number 3, 4, or 5 to represent the number of the selected
        //ToolStripMenuItem (to identify which item was selected)
        private void toolStrip_CheckedChanged(int toolStrip_ID)
        {
            //Checkmark selected ToolStripMenuItem and uncheck the others
            switch (toolStrip_ID)
            {
                case 3:
                    x3ToolStripMenuItem.Checked = true;
                    x4ToolStripMenuItem.Checked = false;
                    x5ToolStripMenuItem.Checked = false;
                    break;
                case 4:
                    x3ToolStripMenuItem.Checked = false;
                    x4ToolStripMenuItem.Checked = true;
                    x5ToolStripMenuItem.Checked = false;
                    break;
                case 5:
                    x3ToolStripMenuItem.Checked = false;
                    x4ToolStripMenuItem.Checked = false;
                    x5ToolStripMenuItem.Checked = true;
                    break;
                default:
                    //Do nothing
                    break;
            }
        }

        private bool PlayerWon()
        {
            //Check if all lights are off
            for (int r = 0; r < NUM_CELLS; r++)
                for (int c = 0; c < NUM_CELLS; c++)
                    if (grid[r, c] == true)
                        return false;

            return true;
        }

        //Called when User selects a different size for the grid
        private void ChangeGrid()
        {
            //Resize grid slots
            grid = new bool[NUM_CELLS, NUM_CELLS];

            //Fill grid with either white or black
            for (int r = 0; r < NUM_CELLS; r++)
                for (int c = 0; c < NUM_CELLS; c++)
                    grid[r, c] = rand.Next(2) == 1;

            //Resize the cells
            CELL_LENGTH = GRID_LENGTH / NUM_CELLS;
            //Redraw grid
            this.Invalidate();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutBox = new AboutForm();
            aboutBox.ShowDialog(this);
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int r = 0; r < NUM_CELLS; r++)
                for (int c = 0; c < NUM_CELLS; c++)
                {
                    //Get proper pen and brush for on/off grid selection
                    Brush brush;
                    Pen pen;

                    if (grid[r,c])
                    {
                        pen = Pens.Black;
                        brush = Brushes.White;      //On
                    }
                    else
                    {
                        pen = Pens.White;
                        brush = Brushes.Black;      //Off
                    }

                    //Determine (x,y) coord of row and col to draw rectangle
                    int x = c * CELL_LENGTH + GRID_OFFSET;
                    int y = r * CELL_LENGTH + GRID_OFFSET;

                    //Draw outline and inner rectangle
                    g.DrawRectangle(pen, x, y, CELL_LENGTH, CELL_LENGTH);
                    g.FillRectangle(brush, x + 1, y + 1, CELL_LENGTH - 1, CELL_LENGTH - 1);
                }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            //Make sure click was inside the grid
            if (e.X < GRID_OFFSET || e.X > CELL_LENGTH * NUM_CELLS + GRID_OFFSET ||
                e.Y < GRID_OFFSET || e.Y > CELL_LENGTH * NUM_CELLS + GRID_OFFSET)
                return;

            //Find row, col of mouse press
            int r = (e.Y - GRID_OFFSET) / CELL_LENGTH;
            int c = (e.X - GRID_OFFSET) / CELL_LENGTH;

            //Invert selected box and all surrounding boxes
            for (int i = r-1; i <= r+1; i++)
                for (int j = c-1; j <= c+1; j++)
                    if (i >= 0 && i < NUM_CELLS && j >= 0 && j < NUM_CELLS)
                        grid[i,j] = !grid[i,j];

            //Redraw grid
            this.Invalidate();

            //Check to see if puzzle has been solved
            if (PlayerWon())
            {
                //Display winner dialog box
                MessageBox.Show(this, "Congratulations! You've won!", "Lights Out!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            ChangeGrid();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newGameButton_Click(sender, e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Change grid size to 3x3
        private void x3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //If already checked, do nothing
            if (x3ToolStripMenuItem.Checked)
                return;
            else
            {
                NUM_CELLS = 3;
                ChangeGrid();
                toolStrip_CheckedChanged(3);
            }
        }

        //Change grid size to 4x4
        private void x4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //If already checked, do nothing
            if (x4ToolStripMenuItem.Checked)
                return;
            else
            {
                NUM_CELLS = 4;
                ChangeGrid();
                toolStrip_CheckedChanged(4);
            }
        }

        //Change grid size to 5x5
        private void x5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //If already checked, do nothing
            if (x5ToolStripMenuItem.Checked)
                return;
            else
            {
                NUM_CELLS = 5;
                ChangeGrid();
                toolStrip_CheckedChanged(5);
            }
        }
    }
}
