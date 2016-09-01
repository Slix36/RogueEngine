using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using libtcod;

namespace RogueEngine
{
    public class Engine
    {
        private int WINDOW_WIDTH;
        private int WINDOW_HEIGHT;
        private int TARGET_FPS;
		private int CONTROL_FPS;
		
        public bool debug;
        public bool running;

		private string windowName;

        private TerminalManager terminalManager;

        private Screen screen;

        private List<KeyHistory> kbHistory;

		public string FONT;
		public int FONT_WIDTH;
		public int FONT_HEIGHT;

        public string GRAPHICS_SETTING;
        public int MASTER_VOLUME;
        public int MUSIC_VOLUME;
        public int SFX_VOLUME;

        public Engine(string windowName, string settings)
		{
			this.windowName = windowName;
			running = true;
			debug = false;

			if (settings == "")
			{
				WINDOW_WIDTH = RogueEngineSettings.DEFAULT_WINDOW_WIDTH;
				WINDOW_HEIGHT = RogueEngineSettings.DEFAULT_WINDOW_HEIGHT;
				TARGET_FPS = RogueEngineSettings.DEFAULT_TARGET_FPS;

				FONT = RogueEngineSettings.DEFAULT_FONT;
				FONT_WIDTH = RogueEngineSettings.DEFAULT_FONT_WIDTH;
				FONT_HEIGHT = RogueEngineSettings.DEFAULT_FONT_HEIGHT;

				GRAPHICS_SETTING = RogueEngineSettings.DEFAULT_GRAPHICS_QUALITY;
				MASTER_VOLUME = RogueEngineSettings.DEFAULT_MASTER_VOLUME;
				MUSIC_VOLUME = RogueEngineSettings.DEFAULT_MUSIC_VOLUME;
				SFX_VOLUME = RogueEngineSettings.DEFAULT_SFX_VOLUME;
			}
			else
			{
				RogueEngineSettings RESettings = new RogueEngineSettings(settings);

				WINDOW_WIDTH = RESettings.WINDOW_WIDTH;
				WINDOW_HEIGHT = RESettings.WINDOW_HEIGHT;
				TARGET_FPS = RESettings.TARGET_FPS;

				FONT = RESettings.FONT;
				FONT_WIDTH = RESettings.FONT_WIDTH;
				FONT_HEIGHT = RESettings.FONT_WIDTH;

				GRAPHICS_SETTING = RESettings.GRAPHICS_QUALITY;
				MASTER_VOLUME = RESettings.MASTER_VOLUME;
				MUSIC_VOLUME = RESettings.MUSIC_VOLUME;
				SFX_VOLUME = RESettings.SFX_VOLUME;
			}

			CONTROL_FPS = TARGET_FPS;

			createWindow();
        }

		private void createWindow()
		{
			int fontflags = (int)TCODFontFlags.Greyscale | (int)TCODFontFlags.LayoutAsciiInRow;
			TCODConsole.setCustomFont(FONT, fontflags);
			TCODConsole.setKeyboardRepeat(500, 5000 / TARGET_FPS);
			TCODConsole.initRoot(WINDOW_WIDTH / FONT_WIDTH, WINDOW_HEIGHT / FONT_HEIGHT, windowName, false, TCODRendererType.SDL);
			TCODSystem.setFps(CONTROL_FPS);

			terminalManager = new TerminalManager(WINDOW_WIDTH / FONT_WIDTH, WINDOW_HEIGHT / FONT_HEIGHT);
		}

		public void adjustFPS()
		{
			if (TCODSystem.getFps() < TARGET_FPS)
			{
				CONTROL_FPS++;
				TCODSystem.setFps(CONTROL_FPS);
			}
			else if (TCODSystem.getFps() > TARGET_FPS)
			{
				CONTROL_FPS--;
				TCODSystem.setFps(CONTROL_FPS);
			}
		}

        public void run(Screen tempscreen)
		{
			screen = tempscreen;
			kbHistory = new List<KeyHistory>();

			TimeSpan startTime = DateTime.Now.TimeOfDay;
			double timectrl = startTime.TotalMilliseconds;
			double timespent = 0;

			while (running && !TCODConsole.isWindowClosed())
            {				
				screen.update();
				screen.printDebug();
				terminalManager.drawTerminals(debug);

				TCODConsole.flush();

				KeyHistory temp = new KeyHistory(DateTime.Now.TimeOfDay - startTime);

				TCODKey key = TCODConsole.checkForKeypress((int)TCODKeyStatus.KeyPressed);
				
				temp.RegisterKeyStatus(key, TCODKeyStatus.KeyPressed);

				kbHistory.Add(temp);
				screen = screen.respondToUserInput(kbHistory);

				timespent = DateTime.Now.TimeOfDay.TotalMilliseconds - timectrl;

				
				if (timespent > 250)
				{
					adjustFPS();
					timectrl = DateTime.Now.TimeOfDay.TotalMilliseconds;
				}
			}
        }

		public TerminalManager TManager
		{
			get
			{
				return terminalManager;
			}
		}
    }
}
