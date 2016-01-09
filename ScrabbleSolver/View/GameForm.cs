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
using ScrabbleSolver.Board;
using ScrabbleSolver.Model.Items;

namespace ScrabbleSolver
{
	public partial class GameForm : Form
	{
		private static int BOARD_SIZE = 15;

		private String TemporaryCopingCharacter;
		private Dictionary<Coordinates, Cell> CellValues;
		private List<Tile> HeldCharacters;

		public GameForm()
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
			CellValues = new Dictionary<Coordinates, Cell>();

			Coordinates coordinates = new Coordinates(12, 12);// test
			CellValues.Add(coordinates, new Cell(coordinates, 5, 5, new Tile(true), false));// test
			HeldCharacters = new List<Tile>();// test
			HeldCharacters.Add(new Tile('K'));// test

			InitAllDataGridViews(); /// TODO get data from GameModel

			InitFormHelper.FormatSingleCell(4, 4, boardGridView, false); // test
			AddAllHeldCharacters();

		}

		private void AddAllHeldCharacters()
		{
			for( int Index = 0 ; Index < HeldCharacters.Count ; Index++)
			{
				FirstHeldCharactersDataGrid[Index, 0].Value = HeldCharacters[Index].GetLetter(); //TODO do this same for all HeldCharacters
			}
		}

		private void InitAllDataGridViews() //TODO move to InitFormHelper?
		{
			// init main board
			InitFormHelper.InitBoard(boardGridView, BOARD_SIZE);

			// init HeldCharactersLabelDataGrid boards
			SecondHeldCharactersLabelDataGrid[0, 0].Value = "Held Characters";/// TODO get data from GameModel
			FirstHeldCharactersLabelDataGrid[0, 0].Value = "Held Characters";/// TODO get data from GameModel

			// init HeldCaractersDataGrid boards
			InitFormHelper.InitBoard(FirstHeldCharactersDataGrid, 1);
			InitFormHelper.InitBoard(SecondHeldCharactersDataGrid, 1);

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

		/// <summary>
		/// Metoda wywolywana gdy wpiszemy coś do danej komorki.
		/// </summary>
		private void board_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				AdjustInsertedString(e);
				foreach (Tile heldCharacter in HeldCharacters)
				{
					string CellLetter = heldCharacter.GetLetter().ToString();
					if (CellLetter.Contains(boardGridView[e.ColumnIndex, e.RowIndex].Value.ToString()))
					{
						Cell cellValue = CellValues[new Coordinates(e.ColumnIndex, e.RowIndex)];
						cellValue.SetTile(heldCharacter);

						heldCharacter.SetIsEmpty(true);

						removeCharacterFromHeldCharactersDataGrid(CellLetter, FirstHeldCharactersDataGrid);
					}
				}
			}
			catch
			{
				
			}
					
		}
		/// <summary>
		/// Metoda przycina string do jednej litery oraz zmienia na wielka litere.
		/// </summary>
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

			foreach (var CellValue in CellValues.Values)
			{
				if (CellValue.GetXCoordinate() == e.ColumnIndex && CellValue.GetYCoordinate() == e.RowIndex)
				{
					//the black background 
					Rectangle rectangle2 = new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height);
					e.Graphics.FillRectangle(Brushes.Black, rectangle2);

					// the white foreground
					Rectangle rectangle = new Rectangle(e.CellBounds.X+1 , e.CellBounds.Y+1 , e.CellBounds.Width-2, e.CellBounds.Height-2);
					e.Graphics.FillRectangle(Brushes.White, rectangle);
					Font f = new Font(e.CellStyle.Font.FontFamily, 7);

					int letterMultiplier = CellValue.GetLetterMultiplier();
					string LetterMultiplierString = ConvertIntToString(letterMultiplier);

					e.Graphics.DrawString(LetterMultiplierString, f, Brushes.Black, rectangle);
					e.PaintContent(e.ClipBounds);
					e.Handled = true;

				}
			}

		}

		private static string ConvertIntToString(int letterMultiplier)
		{
			if (letterMultiplier == 0) // Letter empty
			{
				return "";
			}
			else
			{
				return letterMultiplier.ToString();
			}
		}

		private void dataGridView1_CellContentClick_2(object sender, DataGridViewCellEventArgs e)
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

				TemporaryCopingCharacter = String.Copy(value);
				currentCell.Value = null;
			}
		}

		private void boardGridView_CellMouseUp(object sender2, DataGridViewCellMouseEventArgs e)
		{
			DataGridView sender = (DataGridView)sender2;

			DataGridViewCell currentCell = sender.CurrentCell;

			if (TemporaryCopingCharacter != null)
			{
				currentCell.Value = String.Copy(TemporaryCopingCharacter);
				CellValues[new Coordinates(currentCell.RowIndex, currentCell.ColumnIndex)].GetTile()
					.SetLetter(TemporaryCopingCharacter.ToCharArray()[0]); //TODO now it throws exception because we create new Coordinates object. Find tile in other way.
				TemporaryCopingCharacter = null;
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


		}

		private void FirstHeldCharactersDataGrid_CellMouseDown(object sender2, DataGridViewCellMouseEventArgs e)
		{
			CellMouseDown(sender2);
		}


		public void UptadeForm()
		{
			//TODO
		}
	}
}
