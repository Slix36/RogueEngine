using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libtcod;

namespace RogueEngine
{
	public class TerminalManager
	{
		private int defaultX = 0;
		private int defaultY = 0;
		private int defaultWidth = TCODConsole.root.getWidth();
		private int defaultHeight = TCODConsole.root.getHeight();

		private List<GameTerminal> terminals;

		private int nextTerminalID;
		
		public TerminalManager()
		{
			terminals = new List<GameTerminal>();
			nextTerminalID = 0;
			terminals.Add(new GameTerminal(this, "Debug", 0, 0, defaultWidth, 1));
		}

		public TerminalManager(int width, int height)
		{
			defaultWidth = width;
			defaultHeight = height;

			terminals = new List<GameTerminal>();
			nextTerminalID = 0;
			terminals.Add(new GameTerminal(this, "Debug", 0, 0, width, 1));
		}

		public int DefaultX
		{
			get
			{
				return defaultX;
			}
			set
			{
				defaultX = value;
			}
		}

		public int DefaultY
		{
			get
			{
				return defaultY;
			}
			set
			{
				defaultY = value;
			}
		}

		public int DefaultWidth
		{
			get
			{
				return defaultWidth;
			}
			set
			{
				defaultWidth = value;
			}
		}

		public int DefaultHeight
		{
			get
			{
				return defaultHeight;
			}
			set
			{
				defaultHeight = value;
			}
		}

		public int NextTerminalID
		{
			get
			{
				return nextTerminalID;
			}
		}

		public void createTerminal()
		{
			terminals.Add(new GameTerminal(this, "Terminal" + nextTerminalID, defaultX, defaultY, defaultWidth, defaultHeight));
		}

		public void createTerminal(string name)
		{
			terminals.Add(new GameTerminal(this, name, defaultX, defaultY, defaultWidth, defaultHeight));
		}

		public void createTerminal(string name, int x, int y, int width, int height)
		{
			terminals.Add(new GameTerminal(this, name, x, y, width, height));
		}

		public GameTerminal findTerminal(string name)
		{
			foreach (GameTerminal gt in terminals)
			{
				if (gt.Name == name)
					return gt;
			}
			return null;
		}

		public GameTerminal grabTerminal(int pos)
		{
			if (terminals.Count > pos)
				return terminals[pos];
			return null;
		}

		public void removeTerminal(string name)
		{
			terminals.Remove(findTerminal(name));
		}

		public void clearTerminals()
		{
			terminals.Clear();
			terminals.Add(new GameTerminal(this, "Debug", 0, 0, defaultWidth, 1));
		}

		public void drawTerminals(bool debug)
		{
			GameTerminal debugTerminal = null;

			foreach (GameTerminal terminal in terminals)
			{
				if (terminal.Name == "Debug" && debug)
					debugTerminal = terminal;
				else if (terminal.Name != "Debug")
					terminal.draw(true, false);
			}

			if (debugTerminal != null)
				debugTerminal.draw(true, false);
		}
	}
}
