using System;
using System.Text;
using ScrabbleSolver.Language;

namespace ScrabbleSolver
{
	class Program
	{
		static void Main(string[] args)
		{
			String DictionaryFile = @"..\..\Slowa.txt";

			Encoding.Encoding DictionaryEncoding = new Encoding.Polish();
			Dictionary.Dictionary D = new Dictionary.TrieDictionary(DictionaryFile, DictionaryEncoding);
			Language.Language Language = new Polish();

			//Dictionary.Benchmark.Benchmark1(D);

			//Console.Read();
			D.Reload();
			Model.Model GameModel = new Model.Model(D, Language);

			Controller.Controller GameController = new Controller.Controller(GameModel);

			GameController.Start();
		}
	}
}
