using System;

namespace ScrabbleSolver.Encoding
{
	/// <summary>
	/// Klasa której zadaniem jest przekodowywanie znaków (w tym narodowościowych) na indeksy możliwie małej tablicy
	/// </summary>
	public abstract class Encoding
	{
		/// <summary>
		/// Zwraca rozmiar tablicy potrzebnej do zakodwania wszystkich liter
		/// </summary>
		/// <returns>Rozmiar tablicy</returns>
		abstract public int GetArraySize();

		/// <summary>
		/// Zwraca indeks podanej litery w tablicy
		/// </summary>
		/// <param name="C">Litera</param>
		/// <returns>Kod litery</returns>
		abstract public int ToArrayIndex(char C);

		abstract public char FromArrayIndex(int I);
	}
}
