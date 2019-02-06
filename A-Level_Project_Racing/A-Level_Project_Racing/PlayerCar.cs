using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Timers;
using System.Diagnostics;

namespace A_Level_Project_Racing
{
	class PlayerCar
	{
		public Texture2D texture;
		Vector2 position;
		Vector2 origin;
		public Vector2 savedposition;
		float steer = 0;
		float savedsteer = 0;
		float speed = 0;
		float slide = 0;
		float tempsteer = 0;
		float drift = 0;
		float angle = 0;
		Timer remainingtime;
		Stopwatch time = new Stopwatch();
		public SpriteFont font;
		bool Recover = false;
		public Rectangle bounds;
		public bool timer
		{
			get
			{
				return time.IsRunning;
			}
			set
			{
				if (value)
				{
					time.Start();
				}
				else
				{
					time.Stop();
				}
			}

		}
		public void Init(Texture2D t, Vector2 pos, SpriteFont f)
		{
			position = pos;
			savedposition = pos;
			texture = t;
			font = f;
			origin = new Vector2(t.Width / 2, t.Height / 2);
			remainingtime = new Timer();
			timer = true;
		}
		public void GetInputs(KeyboardState input)
		{
			Recover = false;
			if (input.IsKeyDown(Keys.R))
			{
				Recover = true;
			}
			if (input.IsKeyDown(Keys.T))
			{
				angle = 0;
				steer = savedsteer;
			}
			if (input.IsKeyDown(Keys.S))
			{
				savedposition = position;
				savedsteer = steer;
			}
			if (input.IsKeyDown(Keys.Left) || input.IsKeyDown(Keys.Right))
			{
				if (input.IsKeyDown(Keys.Left) & angle > -.05f)
				{
					if (angle > .001f)
					{
						angle = angle / 1.25f;
					}
					if (speed !=0)
					{
						angle -= (.001f) / (speed/20);
					}
					else
					{
						angle -= .0005f;
					}
					
					
				}
				if (input.IsKeyDown(Keys.Right) & angle < .05f)
				{
					if (angle < -.001f)
					{
						angle = angle / 1.25f;
					}
					if (speed != 0)
					{
						angle += (.001f) / (speed/20);
					}
					else
					{
						angle += .0005f;
					}
					
					
				}
				if (angle > .05f)
				{
					angle = .049f;
				}
				if (angle < -.05f)
				{
					angle = -.049f;
				}
			}
			else
			{
				if (angle > .001f || angle < -.001f)
				{
					angle = angle / 1.1f;
				}
				else
				{
					angle = 0;
				}
			}


			if (speed != 0)
			{
				steer += angle;
			}
			else
			{
				steer += 0;
			}
			
			
			if (input.IsKeyDown(Keys.Up) && speed < 20)
			{
				speed += .25f;
			}
			if (input.IsKeyDown(Keys.Down) && speed > -4)
			{
				if (speed > .5f)
				{
					speed -= (2)/speed + .2f;
				}
				if (speed <= .5f)
				{
					speed -= .175f;
				}
			}

			
			if (speed > .05f)
				speed -= .05f;
			else if (speed < -.05f)
				speed += .05f;
			else
				speed = 0;


			slide = Math.Abs((angle) * 10);
		}
		public void Update(GameTime gameTime)
		{
			GetInputs(Keyboard.GetState());
			position = position + new Vector2(speed * (float)Math.Cos(steer),speed * (float)Math.Sin(steer));
			if (Recover == true)
			{
				position = savedposition;
			}
			bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, origin + new Vector2(640,320), null, null, origin, steer, null, null, SpriteEffects.FlipHorizontally, 0);
			spriteBatch.DrawString(font, "time:"+time.Elapsed, new Vector2(100, 100), Color.Black);
			spriteBatch.DrawString(font, "position:" + position, new Vector2(100, 200), Color.Black);
			spriteBatch.DrawString(font, "bounds:" + bounds, new Vector2(100, 250), Color.Black);
			// Make a 1x1 texture named pixel.  
			Texture2D pixel = new Texture2D(spriteBatch.GraphicsDevice, 10, 10);

			Color[] colorData = new Color[100];
			for (int i = 0; i < 100; i++)
			{
				colorData[i] = Color.White;
			}
			// Create a 1D array of color data to fill the pixel texture with.  
			//Color[] colorData = {
			//			Color.White,
			//		};

			// Set the texture data with our color information.  
			pixel.SetData<Color>(colorData);
			Vector2 temp = new Vector2(bounds.X -position.X, bounds.Y - position.Y) + new Vector2(640,320);
			spriteBatch.Draw(pixel, temp, Color.White);
		}
		public Vector2 Position
		{
			get { return position; }
			set { position = value; }

		}

	}
}
