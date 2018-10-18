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
		public void Init(Texture2D t, Vector2 pos)
		{
			position = pos;
			texture = t;
			origin = new Vector2(t.Width / 2, t.Height / 2);
		}
		public void GetInputs(KeyboardState input)
		{
			if (speed != 0)
			{
				if (input.IsKeyDown(Keys.Left))
				{
					steer -= (.05f);
				}

				if (input.IsKeyDown(Keys.Right))
				{
					steer += (.05f);
				}

			}

			
			if (input.IsKeyDown(Keys.Up) && speed < 15)
			{
				speed += .5f;
			}

			if (input.IsKeyDown(Keys.Down) && speed > -5)
			{
				speed -= .25f;
			}
			if (speed*speed > 200 && (input.IsKeyDown(Keys.Left) || input.IsKeyDown(Keys.Right)))
			{

				float steerdif = steer - tempsteer;
				tempsteer = steer;
				slide = speed * steerdif;
			}
			else
			{
				slide = 0;
			}

			if (speed > 0)
				speed -= .25f;
			else if (speed < 0)
				speed += .125f;
			else
				speed = 0;
		}
		public void Update(GameTime gameTime)
		{
			GetInputs(Keyboard.GetState());
			position = position + new Vector2(speed * (float)Math.Cos(steer), speed * (float)Math.Sin(steer));

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
