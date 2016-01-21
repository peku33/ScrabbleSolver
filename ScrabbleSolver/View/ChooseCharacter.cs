using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScrabbleSolver.View
{
	public partial class ChooseCharacter : Form
	{
		public ChooseCharacter()
		{
			InitializeComponent();
		}

		private void tileReplaceLabel_Click(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Letter = richTextBox1.SelectedText;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void richTextBox1_TextChanged_1(object sender, EventArgs e)
		{
			richTextBox1.SelectedText = e.ToString().ToUpper();
			if (richTextBox1.SelectedText.Length > 1)
			{
				richTextBox1.SelectedText = richTextBox1.SelectedText
					.Substring(0, 1);
			}
		}
		public string Letter { get; set; }
	}
}
