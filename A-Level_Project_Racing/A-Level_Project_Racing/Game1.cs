﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Squared.Tiled;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Media;
using A1r.SimpleTextUI;
using System.Timers;

namespace A_Level_Project_Racing
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		PrimitiveBatch primitiveBatch;
		Texture2D Menuback;
		Map map;
		ObjectGroup collision;
		Vector2 viewportPosition;
		int tilepixel;
		Texture2D cartexture;
		PlayerCar car;
		Texture2D marker;
		bool crash;
		public List<Vector2>[] CollisionList =new List<Vector2>[9];
		Song backgroundsong;
		bool songplaying = false;
		bool songallowed = true;
		bool songingame = true;
		bool songinmenu = true;
		SimpleTextUI menu;
		SimpleTextUI options;
		SimpleTextUI current;
		Timer keytimer;
		SpriteFont small;
		SpriteFont big;
		Vector2 intersect = Vector2.Zero;




		public enum Menu
		{
			main,
			play,
			options,
			exit
		}
		Menu currentmenu = Menu.main;



		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 640;
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
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 640;
			graphics.IsFullScreen = false;
			graphics.ApplyChanges();
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
			primitiveBatch = new PrimitiveBatch(GraphicsDevice);
			map = Map.Load(Path.Combine(Content.RootDirectory, "Track 1.tmx"), Content);
			Menuback = Content.Load<Texture2D>("MenuWallpaper");
			collision = map.ObjectGroups["Object Layer 1"];
			tilepixel = map.TileWidth;
			for (int i = 0; i < 9; i++)
			{
				CollisionList[i] = map.ObjectGroups["Object Layer 1"].Objects["COLLISION" + (i + 1)].PointsList;
			}
			cartexture = Content.Load<Texture2D>("PlayerCar");
			map.ObjectGroups["Object Layer 1"].Objects["PLAYER"].Texture = cartexture;
			Vector2 carPos = new Vector2((graphics.PreferredBackBufferWidth / 2) - (cartexture.Width / 2),
				(graphics.PreferredBackBufferHeight / 2) - (cartexture.Height / 2));
			car = new PlayerCar();
			car.Init(cartexture, carPos, Content.Load<SpriteFont>("Regular"));
			car.Position = new Vector2(map.ObjectGroups["Object Layer 1"].Objects["PLAYER"].X , map.ObjectGroups["Object Layer 1"].Objects["PLAYER"].Y);
			car.savedposition = car.Position;


			big = Content.Load<SpriteFont>("Regular");
			small = Content.Load<SpriteFont>("Regular");
			menu = new SimpleTextUI(this, big, new[] { "Play", "Options", "Credits", "Exit" })
			{
				TextColor = Color.RoyalBlue,
				SelectedElement = new TextElement("--> = ", Color.Goldenrod),
				Align = Alignment.Left
			};

			options = new SimpleTextUI(this, big, new TextElement[]
			{
				new SelectElement("Video", new[]{"Windowed","FullScreen"}),
				new SelectElement("Music", new[]{"Menu and Game","Game only","Menu only","None"}),
				new TextElement("Back")
			})
			{
				TextColor = Color.RoyalBlue,
				SelectedElement = new TextElement("--> = ", Color.Goldenrod),
				Align = Alignment.Left
			};
			current = menu;

			keytimer = new Timer();
			// TODO: use this.Content to load your game content here
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
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				currentmenu = Menu.main;
			KeyboardState keys = Keyboard.GetState();
			bool change = true;
			if (currentmenu != Menu.play)
			{
				if (!keytimer.Enabled)
				{
					if (keys.IsKeyDown(Keys.Up))
					{
						current.Move(Direction.Up);
					}

					else if (keys.IsKeyDown(Keys.Down))
					{
						current.Move(Direction.Down);
					}

					else if (keys.IsKeyDown(Keys.Left))
					{
						current.Move(Direction.Left);
						if (current.GetCurrentCaption() == "Video")
						{
							graphics.IsFullScreen = (current.GetCurrentValue() == "FullScreen");
							if (graphics.IsFullScreen)
							{
								graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
								graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
							}
							else
							{
								graphics.PreferredBackBufferHeight = 640;
								graphics.PreferredBackBufferWidth = 1280;
							}
							graphics.ApplyChanges();
						}
					}

					else if (keys.IsKeyDown(Keys.Right))
					{
						current.Move(Direction.Right);
						if (current.GetCurrentCaption() == "Video")
						{
							graphics.IsFullScreen = (current.GetCurrentValue() == "FullScreen");
							if(graphics.IsFullScreen)
							{
								graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
								graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
							}
							else
							{
								graphics.PreferredBackBufferHeight = 640;
								graphics.PreferredBackBufferWidth = 1280;
							}
							graphics.ApplyChanges();
						}
					}

					else if (keys.IsKeyDown(Keys.Enter))
					{
						string test = current.GetCurrentCaption();

						if (current == menu)
						{
							if (test == "Exit")
								Exit();
							else if (test == "Options")
							{
								current = options;
								currentmenu = Menu.options;
							}
							else if (test == "Play")
							{
								currentmenu = Menu.play;
							}
						}

						else if (current == options)
						{
							if (test == "Back")
							{
								current = menu;
								currentmenu = Menu.main;
							}
						}
					}
					else
						change = false;
					if (current.GetCurrentCaption() == "Music")
					{
						if (current.GetCurrentValue() == "Menu and Game")
						{
							songinmenu = true;
							songingame = true;
						}
						else if (current.GetCurrentValue() == "Menu only")
						{
							songinmenu = true;
							songingame = false;
						}
						else if (current.GetCurrentValue() == "Game only")
						{
							songinmenu = false;
							songingame = true;
						}
						else
						{
							songinmenu = false;
							songingame = false;
						}
					}


					if (change)
					{
						keytimer = new Timer();
						keytimer.Interval = 200;
						keytimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
						keytimer.Enabled = true;
					}
				}
				if (songinmenu == true)
				{
					songallowed = true;
				}
				else
				{
					songallowed = false;
				}
			}
			else if (currentmenu == Menu.play)
			{
				// TODO: Add your update logic here 

				Vector2 temp = car.Position;


				car.Update(gameTime);

				crash = false;
				for (int i = 1; i < 2; i++)
				{
					Vector2 previous = Vector2.Zero;
					Vector2 check = previous;

					crash = false;
					map.ObjectGroups["Object Layer 1"].Objects["PLAYER"].X = (int)car.Position.X;
					map.ObjectGroups["Object Layer 1"].Objects["PLAYER"].Y = (int)car.Position.Y;
					Rectangle pp = new Rectangle((int)map.ObjectGroups["Object Layer 1"].Objects["PLAYER"].X, (int)map.ObjectGroups["Object Layer 1"].Objects["PLAYER"].Y, car.texture.Width, car.texture.Height);


					foreach (Vector2 v in CollisionList[i])
					{
						Vector2 newv = v;
						intersect = Vector2.Zero;
						List<Vector2> carlines = new List<Vector2>();
						Vector2 d = new Vector2(pp.Width, pp.Height);
						Vector2 d2 = new Vector2(pp.Width, pp.Height);
						carlines.Add(d);
						carlines.Add(d2);
						if (newv == Vector2.Zero)
						{
							previous = newv;
						}
						else
						{
							//Console.WriteLine(newv);
							bool checkline = true;
							Vector2 b = newv - previous;
							float dotp = Vector2.Dot(b, d);
							if (dotp == 0)
								crash = false;
							else
							{
								Vector2 c = new Vector2(pp.X, pp.Y) - previous;
								float t = (c.X * d.Y - c.Y * d.X) / dotp;
								float u = (c.X * b.Y - c.Y * b.X) / dotp;
								if (t < 0 || t > 1)
								{
									crash = false;
								}

								
								else if (u < 0 || u > 1)
								{
									crash = false;
								}
								else
								{
									intersect = new Vector2(pp.X, pp.Y) + t * b;
									crash = true;
								}
								Console.WriteLine(t + " " + u);

							}
							//var minX = Math.Min(previous.X, newv.X);
							//var maxX = Math.Max(previous.X, newv.X);
							//var minY = Math.Min(previous.Y, newv.Y);
							//var maxY = Math.Max(previous.Y, newv.Y);

							//if (car.Position.X > maxX || car.Position.X+cartexture.Width < minX)
							//{
							//	checkline = false;
							//}

							//if (car.Position.Y > maxY || car.Position.Y+cartexture.Height < minY)
							//{
							//	checkline = false;
							//}

							//if (car.Position.X < minX && maxX < car.Position.X + cartexture.Width)
							//{
							//	checkline = true;
							//}

							//if (car.Position.Y < minY && maxY < car.Position.Y + cartexture.Height)
							//{
							//	checkline = true;
							//}

							previous = newv;

							if (crash)
								break;
						}

					}
				}
				if (crash)
					car.Position = temp;

				if (songingame == true)
				{
					songallowed = true;
				}
				else
				{
					songallowed = false;
				}



				
			}
			if (songplaying == false && songallowed == true)
				{
					Random r = new Random();
					int i = r.Next(1, 11);
					backgroundsong = Content.Load<Song>("Music/Song" + i);
					MediaPlayer.Play(backgroundsong);
					songplaying = true;
				}
			else if (MediaPlayer.State == MediaState.Stopped)
				{
					songplaying = false;
				}
			else if (songallowed == false)
			{
				MediaPlayer.Stop();
				songplaying = false;
			}
			else
				{

				}

			base.Update(gameTime);
		}


		private void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			keytimer.Enabled = false;
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			
			if (currentmenu == Menu.play)
			{
				GraphicsDevice.Clear(Color.Green);
				if (false)
					spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
				else
					spriteBatch.Begin();
				map.Draw(spriteBatch, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), car.Position - new Vector2(640, 320));
				Vector2 vl = Vector2.Zero;
				Vector2 screen = new Vector2((graphics.PreferredBackBufferWidth / 2), (graphics.PreferredBackBufferHeight / 2));
				if (false)
				{
					foreach (List<Vector2> cl in CollisionList)
					{
						foreach (Vector2 v in cl)
						{
							if (vl == Vector2.Zero)
							{

							}
							else
							{
								primitiveBatch.Begin(PrimitiveType.LineList);
								primitiveBatch.AddVertex(vl - car.Position + screen, Color.Purple);
								primitiveBatch.AddVertex(v - car.Position + screen, Color.Purple);
								primitiveBatch.End();
							}
							vl = v;
						}
						vl = Vector2.Zero;
					}
				}
				car.Draw(spriteBatch);
				if (crash)
				{
					spriteBatch.DrawString(car.font, "collision", new Vector2(100, 150), Color.Black);
				}
				spriteBatch.DrawString(car.font, "intersect : " + intersect, new Vector2(100, 400), Color.Black);


				spriteBatch.End();
			}
			// TODO: Add your drawing code here
			if (currentmenu != Menu.play)
			{
				GraphicsDevice.Clear(Color.Blue);
				spriteBatch.Begin();
				spriteBatch.Draw(Menuback, null, new Rectangle(0,0,graphics.PreferredBackBufferWidth,graphics.PreferredBackBufferHeight), null,null,0,null,null,SpriteEffects.None,0);
				spriteBatch.End();
				current.Draw(gameTime);
			}
			
			base.Draw(gameTime);
		}
	}
}
