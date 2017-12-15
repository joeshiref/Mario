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
    #region BrickManager
    class BrickManager
    {
        public ContentManager Content;
        public List<Brick> Bricks;
        public BrickManager(ContentManager Content)
        {
            this.Content = Content;
            Bricks = new List<Brick>();
        }
        public void AddBrick(Brick brick)
        {
            Bricks.Add(brick);
        }
        public void Update(GameTime gameTIme)
        {
            foreach (Brick brick in Bricks)
            {
                if (brick == null) continue;
                brick.Update(gameTIme);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Brick brick in Bricks)
            {
                if (brick == null) continue;
                brick.Draw(spriteBatch);
            }
        }
    }
    #endregion
    #region Brick
    class Brick
    {
        public Texture2D _Texture;
        public ContentManager Content;
        public Vector2 Position;
        public int LifeSpan, Index;
        public Rectangle Bounds;
        public Brick(Texture2D _Texture, Vector2 Position, ContentManager Content, int Index)
        {
            this._Texture = _Texture;
            this.Position = Position;
            this.Content = Content;
            this.Index = Index;
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, _Texture.Width, _Texture.Height);
            LifeSpan = 2;
        }


        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void Destroy() { }
        public virtual void LoadAnimation() { }
    }
    #endregion
    #region PlatformBrick
    class PlatformBrick : Brick
    {
        public PlatformBrick(Texture2D _Texture, Vector2 Position, ContentManager Content, int Index)
            : base(_Texture, Position, Content, Index)
        {

        }
        public override void Update(GameTime gameTime) { }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_Texture, Position, Color.White);
        }
    }
    #endregion
    #region NormalBrick
    class NormalBrick : Brick
    {
        public float Force;
        Vector2 Save_Positon;
        public NormalBrick(Texture2D _Texture, Vector2 Position, ContentManager Content, int Index)
            : base(_Texture, Position, Content, Index)
        {
            Force = 1;
            Save_Positon = Position;
        }
        public override void Update(GameTime gameTime)
        {
            if (LifeSpan == 1)
            {
                Position.Y += 2;
                Force += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Force >= 3) LifeSpan = 0;
            }
            if (LifeSpan == 3)
            {
                Position.Y++;
                if (Save_Positon == Position)
                    LifeSpan = 2;

            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_Texture, Position, Color.White);
        }
        public override void Destroy()
        {
            LifeSpan = 1;
            _Texture = Content.Load<Texture2D>("DestroyedBrick1");
        }
        public override void LoadAnimation()
        {
            Position.Y -= 10;
            LifeSpan = 3;
        }
    }
    #endregion
    #region SpecialBrick
    class SpecialBrick : Brick
    {
        Texture2D Animation;
        float[,] Elapse;
        public SpecialBrick(Texture2D _Texture, Vector2 Position, ContentManager Content, int Index)
            : base(_Texture, Position, Content, Index)
        {
            Animation = Content.Load<Texture2D>("SpecialBrick");
            _Texture = Content.Load<Texture2D>("DestroyedBrick2");
            Elapse = new float[1, 2];
        }
        public override void Destroy()
        {
            LifeSpan = 1;
            _Texture = Content.Load<Texture2D>("DestroyedBrick2");
        }
        public override void Update(GameTime gameTime)
        {
            Elapse[0, 1] += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Elapse[0, 1] >= 100)
            {
                Elapse[0, 1] = 0;
                Elapse[0, 0] = Elapse[0, 0] == 5 ? 0 : Elapse[0, 0] + 1;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (LifeSpan == 2)
                spriteBatch.Draw(Animation, new Rectangle((int)Position.X, (int)Position.Y, 35, 35), new Rectangle((int)Elapse[0, 0] * 35, 0, 35, 35), Color.White);
            else
                spriteBatch.Draw(_Texture, Position, Color.White);
        }
    }
    #endregion
    #region Pipe
    class Pipe : Brick
    {
        public Pipe(Texture2D _Texture, Vector2 Position, ContentManager Content, int Index)
            : base(_Texture, Position, Content, Index)
        {

        }
        public override void Update(GameTime gameTime) { }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_Texture, Position, Color.White);
        }
    }
    #endregion
    #region BillBlaster
    class BillBlaster : Brick
    {
        public BillBlaster(Texture2D _Texture, Vector2 Position, ContentManager Content, int Index)
             : base(_Texture, Position, Content, Index)
        {

        }
        public override void Update(GameTime gameTime) { }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_Texture, Position, Color.White);
        }
    }
    #endregion
    #region Bullet
    class bullet : Brick
    {
        public string direction;
        public int force, timer;
        public bool up, released;
        Texture2D fire_ball;
        Vector2 tmp_pos;
        public bullet(Texture2D texture, Vector2 position, ContentManager content, string direction, int index) : base(texture, position, content, index)
        {
            this.direction = direction;
            force = 3;
            up = released = false;
            timer = 0;
            fire_ball = Content.Load<Texture2D>("Ball");
            tmp_pos = position;

        }

        public override void Update(GameTime gameTime)
        {
            timer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer >= 1100)
            {
                timer = 0;
                released = false;
            }
            if (up == false)
            {
                if (direction == "Right")
                {
                    Position.X += force;
                    Position.Y += 2;
                }
                else if (direction == "Left")
                {
                    Position.X -= force;
                    Position.Y += 2;
                }
            }
            else
            {
                if (direction == "Right")
                {
                    Position.X += force;
                    Position.Y -= 2;
                }
                else if (direction == "Left")
                {
                    Position.X -= force;
                    Position.Y -= 2;
                }
            }
            if (Position.Y >= 405)
            {
                up = true;
                Position.Y = 405;
            }
            if (Position.Y <= tmp_pos.Y + 20 && timer > 550)
            {
                up = false;
                Position.Y = (tmp_pos.Y + 20);
            }
            Bounds.X = (int)Position.X;
            Bounds.Y = (int)Position.Y;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (direction == "Right")
                spriteBatch.Draw(fire_ball, new Vector2(Position.X + 50, Position.Y + 15), Color.White);
            else
                spriteBatch.Draw(fire_ball, new Vector2(Position.X - 10, Position.Y + 15), Color.White);
        }

    }
    #endregion
    #region stairs
    class stairs : Brick
    {
        public stairs(Texture2D _Texture, Vector2 Position, ContentManager Content, int Index)
            : base(_Texture, Position, Content, Index)
        {
        }
        public override void Update(GameTime gameTime)
        {

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_Texture, Position, Color.White);
        }
    }
    #endregion
    #region FLAG
    class FLAG : Brick
    {
        public FLAG(Texture2D _Texture, Vector2 Position, ContentManager Content, int Index)
            : base(_Texture, Position, Content, Index)
        {
        }
        public override void Update(GameTime gameTime)
        {

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_Texture, Position, Color.White);
        }
    }
    #endregion
    #region Caslte
    class Caslte : Brick
    {
        public Caslte(Texture2D _Texture, Vector2 Position, ContentManager Content, int Index)
            : base(_Texture, Position, Content, Index)
        {
        }
        public override void Update(GameTime gameTime)
        {

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_Texture, Position, Color.White);
        }
    }
    #endregion
    #region Escalator
    class Escalator : Brick
    {
        public Escalator(Texture2D _Texture, Vector2 Position, ContentManager Content, int Index)
            : base(_Texture, Position, Content, Index)
        {
        }
        public override void Update(GameTime gameTime)
        {

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_Texture, Position, Color.White);
        }
    }
    #endregion
    #region INVBrick
    class INVBrick : Brick
    {
        Texture2D Animation;
        float[,] Elapse;
        public INVBrick(Texture2D _Texture, Vector2 Position, ContentManager Content, int Index)
            : base(_Texture, Position, Content, Index)
        {
            Animation = Content.Load<Texture2D>("SpecialBrick");
            _Texture = Content.Load<Texture2D>("DestroyedBrick2");
            Elapse = new float[1, 2];
        }
        public override void Destroy()
        {
            LifeSpan = 1;
            _Texture = Content.Load<Texture2D>("DestroyedBrick2");
        }
        public override void Update(GameTime gameTime)
        {
            Elapse[0, 1] += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Elapse[0, 1] >= 100)
            {
                Elapse[0, 1] = 0;
                Elapse[0, 0] = Elapse[0, 0] == 5 ? 0 : Elapse[0, 0] + 1;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (LifeSpan == 2)
                spriteBatch.Draw(Animation, new Rectangle((int)Position.X, (int)Position.Y, 35, 35), new Rectangle((int)Elapse[0, 0] * 35, 0, 35, 35), Color.White*.0f);
            else
                spriteBatch.Draw(_Texture, Position, Color.White);
        }
    }
    #endregion
}
