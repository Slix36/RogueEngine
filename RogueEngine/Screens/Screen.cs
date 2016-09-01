using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libtcod;

namespace RogueEngine
{
	public abstract class Screen
	{
		protected Engine parent;
		protected TerminalManager terminalManager;

		public Screen(Engine p, TerminalManager manager)
		{
			parent = p;
			terminalManager = manager;
		}
		
		public abstract void displayOutput();
		public abstract void update();
		public abstract Screen respondToUserInput(List<KeyHistory> kbHistory);

		public void printDebug()
		{
			GameTerminal debugTerminal = terminalManager.findTerminal("Debug");

			if (debugTerminal != null)
			{
				if (parent.debug)
				{
					debugTerminal.clear((char)0, new RColor(255, 255, 255, 255), new RColor(0, 0, 0, 30));
					debugTerminal.write("Playtime: " + string.Format("{0:0.00}", TCODSystem.getElapsedSeconds()) + " FPS: " + TCODSystem.getFps().ToString(), 1, 0, new RColor(255, 255, 255, 255), new RColor(0, 0, 0, 30));
				}
			}
		}
	}
}
