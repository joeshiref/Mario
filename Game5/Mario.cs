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
    class Mario
    {
        public int num_coins;
        public int Lives;
        public MarioState Mariostate;
        public Rectangle Bounds;
        public float TransformingTime;
        public bullet bullet;
        public Texture2D fire_ball;
        ContentManager content;
        public Mario(ContentManager Content, Vector2 Position)
        {
            num_coins = 0;
            Lives = 30;
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, 35, 40);
            Mariostate = new MarioSmall(Content, Position, 35, 40);
            Mariostate.Vibrating = false;
            TransformingTime = 0;
            bullet = new bullet(Content.Load<Texture2D>("Ball"), Position, Content, Mariostate.Direction, -1);
            fire_ball = Content.Load<Texture2D>("Ball");
            content = Content;

        }
        public void Update(GameTime gameTime, ref ScreenManager Screenmanager, ref Mario mario)
        {
            Mariostate.UpdatePosition(gameTime, ref Screenmanager, ref mario);
            Mariostate.Update(gameTime);
            Bounds = new Rectangle((int)Mariostate.Position.X, (int)Mariostate.Position.Y, Mariostate.Width, Mariostate.Height);
            if (Mariostate.Vibrating == true)
            {
                TransformingTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (TransformingTime >= 2000)
                {
                    Mariostate.Vibrating = false;
                    TransformingTime = 0;
                }
            }
            if (Mariostate.GetType() == typeof(MarioFire))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    if (bullet.timer == 0)
                    {
                        bullet = new bullet(fire_ball, Mariostate.Position, content, Mariostate.Direction, -1);
                        bullet.released = true;
                        content.Load<SoundEffect>("FireBall").Play();
                    }
                }
                if (bullet.released == true)
                    bullet.Update(gameTime);
            }
           

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Mariostate.Draw(spriteBatch);
            if (Mariostate.GetType() == typeof(MarioFire))
            {
                if (bullet.released == true)
                    bullet.Draw(spriteBatch);
            }
        }
        public void Destroy()
        {
            Mariostate.LifeSpan = 1;
            Mariostate.SteadyDyingTime = 0;
            Mariostate.Gravity = -10;
            content.Load<SoundEffect>("mariodie").Play();
        }
    }
}
