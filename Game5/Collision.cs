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
    class Collision
    {
        #region MarioBrickCollision
        public static void CollideBricks(GameTime gameTIme, ref Mario mario, ref BrickManager Brickmanager, ref ItemManager Itemmanager, ContentManager Content, ref ScreenManager Screenmanager)
        {
            if (mario == null) return;
            if (mario.Mariostate.LifeSpan < 2) return;
            string Landing = mario.Mariostate.Landing, LandingBoolean = "False";
            for (int i = 0; i < Brickmanager.Bricks.Count; i++)
            {
                if (Brickmanager.Bricks[i] == null) continue;
                if (Brickmanager.Bricks[i].LifeSpan == 0)
                {
                    Brickmanager.Bricks[i] = null;
                    continue;
                }
                if (mario.Bounds.Intersects(Brickmanager.Bricks[i].Bounds))
                {
                    if (Brickmanager.Bricks[i].GetType() == typeof(NormalBrick) && Brickmanager.Bricks[i].LifeSpan == 2)
                    {
                        if (mario.Bounds.Y > Brickmanager.Bricks[i].Bounds.Y)
                        {
                            mario.Mariostate.Position.Y = Brickmanager.Bricks[i]._Texture.Height + Brickmanager.Bricks[i].Position.Y;
                            mario.Mariostate.Gravity = 1;
                            if (mario.Mariostate.GetType() != typeof(MarioSmall))
                            {
                                Brickmanager.Bricks[i].Destroy();
                                Content.Load<SoundEffect>("BrickSmash").Play();
                            }
                            else
                            {
                                Brickmanager.Bricks[i].LoadAnimation();
                                Content.Load<SoundEffect>("Bump").Play();
                            }
                        }
                        else if (mario.Mariostate.Gravity >= 0)
                        {
                            Landing = "Normal";
                            LandingBoolean = "True";
                            if (mario.Mariostate.Jumping == "True")
                            {
                                mario.Mariostate.Jumping = "False";
                                mario.Mariostate.Position.Y = Brickmanager.Bricks[i].Position.Y - mario.Mariostate.Height + 3;
                            }
                        }
                    }
                    else if (Brickmanager.Bricks[i].GetType() == typeof(Caslte))
                    {
                        if (mario.Mariostate.Position.X >= 600 && mario.Mariostate.Position.X < 690)
                        {
                            mario.Mariostate.Position.X = 800;
                        }
                    }
                    else if (Brickmanager.Bricks[i].GetType() == typeof(Escalator))
                    {
                        if (mario.Bounds.Y > Brickmanager.Bricks[i].Position.Y - 2)
                        {
                            mario.Mariostate.Position.Y = Brickmanager.Bricks[i]._Texture.Height + Brickmanager.Bricks[i].Position.Y;
                            mario.Mariostate.Gravity = 1;
                        }
                        else
                        {
                            LandingBoolean = "True";
                            if (mario.Mariostate.Jumping == "True")
                            {
                                mario.Mariostate.Jumping = "False";
                                mario.Mariostate.Position.Y = Brickmanager.Bricks[i].Position.Y - mario.Mariostate.Height + 1;
                            }
                        }
                    }
                    else if (Brickmanager.Bricks[i].GetType() == typeof(FLAG))
                    {
                        mario.Mariostate.Gravity = 5;

                    }
                    else if (Brickmanager.Bricks[i].GetType() == typeof(SpecialBrick))
                    {
                        if (mario.Bounds.Y > Brickmanager.Bricks[i].Bounds.Y)
                        {
                            mario.Mariostate.Position.Y = Brickmanager.Bricks[i]._Texture.Height + Brickmanager.Bricks[i].Position.Y;
                            mario.Mariostate.Gravity = 1;
                            if (Itemmanager.Items[Brickmanager.Bricks[i].Index] != null)
                            {
                                if (Itemmanager.Items[Brickmanager.Bricks[i].Index].LifeSpan == 1)
                                {
                                    Itemmanager.Items[Brickmanager.Bricks[i].Index].LifeSpan = 2;
                                    Content.Load<SoundEffect>("PowerUpAppears").Play();
                                }
                            }
                            if (Brickmanager.Bricks[i].LifeSpan == 2) Brickmanager.Bricks[i].Destroy();
                        }
                        else if (mario.Mariostate.Gravity >= 0)
                        {
                            Landing = "Special";
                            LandingBoolean = "True";
                            if (mario.Mariostate.Jumping == "True")
                            {
                                mario.Mariostate.Jumping = "False";
                                mario.Mariostate.Position.Y = Brickmanager.Bricks[i].Position.Y - mario.Mariostate.Height + 3;
                            }
                        }
                    }
                    else if (Brickmanager.Bricks[i].GetType() == typeof(INVBrick))
                    {
                    
                        if (mario.Bounds.Y > Brickmanager.Bricks[i].Bounds.Y&&mario.Mariostate.Gravity<0)
                        {
                            mario.Mariostate.Position.Y = Brickmanager.Bricks[i]._Texture.Height + Brickmanager.Bricks[i].Position.Y;
                            mario.Mariostate.Gravity = 1;
                            if (Itemmanager.Items[Brickmanager.Bricks[i].Index] != null)
                            {
                                if (Itemmanager.Items[Brickmanager.Bricks[i].Index].LifeSpan == 1)
                                {
                                    Itemmanager.Items[Brickmanager.Bricks[i].Index].LifeSpan = 2;
                                    Content.Load<SoundEffect>("PowerUpAppears").Play();
                                }
                            }
                            if (Brickmanager.Bricks[i].LifeSpan == 2) Brickmanager.Bricks[i].Destroy();
                        }
                        else if (mario.Mariostate.Gravity >= 0&& Brickmanager.Bricks[i].LifeSpan == 1)
                        {
                            Landing = "Special";
                            LandingBoolean = "True";
                            if (mario.Mariostate.Jumping == "True")
                            {
                                mario.Mariostate.Jumping = "False";
                                mario.Mariostate.Position.Y = Brickmanager.Bricks[i].Position.Y - mario.Mariostate.Height + 3;
                            }
                        }
                    }
                    else if (Brickmanager.Bricks[i].GetType() == typeof(PlatformBrick))
                    {
                        Landing = "Platform";
                        LandingBoolean = "True";
                        mario.Mariostate.Position.Y = Brickmanager.Bricks[i].Position.Y - mario.Mariostate.Height + 1;
                        if (mario.Mariostate.Jumping == "True")
                        {
                            mario.Mariostate.Jumping = "False";
                        }
                    }
                    else if (Brickmanager.Bricks[i].GetType() == typeof(Pipe))
                    {
                        int MostRight = (int)Brickmanager.Bricks[i].Position.X + Brickmanager.Bricks[i]._Texture.Width;
                        int Length = (int)mario.Mariostate.Position.X + mario.Mariostate.Width;
                        if (mario.Mariostate.Position.Y <= Brickmanager.Bricks[i].Position.Y && mario.Mariostate.Gravity >= 0)
                        {
                            Landing = "Pipe";
                            LandingBoolean = "True";
                            if (Screenmanager.CurrentScreenIndex == 3 && Keyboard.GetState().IsKeyDown(Keys.Down) && mario.Mariostate.Position.X <= 300)
                            {

                                Screenmanager.Forward();
                                mario.Mariostate.Position.X = 25;
                                mario.Mariostate.Position.Y = 0;

                            }
                            else if (Screenmanager.CurrentScreenIndex == 4 && Keyboard.GetState().IsKeyDown(Keys.Down))
                            {
                                Screenmanager.Backward();
                                mario.Mariostate.Position.X = 620;
                            }
                                if (mario.Mariostate.Jumping == "True")
                            {
                                mario.Mariostate.Jumping = "False";
                                mario.Mariostate.Position.Y = Brickmanager.Bricks[i].Position.Y - mario.Mariostate.Height + 1;
                            }
                        }
                        else if (Length < MostRight)
                        {
                            mario.Mariostate.Moving = "False";
                            mario.Mariostate.Position.X = Brickmanager.Bricks[i].Position.X - mario.Mariostate.Width;
                        }
                        else
                        {
                            mario.Mariostate.Moving = "False";
                            mario.Mariostate.Position.X = MostRight;
                        }
                    }
                    else if (Brickmanager.Bricks[i].GetType() == typeof(BillBlaster))
                    {
                        if (Landing != "BillBlaster")
                        {
                            if (mario.Mariostate.Gravity >= 0 && mario.Mariostate.Jumping == "True")
                            {
                                Landing = "BillBlaster";
                                LandingBoolean = "True";
                                mario.Mariostate.Position.Y = Brickmanager.Bricks[i].Position.Y - mario.Mariostate.Height + 1;
                                mario.Mariostate.Jumping = "False";
                            }
                            else if (mario.Mariostate.Position.X < Brickmanager.Bricks[i].Position.X)
                            {
                                mario.Mariostate.Position.X = Brickmanager.Bricks[i].Position.X - mario.Mariostate.Width;
                                mario.Mariostate.Moving = "False";
                            }
                            else
                            {
                                mario.Mariostate.Position.X = Brickmanager.Bricks[i].Position.X + Brickmanager.Bricks[i]._Texture.Width;
                                mario.Mariostate.Moving = "False";
                            }
                        }
                        else LandingBoolean = "True";
                    }
                    else if (Brickmanager.Bricks[i].GetType() == typeof(stairs))
                    {

                        if (mario.Mariostate.GetType() == typeof(MarioSmall))
                        {
                            if (mario.Mariostate.Position.Y + 20 <= Brickmanager.Bricks[i].Position.Y && mario.Mariostate.Gravity >= 0)
                            {
                                LandingBoolean = "True";
                                if (mario.Mariostate.Jumping == "True")
                                {
                                    mario.Mariostate.Jumping = "False";
                                    mario.Mariostate.Position.Y = Brickmanager.Bricks[i].Position.Y - mario.Mariostate.Height + 1;
                                }
                            }
                            else if (mario.Mariostate.Position.X <= Brickmanager.Bricks[i].Position.X)
                            {
                                mario.Mariostate.Moving = "False";
                                mario.Mariostate.Position.X = Brickmanager.Bricks[i].Position.X - mario.Mariostate.Width;
                            }
                            else
                            {
                                mario.Mariostate.Moving = "False";
                                mario.Mariostate.Position.X = Brickmanager.Bricks[i].Position.X + Brickmanager.Bricks[i]._Texture.Width;
                            }
                        }
                        else
                        {
                            if (mario.Mariostate.Position.Y + 50 <= Brickmanager.Bricks[i].Position.Y && mario.Mariostate.Gravity >= 0)
                            {
                                LandingBoolean = "True";
                                if (mario.Mariostate.Jumping == "True")
                                {
                                    mario.Mariostate.Jumping = "False";
                                    mario.Mariostate.Position.Y = Brickmanager.Bricks[i].Position.Y - mario.Mariostate.Height + 1;
                                }
                            }
                            else if (mario.Mariostate.Position.X <= Brickmanager.Bricks[i].Position.X)
                            {
                                mario.Mariostate.Moving = "False";
                                mario.Mariostate.Position.X = Brickmanager.Bricks[i].Position.X - mario.Mariostate.Width;
                            }
                            else
                            {
                                mario.Mariostate.Moving = "False";
                                mario.Mariostate.Position.X = Brickmanager.Bricks[i].Position.X + Brickmanager.Bricks[i]._Texture.Width;
                            }
                        }

                    }

                }
            }
            mario.Mariostate.Landing = Landing;
            if (LandingBoolean == "False")
            {
                if (mario.Mariostate.Jumping == "False")
                {
                    mario.Mariostate.Jumping = "True";
                    mario.Mariostate.Landing = "";
                    mario.Mariostate.Gravity = 0;
                }
            }
        }
        #endregion
        #region MarioItemCollision
        public static void CollideItems(GameTime gameTIme, ref Mario mario, ref ItemManager Itemmanager, ContentManager Content)
        {
            if (mario == null) return;
            if (mario.Mariostate.LifeSpan < 2) return;
            for (int i = 0; i < Itemmanager.Items.Count; i++)
            {
                if (Itemmanager.Items[i] == null) continue;
                if (Itemmanager.Items[i].LifeSpan == 0)
                {
                    Itemmanager.Items[i] = null;
                    continue;
                }
                if (mario.Bounds.Intersects(Itemmanager.Items[i].Bounds))
                {
                    if (Itemmanager.Items[i].GetType() == typeof(Mushroom) && Itemmanager.Items[i].LifeSpan == 2)
                    {
                        string Direction = mario.Mariostate.Direction;
                        if (mario.Mariostate.GetType() == typeof(MarioSmall))
                        {
                            mario.Mariostate = new MarioBig(Content, new Vector2(mario.Bounds.X, mario.Bounds.Y - 38), 45, 78);
                            mario.Mariostate.Direction = Direction;
                            Itemmanager.Items[i] = null;
                            Content.Load<SoundEffect>("PowerUp").Play();
                        }
                    }
                    else if (Itemmanager.Items[i].GetType() == typeof(FireFlower) && Itemmanager.Items[i].LifeSpan == 2)
                    {
                        string Direction = mario.Mariostate.Direction;
                        if (mario.Mariostate.GetType() == typeof(MarioBig))
                        {
                            mario.Mariostate = new MarioFire(Content, new Vector2(mario.Bounds.X, mario.Bounds.Y - 2), 45, 78);
                            mario.Mariostate.Direction = Direction;
                            Itemmanager.Items[i] = null;
                            Content.Load<SoundEffect>("PowerUp").Play();
                        }
                        else if (mario.Mariostate.GetType() == typeof(MarioSmall))
                        {
                            mario.Mariostate = new MarioFire(Content, new Vector2(mario.Bounds.X, mario.Bounds.Y - 38), 45, 78);
                            mario.Mariostate.Direction = Direction;
                            Itemmanager.Items[i] = null;
                            Content.Load<SoundEffect>("PowerUp").Play();
                        }
                        
                    }
                    else if (Itemmanager.Items[i].GetType() == typeof(Coins))
                    {
                        mario.num_coins++;
                        if (mario.num_coins == 5)
                        {
                            mario.num_coins = 0;
                            mario.Lives++;
                        }
                        Itemmanager.Items[i] = null;
                        Content.Load<SoundEffect>("Coin").Play();

                    }
                }
            }
        }
        #endregion
        #region MarioEnemyCollision
        public static void CollideEnemies(GameTime gameTIme, ref Mario mario, ref EnemyManager Enemymanager, ContentManager Content)
        {
            for (int i = 0; i < Enemymanager.Enemies.Count; i++)
            {
                if (mario == null) return;
                if (mario.Mariostate.LifeSpan < 2) return;
                if (Enemymanager.Enemies[i] == null) continue;
                if (Enemymanager.Enemies[i].LifeSpan == 0)
                {
                    Enemymanager.Enemies[i] = null;
                    continue;
                }
                if (mario.Bounds.Intersects(Enemymanager.Enemies[i].Bounds))
                {
                    if (Enemymanager.Enemies[i].GetType() == typeof(Gumba) && Enemymanager.Enemies[i].LifeSpan == 2)
                    {
                        if (mario.Mariostate.Jumping == "True" && mario.Mariostate.Position.Y < Enemymanager.Enemies[i].Position.Y)
                        {
                            Enemymanager.Enemies[i].Destroy();
                            int TempGravity = mario.Mariostate.Gravity;
                            mario.Mariostate.Gravity = -TempGravity + (TempGravity / 2);
                            mario.Mariostate.Position.Y -= 10;
                            Content.Load<SoundEffect>("Kick").Play();
                        }
                        else if (mario.Mariostate.Vibrating == false)
                        {
                            string Direction = mario.Mariostate.Direction;
                            if (mario.Mariostate.GetType() == typeof(MarioFire))
                            {
                                mario.Mariostate = new MarioBig(Content, new Vector2(mario.Bounds.X, mario.Bounds.Y), 45, 78);
                                mario.Mariostate.Direction = Direction;
                                mario.Mariostate.Vibrating = true;
                                Content.Load<SoundEffect>("Shrink").Play();
                            }
                            else if (mario.Mariostate.GetType() == typeof(MarioBig))
                            {
                                mario.Mariostate = new MarioSmall(Content, new Vector2(mario.Bounds.X, mario.Bounds.Y + 38), 35, 40);
                                mario.Mariostate.Direction = Direction;
                                mario.Mariostate.Vibrating = true;
                                Content.Load<SoundEffect>("Shrink").Play();
                            }
                            else mario.Destroy();
                        }
                    }
                    else if (Enemymanager.Enemies[i].GetType() == typeof(Spiny) || Enemymanager.Enemies[i].GetType() == typeof(BillBlasterBullet))
                    {
                        if (Enemymanager.Enemies[i].GetType() == typeof(BillBlasterBullet) && mario.Mariostate.Landing == "BillBlaster") continue;
                        if (mario.Mariostate.Vibrating == false)
                        {
                            string Direction = mario.Mariostate.Direction;
                            if (mario.Mariostate.GetType() == typeof(MarioFire))
                            {
                                mario.Mariostate = new MarioBig(Content, new Vector2(mario.Bounds.X, mario.Bounds.Y), 45, 78);
                                mario.Mariostate.Direction = Direction;
                                mario.Mariostate.Vibrating = true;
                                Content.Load<SoundEffect>("Shrink").Play();
                            }
                            else if (mario.Mariostate.GetType() == typeof(MarioBig))
                            {
                                mario.Mariostate = new MarioSmall(Content, new Vector2(mario.Bounds.X, mario.Bounds.Y + 38), 35, 40);
                                mario.Mariostate.Direction = Direction;
                                mario.Mariostate.Vibrating = true;
                                Content.Load<SoundEffect>("Shrink").Play();
                            }
                            else mario.Destroy();
                        }
                    }
                    else if (Enemymanager.Enemies[i].GetType() == typeof(Plant))
                    {
                        if (mario.Mariostate.Vibrating == false)
                        {
                            string Direction = mario.Mariostate.Direction;
                            if (mario.Mariostate.GetType() == typeof(MarioFire))
                            {
                                mario.Mariostate = new MarioBig(Content, new Vector2(mario.Bounds.X, mario.Bounds.Y), 45, 78);
                                mario.Mariostate.Direction = Direction;
                                mario.Mariostate.Vibrating = true;
                                Content.Load<SoundEffect>("Shrink").Play();
                            }
                            else if (mario.Mariostate.GetType() == typeof(MarioBig))
                            {
                                mario.Mariostate = new MarioSmall(Content, new Vector2(mario.Bounds.X, mario.Bounds.Y + 38), 35, 40);
                                mario.Mariostate.Direction = Direction;
                                mario.Mariostate.Vibrating = true;
                                Content.Load<SoundEffect>("Shrink").Play();
                            }
                            else mario.Destroy();
                        }
                    }
                    else if (Enemymanager.Enemies[i].GetType() == typeof(Duck))
                    {
                        if (mario.Mariostate.Vibrating == false)
                        {
                            if (mario.Mariostate.Jumping == "True")
                            {
                                int TempGravity = mario.Mariostate.Gravity;
                                mario.Mariostate.Gravity = -TempGravity + (TempGravity / 2);
                                mario.Mariostate.Position.Y -= 10;
                                if (Enemymanager.Enemies[i].LifeSpan > 3) Enemymanager.Enemies[i].LifeSpan = 3;
                                else Enemymanager.Enemies[i] = null;
                                Content.Load<SoundEffect>("Kick").Play();
                            }
                            else if (mario.Mariostate.GetType() == typeof(MarioFire))
                            {
                                if (Enemymanager.Enemies[i].LifeSpan == 3)
                                {

                                    if (mario.Mariostate.Position.X <= Enemymanager.Enemies[i].Position.X)
                                    {
                                        Enemymanager.Enemies[i].LifeSpan = 2;
                                        Enemymanager.Enemies[i].Direction = "Right";
                                        Enemymanager.Enemies[i].Position.X += 7;
                                    }
                                    else
                                    {
                                        Enemymanager.Enemies[i].LifeSpan = 1;
                                        Enemymanager.Enemies[i].Direction = "Left";
                                        Enemymanager.Enemies[i].Position.X -= 7;
                                    }

                                }
                                else
                                {
                                    mario.Mariostate = new MarioBig(Content, new Vector2(mario.Bounds.X, mario.Bounds.Y), 45, 78);
                                    mario.Mariostate.Vibrating = true;
                                    Content.Load<SoundEffect>("Shrink").Play();
                                }

                            }
                            else if (mario.Mariostate.GetType() == typeof(MarioBig))
                            {
                                if (Enemymanager.Enemies[i].LifeSpan == 3)
                                {

                                    if (mario.Mariostate.Position.X <= Enemymanager.Enemies[i].Position.X)
                                    {
                                        Enemymanager.Enemies[i].LifeSpan = 2;
                                        Enemymanager.Enemies[i].Direction = "Right";
                                        Enemymanager.Enemies[i].Position.X += 7;
                                    }
                                    else
                                    {
                                        Enemymanager.Enemies[i].LifeSpan = 1;
                                        Enemymanager.Enemies[i].Direction = "Left";
                                        Enemymanager.Enemies[i].Position.X -= 7;
                                    }
                                }
                                else
                                {
                                    mario.Mariostate = new MarioSmall(Content, new Vector2(mario.Bounds.X, mario.Bounds.Y + 38), 35, 40);
                                    mario.Mariostate.Vibrating = true;
                                    Content.Load<SoundEffect>("Shrink").Play();
                                }
                            }
                            else
                            {

                                if (Enemymanager.Enemies[i].LifeSpan == 3)
                                {

                                    if (mario.Mariostate.Position.X <= Enemymanager.Enemies[i].Position.X)
                                    {
                                        Enemymanager.Enemies[i].LifeSpan = 2;
                                        Enemymanager.Enemies[i].Direction = "Right";
                                        Enemymanager.Enemies[i].Position.X += 7;
                                    }
                                    else
                                    {
                                        Enemymanager.Enemies[i].LifeSpan = 1;
                                        Enemymanager.Enemies[i].Direction = "Left";
                                        Enemymanager.Enemies[i].Position.X -= 7;
                                    }
                                }
                                else
                                    mario.Destroy();
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region BulletBrick
        public static void BulletBrick(GameTime gametime, ref Mario mario, ref BrickManager brickmanager, ref EnemyManager enemymanager)
        {
            if (mario == null) return;
            if (mario.Mariostate.LifeSpan < 2) return;
            if (mario.bullet.released == false) return;

            for (int i = 0; i < brickmanager.Bricks.Count; i++)
            {
                if (brickmanager.Bricks[i] != null)
                {
                    if (mario.bullet.Bounds.Intersects(brickmanager.Bricks[i].Bounds))
                    {
                        if (brickmanager.Bricks[i].GetType() != typeof(PlatformBrick))
                        {

                            mario.bullet.released = false;
                            mario.bullet.timer = 0;
                            //mario.bullet = null;
                            break;
                        }

                    }

                }

            }
            for (int i = 0; i < enemymanager.Enemies.Count; i++)
            {
                if (mario == null) return;
                if (mario.Mariostate.LifeSpan < 2) return;
                if (mario.bullet.released == false) return;
                if (enemymanager.Enemies[i] != null)
                {
                    if (enemymanager.Enemies[i].LifeSpan == 0)
                    {
                        enemymanager.Enemies[i] = null;
                        continue;
                    }
                    if (mario.bullet.Bounds.Intersects(enemymanager.Enemies[i].Bounds))
                    {
                        enemymanager.Enemies[i].Destroy();
                        mario.bullet.released = false;
                        mario.bullet.timer = 0;
                        break;
                    }

                }
            }
        }
        #endregion
        #region EnemyBrick
        public static void EnemyBrick(GameTime gametime, ref BrickManager brickmanager, ref EnemyManager enemymanager, ContentManager contentmanager)
        {
            for (int i = 0; i < enemymanager.Enemies.Count; i++)
            {

                bool Landing = false;

                for (int j = 0; j < brickmanager.Bricks.Count; j++)
                {
                    if (enemymanager.Enemies[i] == null || brickmanager.Bricks[j] == null)
                        continue;
                    if (brickmanager.Bricks[j].LifeSpan == 0)
                    {
                        brickmanager.Bricks[j] = null;
                        continue;
                    }
                    if (enemymanager.Enemies[i].LifeSpan == 0)
                    {
                        enemymanager.Enemies[i] = null;
                        continue;
                    }
                    if (enemymanager.Enemies[i].GetType() != typeof(Plant))
                    {
                        if (enemymanager.Enemies[i].Bounds.Intersects(brickmanager.Bricks[j].Bounds))
                        {
                            enemymanager.Enemies[i].on_floor = true;
                            enemymanager.Enemies[i].Gravity = 0;
                            Landing = true;
                            if (enemymanager.Enemies[i].Bounds.Y < brickmanager.Bricks[j].Bounds.Y)
                            {
                                enemymanager.Enemies[i].Bounds.Y = brickmanager.Bricks[j].Bounds.Y - enemymanager.Enemies[i].Height;
                            }
                            if (brickmanager.Bricks[j].GetType() == typeof(Pipe))
                            {
                                if (enemymanager.Enemies[i].Direction == "Right")
                                {
                                    if (enemymanager.Enemies[i].Position.X + enemymanager.Enemies[i].Width >= brickmanager.Bricks[j].Position.X)
                                    {
                                        enemymanager.Enemies[i].Position.X = brickmanager.Bricks[j].Position.X - enemymanager.Enemies[i].Width;
                                        enemymanager.Enemies[i].Direction = "Left";
                                        continue;

                                    }
                                }
                                else
                                {
                                    if ((enemymanager.Enemies[i].Position.X) <= (brickmanager.Bricks[j].Position.X + brickmanager.Bricks[j]._Texture.Width))
                                    {
                                        enemymanager.Enemies[i].Position.X = brickmanager.Bricks[j].Position.X + brickmanager.Bricks[j]._Texture.Width;
                                        enemymanager.Enemies[i].Direction = "Right";
                                        continue;

                                    }
                                }


                            }
                        }
                        else if (Landing == false)
                        {
                            enemymanager.Enemies[i].on_floor = false;
                            enemymanager.Enemies[i].Gravity = 3;
                        }
                    }

                }
            }
        }

        #endregion
        #region CollidEnemyVSDuck
        public static void CollidEnemyVSDuck(GameTime gameTImeo, ref EnemyManager Enemymanager, ContentManager Content)
        {
            for (int i = 0; i < Enemymanager.Enemies.Count; i++)
            {
                if (Enemymanager.Enemies[i] != null)
                {
                    if (Enemymanager.Enemies[i].GetType() == typeof(Duck))
                    {
                        for (int j = 0; j < Enemymanager.Enemies.Count; j++)
                        {
                            if (Enemymanager.Enemies[j] != null && j != i)
                            {
                                if (Enemymanager.Enemies[j].Bounds.Intersects(Enemymanager.Enemies[i].Bounds))
                                {
                                    if (Enemymanager.Enemies[i].LifeSpan == 1 || Enemymanager.Enemies[i].LifeSpan == 2)
                                    {
                                        Enemymanager.Enemies[j].Destroy();
                                        Enemymanager.Enemies[j] = null;
                                    }

                                }
                            }
                        }
                    }

                }
            }
        }
        #endregion
    }
}
