using System;
using System.CodeDom;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScrabbleSolver;
using ScrabbleSolver.Board;
using ScrabbleSolver.Common;
using ScrabbleSolver.Events;
using ScrabbleSolver.Model.Items;
using ScrabbleSolver.View;

namespace ScrabbleSolver
{
	public partial class GameForm : Form
	{
		private static int BOARD_SIZE = 15;

		private PlayerIdEnum _CurrentPlayer;
		private Dictionary<PlayerIdEnum, Dictionary<GameInfoTypeEnum, String>> _GameInfo;
		private String TemporaryCopingCharacter;
		private Dictionary<Coordinates, Cell> CellValues;
		private Dictionary<PlayerIdEnum, List<Tile>> heldCharacters;

		private Dictionary<PlayerIdEnum, DataGridViewCell> PlayerIdEnumToDataGridViewCellLabelDictionary;
		private Dictionary<PlayerIdEnum, DataGridViewCell> PlayerIdEnumToDataGridViewCellDataDictionary;

		private BlockingCollection<ApplicationEvent> viewEvents;
		private Thread Thread;
		private Thread ReplaceTileFormThread;

		private static readonly int SECOND_INDEX = 1;
		private static readonly int FIRST_INDEX = 0;
		private static readonly int ROWS_NUMBER = 1;
		private static readonly int MAX_HELD_CHARACTERS_NUMBER = 7;
		public GameForm(BlockingCollection<ApplicationEvent> viewEvents)
		{
			this.viewEvents = viewEvents;
			InitializeComponent();
		}

		/// <summary>
		/// The method that loads the Form.
		/// </summary>
		private void GameForm_Load(object sender, EventArgs e)
		{
			/// some sample data  - to remove in the future
			CellValues = new Dictionary<Coordinates, Cell>();
			_GameInfo = new Dictionary<PlayerIdEnum, Dictionary<GameInfoTypeEnum, string>>();
			heldCharacters = new Dictionary<PlayerIdEnum,List<Tile>>();

			InitAllDataGridViews();
			InitPlayerIdEnumToDataGridViewCellDictionary();

			

		}

		private void InitPlayerIdEnumToDataGridViewCellDictionary()
		{
			PlayerIdEnumToDataGridViewCellLabelDictionary = new Dictionary<PlayerIdEnum, DataGridViewCell>();
			PlayerIdEnumToDataGridViewCellLabelDictionary.Add(PlayerIdEnum.FIRST_PLAYER, FirstHeldCharactersLabelDataGrid[FIRST_INDEX, FIRST_INDEX]);
			PlayerIdEnumToDataGridViewCellLabelDictionary.Add(PlayerIdEnum.SECOND_PLAYER, FirstHeldCharactersLabelDataGrid[SECOND_INDEX, FIRST_INDEX]);
			PlayerIdEnumToDataGridViewCellLabelDictionary.Add(PlayerIdEnum.THIRD_PLAYER, SecondHeldCharactersLabelDataGrid[FIRST_INDEX, FIRST_INDEX]);
			PlayerIdEnumToDataGridViewCellLabelDictionary.Add(PlayerIdEnum.FOURTH_PLAYER, SecondHeldCharactersLabelDataGrid[SECOND_INDEX, FIRST_INDEX]);

			PlayerIdEnumToDataGridViewCellDataDictionary = new Dictionary<PlayerIdEnum, DataGridViewCell>();
			PlayerIdEnumToDataGridViewCellDataDictionary.Add(PlayerIdEnum.FIRST_PLAYER, FirstHeldCharactersDataGrid[FIRST_INDEX, FIRST_INDEX]);
			PlayerIdEnumToDataGridViewCellDataDictionary.Add(PlayerIdEnum.SECOND_PLAYER, FirstHeldCharactersDataGrid[8, FIRST_INDEX]);
			PlayerIdEnumToDataGridViewCellDataDictionary.Add(PlayerIdEnum.THIRD_PLAYER, SecondHeldCharactersDataGrid[FIRST_INDEX, FIRST_INDEX]);
			PlayerIdEnumToDataGridViewCellDataDictionary.Add(PlayerIdEnum.FOURTH_PLAYER, SecondHeldCharactersDataGrid[8, FIRST_INDEX]);
		}

