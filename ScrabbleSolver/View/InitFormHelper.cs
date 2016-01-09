using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScrabbleSolver
{
	static class InitFormHelper
	{
		private static int SINGLE_CELL_SIZE = 35;


		public static void InitBoard(DataGridView localBoard, int rows)
		{
			localBoard.BackgroundColor = Color.Black;

			for (int i = 0; i < rows; ++i)
			{
				localBoard.Rows.Add();
			}

			InitCells(localBoard);
		}

		public static void InitCells(DataGridView dataGridView)
		{
			foreach (DataGridViewColumn d in dataGridView.Columns)
			{
				d.Width = SINGLE_CELL_SIZE;
			}

			foreach (DataGridViewRow d in dataGridView.Rows)
			{
				d.Height = SINGLE_CELL_SIZE;
			}

			for (int row = 0; row < dataGridView.Rows.Count; ++row)
			{
				for (int col = 0; col < dataGridView.Columns.Count; ++col)
				{
					FormatSingleCell(row, col, dataGridView, false);
				}
			}
		}

		public static void FormatSingleCell(int row, int col, DataGridView dataGridView, bool IsCellReadOnly)
		{
			DataGridViewCell dataGridViewCell = dataGridView[col, row];
			dataGridViewCell.ReadOnly = IsCellReadOnly;
			if (IsCellReadOnly)
			{
				dataGridViewCell.Style.BackColor = Color.Black;
				dataGridViewCell.Style.SelectionBackColor = Color.Black;
			}
			else
			{
				dataGridViewCell.Style.BackColor = Color.White;
				dataGridViewCell.Style.SelectionBackColor = Color.AntiqueWhite;
			}
			dataGridViewCell.Style.ForeColor = Color.Red;
			dataGridViewCell.Style.SelectionForeColor = Color.Red;

		}
	}
}
