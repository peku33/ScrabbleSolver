using System;

namespace ScrabbleSolver.Dictionary
{
	/// <summary>
	/// Wrapper class used to store unique words and allowing fast operations
	/// </summary>
	class WordSet : System.Collections.Generic.HashSet<String>
	{
		public WordSet(): base()
		{

		}
	}
}