		/// <summary>
		/// Metoda inicjująca pola HeldCharactersDataGrid.
		/// </summary>
		private void AddAllHeldCharacters()
		{

			foreach (KeyValuePair<PlayerIdEnum, List<Tile>> KVP in heldCharacters)
			{
				PlayerIdEnum playerIdEnum = KVP.Key;
				List<Tile> value = KVP.Value;
				DataGridViewCell DataGridViewCell = null;
				PlayerIdEnumToDataGridViewCellDataDictionary.TryGetValue(playerIdEnum, out DataGridViewCell);

				if (DataGridViewCell != null)
				{
					int FirstColumnIndexOffset = DataGridViewCell.ColumnIndex;
					DataGridView dataGridView = DataGridViewCell.DataGridView;

					for( int Index = FIRST_INDEX ; Index < value.Count ; Index++)
					{
						dataGridView[Index + FirstColumnIndexOffset, FIRST_INDEX].Value = value[Index].GetLetter(); 
					}

				}
			}
		}

		private void InitAllDataGridViews() //TODO move to InitFormHelper?
		{
			// init main board
			InitFormHelper.InitBoard(boardGridView, BOARD_SIZE);

			// init HeldCharactersLabelDataGrid boards
			SecondHeldCharactersLabelDataGrid[FIRST_INDEX, FIRST_INDEX].Value = "Held Characters Third Player";/// TODO get data from GameModel
			SecondHeldCharactersLabelDataGrid[SECOND_INDEX, FIRST_INDEX].Value = "Held Characters Fourth Player ";/// TODO get data from GameModel
			FirstHeldCharactersLabelDataGrid[FIRST_INDEX, FIRST_INDEX].Value = "Held Characters First Player";/// TODO get data from GameModel
			FirstHeldCharactersLabelDataGrid[SECOND_INDEX, FIRST_INDEX].Value = "Held Characters Second Player";/// TODO get data from GameModel
																											  
			// init HeldCaractersDataGrid boards

			InitFormHelper.InitBoard(FirstHeldCharactersDataGrid, ROWS_NUMBER);
			InitFormHelper.InitBoard(SecondHeldCharactersDataGrid, ROWS_NUMBER);

			InitFormHelper.InitGameInfoBoard(GameInfoDataGrid, _GameInfo);
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
				List<Tile> TileList = null;

				heldCharacters.TryGetValue(_CurrentPlayer, out TileList);//TODO Its just tile list of first player.
				AdjustInsertedString(e);
				foreach (Tile heldCharacter in TileList)
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
					.Substring(FIRST_INDEX, 1);
			}
		}

		private void removeCharacterFromHeldCharactersDataGrid(string cellValue, DataGridView firstHeldCharactersDataGrid) //TODO Its just tile list of first player.
		{
			Size size = firstHeldCharactersDataGrid.Size;

			for (int Index = FIRST_INDEX; Index < size.Width; ++Index)
			{
				if (firstHeldCharactersDataGrid[FIRST_INDEX, Index].Value.ToString().Contains(cellValue))
				{
					firstHeldCharactersDataGrid[FIRST_INDEX, Index].Value = "";
				}
			}

			this.Refresh();
		}

