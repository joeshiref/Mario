#region Using Statements
using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;
//using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Game5
{
    #region EnemyManager
    class EnemyManager
    {
        public ContentManager Content;
        public List<Enemy> Enemies;
        public EnemyManager(ContentManager Content)
        {
            this.Content = Content;
            Enemies = new List<Enemy>();
        }
        public void AddEnemy(Enemy enemy)
        {
            Enemies.Add(enemy);
        }
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Enemies.ToArray().Length; i++)
            {
                if (Enemies[i] == null) continue;
                if (Enemies[i].LifeSpan == 0)
                {
                    Enemies[i] = null;
                    continue;
                }
                Enemies[i].Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Enemies.ToArray().Length; i++)
            {
                if (Enemies[i] == null) continue;
                Enemies[i].Draw(spriteBatch);
            }
        }
    }
    #endregion
    #region Enemy
    class Enemy
    {
        public ContentManager Content;
        public Rectangle Bounds;
        public Vector2 Position;
        public int Width, Height, LifeSpan, BoundariesLeft, BoundariesRight, BoundariesUp, BoundariesDown;
        public float Time;
        public float[,] Elapse;
        public int Force, Gravity;
        public bool on_floor;
        public string Direction;
        public Enemy(ContentManager Content, Vector2 Position, int Width, int Height, int BoundariesLeft, int BoundariesRight, int BoundariesUp, int BoundariesDown)
        {
            this.Content = Content;
            this.Position = Position;
            this.Width = Width;
            this.Height = Height;
            this.BoundariesLeft = BoundariesLeft;
            this.BoundariesRight = BoundariesRight;
            this.BoundariesUp = BoundariesUp;
            this.BoundariesDown = BoundariesDown;
            LifeSpan = 2;
            Force = 4;
            Gravity = -3;
            Time = 0;
            Elapse = new float[1, 2];
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void Destroy() { }
    }
    #endregion
    #region Gumba
    class Gumba : Enemy
    {
        Texture2D Alive, Dead;
        public Gumba(ContentManager Content, Vector2 Position, int Width, int Height, int BoundariesLeft, int BoundariesRight, int BoundariesUp, int BoundariesDown)
            : base(Content, Position, Width, Height, BoundariesLeft, BoundariesRight, BoundariesUp, BoundariesDown)
        {
            Elapse = new float[1, 2];
            Alive = Content.Load<Texture2D>("GumbaAlive");
            Dead = Content.Load<Texture2D>("GumbaDead");
            Force = 4;
            Direction = "Left";
            on_floor = false;
        }
        public override void Update(GameTime gameTime)
        {
            Elapse[0, 1] += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Elapse[0, 1] >= 120)
            {
                Elapse[0, 1] = 0;
                Elapse[0, 0] = Elapse[0, 0] == 0 ? 1 : 0;
            }
            if (on_floor == false)
            {
                Position.Y += Gravity;
                Gravity++;
            }
            else
            {
                if (this.LifeSpan == 2 && Direction == "Right")
                    Position.X += Force;
                else if (this.LifeSpan == 2 && Direction == "Left")
                    Position.X -= Force;

                if (Position.X < 0)
                {
                    Position.X = 0;
                    Direction = "Right";

                }
                if (Position.X + Width > 800)
                {
                    Position.X = 800 - Width;
                    Direction = "Left";

                }
            }
            Bounds.X = (int)Position.X;
            Bounds.Y = (int)Position.Y;
            if (this.LifeSpan == 1)
            {
                Time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (Time >= 900)
                {
                    this.LifeSpan = 0;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.LifeSpan == 2) spriteBatch.Draw(Alive, Bounds, new Rectangle(70 * (int)Elapse[0, 0], 0, Width, Height), Color.White);
            else spriteBatch.Draw(Dead, new Vector2(Position.X, Position.Y + 17), Color.White);
        }
        public override void Destroy()
        {
            this.LifeSpan = 1;
        }
    }
    #endregion
    #region Spiny
    class Spiny : Enemy
    {
        public Texture2D MovingRight, MovingLeft;
        public Spiny(ContentManager Content, Vector2 Position, int Width, int Height, int BoundariesLeft, int BoundariesRight, int BoundariesUp, int BoundariesDown)
            : base(Content, Position, Width, Height, BoundariesLeft, BoundariesRight, BoundariesUp, BoundariesDown)
        {
            MovingRight = Content.Load<Texture2D>("SpinyRight");
            MovingLeft = Content.Load<Texture2D>("SpinyLeft");
            Direction = "Right";
        }
        public override void Update(GameTime gameTime)
        {
            Position.X += Force;
            if (Position.X <= BoundariesLeft) { Position.X = BoundariesLeft; Force *= -1; Direction = "Right"; }
            if (Position.X > +BoundariesRight) { Position.X = BoundariesRight; Force *= -1; Direction = "Left"; }
            Elapse[0, 1] += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Elapse[0, 1] >= 120)
            {
                Elapse[0, 1] = 0;
                Elapse[0, 0] = Elapse[0, 0] == 0 ? 1 : 0;
            }
            Bounds.X = (int)Position.X;
            Bounds.Y = (int)Position.Y;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Direction == "Right")
                spriteBatch.Draw(MovingRight, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), new Rectangle((int)Elapse[0, 0] * 70, 0, Width, Height), Color.White);
            else
                spriteBatch.Draw(MovingLeft, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), new Rectangle((int)Elapse[0, 0] * 70, 0, Width, Height), Color.White);
        }
    }
    #endregion
    #region Plant
    class Plant : Enemy
    {
        bool Steady;
        float SteadyTime;
        Texture2D _Plant;
        public Plant(ContentManager Content, Vector2 Position, int Width, int Height, int BoundariesLeft, int BoundariesRight, int BoundariesUp, int BoundariesDown)
            : base(Content, Position, Width, Height, BoundariesLeft, BoundariesRight, BoundariesUp, BoundariesDown)
        {
            Steady = false;
            _Plant = Content.Load<Texture2D>("Plant");
        }
        public override void Update(GameTime gameTime)
        {
            Elapse[0, 1] += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Elapse[0, 1] >= 315)
            {
                Elapse[0, 1] = 0;
                Elapse[0, 0] = Elapse[0, 0] == 0 ? 1 : 0;
            }
            if (Steady == false) Position.Y += Gravity;
            if (Position.Y <= BoundariesUp)
            {
                if (Steady == false)
                {
                    Steady = true;
                    SteadyTime = 0;
                    Position.Y = BoundariesUp;
                    Gravity *= -1;
                }
            }
            if (Position.Y + Height >= BoundariesDown)
            {
                if (Steady == false)
                {
                    Steady = true;
                    Position.Y = BoundariesDown - Height;
                    Gravity *= -1;
                    SteadyTime = 0;
                }
            }
            if (Steady == true)
            {
                SteadyTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (SteadyTime >= 1500)
                {
                    Steady = false;
                }
            }
            Bounds.X = (int)Position.X;
            Bounds.Y = (int)Position.Y;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_Plant, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), new Rectangle((int)Elapse[0, 0] * 65, 0, Width, Height), Color.White);
        }
    }
    #endregion
    #region DUck
    class Duck : Enemy
    {
        Texture2D MovingRight, MovingLeft, Revivng, Steady;
        public Duck(ContentManager Content, Vector2 Position, int Width, int Height, int BoundariesLeft, int BoundariesRight, int BoundariesUp, int BoundariesDown)
            : base(Content, Position, Width, Height, BoundariesLeft, BoundariesRight, BoundariesUp, BoundariesDown)
        {
            LifeSpan = 5;
            Direction = "Left";
            Force = 4;
            MovingRight = Content.Load<Texture2D>("DuckRight");
            MovingLeft = Content.Load<Texture2D>("DuckLeft");
            Steady = Content.Load<Texture2D>("DuckSteady");
            Revivng = Content.Load<Texture2D>("DuckReviving");
        }
        public override void Update(GameTime gameTime)
        {
            if (LifeSpan == 2)
            {
                //Direction = "Right";
                if (Direction == "Right")
                    Position.X += 7;
                else
                    Position.X -= 7;

                if (Direction == "Left")
                    LifeSpan = 1;


            }
            else if (LifeSpan == 1)
            {

                //Direction = "Left";
                if (Direction == "Right")
                    Position.X += 7;
                else
                    Position.X -= 7;
                if (Direction == "Right")
                    LifeSpan = 2;

            }
            else if (LifeSpan == 5)
            {
                Width = 42;
                Height = 61;
                Elapse[0, 1] += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (Elapse[0, 1] >= 120)
                {
                    Elapse[0, 1] = 0;
                    Elapse[0, 0] = Elapse[0, 0] == 0 ? 1 : 0;
                }
                if (on_floor == false)
                {
                    Position.Y += Gravity;
                }
                else
                {
                    if (Direction == "Right")
                        Position.X += Force;
                    else
                        Position.X -= Force;
                    if (Position.X >= BoundariesRight)
                    {
                        Position.X = BoundariesRight;
                        Direction = "Left";
                    }
                    if (Position.X <= BoundariesLeft)
                    {
                        Position.X = BoundariesLeft;
                        Direction = "Right";
                    }
                }
                Bounds.X = (int)Position.X;
                Bounds.Y = (int)Position.Y;
            }
            else if (LifeSpan == 4)
            {
                Width = 42;
                Height = 39;
                Time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                Elapse[0, 0] = 0;
                Elapse[0, 1] = 0;
                if (Time >= 800)
                {
                    Time = 0;
                    LifeSpan = 5;

                }
            }
            else
            {
                Width = 42;
                Height = 37;
                Time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                Elapse[0, 0] = 0;
                Elapse[0, 1] = 0;
                if (Time >= 2000)
                {
                    Time = 0;
                    LifeSpan = 4;
                }
            }
            Bounds.X = (int)Position.X;
            Bounds.Y = (int)Position.Y;
            if (Position.X + Width >= BoundariesRight)
            {
                Position.X = BoundariesRight - Width;
                Direction = "Left";
            }
            if (Position.X <= BoundariesLeft)
            {
                Position.X = BoundariesLeft;
                Direction = "Right";
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (LifeSpan == 5)
            {
                if (Direction == "Right")
                    spriteBatch.Draw(MovingRight, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), new Rectangle((int)Elapse[0, 0] * 70, 0, Width, Height), Color.White);
                else
                    spriteBatch.Draw(MovingLeft, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), new Rectangle((int)Elapse[0, 0] * 70, 0, Width, Height), Color.White);
            }
            else if (LifeSpan == 3) spriteBatch.Draw(Steady, new Rectangle((int)Position.X, (int)Position.Y + 24, 42, 37), Color.White);
            else if (LifeSpan == 4) spriteBatch.Draw(Revivng, new Rectangle((int)Position.X, (int)Position.Y + 22, 42, 39), Color.White);
            else if (LifeSpan == 1) spriteBatch.Draw(Steady, new Rectangle((int)Position.X, (int)Position.Y + 24, 42, 37), Color.White);
            else if (LifeSpan == 2) spriteBatch.Draw(Steady, new Rectangle((int)Position.X, (int)Position.Y + 24, 42, 37), Color.White);
        }
        public override void Destroy()
        {
            this.LifeSpan = 4;
        }

    }
    #endregion
    #region BillBlasterBullet
    class BillBlasterBullet : Enemy
    {
        Texture2D _Texture;
        int ForceX, ForceY;
        public BillBlasterBullet
            (ContentManager Content, Vector2 Position, int Width, int Height, int BoundariesLeft, int BoundariesRight, int BoundariesUp, int BoundariesDown, int ForceX, int ForceY)
            : base(Content, Position, Width, Height, BoundariesLeft, BoundariesRight, BoundariesUp, BoundariesDown)
        {
            if (ForceX != 0) _Texture = Content.Load<Texture2D>("BulletLeft");
            else _Texture = Content.Load<Texture2D>("BulletDown");
            this.ForceX = ForceX;
            this.ForceY = ForceY;
        }
        public override void Update(GameTime gameTime)
        {
            Position.X += ForceX;
            Position.Y += ForceY;
            if (Position.X < 0) Position.X = BoundariesRight;
            if (Position.Y > 1500) Position.Y = BoundariesUp;
            Bounds.X = (int)Position.X;
            Bounds.Y = (int)Position.Y;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_Texture, Position, Color.White);
        }
    }
    #endregion
}
