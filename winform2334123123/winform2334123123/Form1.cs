using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace winform2334123123
{
    public partial class Form1 : Form
    {
        enum TileType
        {
            Covered,
            Uncovered,
            Bomb,
        }
        enum LabelState
        {
            Hidden,
            Showing
        }
        enum Flag
        {
            Unflagged,
            Flagged
        }
        TileTypeBase uncoveredTile;
        TileTypeBase coveredTile;
        TileTypeBase bomb;
        Label[] labels;
        Flag[] gridFlags;
        TileType[] gridTiles;
        LabelState[] gridLabels;
        bool firstClick = true;

        RealTime realTime = new RealTime();
        Random random = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            coveredTile = new TileTypeBase();
            coveredTile.colour = Color.HotPink;
            coveredTile.clickAction = TileTypeBase.ClickAction.ChangeType;

            uncoveredTile = new TileTypeBase();
            uncoveredTile.colour = Color.Gainsboro;
            uncoveredTile.clickAction = TileTypeBase.ClickAction.None;

            bomb = new TileTypeBase();
            bomb.colour = Color.HotPink;
            bomb.clickAction = TileTypeBase.ClickAction.Explode;

            gridTiles = new TileType[100];
            gridLabels = new LabelState[100];
            gridFlags = new Flag[100];
            labels = new Label[100];
            for (int k = 0; k < tableLayoutPanel1.Controls.Count; k++)
            {   
                labels[k] = (Label)tableLayoutPanel1.Controls[k].Controls[0];
            }
            for (int i = 0; i < gridTiles.Length; i++)
            {
                SetType(TileType.Covered, i);
            }
        }

        private void tableLayoutPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            Point relativePoint = tableLayoutPanel1.PointToClient(Cursor.Position);
            Point mouseTile = new Point((relativePoint.X / 50), (relativePoint.Y / 50));
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (firstClick == true)
                    {
                        FirstClick();
                    }
                    else
                    {
                        TileClickAction(mouseTile);
                        WinCheck();
                    }
                    break;

                case MouseButtons.Right:
                    DoFlag(mouseTile);
                    break;
            }
        }

        private TileTypeBase EnumToInstance(TileType type)
        {
            switch (type)
            {
                case TileType.Covered:
                    return coveredTile;

                case TileType.Uncovered:
                    return uncoveredTile;

                case TileType.Bomb:
                    return bomb;
            }
            return null;
        }

        private void TileClickAction(Point gridpoint)
        {
            int index = gridpoint.X + gridpoint.Y * 10;
            switch (EnumToInstance(gridTiles[index]).clickAction)
            {
                default:
                    break;

                case TileTypeBase.ClickAction.ChangeType:
                    SetType(TileType.Uncovered, index);
                    break;

                case TileTypeBase.ClickAction.Explode:
                    MessageBox.Show("u lost loser");
                    Application.Restart();
                    break;
            }
        }

        private void SetType(TileType type, int index)
        {
            var cell = tableLayoutPanel1.Controls[99 - index];
            switch (type)
            {
                case TileType.Covered:
                    gridTiles[index] = TileType.Covered;
                    cell.BackColor = EnumToInstance(gridTiles[index]).colour;
                    LabelVisibility(LabelState.Hidden, index);
                    break;

                case TileType.Uncovered:
                    gridTiles[index] = TileType.Uncovered;
                    cell.BackColor = EnumToInstance(gridTiles[index]).colour;
                    LabelVisibility(LabelState.Showing, index);
                    break;

                case TileType.Bomb:
                    gridTiles[index] = TileType.Bomb;
                    cell.BackColor = EnumToInstance(gridTiles[index]).colour;
                    break;
            }
        }
        private void LabelVisibility(LabelState state, int index)
        {
            int oIndex = 99 - index;
            switch (state)
            {
                case LabelState.Hidden:
                    gridLabels[oIndex] = LabelState.Hidden;
                    labels[oIndex].Visible = true;
                    labels[oIndex].Text = "";
                    break;

                case LabelState.Showing:
                    gridLabels[oIndex] = LabelState.Showing;
                    labels[oIndex].Visible = true;
                    labels[oIndex].Text = CheckBombs(index);
                    break;
            }
        }
        private void UpdateNums()
        {
            for (int i = 0; i < gridTiles.Length; i++)
            {
                if (gridTiles[i] == TileType.Uncovered)
                {
                    LabelVisibility(LabelState.Showing, i);
                }
            }
        }
        private void DoFlag(Point gridpoint)
        {
            int index = gridpoint.X + gridpoint.Y * 10;
            var cell = tableLayoutPanel1.Controls[99 - index];
            switch (gridFlags[index])
            {
                case Flag.Unflagged:
                    gridFlags[index] = Flag.Flagged;
                    cell.BackColor = Color.Firebrick;
                    break;
                case Flag.Flagged:
                    gridFlags[index] = Flag.Unflagged;
                    cell.BackColor = EnumToInstance(gridTiles[index]).colour;
                    break;
            }
        }
        private void WinCheck()
        {
            bool win = true;
            for (int i = 0; i < gridTiles.Length; i++)
            {
                if (gridTiles[i] == TileType.Covered)
                {
                    win = false;
                    break;
                }
            }
            if(win == true)
            {
                realTime.EndTime();
                MessageBox.Show("You win");
                realTime.ShowTime();
                Application.Restart();
            }   
        }
        private void FirstClick()
        {
            realTime.StartTime();
            firstClick = false;
            Point relativePoint = tableLayoutPanel1.PointToClient(Cursor.Position);
            Point mouseTile = new Point(relativePoint.X / 50, relativePoint.Y / 50);
            int index = mouseTile.X + mouseTile.Y * 10;
            try
            {
                SetType(TileType.Uncovered, index);
                SetType(TileType.Uncovered, index + 1);
                SetType(TileType.Uncovered, index + 9);
                SetType(TileType.Uncovered, index + 10);
                SetType(TileType.Uncovered, index + 11);
            }
            catch
            {

            }
            try
            {
                SetType(TileType.Uncovered, index - 1);
                SetType(TileType.Uncovered, index - 9);
                SetType(TileType.Uncovered, index - 10);
                SetType(TileType.Uncovered, index - 11);
            }
            catch
            {

            }
           
            int i = 0;
            while (i < 30)
            {
                int rand = random.Next(0, 100);
                if (gridTiles[rand] != TileType.Uncovered)
                {
                    try
                    {
                        if (gridTiles[rand + 1] == TileType.Uncovered)
                        {
                            SetType(TileType.Uncovered, rand);
                            i++;
                        }
                    }
                    catch
                    {

                    }
                    try
                    {
                        if (gridTiles[rand - 1] == TileType.Uncovered)
                        {
                            SetType(TileType.Uncovered, rand);
                            i++;
                        }
                    }
                    catch
                    {

                    }
                    try
                    {
                        if (gridTiles[rand + 10] == TileType.Uncovered)
                        {
                            SetType(TileType.Uncovered, rand);
                            i++;
                        }
                    }
                    catch
                    {

                    }
                    try
                    {
                        if (gridTiles[rand - 10] == TileType.Uncovered)
                        {
                            SetType(TileType.Uncovered, rand);
                            i++;
                        }
                    }
                    catch
                    {

                    }

                }
            }
            int j = 0;
            while (j < 16)
            {
                int rand = random.Next(0, 100);
                if (gridTiles[rand] != TileType.Bomb && gridTiles[rand] != TileType.Uncovered)
                {
                    SetType(TileType.Bomb, rand);
                    j++;
                }
            }
            UpdateNums();
        }
        private string CheckBombs(int index)
        {
            var cell = tableLayoutPanel1.Controls[index];
            var row = tableLayoutPanel1.GetRow(cell);
            int bombs = 0;

            try
            {
                if (gridTiles[index - 1] == TileType.Bomb)
                {
                    var newCell = tableLayoutPanel1.Controls[index - 1];
                    var newRow = tableLayoutPanel1.GetRow(newCell);
                    if (newRow == row)
                    {
                        bombs += 1;
                    }
                }
            }
            catch
            {

            }

            try
            {
                if (gridTiles[index - 9] == TileType.Bomb)
                {
                    var newCell = tableLayoutPanel1.Controls[index - 9];
                    var newRow = tableLayoutPanel1.GetRow(newCell);
                    if (newRow == row + 1)
                    {
                        bombs++;
                    }
                }
            }
            catch
            {

            }

            try
            {
                if (gridTiles[index - 10] == TileType.Bomb)
                {
                    var newCell = tableLayoutPanel1.Controls[index - 10];
                    var newRow = tableLayoutPanel1.GetRow(newCell);
                    if (newRow == row + 1)
                    {
                        bombs++;
                    }
                }
            }
            catch
            {

            }

            try
            {
                if (gridTiles[index - 11] == TileType.Bomb)
                {
                    var newCell = tableLayoutPanel1.Controls[index - 11];
                    var newRow = tableLayoutPanel1.GetRow(newCell);
                    if (newRow == row + 1)
                    {
                        bombs++;
                    }
                }
            }
            catch
            {

            }

            try
            {
                if (gridTiles[index + 1] == TileType.Bomb)
                {
                    var newCell = tableLayoutPanel1.Controls[index + 1];
                    var newRow = tableLayoutPanel1.GetRow(newCell);
                    if (newRow == row)
                    {
                        bombs++;
                    }
                }
            }
            catch
            {

            }

            try
            {
                if (gridTiles[index + 9] == TileType.Bomb)
                {
                    var newCell = tableLayoutPanel1.Controls[index + 9];
                    var newRow = tableLayoutPanel1.GetRow(newCell);
                    if (newRow == row - 1)
                    {
                        bombs++;
                    }
                }
            }
            catch
            {

            }

            try
            {
                if (gridTiles[index + 10] == TileType.Bomb)
                {
                    var newCell = tableLayoutPanel1.Controls[index + 10];
                    var newRow = tableLayoutPanel1.GetRow(newCell);
                    if (newRow == row - 1)
                    {
                        bombs++;
                    }
                }
            }
            catch
            {

            }

            try
            {
                if (gridTiles[index + 11] == TileType.Bomb)
                {
                    var newCell = tableLayoutPanel1.Controls[index + 11];
                    var newRow = tableLayoutPanel1.GetRow(newCell);
                    if (newRow == row - 1)
                    {
                        bombs++;
                    }
                }
            }
            catch
            {

            }
            if (bombs == 0)
            {
                return "";
            }
            else
            {
                return bombs.ToString();
            }
        }

    }
}
