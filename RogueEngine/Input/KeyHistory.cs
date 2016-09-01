using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libtcod;

namespace RogueEngine
{
	public struct KeyStatus
	{
		public char character;
		public TCODKeyCode keyCode;
		public TCODKeyStatus status;
		
		public KeyStatus(char character, TCODKeyCode keyCode, TCODKeyStatus status)
		{
			this.character = character;
			this.keyCode = keyCode;
			this.status = status;
		}
	}

	public class KeyHistory
	{
		private TimeSpan timeStamp;

		private KeyStatus[] ksMap;

		public KeyHistory(TimeSpan timeStamp)
		{
			this.timeStamp = timeStamp;
			ksMap = new KeyStatus[250];
		}

		public void RegisterKeyStatus(TCODKey key, TCODKeyStatus status)
		{
			if (key.KeyCode == TCODKeyCode.Char)
			{
				int it = Enum.GetValues(typeof(TCODKeyCode)).Length + (int)key.Character;
				ksMap[it] = new KeyStatus(key.Character, key.KeyCode, status);
			}
			else
			{
				ksMap[(int)key.KeyCode] = new KeyStatus(key.Character, key.KeyCode, status);
			}
		}

		public TimeSpan TimeStamp
		{
			get
			{
				return timeStamp;
			}
		}

		public KeyStatus[] KSMap
		{
			get
			{
				return ksMap;
			}
		}
	}
}
