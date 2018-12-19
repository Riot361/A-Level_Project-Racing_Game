using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace A_Level_Project_Racing
{
	class PlayerCar
	{
		Texture2D texture;
		Vector2 position;
		Vector2 origin;
		float steer = 0;
		float speed = 0;
		float slide = 0;
		float tempsteer = 0;
		float drift = 0;
		float angle = 0;
		SpriteFont font;
		public void Init(Texture2D t, Vector2 pos, SpriteFont f)
		{
			position = pos;
			texture = t;
			font = f;
			origin = new Vector2(t.Width / 2, t.Height / 2);
		}
		public void GetInputs(KeyboardState input)
		{
			if (input.IsKeyDown(Keys.Left) || input.IsKeyDown(Keys.Right))
			{
				if (input.IsKeyDown(Keys.Left) & angle > -.05f)
				{
					if (angle > .001f)
					{
						angle = angle / 1.2f;
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
						angle = angle / 1.2f;
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

		}
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, origin+new Vector2(640,320), null, null, origin, steer, null, null, SpriteEffects.FlipHorizontally, 0);
		}
		public Vector2 Position
		{
			get { return position; }
			set { position = value; }

		}

	}
}
