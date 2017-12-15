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
    class MarioState
    {
        public ContentManager Content;
        public Vector2 Position;
        public string Direction, Moving, Jumping, Landing;
        public int Width, Height, Gravity, VibrationTime, LifeSpan;
        public bool Vibrating;
        public float SteadyDyingTime;
        public List<SoundEffect> SoundEffects;
        public MarioState(ContentManager Content, Vector2 Position, int Width, int Height)
        {
            Direction = "Right";
            Moving = "False";
            Jumping = "False";
            Landing = "Platform";
            LifeSpan = 2;
            this.Width = Width;
            this.Height = Height;
            this.Content = Content;
            this.Position = Position;
            Gravity = 0;
            Vibrating = false;
            VibrationTime = 0;
            SoundEffects = new List<SoundEffect>();
            LoadSoundEffects();
        }
        void LoadSoundEffects()
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void Update(GameTime gameTime) { }
        #region UpdatePosition
        public void UpdatePosition(GameTime gameTime, ref ScreenManager Screenmanager, ref Mario mario)
        {
            if (LifeSpan < 2) return;
            Moving = "False";
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Position.X += 4;
                Direction = "Right";
                Moving = "True";
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Position.X -= 4;
                Direction = "Left";
                Moving = "True";
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (Jumping == "False")
                {
                    if (this.GetType() == typeof(MarioSmall)) Content.Load<SoundEffect>("JumpSmall").Play();
                    else Content.Load<SoundEffect>("JumpSuper").Play();
                    Jumping = "True";
                    Gravity = -20;
                }
            }
            if (Jumping == "True")
            {
                Position.Y += Gravity;
                Gravity++;
            }
            if (Position.X <= 0) Screenmanager.Backward();
            if (Position.X + Width >= 800) Screenmanager.Forward();
            if (Position.Y > 456)
            {
                mario.Mariostate = new MarioSmall(Content, mario.Mariostate.Position, 35, 40);
                mario.Destroy();
            }
        }

        #endregion
    }
}
