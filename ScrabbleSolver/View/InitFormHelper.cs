using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScrabbleSolver.Common;

namespace ScrabbleSolver
{
	static class InitFormHelper
	{
		private static readonly int SINGLE_CELL_SIZE = 38;
		private static readonly int FIRST_INDEX = 0;
		private static readonly short MAX_NUM_OF_PLAYERS = 4;
		private static readonly short GAME_INFO_DATA_PER_PLAYER = 2;
		private static readonly int GAME_INFO_BOARD_ROWS = MAX_NUM_OF_PLAYERS * GAME_INFO_DATA_PER_PLAYER + 1;

		public static void InitBoard(DataGridView localBoard, int rows)
		{
			localBoard.BackgroundColor = Color.Black;

			AddRowsToDataGridView(localBoard, rows);

			InitCells(localBoard);
		}

		public static void AddRowsToDataGridView(DataGridView localBoard, int rows)
		{
			for (int i = 0; i < rows; ++i)
			{
				localBoard.Rows.Add();
			}
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

		public static void InitGameInfoBoard(DataGridView gameInfoDataGrid, Dictionary<PlayerIdEnum, Dictionary<GameInfoTypeEnum, string>> GameInfo)
		{
			int GameInfoDataGridRowIndex = FIRST_INDEX;
			InitFormHelper.AddRowsToDataGridView(gameInfoDataGrid, GAME_INFO_BOARD_ROWS);
			gameInfoDataGrid[FIRST_INDEX, GameInfoDataGridRowIndex].Value = "Game player info";


		}

		public static void UpdateGameInfoBoard(DataGridView gameInfoDataGrid,
			Dictionary<PlayerIdEnum, Dictionary<GameInfoTypeEnum, string>> GameInfo)
		{
			int GameInfoDataGridRowIndex = FIRST_INDEX;
			if (GameInfo != null)
			{
				foreach (KeyValuePair<PlayerIdEnum, Dictionary<GameInfoTypeEnum, string>> PlayerIdEnumGameInfoKeyValuePair in GameInfo)
				{
					++GameInfoDataGridRowIndex;
					PlayerIdEnum PlayerIdEnum = PlayerIdEnumGameInfoKeyValuePair.Key;
					gameInfoDataGrid[FIRST_INDEX, GameInfoDataGridRowIndex].Value = Consts.GetStringByPlayerIdEnum(PlayerIdEnum).ToUpper();
					Dictionary<GameInfoTypeEnum, string> Dictionary = PlayerIdEnumGameInfoKeyValuePair.Value;
					if (Dictionary != null)
					{
						foreach (KeyValuePair<GameInfoTypeEnum, string> GameInfoTypeEnumStringValuePair in Dictionary)
						{
							++GameInfoDataGridRowIndex;
							GameInfoTypeEnum GameInfoTypeEnum = GameInfoTypeEnumStringValuePair.Key;
							string Value = GameInfoTypeEnumStringValuePair.Value;
							gameInfoDataGrid[FIRST_INDEX, GameInfoDataGridRowIndex].Value =
								Consts.GetStringByGameInfoTypeEnum(GameInfoTypeEnum) + ": " + Value;

						}
					}
				}
			}
		}
	}
}