		private void board_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{

			foreach (var CellValue in CellValues.Values)
			{
				if (CellValue.GetXColumnCoordinate() == e.ColumnIndex && CellValue.GetYRowCoordinate() == e.RowIndex)
				{
					//the black background
					Rectangle rectangle2 = new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height);
					e.Graphics.FillRectangle(Brushes.Black, rectangle2);

					// the white foreground
					Rectangle rectangle = new Rectangle(e.CellBounds.X+1 , e.CellBounds.Y+1 , e.CellBounds.Width-2, e.CellBounds.Height-2);
					e.Graphics.FillRectangle(Brushes.White, rectangle);
					Font f = new Font(e.CellStyle.Font.FontFamily, 7);

					string StringToPrint = "";
					if (CellValue.GetLetterMultiplier() != 1 && CellValue.GetLetterMultiplier() != FIRST_INDEX)
					{
						StringToPrint += "L " + CellValue.GetLetterMultiplier().ToString() + " ";
					}

					if (CellValue.GetWordMultiplier() != 1 && CellValue.GetWordMultiplier() != FIRST_INDEX)
					{
						StringToPrint += "W " + CellValue.GetWordMultiplier().ToString();
					}

					e.Graphics.DrawString(StringToPrint, f, Brushes.Black, rectangle);
					e.PaintContent(e.ClipBounds);
					e.Handled = true;

				}
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

		private void CellMouseDown(object s)
		{
			DataGridView Sender = (DataGridView) s;

			DataGridViewCell CurrentCell = Sender.CurrentCell;

			if (CurrentCell.Value != null)
			{
				String value = CurrentCell.Value.ToString();

				if (TemporaryCopingCharacter != null)
				{
					PutAwayTempLetter(TemporaryCopingCharacter);
				}
				GetLetterFromHeldCharacters(String.Copy(value));
				TemporaryCopingCharacter = String.Copy(value);
				CurrentCell.Value = null;

			}
		}

		private void GetLetterFromHeldCharacters(string copy)
		{
			List<Tile> TileList = null;
			heldCharacters.TryGetValue(_CurrentPlayer, out TileList);

			foreach (Tile tile in TileList)
			{
				if (!tile.IsEmpty() && tile.GetLetter() == copy.ToCharArray()[FIRST_INDEX])
				{
					tile.SetIsEmpty(true);
				}
			}
		}

		private void PutAwayTempLetter(string temporaryCopingCharacter)
		{
			DataGridViewCell DGVC;
			PlayerIdEnumToDataGridViewCellDataDictionary.TryGetValue(_CurrentPlayer, out DGVC);

			List<Tile> TileList = null;
			heldCharacters.TryGetValue(_CurrentPlayer, out TileList);


			if (DGVC != null)
			{
				int FirstColumnIndexOffset = DGVC.ColumnIndex;
				DataGridView dataGridView = DGVC.DataGridView;

				for (int Index = FIRST_INDEX; Index < MAX_HELD_CHARACTERS_NUMBER; Index++)
				{
					if (dataGridView[Index + FirstColumnIndexOffset, FIRST_INDEX].Value == null ||
					    dataGridView[Index + FirstColumnIndexOffset, FIRST_INDEX].ToString() == "")
					{
						dataGridView[Index + FirstColumnIndexOffset, FIRST_INDEX].Value = temporaryCopingCharacter;
						break;
					}
				}

			}

			foreach (Tile tile in TileList)
			{
				if (tile.IsEmpty())
				{
					tile.SetIsEmpty(false);
					tile.SetLetter(temporaryCopingCharacter.ToCharArray()[FIRST_INDEX]);
					break;
				}
			}
		}

		private void boardGridView_CellMouseUp(object s, DataGridViewCellMouseEventArgs e)
		{
			DataGridView Sender = (DataGridView)s;

			DataGridViewCell CurrentCell = Sender.CurrentCell;

			if (TemporaryCopingCharacter != null)
			{
				bool IsBlank = TemporaryCopingCharacter == " ";

				if (IsBlank)
				{
					CurrentCell.Value = OpenFormAndGetBlankValue();

				}
				else
				{
					CurrentCell.Value = String.Copy(TemporaryCopingCharacter);
				}

				Coordinates Coordinates = new Coordinates(CurrentCell.ColumnIndex, CurrentCell.RowIndex);
				UpdateLetter(Coordinates, IsBlank);

				TemporaryCopingCharacter = null;
			}
		}

		private string OpenFormAndGetBlankValue()
		{
			using (var form = new ChooseCharacter())
			{
				var result = form.ShowDialog();
				if (result == DialogResult.OK)
				{
					return form.Letter;            //values preserved after close
				}
			}
			return "";
		}

		/// <summary>
		/// Metoda aktualizuje litere na danej komórce, lub jeśli nie istnieje komórka o danych współrzędnych - dodaje nową.
		/// </summary>
		private void UpdateLetter(Coordinates Coordinates, bool isBlank)
		{
			if (CellValues.ContainsKey(Coordinates)) //TODO bug: if we comprare two object with the same coordinates we get false....
			{
				if (CellValues[Coordinates].GetTile() != null)
				{
					CellValues[Coordinates].GetTile()
						.SetLetter(TemporaryCopingCharacter.ToCharArray()[FIRST_INDEX]);
					CellValues[Coordinates].GetTile().SetIsBlank(isBlank);
				}
				else
				{
					CellValues[Coordinates].SetTile(new Tile(TemporaryCopingCharacter.ToCharArray()[FIRST_INDEX], isBlank));
				}

			}
			else
			{
				Cell Cell = new Cell(Coordinates, 0, 0, new Tile(TemporaryCopingCharacter.ToCharArray()[FIRST_INDEX], isBlank), false);
				CellValues.Add(Coordinates, Cell);
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


		public void UpdateForm(UpdateViewEvent UpdateViewEvent)
		{

			foreach (Cell boardCell in UpdateViewEvent.BoardCells)
			{

				Coordinates coordinates = new Coordinates(boardCell.GetXColumnCoordinate(), boardCell.GetYRowCoordinate());
				CellValues.Remove(coordinates);
				CellValues.Add(coordinates, boardCell);

				if (boardCell.GetTile() != null)
				{
					boardGridView[boardCell.GetXColumnCoordinate(), boardCell.GetYRowCoordinate()].Value =
						boardCell.GetTile().GetLetter().ToString().ToUpper();
				}
				else
				{
					boardGridView[boardCell.GetXColumnCoordinate(), boardCell.GetYRowCoordinate()].Value = "";
				}
			}


			 _GameInfo = UpdateViewEvent.GameInfo;
			heldCharacters = UpdateViewEvent.HeldCharacters;
			_CurrentPlayer = UpdateViewEvent.CurrentPlayer;
			AddAllHeldCharacters();

			Invalidate();
			Update();
		}

		private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			InitAllCellValues();
			Thread = new Thread(OpenNewGameForm);
			Thread.SetApartmentState(ApartmentState.STA);
			Thread.Start();
		}

		private void InitAllCellValues()
		{
			CellValues.Clear();
			for (int IndexYCoordinate = FIRST_INDEX; IndexYCoordinate < BOARD_SIZE; ++IndexYCoordinate)
			{
				for (int IndexXCoordinate = FIRST_INDEX; IndexXCoordinate < BOARD_SIZE; ++IndexXCoordinate)
				{
					Coordinates coordinates = new Coordinates(IndexXCoordinate, IndexYCoordinate);// test
					CellValues.Add(coordinates, new Cell(coordinates, 0, 0, new Tile(true), false));// test
				}
			}

		}

		private void OpenNewGameForm()
		{
			Application.Run(new NewGameForm(viewEvents));
		}

		private void nextTurnButton_Click(object sender, EventArgs e)
		{
			List<Tile> heldCharacter = removeEmptyTilesFromList(heldCharacters[_CurrentPlayer]);
			viewEvents.Add(new PutWordEvent(CellValues.Values.ToList(), heldCharacter));
		}

		private List<Tile> removeEmptyTilesFromList(List<Tile> heldCharacters)
		{
			List<Tile> HeldCharactersWithoutEmptyTiles = new List<Tile>();
			foreach (Tile heldCharacter in heldCharacters)
			{
				if (!heldCharacter.IsEmpty())
				{
					HeldCharactersWithoutEmptyTiles.Add(heldCharacter);
				}
			}
			return HeldCharactersWithoutEmptyTiles;
		}
		private void replaceTileButton_Click(object sender, EventArgs e)
		{
			ReplaceTileFormThread = new Thread(OpenReplaceTileForm);
			ReplaceTileFormThread.SetApartmentState(ApartmentState.STA);
			ReplaceTileFormThread.Start();

		}

		private void OpenReplaceTileForm()
		{
			Application.Run(new ReplaceTileForm(viewEvents));
		}
	}
}
