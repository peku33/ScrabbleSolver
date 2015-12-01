using System;

namespace ScrabbleSolver.Dictionary
{
	/// <summary>
	/// Klasa testująca wydajność słowników
	/// </summary>
	abstract class Benchmark
	{
		/// <summary>
		/// Testowanie prędkości:
		///		* Ładowanie słownika
		///		* Wyszukiwanie istnienia słów:
		///			- ęą
		///			- żyznościach
		///			- żywotopisarstwa
		///			- dwuwiosłowymi
		///			- żżż
		///		* Dopasowanie:
		///			- żyźnicom do m @ 7, cionyźż
		///			- żyźnicom do m @ 7, cio yźż
		/// </summary>
		/// <param name="D">Implementacja słownika do przetestowania</param>
		public static void Benchmark1(Dictionary D)
		{
			System.Diagnostics.Stopwatch SW = new System.Diagnostics.Stopwatch();

			Console.WriteLine("=== {0} ===", D.GetType().Name);

			///////////////////////////////////////////////////////////

			SW.Restart();
			D.Reload();
			SW.Stop();

			Console.WriteLine("Load: {0}ms", SW.ElapsedMilliseconds);

			///////////////////////////////////////////////////////////

			String[] FindWords = new string[] { "ęą", "żyznościach", "żywotopisarstwa", "dwuwiosłowymi", "żżż"};
            foreach(String FindWord in FindWords)
			{
				SW.Restart();
				bool Result = D.Exists(FindWord);
				SW.Stop();
				Console.WriteLine("Exists: '{0}': {1}ms ({2})", FindWord, SW.ElapsedMilliseconds, Result);
            }

			///////////////////////////////////////////////////////////

			Dictionary.AlreadySetLetters ASL = new Dictionary.AlreadySetLetters();
			ASL.Set(7, 'm');

			Dictionary.HeldCharacters HC1 = new Dictionary.HeldCharacters(D.GetDictionaryEncoding());
			HC1.Add("cionyźż");

			Dictionary.HeldCharacters HC2 = new Dictionary.HeldCharacters(D.GetDictionaryEncoding());
			HC2.Add("cio yźż");

			SW.Restart();
			Dictionary.WordsFound WF1 = D.Find(ASL, HC1);
			SW.Stop();

			Console.WriteLine("Find1 (m): {0}ms ({1})", SW.ElapsedMilliseconds, WF1.Count);

			SW.Restart();
			Dictionary.WordsFound WF2 = D.Find(ASL, HC2);
			SW.Stop();

			Console.WriteLine("Find2 (blank): {0}ms ({1})", SW.ElapsedMilliseconds, WF2.Count);

			///////////////////////////////////////////////////////////

			foreach(Dictionary.WordFound WF in WF1)
				Console.Write(WF.GetWord() + ", ");

			Console.WriteLine();
			Console.WriteLine();

			foreach(Dictionary.WordFound WF in WF2)
				Console.Write(WF.GetWord() + ", ");

			///////////////////////////////////////////////////////////


			Console.WriteLine();
		}
	}
}
