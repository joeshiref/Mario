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
    #region ScreenManager
    class ScreenManager
    {
        public ContentManager Content;
        public List<Screen> Screens;
        ScreenManager TempScreenManager;
        public int CurrentScreenIndex;
        public Mario mario;
        public ScreenManager(ContentManager Content)
        {
            this.Content = Content;
            mario = new Mario(Content, new Vector2(1, 392));
            Screens = new List<Screen>();
            LoadScreens();
            CurrentScreenIndex = 0;
            TempScreenManager = this;
        }
        public void LoadScreens()
        {
            Screens.Add(new ScreenOne(Content, mario));
            Screens.Add(new ScreenTwo(Content, null));
            Screens.Add(new ScreenThree(Content, null));
            Screens.Add(new ScreenFour(Content, null));
            Screens.Add(new ScreenEleven(Content, null));
            Screens.Add(new ScreenFive(Content, null));
            Screens.Add(new ScreenSix(Content, null));
            Screens.Add(new ScreenSeven(Content, null));
            Screens.Add(new ScreenEight(Content, null));
            Screens.Add(new ScreenNine(Content, null));
            Screens.Add(new ScreenTen(Content, null));

        }
        public void Update(GameTime gameTime)
        {
            Screens[CurrentScreenIndex].Update(gameTime, ref TempScreenManager);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Screens[CurrentScreenIndex].Draw(spriteBatch);
        }
        public void Forward()
        {
            if (CurrentScreenIndex == 3 && mario.Mariostate.Position.X > 350) {

                Screens[CurrentScreenIndex + 1].mario = Screens[CurrentScreenIndex].mario;
                Screens[CurrentScreenIndex + 1].mario.Mariostate.Position.X = 1;
                CurrentScreenIndex++;
            }
            if (CurrentScreenIndex < 11)
            {
                Screens[CurrentScreenIndex + 1].mario = Screens[CurrentScreenIndex].mario;
                Screens[CurrentScreenIndex + 1].mario.Mariostate.Position.X = 1;
                   CurrentScreenIndex++;
            }
            else
            {
                mario.Mariostate.Position.X = 800 - mario.Mariostate.Width;
                mario.Mariostate.Moving = "False";
            }
        }
        public void Backward()
        {
            if (CurrentScreenIndex == 5 )
            {

                Screens[CurrentScreenIndex - 1].mario = Screens[CurrentScreenIndex].mario;
                Screens[CurrentScreenIndex - 1].mario.Mariostate.Position.X = 798 - mario.Mariostate.Width; 
                CurrentScreenIndex--;
            }
            if (CurrentScreenIndex > 0)
            {
                Screens[CurrentScreenIndex - 1].mario = Screens[CurrentScreenIndex].mario;
                Screens[CurrentScreenIndex - 1].mario.Mariostate.Position.X = 798 - mario.Mariostate.Width;
                CurrentScreenIndex--;
            }
            else
            {
                mario.Mariostate.Position.X = 0;
                mario.Mariostate.Moving = "False";
            }
        }
    }
    #endregion
}
