using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrabbleSolver.Common
{
	class Consts
	{
		public static readonly string FIRST_PLAYER = "First player: ";
		public static readonly string SECOND_PLAYER = "Second player: ";
		public static readonly string THIRD_PLAYER = "Third player: ";
		public static readonly string FOURTH_PLAYER = "Fourth player: ";

		public static readonly string PLAYER_SCORE = "Player score";


		public static String GetStringByPlayerIdEnum(PlayerIdEnum playerIdEnum)
		{
			switch (playerIdEnum)
			{
				case PlayerIdEnum.FIRST_PLAYER: return FIRST_PLAYER;
				case PlayerIdEnum.SECOND_PLAYER: return SECOND_PLAYER;
				case PlayerIdEnum.THIRD_PLAYER: return THIRD_PLAYER;
				case PlayerIdEnum.FOURTH_PLAYER: return FOURTH_PLAYER;
			}
			return "";
		}

		public static String GetStringByGameInfoTypeEnum(GameInfoTypeEnum GameInfoTypeEnum)
		{
			switch (GameInfoTypeEnum)
			{
				case GameInfoTypeEnum.PLAYER_SCORE: return PLAYER_SCORE;
			}
			return "";
		}
	}
}
