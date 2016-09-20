using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Onlywatch
{
	class Cell
	{
		// these values hold grid coordinates for each corner of the cell
		public int x1;
		public int x2;
		public int y1;
		public int y2;

		// width and height of room in terms of grid
		public double w;
		public double h;

		// center point of the cell
		public Point center;

		// velocity of cell
		public Point velocity;

		public void move(Point newCord)
		{
			x1 += newCord.X;
			y1 += newCord.Y;
			x2 = x1 + (int)w;
			y2 = y1 + (int)h;
			center = new Point((int)Math.Floor((decimal)((x1 + x2) / 2)),
				(int)Math.Floor((decimal)((y1 + y2) / 2)));
			velocity = new Point();
		}

		// constructor for creating new rooms
		public Cell(int x, int y, int w, int h)
		{

			x1 = x;
			x2 = x + w;
			y1 = y;
			y2 = y + h;
			/*x = x * Main.TILE_WIDTH;
			y = y * Main.TILE_HEIGHT;*/
			this.w = w;
			this.h = h;
			center = new Point((int)Math.Floor((decimal)((x1 + x2) / 2)),
				(int)Math.Floor((decimal)((y1 + y2) / 2)));
		}

		// return true if this room intersects provided room
		public bool intersects(Cell cell)
		{
			return (x1 <= cell.x2 && x2 >= cell.x1 &&
				y1 <= cell.y2 && y2 >= cell.y1);
		}
	}
}
