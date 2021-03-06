﻿using System;
using C5;
using SCG=System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xpdm.Catan.Core.Board
{
    class Gameboard : System.ComponentModel.INotifyPropertyChanged
    {
        private readonly IList<IList<HexTile>> board;
        public int Rows
        {
            get;
            private set;
        }
        public int Columns
        {
            get;
            private set;
        }

        public Gameboard(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;

            board = new ArrayList<IList<HexTile>>(rows);
            for (int i = 0; i < rows; ++i)
            {
                board.Add(new ArrayList<HexTile>(columns));
                for (int j = 0; j < columns; ++j)
                {
                    board[i].Add(new EmptyHexTile());
                }
            }
        }

        public static bool IsOffset(int col)
        {
            return (col % 2 != 0);
        }

        public static int DistanceBetween(int x1, int y1, int x2, int y2)
        {
            var dY = Math.Abs(y1 - y2);
            if (x1 == x2)
                return dY;

            var dX = Math.Abs(x1 - x2);
            if (y1 == y2)
                return dX;

            var d = (!IsOffset(x2) && IsOffset(x1) && y1 < y2 || IsOffset(x2) && !IsOffset(x1) && y1 > y2);
            return dX + dY - Math.Min(dX, (d ? 0 : 2) + dY) / 2 - (d ? 1 : 0);
        }


        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
