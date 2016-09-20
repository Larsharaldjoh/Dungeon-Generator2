using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Onlywatch
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		SpriteFont font;
		Texture2D cellTexture;
		Color cellColor;
		List<Cell> cellArray = new List<Cell>();
		private int cellAmount = 150;
		private int radiusYMax = 300;
		private int radiusYMin = 100;
		private int radiusXMax = 450;
		private int radiusXMin = 200;
		Random generalRandom = new Random();
		private const float _delay = 100; // milliseconds
		private float _remainingDelay = _delay;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			graphics.IsFullScreen = false;
			Window.IsBorderless = true;
			graphics.PreferredBackBufferWidth = 1000;
			graphics.PreferredBackBufferHeight = 800;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			IsMouseVisible = true; 
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			cellTexture = new Texture2D(GraphicsDevice, 1, 1);
			cellTexture.SetData(new Color[] { Color.White });
			cellColor = Color.Black;

			font = Content.Load<SpriteFont>("Roboto");

			GenerateCells();
			
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				this.Exit();


			var timer = (float)gameTime.ElapsedGameTime.Milliseconds;
			_remainingDelay -= timer;

			if (_remainingDelay <= 0)
			{
				if (cellArray.Count > 0)
				{
					foreach (Cell cell in cellArray)
					{
						Vector2 separation = computeSeparation(cell);
						cell.velocity.X += (int)separation.X;
						cell.velocity.Y += (int)separation.Y;
						cell.velocity.ToVector2().Normalize();
						cell.move(cell.velocity);
					}
				}
			
				_remainingDelay = _delay;
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			cursorCoordinates();
			spriteBatch.Begin();
			spriteBatch.DrawString(font, cellArray.Count.ToString(), new Vector2(20, 10), Color.Red);
			spriteBatch.End();

			drawCells();

			base.Draw(gameTime);
		}
		private Vector2 computeSeparation(Cell myCell)
		{
			Vector2 v = new Vector2(0, 0);
			var neighborCount = 0;
			foreach (Cell cell in cellArray) 
			{
				if (cell != myCell)
				{
					if (Vector2.Distance(new Vector2(myCell.x1, myCell.y1), new Vector2(cell.x1, cell.y1)) > 1)
					{
						v.X += cell.x1 - myCell.x1;
						v.Y += cell.y1 - myCell.y1;
						neighborCount++;
					}//DISTANCE FROM
				}
	
			}
			if (neighborCount == 0)
				return v;
			v.X /= neighborCount;
			v.Y /= neighborCount;
			v.X *= -1;
			v.Y *= -1;
			v.Normalize();
			// DIVIDE BY LENGTH NORMALIZE IS TO REDUCE THE LENGTH OF THE NUMBER TO SINGLE DIGIT
			
			
			return v;
		}

		private void cursorCoordinates()
		{
			Point mousePosition = Mouse.GetState().Position;
			spriteBatch.Begin();
			spriteBatch.DrawString(font, "X:" + mousePosition.X.ToString(), new Vector2(20, 60), Color.Red);
			spriteBatch.DrawString(font, "Y:" + mousePosition.Y.ToString(), new Vector2(20, 75), Color.Red);
			spriteBatch.End();

		}

		private void drawCells()
		{
			foreach (Cell cell in cellArray)
			{
				spriteBatch.Begin();
				Rectangle temprect;
				temprect = new Rectangle(cell.x1, cell.y1, (int)cell.w, (int)cell.h);

				spriteBatch.Draw(cellTexture, temprect, cellColor);
				DrawBorder(temprect, 2, Color.Red);
				spriteBatch.End();
			}
		}

		/// <summary>
		/// Will draw a border (hollow rectangle) of the given 'thicknessOfBorder' (in pixels)
		/// of the specified color.
		/// </summary>
		/// <param name="rectangleToDraw"></param>
		/// <param name="thicknessOfBorder"></param>
		private void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
		{
			// Draw top line
			spriteBatch.Draw(cellTexture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

			// Draw left line
			spriteBatch.Draw(cellTexture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

			// Draw right line
			spriteBatch.Draw(cellTexture, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
											rectangleToDraw.Y,
											thicknessOfBorder,
											rectangleToDraw.Height), borderColor);
			// Draw bottom line
			spriteBatch.Draw(cellTexture, new Rectangle(rectangleToDraw.X,
											rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
											rectangleToDraw.Width,
											thicknessOfBorder), borderColor);
		}

		private void GenerateCells()
		{
			for (int i = 0; i < cellAmount; i++)
			{

				Cell newCell = new Cell(generalRandom.Next(radiusXMin, radiusXMax), generalRandom.Next(radiusYMin, radiusYMax), generalRandom.Next(40), generalRandom.Next(40));
				while ((newCell.w  / newCell.h < 0.25) || (newCell.h / newCell.w < 0.25))
				{
					newCell.w = generalRandom.Next(40);
					newCell.h = generalRandom.Next(40);
				}
				cellArray.Add(newCell);
			}
		}

	}
}
