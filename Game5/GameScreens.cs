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
    #region Screen
    class Screen
    {
        public ContentManager Content;
        public Mario mario;
        public SpriteFont spriteFont;
        public Screen(ContentManager Content, Mario mario)
        {
            this.Content = Content;
            this.mario = mario;
            spriteFont = Content.Load<SpriteFont>("MyFont");
        }
        public virtual void Update(GameTime gameTime, ref ScreenManager Screenmanager) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
        public void UpdateMarioState()
        {
            if (mario == null) return;
            if (mario.Mariostate.LifeSpan == 0)
            {
                if (mario.Lives == 1) mario = null;
                else
                {
                    mario.Mariostate.LifeSpan = 2;
                    mario.Mariostate.Direction = "Right";
                    mario.Mariostate.Position = new Vector2(1, 370);
                    mario.Lives--;
                }
            }
        }
    }
    #endregion
    #region ScreenOne
    class ScreenOne : Screen
    {
        public BrickManager Brickmanager;
        public ItemManager Itemmanager;
        public EnemyManager Enemymanager;
        public int SpecialBricksCount;
        public ScreenOne(ContentManager Content, Mario mario)
            : base(Content, mario)
        {
            Brickmanager = new BrickManager(Content);
            Itemmanager = new ItemManager(Content);
            Enemymanager = new EnemyManager(Content);
            SpecialBricksCount = 0;
            LoadBricks();
            LoadItems();
            LoadEnemies();
        }
        public void LoadBricks()
        {
            for (int i = 0; i <= 800 / 24; i++)
            {
                Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 432), Content, -1));
                Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 456), Content, -1));
            }
            for (int i = 11; i <= 15; i++)
            {
                if (i % 2 == 1) Brickmanager.AddBrick(new NormalBrick(Content.Load<Texture2D>("Brick3"), new Vector2(i * 35, 280), Content, -1));
                else Brickmanager.AddBrick(new SpecialBrick(Content.Load<Texture2D>("Brick4"), new Vector2(i * 35, 280), Content, SpecialBricksCount++));
            }
        }
        public void LoadItems()
        {
            Itemmanager.AddItem(new Mushroom(Content, new Vector2(420, 239), 42, 41, 1));
            Itemmanager.AddItem(null);
        }
        public void LoadEnemies()
        {
            Enemymanager.AddEnemy(new Gumba(Content, new Vector2(400, 392), 44, 41, 0, 800, 0, 480));
        }
        public override void Update(GameTime gameTime, ref ScreenManager Screenmanager)
        {
            UpdateMarioState();
            if (mario != null) mario.Update(gameTime, ref Screenmanager, ref mario);
            Brickmanager.Update(gameTime);
            Itemmanager.Update(gameTime);
            Enemymanager.Update(gameTime);
            Collision.CollideBricks(gameTime, ref mario, ref Brickmanager, ref Itemmanager, Content ,ref  Screenmanager);
            Collision.CollideItems(gameTime, ref mario, ref Itemmanager, Content);
            Collision.CollideEnemies(gameTime, ref mario, ref Enemymanager, Content);
            Collision.BulletBrick(gameTime, ref mario, ref Brickmanager, ref Enemymanager);
            Collision.EnemyBrick(gameTime, ref Brickmanager, ref Enemymanager, Content);
            Collision.CollidEnemyVSDuck(gameTime, ref Enemymanager, Content);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 1; i <= 3; i++)
            {
                if (i % 2 == 1) spriteBatch.Draw(Content.Load<Texture2D>("Cloud1"), new Vector2(150 * i, 100), Color.White);
                else spriteBatch.Draw(Content.Load<Texture2D>("Cloud2"), new Vector2(150 * i, 100), Color.White);
            }
            spriteBatch.Draw(Content.Load<Texture2D>("Grass3"), new Vector2(100, 405), Color.White);
            spriteBatch.Draw(Content.Load<Texture2D>("Grass2"), new Vector2(500, 386), Color.White);
            Brickmanager.Draw(spriteBatch);
            Itemmanager.Draw(spriteBatch);
            Enemymanager.Draw(spriteBatch);
            if (mario != null)
            {
                spriteBatch.Draw(Content.Load<Texture2D>("MarioLives"), new Vector2(20, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.Lives.ToString(), new Vector2(112, 29), Color.White);
                mario.Draw(spriteBatch);
                spriteBatch.Draw(Content.Load<Texture2D>("count coins"), new Vector2(200, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.num_coins.ToString(), new Vector2(280, 29), Color.White);
                mario.Draw(spriteBatch);
            }
        }
    }
    #endregion
    #region ScreenTwo
    class ScreenTwo : Screen
    {
        public BrickManager Brickmanager;
        public ItemManager Itemmanager;
        public EnemyManager Enemymanager;
        public int SpecialBricksCount;
        public ScreenTwo(ContentManager Content, Mario mario)
            : base(Content, mario)
        {
            Brickmanager = new BrickManager(Content);
            Itemmanager = new ItemManager(Content);
            Enemymanager = new EnemyManager(Content);
            LoadBricks();
            LoadItems();
            LoadEnemies();
            SpecialBricksCount = 0;
        }
        public void LoadBricks()
        {
            for (int i = 0; i <= 800 / 24; i++)
            {
                Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 432), Content, -1));
                Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 456), Content, -1));
            }
            for (int i = 10; i <= 16; i++)
            {
                if (i == 13) continue;
                Brickmanager.AddBrick(new NormalBrick(Content.Load<Texture2D>("Brick3"), new Vector2(i * 35, 226), Content, -1));
            }
            Brickmanager.AddBrick(new Pipe(Content.Load<Texture2D>("Pipe1"), new Vector2(150, 279), Content, -1));
            Brickmanager.AddBrick(new SpecialBrick(Content.Load<Texture2D>("Brick4"), new Vector2(455, 226), Content, 0));
        }
        public void LoadItems()
        {
            Itemmanager.AddItem(new FireFlower(Content, new Vector2(455, 184), 44, 42, 1));
            for (int i = 10; i <= 16; i++)
            {
                

            Itemmanager.AddItem(new Coins(Content, new Vector2(i * 35, 190), 22, 34, 2));
            }
         
        }
        public void LoadEnemies()
        {
            Enemymanager.AddEnemy(new Spiny(Content, new Vector2(300, 393), 43, 41, 226, 757, 0, 480));
            Enemymanager.AddEnemy(new Plant(Content, new Vector2(168, 270), 44, 56, 0, 800, 223, 360));
            Enemymanager.AddEnemy(new Duck(Content, new Vector2(650, 374), 42, 60, 0, 800, 0, 480));
        }
        public override void Update(GameTime gameTime, ref ScreenManager Screenmanager)
        {
            UpdateMarioState();
            if (mario != null) mario.Update(gameTime, ref Screenmanager, ref mario);
            Brickmanager.Update(gameTime);
            Itemmanager.Update(gameTime);
            Enemymanager.Update(gameTime);
            Collision.CollideBricks(gameTime, ref mario, ref Brickmanager, ref Itemmanager, Content, ref  Screenmanager);
            Collision.CollideItems(gameTime, ref mario, ref Itemmanager, Content);
            Collision.CollideEnemies(gameTime, ref mario, ref Enemymanager, Content);
            Collision.EnemyBrick(gameTime, ref Brickmanager, ref Enemymanager, Content);
            Collision.BulletBrick(gameTime, ref mario, ref Brickmanager, ref Enemymanager);
            Collision.CollidEnemyVSDuck(gameTime, ref Enemymanager, Content);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 1; i <= 3; i++)
            {
                if (i % 2 == 1) spriteBatch.Draw(Content.Load<Texture2D>("Cloud1"), new Vector2(150 * i, 100), Color.White);
                else spriteBatch.Draw(Content.Load<Texture2D>("Cloud2"), new Vector2(150 * i, 100), Color.White);
            }
            spriteBatch.Draw(Content.Load<Texture2D>("Grass4"), new Vector2(300, 410), Color.White);
            spriteBatch.Draw(Content.Load<Texture2D>("Grass4"), new Vector2(500, 410), Color.White);
            Enemymanager.Draw(spriteBatch);
            Brickmanager.Draw(spriteBatch);
            Itemmanager.Draw(spriteBatch);
            if (mario != null)
            {
                spriteBatch.Draw(Content.Load<Texture2D>("MarioLives"), new Vector2(20, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.Lives.ToString(), new Vector2(112, 29), Color.White);
                mario.Draw(spriteBatch);
                spriteBatch.Draw(Content.Load<Texture2D>("count coins"), new Vector2(200, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.num_coins.ToString(), new Vector2(280, 29), Color.White);
                mario.Draw(spriteBatch);
            }
          
        }
    }
    #endregion
    #region ScreenThree
    class ScreenThree : Screen
    {
        public BrickManager Brickmanager;
        public ItemManager Itemmanager;
        public EnemyManager Enemymanager;
        int SpecialBricks;
        public ScreenThree(ContentManager Content, Mario mario)
            : base(Content, mario)
        {
            Brickmanager = new BrickManager(Content);
            Itemmanager = new ItemManager(Content);
            Enemymanager = new EnemyManager(Content);
            SpecialBricks = 0;
            LoadEnemies();
            LoadBricks();
            LoadItems();
        }
        public void LoadItems()
        {
            Itemmanager.AddItem(null);
            Itemmanager.AddItem(null);
        }
        public void LoadBricks()
        {
            for (int i = 0; i <= 800 / 24; i++)
            {
                if (i < 3 || i > 7)
                    Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 456), Content, -1));
            }
            for (int i = 0; i <= 800 / 24; i++)
            {
                if (i < 3 || i > 7)
                    Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 432), Content, -1));
            }
            Brickmanager.AddBrick(new NormalBrick(Content.Load<Texture2D>("Brick3"), new Vector2(765, 192), Content, -1));
            Brickmanager.AddBrick(new NormalBrick(Content.Load<Texture2D>("Brick3"), new Vector2(730, 192), Content, -1));
            Brickmanager.AddBrick(new SpecialBrick(Content.Load<Texture2D>("Brick4"), new Vector2(695, 192), Content, SpecialBricks++));
            Brickmanager.AddBrick(new NormalBrick(Content.Load<Texture2D>("Brick3"), new Vector2(660, 192), Content, -1));
            Brickmanager.AddBrick(new NormalBrick(Content.Load<Texture2D>("Brick3"), new Vector2(625, 192), Content, -1));
            Brickmanager.AddBrick(new NormalBrick(Content.Load<Texture2D>("Brick3"), new Vector2(590, 192), Content, -1));
            Brickmanager.AddBrick(new NormalBrick(Content.Load<Texture2D>("Brick3"), new Vector2(560, 307), Content, -1));
            Brickmanager.AddBrick(new NormalBrick(Content.Load<Texture2D>("Brick3"), new Vector2(525, 307), Content, -1));
            Brickmanager.AddBrick(new SpecialBrick(Content.Load<Texture2D>("Brick4"), new Vector2(490, 307), Content, SpecialBricks++));
            Brickmanager.AddBrick(new NormalBrick(Content.Load<Texture2D>("Brick3"), new Vector2(455, 307), Content, -1));
        }
        public void LoadEnemies()
        {
            Enemymanager.AddEnemy(new Gumba(Content, new Vector2(765, 150), 44, 41, 0, 800, 0, 480));
            Enemymanager.AddEnemy(new Gumba(Content, new Vector2(690, 150), 44, 41, 0, 800, 0, 480));
        }
        public override void Update(GameTime gameTime, ref ScreenManager Screenmanager)
        {
            UpdateMarioState();
            if (mario != null) mario.Update(gameTime, ref Screenmanager, ref mario);
            Brickmanager.Update(gameTime);
            Itemmanager.Update(gameTime);
            Enemymanager.Update(gameTime);
            Collision.CollideBricks(gameTime, ref mario, ref Brickmanager, ref Itemmanager, Content, ref  Screenmanager);
            Collision.CollideItems(gameTime, ref mario, ref Itemmanager, Content);
            Collision.CollideEnemies(gameTime, ref mario, ref Enemymanager, Content);
            Collision.EnemyBrick(gameTime, ref Brickmanager, ref Enemymanager, Content);
            Collision.BulletBrick(gameTime, ref mario, ref Brickmanager, ref Enemymanager);
            Collision.CollidEnemyVSDuck(gameTime, ref Enemymanager, Content);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Content.Load<Texture2D>("Cloud1"), new Vector2(50, 100), Color.White);
            spriteBatch.Draw(Content.Load<Texture2D>("Cloud1"), new Vector2(250, 100), Color.White);
            spriteBatch.Draw(Content.Load<Texture2D>("Cloud1"), new Vector2(450, 100), Color.White);
            spriteBatch.Draw(Content.Load<Texture2D>("Grass4"), new Vector2(730, 410), Color.White);
            Brickmanager.Draw(spriteBatch);
            Enemymanager.Draw(spriteBatch);
            if (mario != null)
            {
                spriteBatch.Draw(Content.Load<Texture2D>("MarioLives"), new Vector2(20, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.Lives.ToString(), new Vector2(112, 29), Color.White);
                mario.Draw(spriteBatch);
                spriteBatch.Draw(Content.Load<Texture2D>("count coins"), new Vector2(200, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.num_coins.ToString(), new Vector2(280, 29), Color.White);
                mario.Draw(spriteBatch);
            }
        }
    }
    #endregion
    #region ScreenFour
    class ScreenFour : Screen
    {
        public BrickManager Brickmanager;
        public ItemManager Itemmanager;
        public EnemyManager Enemymanager;
        public int SpecialBricksCount;
        public ScreenFour(ContentManager Content, Mario mario)
            : base(Content, mario)
        {
            Brickmanager = new BrickManager(Content);
            Itemmanager = new ItemManager(Content);
            Enemymanager = new EnemyManager(Content);
            LoadBricks();
            LoadItems();
            LoadEnemies();
            SpecialBricksCount = 0;
        }
        public void LoadBricks()
        {
            for (int i = 0; i <= 800 / 24; i++)
            {
                Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 432), Content, -1));
                Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 456), Content, -1));
            }

            Brickmanager.AddBrick(new Pipe(Content.Load<Texture2D>("Pipe1"), new Vector2(150, 279), Content, -1));
            Brickmanager.AddBrick(new Pipe(Content.Load<Texture2D>("Pipe1"), new Vector2(600, 279), Content, -1));

        }
        public void LoadItems()
        {
        }
        public void LoadEnemies()
        {
            Enemymanager.AddEnemy(new Duck(Content, new Vector2(650, 374), 42, 60, 0, 800, 0, 480));
        }
        public override void Update(GameTime gameTime, ref ScreenManager Screenmanager)
        {
            UpdateMarioState();
            if (mario != null) mario.Update(gameTime, ref Screenmanager, ref mario);
            Brickmanager.Update(gameTime);
            Itemmanager.Update(gameTime);

            Enemymanager.Update(gameTime);
            Collision.CollideBricks(gameTime, ref mario, ref Brickmanager, ref Itemmanager, Content, ref  Screenmanager);
            Collision.CollideItems(gameTime, ref mario, ref Itemmanager, Content);
            Collision.CollideEnemies(gameTime, ref mario, ref Enemymanager, Content);
            Collision.EnemyBrick(gameTime, ref Brickmanager, ref Enemymanager, Content);
            Collision.BulletBrick(gameTime, ref mario, ref Brickmanager, ref Enemymanager);
            Collision.CollidEnemyVSDuck(gameTime, ref Enemymanager, Content);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 1; i <= 3; i++)
            {
                if (i % 2 == 1) spriteBatch.Draw(Content.Load<Texture2D>("Cloud1"), new Vector2(150 * i, 100), Color.White);
                else spriteBatch.Draw(Content.Load<Texture2D>("Cloud2"), new Vector2(150 * i, 100), Color.White);
            }
            spriteBatch.Draw(Content.Load<Texture2D>("Grass4"), new Vector2(300, 410), Color.White);
            spriteBatch.Draw(Content.Load<Texture2D>("Grass4"), new Vector2(500, 410), Color.White);
            Enemymanager.Draw(spriteBatch);
            Brickmanager.Draw(spriteBatch);
            Itemmanager.Draw(spriteBatch);
            if (mario != null)
            {
                spriteBatch.Draw(Content.Load<Texture2D>("MarioLives"), new Vector2(20, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.Lives.ToString(), new Vector2(112, 29), Color.White);
                mario.Draw(spriteBatch);
                spriteBatch.Draw(Content.Load<Texture2D>("count coins"), new Vector2(200, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.num_coins.ToString(), new Vector2(280, 29), Color.White);
                mario.Draw(spriteBatch);
            }
        }
    }
    #endregion
    #region ScreenFive
    class ScreenFive : Screen
    {
        public BrickManager Brickmanager;
        public ItemManager Itemmanager;
        public EnemyManager Enemymanager;
        public int SpecialBricksCount;
        public ScreenFive(ContentManager Content, Mario mario)
            : base(Content, mario)
        {
            Brickmanager = new BrickManager(Content);
            Itemmanager = new ItemManager(Content);
            Enemymanager = new EnemyManager(Content);
            SpecialBricksCount = 0;
            LoadBricks();
            LoadItems();
            LoadEnemies();
        }
        public void LoadBricks()
        {
            for (int i = 0; i <= 800 / 24; i++)
            {
                if (i <= 10 || i > 14)
                {
                    Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 432), Content, -1));
                    Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 456), Content, -1));
                }
                if (i >= 5 && i <= 8)
                    Brickmanager.AddBrick(new stairs(Content.Load<Texture2D>("stairs"), new Vector2(i * 30 - 6, 403), Content, -1));

                if (i >= 6 && i <= 8)
                    Brickmanager.AddBrick(new stairs(Content.Load<Texture2D>("stairs"), new Vector2(i * 30 - 6, 373), Content, -1));
                if (i >= 7 && i <= 8)
                    Brickmanager.AddBrick(new stairs(Content.Load<Texture2D>("stairs"), new Vector2(i * 30 - 6, 343), Content, -1));
                if (i == 8)
                    Brickmanager.AddBrick(new stairs(Content.Load<Texture2D>("stairs"), new Vector2(i * 30 - 6, 313), Content, -1));
                if (i >= 12 && i <= 15)
                    Brickmanager.AddBrick(new stairs(Content.Load<Texture2D>("stairs"), new Vector2(i * 30, 403), Content, -1));
                if (i >= 13 && i <= 15)
                    Brickmanager.AddBrick(new stairs(Content.Load<Texture2D>("stairs"), new Vector2(i * 30 - 30, 373), Content, -1));
                if (i >= 14 && i <= 15)
                    Brickmanager.AddBrick(new stairs(Content.Load<Texture2D>("stairs"), new Vector2(i * 30 - 60, 343), Content, -1));
                if (i == 15)
                    Brickmanager.AddBrick(new stairs(Content.Load<Texture2D>("stairs"), new Vector2(i * 30 - 90, 313), Content, -1));

            }
        }
        public void LoadItems()
        {
            Itemmanager.AddItem(new Mushroom(Content, new Vector2(420, 239), 42, 41, 1));
            Itemmanager.AddItem(null);
        }
        public void LoadEnemies()
        {
            Enemymanager.AddEnemy(new Spiny(Content, new Vector2(300, 393), 43, 41, 480, 757, 0, 480));
        }
        public override void Update(GameTime gameTime, ref ScreenManager Screenmanager)
        {
            UpdateMarioState();
            if (mario != null) mario.Update(gameTime, ref Screenmanager, ref mario);
            Brickmanager.Update(gameTime);
            Itemmanager.Update(gameTime);
            Enemymanager.Update(gameTime);
            Collision.CollideBricks(gameTime, ref mario, ref Brickmanager, ref Itemmanager, Content, ref  Screenmanager);
            Collision.CollideItems(gameTime, ref mario, ref Itemmanager, Content);
            Collision.CollideEnemies(gameTime, ref mario, ref Enemymanager, Content);
            Collision.EnemyBrick(gameTime, ref Brickmanager, ref Enemymanager, Content);
            Collision.BulletBrick(gameTime, ref mario, ref Brickmanager, ref Enemymanager);
            Collision.CollidEnemyVSDuck(gameTime, ref Enemymanager, Content);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 1; i <= 3; i++)
            {
                if (i % 2 == 1) spriteBatch.Draw(Content.Load<Texture2D>("Cloud1"), new Vector2(150 * i, 100), Color.White);
                else spriteBatch.Draw(Content.Load<Texture2D>("Cloud2"), new Vector2(150 * i, 100), Color.White);
            }
            spriteBatch.Draw(Content.Load<Texture2D>("Grass3"), new Vector2(100, 405), Color.White);
            spriteBatch.Draw(Content.Load<Texture2D>("Grass2"), new Vector2(500, 386), Color.White);
            Brickmanager.Draw(spriteBatch);
            Itemmanager.Draw(spriteBatch);
            Enemymanager.Draw(spriteBatch);
            if (mario != null)
            {
                spriteBatch.Draw(Content.Load<Texture2D>("MarioLives"), new Vector2(20, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.Lives.ToString(), new Vector2(112, 29), Color.White);
                mario.Draw(spriteBatch);
                spriteBatch.Draw(Content.Load<Texture2D>("count coins"), new Vector2(200, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.num_coins.ToString(), new Vector2(280, 29), Color.White);
                mario.Draw(spriteBatch);
            }
        }
    }
    #endregion
    #region ScreenSix
    class ScreenSix : Screen
    {
        public BrickManager Brickmanager;
        public ItemManager Itemmanager;
        public EnemyManager Enemymanager;
        public int SpecialBricksCount;
        public ScreenSix(ContentManager Content, Mario mario)
            : base(Content, mario)
        {
            Brickmanager = new BrickManager(Content);
            Itemmanager = new ItemManager(Content);
            Enemymanager = new EnemyManager(Content);
            LoadBricks();
            LoadEnemies();
            SpecialBricksCount = 0;
        }
        public void LoadBricks()
        {
            for (int i = 0; i <= 3; i++)
            {
                Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 432), Content, -1));
                Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 456), Content, -1));
            }
            for (int i = 9; i <= 800 / 24; i++)
            {
                Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 432), Content, -1));
                Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 456), Content, -1));
            }

            Brickmanager.AddBrick(new Pipe(Content.Load<Texture2D>("Pipe1"), new Vector2(216, 279), Content, -1));
            Brickmanager.AddBrick(new Pipe(Content.Load<Texture2D>("Pipe1"), new Vector2(520, 279), Content, -1));
        }
        public void LoadEnemies()
        {
            Enemymanager.AddEnemy(new Gumba(Content, new Vector2(285, 392), 44, 41, 285, 480, 0, 480));
            Enemymanager.AddEnemy(new Plant(Content, new Vector2(230, 270), 44, 56, 0, 800, 223, 345));
            Enemymanager.AddEnemy(new Plant(Content, new Vector2(534, 270), 44, 56, 0, 800, 223, 345));
            Enemymanager.AddEnemy(new Duck(Content, new Vector2(500, 374), 42, 60, 0, 800, 0, 480));
        }
        public override void Update(GameTime gameTime, ref ScreenManager Screenmanager)
        {
            UpdateMarioState();
            if (mario != null) mario.Update(gameTime, ref Screenmanager, ref mario);
            Brickmanager.Update(gameTime);
            Itemmanager.Update(gameTime);
            Enemymanager.Update(gameTime);
            Collision.CollideBricks(gameTime, ref mario, ref Brickmanager, ref Itemmanager, Content, ref  Screenmanager);
            Collision.CollideItems(gameTime, ref mario, ref Itemmanager, Content);
            Collision.CollideEnemies(gameTime, ref mario, ref Enemymanager, Content);
            Collision.EnemyBrick(gameTime, ref Brickmanager, ref Enemymanager, Content);
            Collision.BulletBrick(gameTime, ref mario, ref Brickmanager, ref Enemymanager);
            Collision.CollidEnemyVSDuck(gameTime, ref Enemymanager, Content);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 1; i <= 3; i++)
            {
                if (i % 2 == 1) spriteBatch.Draw(Content.Load<Texture2D>("Cloud1"), new Vector2(150 * i, 100), Color.White);
                else spriteBatch.Draw(Content.Load<Texture2D>("Cloud2"), new Vector2(150 * i, 100), Color.White);
            }
            spriteBatch.Draw(Content.Load<Texture2D>("Grass4"), new Vector2(300, 410), Color.White);
            spriteBatch.Draw(Content.Load<Texture2D>("Grass4"), new Vector2(500, 410), Color.White);
            Enemymanager.Draw(spriteBatch);
            Brickmanager.Draw(spriteBatch);
            Itemmanager.Draw(spriteBatch);
            if (mario != null)
            {
                spriteBatch.Draw(Content.Load<Texture2D>("MarioLives"), new Vector2(20, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.Lives.ToString(), new Vector2(112, 29), Color.White);
                mario.Draw(spriteBatch);
                spriteBatch.Draw(Content.Load<Texture2D>("count coins"), new Vector2(200, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.num_coins.ToString(), new Vector2(280, 29), Color.White);
                mario.Draw(spriteBatch);
            }
        }
    }
    #endregion
    #region ScreenSeven
    class ScreenSeven : Screen
    {
        public BrickManager Brickmanager;
        public ItemManager Itemmanager;
        public EnemyManager Enemymanager;
        int specialbricks;
        public ScreenSeven(ContentManager Content, Mario mario)
            : base(Content, mario)
        {
            Brickmanager = new BrickManager(Content);
            Itemmanager = new ItemManager(Content);
            Enemymanager = new EnemyManager(Content);
            LoadBricks();
            LoadEnemies();
            LoadItems();
            specialbricks = 0;
        }
        public void LoadItems()
        {
            Itemmanager.AddItem(null);
        }
        public void LoadBricks()
        {
            for (int i = 0; i <= 800 / 24; i++)
            {
                Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 432), Content, -1));
                Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 456), Content, -1));
            }
            for (int i = 6; i <= 13; i++)
            {
                if (i == 12)
                {
                    Brickmanager.AddBrick(new SpecialBrick(Content.Load<Texture2D>("Brick4"), new Vector2(i * 35, 216), Content, specialbricks));
                    continue;
                }
                Brickmanager.AddBrick(new NormalBrick(Content.Load<Texture2D>("Brick3"), new Vector2(i * 35, 216), Content, -1));

            }
            Brickmanager.AddBrick(new Pipe(Content.Load<Texture2D>("pipe"), new Vector2(100, 334), Content, -1));
            Brickmanager.AddBrick(new Pipe(Content.Load<Texture2D>("pipe"), new Vector2(660, 334), Content, -1));

        }
        public void LoadEnemies()
        {
            Enemymanager.AddEnemy(new Gumba(Content, new Vector2(150, 387), 44, 41, 0, 800, 0, 480));
            Enemymanager.AddEnemy(new Duck(Content, new Vector2(520, 372), 42, 60, 0, 800, 0, 480));
            Enemymanager.AddEnemy(new Plant(Content, new Vector2(690, 277), 44, 56, 0, 800, 277, 400));
        }
        public override void Update(GameTime gameTime, ref ScreenManager Screenmanager)
        {
            UpdateMarioState();
            if (mario != null) mario.Update(gameTime, ref Screenmanager, ref mario);
            Brickmanager.Update(gameTime);
            Itemmanager.Update(gameTime);
            Enemymanager.Update(gameTime);
            Collision.EnemyBrick(gameTime, ref Brickmanager, ref Enemymanager, Content);
            Collision.BulletBrick(gameTime, ref mario, ref Brickmanager, ref Enemymanager);
            Collision.CollideBricks(gameTime, ref mario, ref Brickmanager, ref Itemmanager, Content, ref  Screenmanager);
            Collision.CollideItems(gameTime, ref mario, ref Itemmanager, Content);
            Collision.CollideEnemies(gameTime, ref mario, ref Enemymanager, Content);
            Collision.CollidEnemyVSDuck(gameTime, ref Enemymanager, Content);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 1; i <= 3; i++)
            {
                if (i % 2 == 1) spriteBatch.Draw(Content.Load<Texture2D>("Cloud1"), new Vector2(150 * i, 100), Color.White);
                else spriteBatch.Draw(Content.Load<Texture2D>("Cloud2"), new Vector2(150 * i, 100), Color.White);
            }
            spriteBatch.Draw(Content.Load<Texture2D>("Grass4"), new Vector2(300, 410), Color.White);
            spriteBatch.Draw(Content.Load<Texture2D>("Grass4"), new Vector2(500, 410), Color.White);
            Enemymanager.Draw(spriteBatch);
            Brickmanager.Draw(spriteBatch);
            Itemmanager.Draw(spriteBatch);
            if (mario != null)
            {
                spriteBatch.Draw(Content.Load<Texture2D>("MarioLives"), new Vector2(20, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.Lives.ToString(), new Vector2(112, 29), Color.White);
                mario.Draw(spriteBatch);
                spriteBatch.Draw(Content.Load<Texture2D>("count coins"), new Vector2(200, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.num_coins.ToString(), new Vector2(280, 29), Color.White);
                mario.Draw(spriteBatch);
            }
        }
    }
    #endregion
    #region ScreenEight
    class ScreenEight : Screen
    {
        public BrickManager Brickmanager;
        public ItemManager Itemmanager;
        public EnemyManager Enemymanager;
        public int SpecialBricksCount;
        public ScreenEight(ContentManager Content, Mario mario)
            : base(Content, mario)
        {
            Brickmanager = new BrickManager(Content);
            Itemmanager = new ItemManager(Content);
            Enemymanager = new EnemyManager(Content);
            SpecialBricksCount = 0;
            LoadBricks();
            LoadItems();
            LoadEnemies();
        }
        public void LoadBricks()
        {
            for (int i = 0; i <= 800 / 24; i++)
            {
                Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 432), Content, -1));
                Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 456), Content, -1));
            }
            Brickmanager.AddBrick(new BillBlaster(Content.Load<Texture2D>("BillBlaster"), new Vector2(550, 367), Content, -1));
            for (int i = 1; i <= 800 / 120; i++) Brickmanager.AddBrick(new BillBlaster(Content.Load<Texture2D>("BillBlaster"), new Vector2(i * 120, 0), Content, -1));
        }
        public void LoadItems()
        {

        }
        public void LoadEnemies()
        {
            Enemymanager.AddEnemy(new BillBlasterBullet(Content, new Vector2(515, 365), 37, 27, -1000, 515, 1000, -1000, -4, 0));
            for (int i = 1; i <= 800 / 120; i++) Enemymanager.AddEnemy(new BillBlasterBullet(Content, new Vector2(i * 120, 65), 37, 27, -1000, 515, 65, -1000, 0, 20));
        }
        public override void Update(GameTime gameTime, ref ScreenManager Screenmanager)
        {
            UpdateMarioState();
            if (mario != null) mario.Update(gameTime, ref Screenmanager, ref mario);
            Brickmanager.Update(gameTime);
            Itemmanager.Update(gameTime);
            Enemymanager.Update(gameTime);
            Collision.CollideBricks(gameTime, ref mario, ref Brickmanager, ref Itemmanager, Content, ref  Screenmanager);
            Collision.CollideItems(gameTime, ref mario, ref Itemmanager, Content);
            Collision.CollideEnemies(gameTime, ref mario, ref Enemymanager, Content);
            Collision.EnemyBrick(gameTime, ref Brickmanager, ref Enemymanager, Content);
            Collision.BulletBrick(gameTime, ref mario, ref Brickmanager, ref Enemymanager);
            Collision.CollidEnemyVSDuck(gameTime, ref Enemymanager, Content);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 1; i <= 3; i++)
            {
                if (i % 2 == 1) spriteBatch.Draw(Content.Load<Texture2D>("Cloud1"), new Vector2(150 * i, 100), Color.White);
                else spriteBatch.Draw(Content.Load<Texture2D>("Cloud2"), new Vector2(150 * i, 100), Color.White);
            }
            spriteBatch.Draw(Content.Load<Texture2D>("Grass3"), new Vector2(100, 405), Color.White);
            spriteBatch.Draw(Content.Load<Texture2D>("Grass2"), new Vector2(500, 386), Color.White);
            Brickmanager.Draw(spriteBatch);
            Itemmanager.Draw(spriteBatch);
            Enemymanager.Draw(spriteBatch);
            if (mario != null)
            {
                spriteBatch.Draw(Content.Load<Texture2D>("MarioLives"), new Vector2(20, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.Lives.ToString(), new Vector2(112, 29), Color.White);
                mario.Draw(spriteBatch);
                spriteBatch.Draw(Content.Load<Texture2D>("count coins"), new Vector2(200, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.num_coins.ToString(), new Vector2(280, 29), Color.White);
                mario.Draw(spriteBatch);
            }
        }
    }
    #endregion
    #region ScreenNine
    class ScreenNine : Screen
    {
        public BrickManager Brickmanager;
        public ItemManager Itemmanager;
        public EnemyManager Enemymanager;
        public int SpecialBricksCount;
        public int force1 = 1;
        public int force2 = 1;
        public ScreenNine(ContentManager Content, Mario mario)
            : base(Content, mario)
        {
            Brickmanager = new BrickManager(Content);
            Itemmanager = new ItemManager(Content);
            Enemymanager = new EnemyManager(Content);
            LoadBricks();
            LoadItems();
            SpecialBricksCount = 0;
        }
        public void LoadItems()
        {
            Itemmanager.AddItem(null);
        }
        public void LoadBricks()
        {
            for (int i = 0; i <= 800 / 24; i++)
            {
                if (i <= 5 || i > 26)
                {

                    Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 432), Content, -1));
                    Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 456), Content, -1));
                }
                if (i==3)
                    Brickmanager.AddBrick(new INVBrick(Content.Load<Texture2D>("Brick4"), new Vector2(150, 300), Content, 0));

            }
            Brickmanager.AddBrick(new Escalator(Content.Load<Texture2D>("Escalator"), new Vector2(240, 250), Content, -1));
            Brickmanager.AddBrick(new Escalator(Content.Load<Texture2D>("Escalator"), new Vector2(340, 180), Content, -1));
        }
        public override void Update(GameTime gameTime, ref ScreenManager Screenmanager)
        {
            UpdateMarioState();
            for (int i = 0; i < Brickmanager.Bricks.Count; i++)
            {
                if (Brickmanager.Bricks[i].GetType() == typeof(Escalator))
                {
                    int x = Brickmanager.Bricks[i].Bounds.X;
                    int y = Brickmanager.Bricks[i].Bounds.Y;
                    Brickmanager.Bricks.RemoveAt(i);
                    if (y == 180)
                    {
                        x += force2;
                        if (mario.Mariostate.Position.Y <= y && mario.Mariostate.Position.X <= 623 && mario.Mariostate.Position.X >= x - mario.Mariostate.Width)
                            mario.Mariostate.Position.X += force2;
                        if (x > 450 || x < 50)
                            force2 *= -1;
                    }
                    /*
                    if (y == 250)
                    {
                        x += force1;
                        if (mario.Mariostate.Position.Y < y && mario.Mariostate.Position.Y > 150 && mario.Mariostate.Position.X <= 623 && mario.Mariostate.Position.X >= x - mario.Mariostate.Width)
                            mario.Mariostate.Position.X += force1;
                        if (x > 250 || x < 200)
                            force1 *= -1;
                    }*/
                    Brickmanager.AddBrick(new Escalator(Content.Load<Texture2D>("Escalator"), new Vector2(x, y), Content, -1));
                }
            }
            Collision.CollideBricks(gameTime, ref mario, ref Brickmanager, ref Itemmanager, Content, ref  Screenmanager);
            if (mario != null) mario.Update(gameTime, ref Screenmanager, ref mario);

            Brickmanager.Update(gameTime);

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 1; i <= 3; i++)
            {
                if (i % 2 == 1) spriteBatch.Draw(Content.Load<Texture2D>("Cloud1"), new Vector2(150 * i, 100), Color.White);
                else spriteBatch.Draw(Content.Load<Texture2D>("Cloud2"), new Vector2(150 * i, 100), Color.White);
            }
            Brickmanager.Draw(spriteBatch);
            if (mario != null)
            {
                spriteBatch.Draw(Content.Load<Texture2D>("MarioLives"), new Vector2(20, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.Lives.ToString(), new Vector2(112, 29), Color.White);
                mario.Draw(spriteBatch);
                spriteBatch.Draw(Content.Load<Texture2D>("count coins"), new Vector2(200, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.num_coins.ToString(), new Vector2(280, 29), Color.White);
                mario.Draw(spriteBatch);
            }
        }
    }
    #endregion
    #region ScreenTen
    class ScreenTen : Screen
    {
        public BrickManager Brickmanager;
        public ItemManager Itemmanager;
        public int SpecialBricksCount;
        public bool flag = false;
        public ScreenTen(ContentManager Content, Mario mario)
            : base(Content, mario)
        {
            Brickmanager = new BrickManager(Content);

            SpecialBricksCount = 0;
            LoadBricks();
        }
        public void LoadBricks()
        {
            int high = 403;
            for (int i = 0; i <= 800 / 24; i++)
            {

                Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 432), Content, -1));
                Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 456), Content, -1));
                int temp = i;
                bool flag = false;
                while (temp >= 2 && temp <= 9 && i <= 8)
                {
                    Brickmanager.AddBrick(new stairs(Content.Load<Texture2D>("stairs"), new Vector2((temp * 30) - 6, high), Content, -1));
                    temp++;
                    flag = true;
                }
                if (flag)
                    high -= 30;
            }
            Brickmanager.AddBrick(new FLAG(Content.Load<Texture2D>("FLAG"), new Vector2(350, 40), Content, -1));
            Brickmanager.AddBrick(new Caslte(Content.Load<Texture2D>("Castle"), new Vector2(520, 133), Content, -1));

        }
        public override void Update(GameTime gameTime, ref ScreenManager Screenmanager)
        {
            UpdateMarioState();
            if (mario.Mariostate.Position.X >= 340 && mario.Mariostate.Position.Y > 300)
                mario.Mariostate.Position.X++;
            if (mario != null) mario.Update(gameTime, ref Screenmanager, ref mario);

            Brickmanager.Update(gameTime);
            Collision.CollideBricks(gameTime, ref mario, ref Brickmanager, ref Itemmanager, Content, ref  Screenmanager);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 1; i <= 3; i++)
            {
                if (i % 2 == 1) spriteBatch.Draw(Content.Load<Texture2D>("Cloud1"), new Vector2(150 * i, 100), Color.White);
                else spriteBatch.Draw(Content.Load<Texture2D>("Cloud2"), new Vector2(150 * i, 100), Color.White);
            }

            Brickmanager.Draw(spriteBatch);
            if (mario != null)
            {
                spriteBatch.Draw(Content.Load<Texture2D>("MarioLives"), new Vector2(20, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.Lives.ToString(), new Vector2(112, 29), Color.White);
                mario.Draw(spriteBatch);
                spriteBatch.Draw(Content.Load<Texture2D>("count coins"), new Vector2(200, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.num_coins.ToString(), new Vector2(280, 29), Color.White);
                mario.Draw(spriteBatch);
            }
        }
    }
    #endregion
    #region ScreenEleven
    class ScreenEleven : Screen
    {
        public BrickManager Brickmanager;
        public ItemManager Itemmanager;
        public EnemyManager Enemymanager;
        public int SpecialBricksCount;
        public ScreenEleven(ContentManager Content, Mario mario)
            : base(Content, mario)
        {
            Brickmanager = new BrickManager(Content);
            Itemmanager = new ItemManager(Content);
            Enemymanager = new EnemyManager(Content);
            LoadBricks();
            LoadItems();
            SpecialBricksCount = 0;
        }
        public void LoadBricks()
        {
            for (int i = 0; i <= 800 / 24; i++)
            {
                if (i <= 1)
                {
                    int z = 0;
                    while (z <= 800)
                    {
                        Brickmanager.AddBrick(new stairs(Content.Load<Texture2D>("stairs"), new Vector2(i * 24, z), Content, -1));
                        z += 24;
                    }
                }
                if (i > 5 && i <= 25)
                {
                    int z = 337;
                    while (z <= 800)
                    {
                        Brickmanager.AddBrick(new stairs(Content.Load<Texture2D>("stairs"), new Vector2(i * 24, z), Content, -1));
                        z += 24;
                    }
                }
                Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 432), Content, -1));
                Brickmanager.AddBrick(new PlatformBrick(Content.Load<Texture2D>("Brick2"), new Vector2(i * 24, 456), Content, -1));
            }

            Brickmanager.AddBrick(new Pipe(Content.Load<Texture2D>("Pipe1"), new Vector2(720, 279), Content, -1));
        }
        public void LoadItems()
        {
            for (int i = 0; i <= 13; i++)
            {

                Itemmanager.AddItem(new Coins(Content, new Vector2(2 * 35, i * 30), 22, 34, 2));
            }
            Itemmanager.AddItem(new Coins(Content, new Vector2(3 * 35, 13 * 30), 22, 34, 2));
            Itemmanager.AddItem(new Coins(Content, new Vector2(3 * 35, 12 * 30), 22, 34, 2));
            Itemmanager.AddItem(new Coins(Content, new Vector2(3 * 35, 11 * 30), 22, 34, 2));
            Itemmanager.AddItem(new Coins(Content, new Vector2(3 * 35, 10 * 30), 22, 34, 2));
            for (int i = 3; i <= 17; i++)
            {

                Itemmanager.AddItem(new Coins(Content, new Vector2(i * 35, 10 * 30), 22, 34, 2));
                Itemmanager.AddItem(new Coins(Content, new Vector2(i * 35, 9 * 30), 22, 34, 2));
                Itemmanager.AddItem(new Coins(Content, new Vector2(i * 35, 8 * 30), 22, 34, 2));
            }
        }
        public override void Update(GameTime gameTime, ref ScreenManager Screenmanager)
        {
            if (mario != null) mario.Update(gameTime, ref Screenmanager, ref mario);
            Brickmanager.Update(gameTime);
            Itemmanager.Update(gameTime);
            Collision.CollideBricks(gameTime, ref mario, ref Brickmanager, ref Itemmanager, Content, ref Screenmanager);
            Collision.CollideItems(gameTime, ref mario, ref Itemmanager, Content);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {

            if (mario != null) mario.Draw(spriteBatch);
            Brickmanager.Draw(spriteBatch);
            Itemmanager.Draw(spriteBatch);
            if (mario != null)
            {
                spriteBatch.Draw(Content.Load<Texture2D>("MarioLives"), new Vector2(20, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.Lives.ToString(), new Vector2(112, 29), Color.White);
                mario.Draw(spriteBatch);
                spriteBatch.Draw(Content.Load<Texture2D>("count coins"), new Vector2(200, 20), Color.White);
                spriteBatch.DrawString(spriteFont, mario.num_coins.ToString(), new Vector2(280, 29), Color.White);
                mario.Draw(spriteBatch);
            }
        }
    }
    #endregion
}