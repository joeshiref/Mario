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
    #region ItemManager
    class ItemManager
    {
        public ContentManager Content;
        public List<Item> Items;
        public ItemManager(ContentManager Content)
        {
            this.Content = Content;
            Items = new List<Item>();
        }
        public void AddItem(Item item)
        {
            Items.Add(item);
        }
        public void Update(GameTime gameTIme)
        {
            for (int i = 0; i < Items.ToArray().Length; i++)
            {
                if (Items[i] == null) continue;
                if (Items[i].LifeSpan == 2) Items[i].Update(gameTIme);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Items.ToArray().Length; i++)
            {
                if (Items[i] == null) continue;
                if (Items[i].LifeSpan == 2) Items[i].Draw(spriteBatch);
            }
        }
    }
    #endregion
    #region Item
    class Item
    {
        ContentManager Content;
        public Texture2D _Texture;
        public Vector2 Position;
        public Rectangle Bounds;
        public int LifeSpan;
        public int Force, Width, Height, Gravity;
        public Item(ContentManager Content, Vector2 Position, int Width, int Height, int LifeSpan)
        {
            this.Content = Content;
            this.Position = Position;
            this.Width = Width;
            this.Height = Height;
            this.LifeSpan = LifeSpan;
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            Force = 2;
            Gravity = 0;
        }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
    #endregion
    #region Mushroom
    class Mushroom : Item
    {
        public float Time;
        public bool GravityBoolean;
        public Mushroom(ContentManager Content, Vector2 Position, int Width, int Height, int LifeSpan)
            : base(Content, Position, Width, Height, LifeSpan)
        {
            _Texture = Content.Load<Texture2D>("Mushroom");
            GravityBoolean = false;
            Time = 0;
        }
        public override void Update(GameTime gameTime)
        {
            Position.X += Force;
            if (Position.X < 0) { Position.X = 0; Force = -Force; }
            if (Position.X + 42 > 800) { Position.X = 758; Force = -Force; }
            Time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Time >= 1000) GravityBoolean = true;
            if (GravityBoolean == true)
            {
                if (Position.Y < 391)
                {
                    Position.Y += Gravity;
                    Gravity++;
                }
            }
            Bounds.X = (int)Position.X;
            Bounds.Y = (int)Position.Y;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_Texture, Position, Color.White);
        }
    }
    #endregion
    #region FireFlower
    class FireFlower : Item
    {
        public FireFlower(ContentManager Content, Vector2 Position, int Width, int Height, int LifeSpan)
            : base(Content, Position, Width, Height, LifeSpan)
        {
            _Texture = Content.Load<Texture2D>("FireFlower");
        }
        public override void Update(GameTime gameTime) { }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_Texture, Position, Color.White);
        }
    }
    #endregion
    #region Coins
    class Coins : Item
    {
        float[,] Elapse;
        public Coins(ContentManager Content, Vector2 Position, int Width, int Height, int LifeSpan) : base(Content, Position, Width, Height, LifeSpan)
        {
           
                _Texture = Content.Load<Texture2D>("Coins");
            Elapse = new float[1, 2];
        }
        public override void Update(GameTime gameTime)
        {

            Elapse[0, 0] += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Elapse[0, 0] >= 100)
            {
                Elapse[0, 0] = 0;
                Elapse[0, 1] = Elapse[0, 1] == 4 ? 0 : Elapse[0, 1] + 1;
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {


            spriteBatch.Draw(_Texture, new Rectangle((int)Position.X, (int)Position.Y, 22, 34), new Rectangle((int)Elapse[0, 1] * 22, 0, 22, 34), Color.White);
        }
        #endregion

    }
}
