using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScrabbleSolver.Common;
using ScrabbleSolver.Events;

namespace ScrabbleSolver.View
{
	public partial class NewGameForm : Form
	{
		private BlockingCollection<ApplicationEvent> viewEvents;
		public Dictionary<PlayerIdEnum, Boolean> ShouldAddPlayerDictionary;
		public Dictionary<PlayerIdEnum, Boolean> IsComputerPlayer;
		public NewGameForm(BlockingCollection<ApplicationEvent> viewEvents)
		{
			this.viewEvents = viewEvents;
			InitializeComponent();
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void label2_Click(object sender, EventArgs e)
		{

		}

		private void label3_Click(object sender, EventArgs e)
		{

		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
		{

		}

		private void newGame_Click(object sender, EventArgs e)
		{
			ShouldAddPlayerDictionary = new Dictionary<PlayerIdEnum, bool>();
			IsComputerPlayer = new Dictionary<PlayerIdEnum, bool>();
			ShouldAddPlayerDictionary.Add(PlayerIdEnum.FIRST_PLAYER, firstPlayerAddPlayerCheckBox.Checked);
			ShouldAddPlayerDictionary.Add(PlayerIdEnum.SECOND_PLAYER, secondPlayerAddPlayerCheckBox.Checked);
			ShouldAddPlayerDictionary.Add(PlayerIdEnum.THIRD_PLAYER, thirdPlayerAddPlayerCheckBox.Checked);
			ShouldAddPlayerDictionary.Add(PlayerIdEnum.FOURTH_PLAYER, fourthPlayerAddPlayerCheckBox.Checked);

			IsComputerPlayer.Add(PlayerIdEnum.FIRST_PLAYER, firstPlayerIsComputerCheckBox.Checked);
			IsComputerPlayer.Add(PlayerIdEnum.SECOND_PLAYER, secondPlayerIsComputerCheckBox.Checked);
			IsComputerPlayer.Add(PlayerIdEnum.THIRD_PLAYER, thirdPlayerIsComputerCheckBox.Checked);
			IsComputerPlayer.Add(PlayerIdEnum.FOURTH_PLAYER, fourthPlayerIsComputerCheckBox.Checked);
			
			viewEvents.Add(new NewGameEvent(ShouldAddPlayerDictionary, IsComputerPlayer));
			Close();
		}

		private void newGame_MouseClick(object sender, MouseEventArgs e)
		{

		}

		private void firstPlayerAddPlayerCheckBox_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void firstPlayerIsComputerCheckBox_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void secondPlayerAddPlayerCheckBox_CheckedChanged(object sender, EventArgs e)
		{

		}
	}
}
