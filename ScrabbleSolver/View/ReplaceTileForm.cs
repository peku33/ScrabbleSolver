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
using ScrabbleSolver.Events;

namespace ScrabbleSolver.View
{
	public partial class ReplaceTileForm : Form
	{
		private BlockingCollection<ApplicationEvent> viewEvents;
		public ReplaceTileForm(BlockingCollection<ApplicationEvent> viewEvents)
		{
			this.viewEvents = viewEvents;
			InitializeComponent();
		}

		private void richTextBox1_TextChanged(object sender, EventArgs e)
		{
			richTextBox1.Text = richTextBox1.Text
				.Substring(0, 1).ToUpper();
		}

		private void ReplaceTileForm_Click(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			viewEvents.Add(new ReplaceTileEvent(richTextBox1.Text.ToLower()));
			Close();
		}





	}
}
