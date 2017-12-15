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
    #region MarioSmall
    class MarioSmall : MarioState
    {
        float[,] Elapse;
        List<Texture2D> TexturesSmall;
        public MarioSmall(ContentManager Content, Vector2 Position, int Width, int Height)
            : base(Content, Position, Width, Height)
        {
            Elapse = new float[1, 2];
            TexturesSmall = new List<Texture2D>();
            Load();
        }
        public void Load()
        {
            TexturesSmall.Add(Content.Load<Texture2D>("SmallJumpLeft"));
            TexturesSmall.Add(Content.Load<Texture2D>("SmallWalkLeft"));
            TexturesSmall.Add(Content.Load<Texture2D>("SmallStandLeft"));
            TexturesSmall.Add(Content.Load<Texture2D>("SmallStandRight"));
            TexturesSmall.Add(Content.Load<Texture2D>("SmallWalkRight"));
            TexturesSmall.Add(Content.Load<Texture2D>("SmallJumpRight"));
            TexturesSmall.Add(Content.Load<Texture2D>("MarioDead"));
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (LifeSpan == 2)
            {
                if ((Vibrating == true && VibrationTime % 5 == 0) || Vibrating == false)
                {
                    if (Jumping == "True")
                    {
                        if (Direction == "Right") spriteBatch.Draw(TexturesSmall[5], Position, Color.White);
                        else spriteBatch.Draw(TexturesSmall[0], Position, Color.White);
                    }
                    else if (Moving == "True")
                    {
                        if (Direction == "Right")
                            spriteBatch.Draw(TexturesSmall[4], new Rectangle((int)Position.X, (int)Position.Y, 35, 42), new Rectangle((int)Elapse[0, 1] * 73, 0, 35, 42), Color.White);
                        else
                            spriteBatch.Draw(TexturesSmall[1], new Rectangle((int)Position.X, (int)Position.Y, 35, 42), new Rectangle((int)Elapse[0, 1] * 73, 0, 35, 42), Color.White);
                    }
                    else if (Direction == "Right") spriteBatch.Draw(TexturesSmall[3], Position, Color.White);
                    else spriteBatch.Draw(TexturesSmall[2], Position, Color.White);
                }
            }
            else if (LifeSpan == 1)
                spriteBatch.Draw(TexturesSmall[6], Position, Color.White);
        }
        public override void Update(GameTime gameTime)
        {
            if (LifeSpan == 2)
            {
                VibrationTime = VibrationTime > 999 ? 0 : VibrationTime + 1;
                if (Moving == "True")
                {
                    Elapse[0, 0] += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (Elapse[0, 0] >= 120)
                    {
                        Elapse[0, 0] = 0;
                        Elapse[0, 1] = Elapse[0, 1] == 2 ? 0 : Elapse[0, 1] + 1;
                    }
                }
                else
                {
                    Elapse[0, 0] = 0;
                    Elapse[0, 1] = 0;
                }
            }
            else if (LifeSpan == 1)
            {
                if (SteadyDyingTime != -1000)
                {
                    SteadyDyingTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (SteadyDyingTime >= 1500) SteadyDyingTime = -1000;
                }
                else
                {
                    Position.Y += Gravity;
                    Gravity++;
                    if (Position.Y > 480) LifeSpan = 0;
                }
            }
        }
    }
    #endregion
    #region MarioBig
    class MarioBig : MarioState
    {
        List<Texture2D> TexturesBig;
        float[,] Elapse;
        public MarioBig(ContentManager Content, Vector2 Position, int Width, int Height)
            : base(Content, Position, Width, Height)
        {
            Elapse = new float[1, 2];
            TexturesBig = new List<Texture2D>();
            Load();
        }
        public void Load()
        {
            TexturesBig.Add(Content.Load<Texture2D>("BigJumpLeft"));
            TexturesBig.Add(Content.Load<Texture2D>("BigWalkLeft"));
            TexturesBig.Add(Content.Load<Texture2D>("BigStandLeft"));
            TexturesBig.Add(Content.Load<Texture2D>("BigStandRight"));
            TexturesBig.Add(Content.Load<Texture2D>("BigWalkRight"));
            TexturesBig.Add(Content.Load<Texture2D>("BigJumpRight"));
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if ((Vibrating == true && VibrationTime % 5 == 0) || Vibrating == false)
            {
                if (Jumping == "True")
                {
                    if (Direction == "Right") spriteBatch.Draw(TexturesBig[5], Position, Color.White);
                    else spriteBatch.Draw(TexturesBig[0], Position, Color.White);
                }
                else if (Moving == "True")
                {
                    if (Direction == "Right")
                        spriteBatch.Draw(TexturesBig[4], new Rectangle((int)Position.X, (int)Position.Y, 47, 79), new Rectangle((int)Elapse[0, 1] * 73, 0, 47, 79), Color.White);
                    else
                        spriteBatch.Draw(TexturesBig[1], new Rectangle((int)Position.X, (int)Position.Y, 47, 79), new Rectangle((int)Elapse[0, 1] * 73, 0, 47, 79), Color.White);
                }
                else if (Direction == "Right") spriteBatch.Draw(TexturesBig[3], Position, Color.White);
                else spriteBatch.Draw(TexturesBig[2], Position, Color.White);
            }
        }
        public override void Update(GameTime gameTime)
        {
            VibrationTime = VibrationTime > 999 ? 0 : VibrationTime + 1;
            if (Moving == "True")
            {
                Elapse[0, 0] += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (Elapse[0, 0] >= 120)
                {
                    Elapse[0, 0] = 0;
                    Elapse[0, 1] = Elapse[0, 1] == 2 ? 0 : Elapse[0, 1] + 1;
                }
            }
            else
            {
                Elapse[0, 0] = 0;
                Elapse[0, 1] = 0;
            }
        }

    }
    #endregion
    #region MarioFire
    class MarioFire : MarioState
    {
        List<Texture2D> TexturesFire;
        float[,] Elapse;
        public MarioFire(ContentManager Content, Vector2 Position, int Width, int Height)
            : base(Content, Position, Width, Height)
        {
            Elapse = new float[1, 2];
            TexturesFire = new List<Texture2D>();
            Load();
        }
        public void Load()
        {
            TexturesFire.Add(Content.Load<Texture2D>("FireJumpLeft"));
            TexturesFire.Add(Content.Load<Texture2D>("FireWalkLeft"));
            TexturesFire.Add(Content.Load<Texture2D>("FireStandLeft"));
            TexturesFire.Add(Content.Load<Texture2D>("FireStandRight"));
            TexturesFire.Add(Content.Load<Texture2D>("FireWalkRight"));
            TexturesFire.Add(Content.Load<Texture2D>("FireJumpRight"));
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if ((Vibrating == true && VibrationTime % 5 == 0) || (Vibrating == false))
            {
                if (Jumping == "True")
                {
                    if (Direction == "Right") spriteBatch.Draw(TexturesFire[5], Position, Color.White);
                    else spriteBatch.Draw(TexturesFire[0], Position, Color.White);
                }
                else if (Moving == "True")
                {
                    if (Direction == "Right")
                        spriteBatch.Draw(TexturesFire[4], new Rectangle((int)Position.X, (int)Position.Y, 47, 79), new Rectangle((int)Elapse[0, 1] * 61, 0, 47, 79), Color.White);
                    else
                        spriteBatch.Draw(TexturesFire[1], new Rectangle((int)Position.X, (int)Position.Y, 47, 79), new Rectangle((int)Elapse[0, 1] * 61, 0, 47, 79), Color.White);
                }
                else if (Direction == "Right") spriteBatch.Draw(TexturesFire[3], Position, Color.White);
                else spriteBatch.Draw(TexturesFire[2], Position, Color.White);
            }
        }
        public override void Update(GameTime gameTime)
        {
            VibrationTime = VibrationTime > 999 ? 0 : VibrationTime + 1;
            if (Moving == "True")
            {
                Elapse[0, 0] += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (Elapse[0, 0] >= 120)
                {
                    Elapse[0, 0] = 0;
                    Elapse[0, 1] = Elapse[0, 1] == 2 ? 0 : Elapse[0, 1] + 1;
                }
            }
            else
            {
                Elapse[0, 0] = 0;
                Elapse[0, 1] = 0;
            }
        }
    }
    #endregion
    #region MarioSuper
    class MarioSuper : MarioState
    {
        List<Texture2D> TexturesSuper;
        public MarioSuper(ContentManager Content, Vector2 Position, int Width, int Height)
            : base(Content, Position, Width, Height)
        {
            TexturesSuper = new List<Texture2D>();
        }
    }
    #endregion
}
