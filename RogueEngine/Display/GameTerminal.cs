using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libtcod;

namespace RogueEngine
{
	public class Tile
	{
		public char glyph = (char)0;
		public RColor foreGround = new RColor(255, 255, 255, 255);
		public RColor backGround = new RColor(0, 0, 0, 0);

		public Tile()
		{
		}

		public Tile(char glyph, RColor fore, RColor back)
		{
			this.glyph = glyph;
			foreGround = fore;
			backGround = back;
		}
	}

	public class RColor
	{
		private int red;
		private int green;
		private int blue;
		private int alpha;

		public RColor(int r, int g, int b)
		{
			red = r;
			green = g;
			blue = b;
			alpha = 255;
		}

		public RColor(int r, int g, int b, int a)
		{
			red = r;
			green = g;
			blue = b;
			alpha = a;
		}

		public TCODColor GetTCODColor()
		{
			return new TCODColor(red, green, blue);
		}

		public TCODColor GetTCODColor(TCODColor blend)
		{
			return TCODColor.Interpolate(blend, new TCODColor(red, green, blue), alpha / 255.0f);
		}

		public int Red
		{
			get
			{
				return red;
			}
			set
			{
				red = value;
			}
		}

		public int Green
		{
			get
			{
				return green;
			}
			set
			{
				green = value;
			}
		}

		public int Blue
		{
			get
			{
				return blue;
			}
			set
			{
				blue = value;
			}
		}

		public int Alpha
		{
			get
			{
				return alpha;
			}
			set
			{
				alpha = value;
			}
		}

		public static explicit operator RColor(TCODColor color)
		{
			RColor temp = new RColor(color.Red, color.Green, color.Blue, 255);

			return temp;
		}
	}

    public class GameTerminal
    {
		private TerminalManager manager;
		private string name;

		private int x, y, width, height, cursorX, cursorY;

		private Tile[,] glyphs;
		private Tile[,] oldGlyphs;

		private char clearChar = ' ';
		private RColor defaultForeground = new RColor(255, 255, 255, 255);
		private RColor defaultBackground = new RColor(0, 0, 0, 0);

		private TCODConsole terminal;

		public GameTerminal(TerminalManager manager)
		{
			this.manager = manager;
			this.name = "Terminal" + manager.NextTerminalID;

			this.x = manager.DefaultX;
			this.y = manager.DefaultY;
			this.width = manager.DefaultWidth;
			this.height = manager.DefaultHeight;

			terminal = new TCODConsole(width, height);

			cursorX = 0;
			cursorY = 0;

			glyphs = new Tile[width, height];
			oldGlyphs = new Tile[width, height];

			clear();
		}

		public GameTerminal(TerminalManager manager, string name)
		{
			this.manager = manager;
			this.name = name;

			this.x = manager.DefaultX;
			this.y = manager.DefaultY;
			this.width = manager.DefaultWidth;
			this.height = manager.DefaultHeight;

			terminal = new TCODConsole(width, height);

			cursorX = 0;
			cursorY = 0;

			glyphs = new Tile[width, height];
			oldGlyphs = new Tile[width, height];

			clear();
		}

		public GameTerminal(TerminalManager manager, string name, int x, int y, int width, int height)
		{
			this.manager = manager;
			this.name = name;

			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;

			terminal = new TCODConsole(width, height);
			
			cursorX = 0;
			cursorY = 0;

			glyphs = new Tile[width, height];
			oldGlyphs = new Tile[width, height];

			clear();
		}

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		public int X
		{
			get
			{
				return x;
			}
			set
			{
				x = value;
			}
		}

		public int Y
		{
			get
			{
				return y;
			}
			set
			{
				y = value;
			}
		}

		public int Width
		{
			get
			{
				return width;
			}
		}

		public int Height
		{
			get
			{
				return height;
			}
		}

		public char charAt(int x, int y)
		{
			return glyphs[x, y].glyph;
		}

