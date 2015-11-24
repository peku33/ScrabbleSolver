using System;
using System.Text;

namespace ScrabbleSolver
{
	class Program
	{
		static void Main(string[] args)
		{
			String FileName = @"..\..\Slowa.txt";
			Dictionary.Dictionary D = new Dictionary.Dictionary(FileName);

			while(true)
			{
				Console.Write("Podaj litery do ulozenia: ");
				String Characters = Console.ReadLine();
				if(Characters.Length == 0)
					break;

				Dictionary.WordSet Result = D.FindWordsToBeCreatedWithCharacters(Characters);

				Console.WriteLine();
				foreach(String S in Result)
				{
					Console.WriteLine("\t{0}", S);
				}
				Console.WriteLine();
			}

		}
	}
}
