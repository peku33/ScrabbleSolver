using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScrabbleSolver;

namespace ScrabbleSolver
{
	public partial class Form1 : Form
	{
		private static int BOARD_SIZE = 15;

		private String toCopyString;
		private List<Tuple<int, int, string>> CellValues;
		private List<Tuple<int, int, String, String>> HeldCharacters;

		public Form1()
		{
			InitializeComponent();
		}

		private void panel1_Paint(object sender, PaintEventArgs e)
		{

		}

		/// <summary>
		/// The method that loads the Form.
		/// </summary>
		private void Form1_Load(object sender, EventArgs e)
		{
			/// some sample data  - to remove in the future
			CellValues = new List<Tuple<int, int, string>>();
			CellValues.Add(new Tuple<int, int, string>(7,7,"d"));
			CellValues.Add(new Tuple<int, int, string>(12, 12, "d"));
			
			HeldCharacters = new List<Tuple<int, int, string, string>>();
			HeldCharacters.Add(new Tuple<int, int, string, string>(0, 0, "d", "K"));

			InitAllDataGridViews(); /// do not remove

			FormModelHelper.FormatSingleCell(4, 4, boardGridView, false);


	
			SecondHeldCharactersDataGrid[0, 0].Value = "L";
			FirstHeldCharactersDataGrid[0, 0].Value = HeldCharacters[0].Item4;


		}

		private void InitAllDataGridViews()
		{
			// init main board
			FormModelHelper.InitBoard(boardGridView, BOARD_SIZE);

			// init HeldCharactersLabelDataGrid boards
			SecondHeldCharactersLabelDataGrid[0, 0].Value = "Held Characters";
			FirstHeldCharactersLabelDataGrid[0, 0].Value = "Held Characters";

			// init HeldCaractersDataGrid boards
			FormModelHelper.InitBoard(FirstHeldCharactersDataGrid, 1);
			FormModelHelper.InitBoard(SecondHeldCharactersDataGrid, 1);

			GameInfoDataGrid[0, 0].Value = "Game player info";
		}



		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("About message");
		}

		private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
		{

		}

		private void board_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				AdjustInsertedString(e);

				foreach (Tuple<int, int, string, string> heldCharacter in HeldCharacters)
				{
					string CellValue = heldCharacter.Item4;
					if (CellValue.Contains(boardGridView[e.ColumnIndex, e.RowIndex].Value.ToString()))
					{
						CellValues.Add(new Tuple<int, int, string>(e.ColumnIndex, e.RowIndex, heldCharacter.Item3));
						HeldCharacters.Remove(heldCharacter);
						removeCharacterFromHeldCharactersDataGrid(CellValue, FirstHeldCharactersDataGrid);
					}
				}
			}
			catch
			{
				
			}
					
		}

		private void AdjustInsertedString(DataGridViewCellEventArgs e)
		{
			boardGridView[e.ColumnIndex, e.RowIndex].Value = boardGridView[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper();
			if (boardGridView[e.ColumnIndex, e.RowIndex].Value.ToString().Length > 1)
			{
				boardGridView[e.ColumnIndex, e.RowIndex].Value = boardGridView[e.ColumnIndex, e.RowIndex].Value.ToString()
					.Substring(0, 1);
			}
		}

		private void removeCharacterFromHeldCharactersDataGrid(string cellValue, DataGridView firstHeldCharactersDataGrid)
		{
			Size size = firstHeldCharactersDataGrid.Size;

			for (int i = 0; i < size.Width; ++i)
			{
				if (firstHeldCharactersDataGrid[0, i].Value.ToString().Contains(cellValue))
				{
					firstHeldCharactersDataGrid[0, i].Value = "";
				}
			}

			this.Refresh();
		}

		private void board_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{

			foreach (var CellValue in CellValues)
			{
				if (CellValue.Item1 == e.ColumnIndex && CellValue.Item2 == e.RowIndex) 
				{
					//the black background 
					Rectangle rectangle2 = new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height);
					e.Graphics.FillRectangle(Brushes.Black, rectangle2);

					// the white foregroudn
					Rectangle rectangle = new Rectangle(e.CellBounds.X+1 , e.CellBounds.Y+1 , e.CellBounds.Width-2, e.CellBounds.Height-2);
					e.Graphics.FillRectangle(Brushes.White, rectangle);
					Font f = new Font(e.CellStyle.Font.FontFamily, 7);
					e.Graphics.DrawString(CellValue.Item3, f, Brushes.Black, rectangle);
					e.PaintContent(e.ClipBounds);
					e.Handled = true;
				}
			}

		}

		private void dataGridView1_CellContentClick_2(object sender, DataGridViewCellEventArgs e)
		{

		}

		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void GameInfoDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}

		private void HeldCharactersDataGrid_DragEnter(object sender, DragEventArgs e)
		{

		}

		private void board_DragDrop(object sender, DragEventArgs e)
		{

		}

		private void HeldCharactersDataGrid_CellMouseDown(object sender2, DataGridViewCellMouseEventArgs e)
		{
			CellMouseDown(sender2);
		}

		private void CellMouseDown(object sender2)
		{
			DataGridView sender = (DataGridView) sender2;

			DataGridViewCell currentCell = sender.CurrentCell;

			if (currentCell.Value != null)
			{
				String value = currentCell.Value.ToString();

				toCopyString = String.Copy(value);
				currentCell.Value = null;
			}
		}

		private void boardGridView_CellMouseUp(object sender2, DataGridViewCellMouseEventArgs e)
		{
			DataGridView sender = (DataGridView)sender2;

			DataGridViewCell currentCell = sender.CurrentCell;

			if (toCopyString != null)
			{
				currentCell.Value = String.Copy(toCopyString);
				toCopyString = null;
			}
		}

		private void boardGridView_CellMouseDown(object sender2, DataGridViewCellMouseEventArgs e)
		{

				
		}

		private void FirstHeldCharactersDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}

		private void FirstHeldCharactersDataGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			foreach (var CellValue in HeldCharacters)
			{
				if (CellValue.Item1 == e.ColumnIndex && CellValue.Item2 == e.RowIndex)
				{
					//the black background 
					Rectangle rectangle2 = new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height);
					e.Graphics.FillRectangle(Brushes.Black, rectangle2);

					// the white foregroudn
					Rectangle rectangle = new Rectangle(e.CellBounds.X + 1, e.CellBounds.Y + 1, e.CellBounds.Width - 2, e.CellBounds.Height - 2);
					e.Graphics.FillRectangle(Brushes.White, rectangle);
					Font f = new Font(e.CellStyle.Font.FontFamily, 7);
					e.Graphics.DrawString(CellValue.Item3, f, Brushes.Black, rectangle);
					e.PaintContent(e.ClipBounds);
					e.Handled = true;
				}
			}

		}

		private void FirstHeldCharactersDataGrid_CellMouseDown(object sender2, DataGridViewCellMouseEventArgs e)
		{
			CellMouseDown(sender2);
		} 


	}
}
