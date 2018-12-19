using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Squared.Tiled;
using System.Collections.Generic;

namespace A_Level_Project_Racing
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Map map;
		ObjectGroup collision;
		Vector2 viewportPosition;
		int tilepixel;
		Texture2D cartexture;
		PlayerCar car;
		Texture2D marker;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
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
			map = Map.Load(Path.Combine(Content.RootDirectory, "Track 1.tmx"), Content);
			collision = map.ObjectGroups["Object Layer 1"];
			tilepixel = map.TileWidth;
			
			cartexture = Content.Load<Texture2D>("PlayerCar");
			map.ObjectGroups["Object Layer 1"].Objects["PLAYER"].Texture = cartexture;
			Vector2 carPos = new Vector2((graphics.PreferredBackBufferWidth / 2) - (cartexture.Width / 2),
				(graphics.PreferredBackBufferHeight / 2) - (cartexture.Height / 2));
			car = new PlayerCar();
			car.Init(cartexture, carPos, Content.Load<SpriteFont>("Regular"));
			car.Position = new Vector2(map.ObjectGroups["Object Layer 1"].Objects["PLAYER"].X - 640 , map.ObjectGroups["Object Layer 1"].Objects["PLAYER"].Y-320);
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
				Exit();

			// TODO: Add your update logic here

			base.Update(gameTime);
			car.Update(gameTime);
			map.ObjectGroups["Object Layer 1"].Objects["PLAYER"].X = (int)car.Position.X;
			map.ObjectGroups["Object Layer 1"].Objects["PLAYER"].Y = (int)car.Position.Y;

		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Green);
			spriteBatch.Begin();
			map.Draw(spriteBatch, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), car.Position);

			car.Draw(spriteBatch);
			spriteBatch.End();

			// TODO: Add your drawing code here

			base.Draw(gameTime);
		}
	}
}