		public RColor backgroundAt(int x, int y)
		{
			return glyphs[x, y].backGround;
		}

		public RColor foregroundAt(int x, int y)
		{
			return glyphs[x, y].foreGround;
		}

		public void clear()
		{
			clear(clearChar, defaultForeground, defaultBackground);
		}

		public void clear(char c)
		{
			clear(c, defaultForeground, defaultBackground);
		}

		public void clear(char c, RColor fore)
		{
			clear(c, fore, defaultBackground);
		}

		public void clear(char c, RColor fore, RColor back)
		{
			Tile temp = new Tile();

			temp.glyph = c;
			temp.foreGround = fore;
			temp.backGround = back;

			clear(temp);
		}

		public void clear(Tile t)
		{
			oldGlyphs = (Tile[,])glyphs.Clone();

			cursorX = 0;
			cursorY = 0;

			for (int j = 0; j < height; j++)
			{
				for (int i = 0; i < width; i++)
				{
					glyphs[i, j] = t;
				}
			}
		}

		public void write(char c)
		{
			write(c, cursorX, cursorY, defaultForeground, defaultBackground);
		}

		public void write(char c, RColor fore)
		{
			write(c, cursorX, cursorY, fore, defaultBackground);
		}

		public void write(char c, RColor fore, RColor back)
		{
			write(c, cursorX, cursorY, fore, back);
		}

		public void write(char c, int x, int y)
		{
			write(c, x, y, defaultForeground, defaultBackground);
		}

		public void write(char c, int x, int y, RColor fore)
		{
			write(c, x, y, fore, defaultBackground);
		}

		public void write(char c, int x, int y, RColor fore, RColor back)
		{
			Tile temp = new Tile(c, fore, back);
			
			oldGlyphs[x,y] = glyphs[x,y];

			glyphs[x,y] = temp;

			cursorX++;
			if (cursorX == width)
			{
				cursorX = 0;
				cursorY++;
			}
		}

		public void write(string s)
		{
			write(s, cursorX, cursorY, defaultForeground, defaultBackground);
		}

		public void write(string s, RColor fore)
		{
			write(s, cursorX, cursorY, fore, defaultBackground);
		}

		public void write(string s, RColor fore, RColor back)
		{
			write(s, cursorX, cursorY, fore, back);
		}

		public void write(string s, int x, int y)
		{
			write(s, x, y, defaultForeground, defaultBackground);
		}

		public void write(string s, int x, int y, RColor fore)
		{
			write(s, x, y, fore, defaultBackground);
		}

		public void write(string s, int x, int y, RColor fore, RColor back)
		{
			cursorX = x;
			cursorY = y;

			foreach (char c in s)
				write(c, cursorX, cursorY, fore, back);
		}

		public void writeCenter(string s, int y)
		{
			writeCenter(s, y, defaultForeground, defaultBackground);
		}

		public void writeCenter(string s, int y, RColor fore)
		{
			writeCenter(s, y, fore, defaultBackground);
		}

		public void writeCenter(string s, int y, RColor fore, RColor back)
		{
			write(s, width / 2 - s.Length / 2, y, fore, back);
		}

		public void writeOver(char c)
		{
			writeOver(c, cursorX, cursorY, defaultForeground, defaultBackground);
		}

		public void writeOver(char c, RColor fore)
		{
			writeOver(c, cursorX, cursorY, fore, defaultBackground);
		}

		public void writeOver(char c, RColor fore, RColor back)
		{
			writeOver(c, cursorX, cursorY, fore, back);
		}

		public void writeOver(char c, int x, int y)
		{
			writeOver(c, x, y, defaultForeground, defaultBackground);
		}

		public void writeOver(char c, int x, int y, RColor fore)
		{
			writeOver(c, x, y, fore, defaultBackground);
		}

