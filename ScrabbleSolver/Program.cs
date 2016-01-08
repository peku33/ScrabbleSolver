﻿using System;
using System.Text;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace ScrabbleSolver
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			String DictionaryFile = @"..\..\Slowa.txt";

			Encoding.Encoding DictionaryEncoding = new Encoding.Polish();
			Dictionary.Dictionary D = new Dictionary.TrieDictionary(DictionaryFile, DictionaryEncoding);

			//Dictionary.Benchmark.Benchmark1(D);

			//Console.Read();
			D.Reload();
			Model.Model GameModel = new Model.Model(D);

			Controller.Controller GameController = new Controller.Controller(GameModel);

			GameController.Start();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
}
