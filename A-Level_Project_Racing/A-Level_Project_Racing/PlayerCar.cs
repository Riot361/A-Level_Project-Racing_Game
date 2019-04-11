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
	class AICar : PlayerCar
	{
		public Vector2 NextCheckPoint;
		Vector2 oldpos;
		bool up = false;
		bool down = false;
		bool left = false;
		bool right = false;
		public float tancheckdirection;
		public float checkdirection;
		public float squarecheckdistance;
		public float checkdistance;

		public virtual void Update(GameTime gameTime, bool carrecover, A_Level_Project_Racing.Game1.Menu currentmenu, int AIcheckpoint, List<Vector2>[] checkpoint)
		{
			restart = carrecover;
			Recover = restart;
			NextCheckPoint = (checkpoint[AIcheckpoint][2])-((checkpoint[AIcheckpoint][2]-checkpoint[AIcheckpoint][0])/2);
			squarecheckdistance = ((Position.X - NextCheckPoint.X) * (Position.X - NextCheckPoint.X)) + ((Position.Y - NextCheckPoint.Y) * (Position.Y - NextCheckPoint.Y));
			checkdistance = (float)Math.Sqrt(squarecheckdistance);

			if (speed < 14 & (checkdistance > 800) & (steer < checkdirection + 0.5f && steer > checkdirection - 0.5f))
			{
					speed += 0.25f;
				
			}
			else if (speed < 5 & checkdistance > 150)
			{
				speed += 0.1f;
			}
			else if (speed < 3 & checkdistance > 0)
			{
				speed += 0.2f;
			}
			else if (speed > 8 & checkdistance <= 500)
			{
				speed -= 0.4f;
			}
			else if (checkdistance <= 300)
			{
				speed -= 0.1f;
			}
			else if (checkdistance <= 50)
			{
				speed -= 0.2f;
			}
			else
			{
				speed -= 0.01f;
			}

			if (NextCheckPoint != null)
			{
				tancheckdirection = (NextCheckPoint.Y - Position.Y) / (NextCheckPoint.X - Position.X);
				if (NextCheckPoint.X < Position.X)
				{
					checkdirection = ((float)Math.Atan(tancheckdirection)) - ((float)(Math.PI));
				}
				else if (NextCheckPoint.X > Position.X)
				{
					checkdirection = ((float)Math.Atan(tancheckdirection));
				}

				if (steer > (float)(2 * (Math.PI)))
				{
					steer = steer - (float)(2*(Math.PI));
				}
				else if (steer < 0)
				{
					steer = steer + (float)(2 * (Math.PI));
				}

				if (checkdirection > (float)(2 * (Math.PI)))
				{
					checkdirection = checkdirection - (float)(2 * (Math.PI));
				}
				else if (checkdirection < 0)
				{
					checkdirection = checkdirection + (float)(2 * (Math.PI));
				}

				if (checkdirection > steer)
				{
					if (steer + (float)Math.PI < checkdirection)
					{
						left = true;
						right = false;
					}
					else
					{
						right = true;
						left = false;
					}
					
				}
				else if (checkdirection < steer)
				{
					if (steer - (float)Math.PI > checkdirection)
					{
						right = true;
						left = false;
					}
					else
					{
						left = true;
						right = false;
					}
					
				}
				if (left == true & angle > -.08f)
				{
					if (angle > .001f)
					{
						angle = angle / 1.25f;
					}
					if (speed != 0)
					{
						angle -= (.001f) / (speed / 20);
					}
					else
					{
						angle -= .0005f;
					}
				}
				else if (right == true & angle < .08f)
				{
					if (angle < -.001f)
					{
						angle = angle / 1.25f;
					}
					if (speed != 0)
					{
						angle += (.001f) / (speed / 20);
					}
					else
					{
						angle += .0005f;
					}
				}
				else
				{
					angle = 0;
				}

				if (speed != 0)
				{
					steer += angle;
				}
				else
				{
					steer += 0;
				}
				

			}


			Position = Position + new Vector2(speed * (float)Math.Cos(steer), speed * (float)Math.Sin(steer));
			if (Recover == true)
			{
				Position = savedposition;
			}
			bounds = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
		}
		public virtual void Draw(SpriteBatch spriteBatch, Vector2 vp)
		{
			spriteBatch.Draw(texture, Position-vp + origin + new Vector2(640, 320), null, null, origin, steer, null, null, SpriteEffects.FlipHorizontally, 0);
		}
	}
	class PlayerCar
	{
		public Texture2D texture;
		Vector2 position;
		public Vector2 origin;
		public Vector2 savedposition;
		public float steer = 0;
		public float savedsteer = 0;
		public float speed = 0;
		public float slide = 0;
		public float tempsteer = 0;
		public float drift = 0;
		public float angle = 0;
		public Timer remainingtime;
		public Stopwatch time = new Stopwatch();
		public SpriteFont font;
		public bool Recover = false;
		public Rectangle bounds;
		public bool restart = false;
		public bool timer
		{
			get
			{
				return time.IsRunning;
			}
			set
			{
					time.Stop();
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
			if (restart == true)
			{
				Recover = true;
				steer = savedsteer;
				speed = 0;
				angle = 0;
				time.Restart();
			}
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
		public virtual void Update(GameTime gameTime, bool carrecover, A_Level_Project_Racing.Game1.Menu currentmenu)
		{
			if (currentmenu == Game1.Menu.play)
			{
				time.Start();
			}
			else
			{
				time.Stop();
			}
			restart = carrecover;
			GetInputs(Keyboard.GetState());
			position = position + new Vector2(speed * (float)Math.Cos(steer),speed * (float)Math.Sin(steer));
			if (Recover == true)
			{
				position = savedposition;
			}
			bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
		}
		public virtual void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, origin + new Vector2(640,320), null, null, origin, steer, null, null, SpriteEffects.FlipHorizontally, 0);
			spriteBatch.DrawString(font, "time:"+time.Elapsed, new Vector2(100, 100), Color.Black);
			spriteBatch.DrawString(font, "position:" + position, new Vector2(100, 200), Color.Black);
			spriteBatch.DrawString(font, "bounds:" + bounds, new Vector2(100, 250), Color.Black);

		}
		public Vector2 Position
		{
			get { return position; }
			set { position = value; }

		}

	}
}
