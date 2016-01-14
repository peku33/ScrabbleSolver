﻿using System;
﻿using System.Collections.Concurrent;
﻿using System.Text;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;
﻿using ScrabbleSolver.Events;
﻿using ScrabbleSolver.Language;

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
			Language.Language Language = new Polish();
			//Dictionary.Benchmark.Benchmark1(D);

			//Console.Read();
			D.Reload();
			BlockingCollection<ApplicationEvent> ViewEvents = new BlockingCollection<ApplicationEvent>();
            Model.Model GameModel = new Model.Model(D, Language);
			//GameForm GameForm = new GameForm();
			GameForm GameForm = null;
			Controller.Controller GameController = new Controller.Controller(GameModel, GameForm, ViewEvents);

			GameController.AddStrategies();

			GameController.Start();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new GameForm()); //TODO use GameForm implemented above
		}
	}
}