		public void writeOver(char c, int x, int y, RColor fore, RColor back)
		{
			Tile temp = new Tile();

			if (c == (char)0)
				temp.glyph = glyphs[x, y].glyph;
			else
				temp.glyph = c;

			temp.backGround = (RColor)back.GetTCODColor(glyphs[x, y].backGround.GetTCODColor());
			temp.foreGround = (RColor)fore.GetTCODColor(temp.backGround.GetTCODColor());

			oldGlyphs[x, y] = glyphs[x, y];

			glyphs[x, y] = temp;

			cursorX++;
			if (cursorX == width)
			{
				cursorX = 0;
				cursorY++;
			}
		}

		public void writeOver(string s)
		{
			writeOver(s, cursorX, cursorY, defaultForeground, defaultBackground);
		}

		public void writeOver(string s, RColor fore)
		{
			writeOver(s, cursorX, cursorY, fore, defaultBackground);
		}

		public void writeOver(string s, RColor fore, RColor back)
		{
			writeOver(s, cursorX, cursorY, fore, back);
		}

		public void writeOver(string s, int x, int y)
		{
			writeOver(s, x, y, defaultForeground, defaultBackground);
		}

		public void writeOver(string s, int x, int y, RColor fore)
		{
			writeOver(s, x, y, fore, defaultBackground);
		}

		public void writeOver(string s, int x, int y, RColor fore, RColor back)
		{
			cursorX = x;
			cursorY = y;

			foreach (char c in s)
				writeOver(c, cursorX, cursorY, fore, back);
		}

		public void writeOverCenter(string s, int y)
		{
			writeOverCenter(s, y, defaultForeground, defaultBackground);
		}

		public void writeOverCenter(string s, int y, RColor fore)
		{
			writeOverCenter(s, y, fore, defaultBackground);
		}

		public void writeOverCenter(string s, int y, RColor fore, RColor back)
		{
			writeOver(s, width / 2 - s.Length / 2, y, fore, back);
		}

		public void draw(bool transparency, bool direct)
		{
			if (direct)
			{
				for (int j = 0; j < height; j++)
				{
					for (int i = 0; i < width; i++)
					{
						Tile temp = glyphs[i, j];

						if (transparency)
						{
							TCODConsole.root.putCharEx(i, j, temp.glyph, temp.foreGround.GetTCODColor(TCODConsole.root.getCharForeground(i - x, j - y)), temp.backGround.GetTCODColor(TCODConsole.root.getCharBackground(i - x, j - y)));
						}
						else
						{
							TCODConsole.root.putCharEx(i, j, temp.glyph, temp.foreGround.GetTCODColor(), temp.backGround.GetTCODColor());
						}
					}
				}
			}
			else
			{
				for (int j = 0; j < height; j++)
				{
					for (int i = 0; i < width; i++)
					{
						Tile temp = glyphs[i, j];

						if (transparency)
						{
							terminal.putCharEx(i, j, temp.glyph, temp.foreGround.GetTCODColor(TCODConsole.root.getCharForeground(i - x, j - y)), temp.backGround.GetTCODColor(TCODConsole.root.getCharBackground(i - x, j - y)));
						}
						else
						{
							terminal.putCharEx(i, j, temp.glyph, temp.foreGround.GetTCODColor(), temp.backGround.GetTCODColor());
						}
					}
				}

				TCODConsole.blit(terminal, 0, 0, terminal.getWidth(), terminal.getHeight(), TCODConsole.root, x, y);
			}
		}

		public void TileBlit(Tile[,] tiles, int x, int y)
		{
			oldGlyphs = (Tile[,])glyphs.Clone();

			for (int j = 0; j + y < width && j < tiles.GetLength(1); j++)
			{
				for (int i = 0; i + x < height && i < tiles.GetLength(0); i++)
				{
					glyphs[i + x, j + y] = tiles[i,j];
				}
			}
		}

		public TCODConsole Terminal
		{
			get
			{
				return terminal;
			}
		}
	}
}