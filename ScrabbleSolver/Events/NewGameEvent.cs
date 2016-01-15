using System;
using System.Collections.Generic;
using ScrabbleSolver.Common;
using ScrabbleSolver.Model.Items;

namespace ScrabbleSolver.Events
{
	public class NewGameEvent : ApplicationEvent
	{
		public readonly Dictionary<PlayerIdEnum, Boolean> ShouldAddPlayerDictionary;
		public readonly Dictionary<PlayerIdEnum, Boolean> IsComputerPlayer;

		public NewGameEvent(Dictionary<PlayerIdEnum, bool> shouldAddPlayerDictionary, Dictionary<PlayerIdEnum, bool> isComputerPlayer)
		{
			ShouldAddPlayerDictionary = shouldAddPlayerDictionary;
			IsComputerPlayer = isComputerPlayer;
		}


	}
}