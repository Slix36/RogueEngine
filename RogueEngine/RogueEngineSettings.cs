using System;
using System.Windows.Forms;
using System.IO;

namespace RogueEngine
{
	public class RogueEngineSettings
	{
		public static int DEFAULT_WINDOW_WIDTH = 800;
		public static int DEFAULT_WINDOW_HEIGHT = 600;
		public static int DEFAULT_TARGET_FPS = 60;

		public static string DEFAULT_FONT = "Fonts/terminal10x10.png";
		public static int DEFAULT_FONT_HEIGHT = 10;
		public static int DEFAULT_FONT_WIDTH = 10;

		public static string DEFAULT_GRAPHICS_QUALITY = "High";
		public static int DEFAULT_MASTER_VOLUME = 100;
		public static int DEFAULT_MUSIC_VOLUME = 100;
		public static int DEFAULT_SFX_VOLUME = 100;

		public int WINDOW_WIDTH;
		public int WINDOW_HEIGHT;
		public int TARGET_FPS;

		public string FONT;
		public int FONT_HEIGHT;
		public int FONT_WIDTH;

		public string GRAPHICS_QUALITY;
		public int MASTER_VOLUME;
		public int MUSIC_VOLUME;
		public int SFX_VOLUME;

		public RogueEngineSettings(string path)
		{
			try
			{
				if (File.Exists(path))
				{
					WINDOW_WIDTH = DEFAULT_WINDOW_WIDTH;
					WINDOW_HEIGHT = DEFAULT_WINDOW_HEIGHT;
					TARGET_FPS = DEFAULT_TARGET_FPS;

					FONT = DEFAULT_FONT;
					FONT_WIDTH = DEFAULT_FONT_WIDTH;
					FONT_HEIGHT = DEFAULT_FONT_HEIGHT;

					GRAPHICS_QUALITY = DEFAULT_GRAPHICS_QUALITY;
					MASTER_VOLUME = DEFAULT_MASTER_VOLUME;
					MUSIC_VOLUME = DEFAULT_MUSIC_VOLUME;
					SFX_VOLUME = DEFAULT_SFX_VOLUME;

					LoadData(path);
				}
				else
				{
					WINDOW_WIDTH = DEFAULT_WINDOW_WIDTH;
					WINDOW_HEIGHT = DEFAULT_WINDOW_HEIGHT;
					TARGET_FPS = DEFAULT_TARGET_FPS;

					FONT = DEFAULT_FONT;
					FONT_WIDTH = DEFAULT_FONT_WIDTH;
					FONT_HEIGHT = DEFAULT_FONT_HEIGHT;

					GRAPHICS_QUALITY = DEFAULT_GRAPHICS_QUALITY;
					MASTER_VOLUME = DEFAULT_MASTER_VOLUME;
					MUSIC_VOLUME = DEFAULT_MUSIC_VOLUME;
					SFX_VOLUME = DEFAULT_SFX_VOLUME;

					CreateData();
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				throw new Exception("ExitProgram");
			}
		}

		private void CreateData()
		{
			FileStream data = File.Create("settings.ini");
			StreamWriter sw = new StreamWriter(data);

			sw.WriteLine("font=" + DEFAULT_FONT + "|" + DEFAULT_FONT_WIDTH + "x" + DEFAULT_FONT_HEIGHT);
			sw.WriteLine("graphics_quality=" + DEFAULT_GRAPHICS_QUALITY);
			sw.WriteLine("master_volume=" + DEFAULT_MASTER_VOLUME);
			sw.WriteLine("music_volume=" + DEFAULT_MUSIC_VOLUME);
			sw.WriteLine("sfx_volume=" + DEFAULT_SFX_VOLUME);
			sw.WriteLine("target_fps=" + DEFAULT_TARGET_FPS);
			sw.WriteLine("window_size=" + DEFAULT_WINDOW_WIDTH + "x" + DEFAULT_WINDOW_HEIGHT);

			sw.Flush();
			sw.Close();
		}

		private void LoadData(string path)
		{
			FileStream data = File.Open(path, FileMode.Open);
			StreamReader sr = new StreamReader(data);

			while (!sr.EndOfStream)
			{
				SetData(sr.ReadLine());
			}

			sr.Close();
		}

		private void SetData(string data)
		{
			data = data.Replace(" ", "");

			data = data.ToLower();

			string[] variable = data.Split('=');

			try
			{
				switch (variable[0])
				{
					case "font":
						if (!variable[1].Contains("|"))
						{
							FONT = variable[1];
						}
						else
						{
							string[] fontVars = variable[1].Split('|');
							FONT = fontVars[0];
							string[] fontSize = fontVars[1].Split("xX".ToCharArray());
							FONT_WIDTH = Convert.ToInt32(fontSize[0]);
							FONT_HEIGHT = Convert.ToInt32(fontSize[1]);
						}
						break;
					case "font_width":
						FONT_WIDTH = Convert.ToInt32(variable[1]);
						break;
					case "font_height":
						FONT_HEIGHT = Convert.ToInt32(variable[1]);
						break;
					case "graphics_quality":
						GRAPHICS_QUALITY = variable[1];
						break;
					case "master_volume":
						MASTER_VOLUME = Convert.ToInt32(variable[1]);
						break;
					case "music_volume":
						MUSIC_VOLUME = Convert.ToInt32(variable[1]);
						break;
					case "sfx_volume":
						SFX_VOLUME = Convert.ToInt32(variable[1]);
						break;
					case "target_fps":
						TARGET_FPS = Convert.ToInt32(variable[1]);
						break;
					case "window_size":
						if (variable[1].Contains("x") || variable[1].Contains("X"))
						{
							string[] windowVars = variable[1].Split("xX".ToCharArray());
							WINDOW_WIDTH = Convert.ToInt32(windowVars[0]);
							WINDOW_HEIGHT = Convert.ToInt32(windowVars[1]);
						}
						else
						{
							string[] windowVars = variable[1].Split('|');
							WINDOW_WIDTH = Convert.ToInt32(windowVars[0]);
							WINDOW_HEIGHT = Convert.ToInt32(windowVars[1]);
						}
						break;
					default:
						break;
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}