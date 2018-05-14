using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TwoD00m.cWorld;
using TwoD00m.Interface;
using TwoD00m.Control;
using TwoD00m.Drawble;
using TwoD00m.PlayerItems;


namespace TwoD00m
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        const int windowWidth = 1024;  // setUp экрана и всего остольного куда-нибудь отдельно
        const int windowHeight = 720;
        int screenWidth;
        int screenHeight;

        Matrix screenXform;          // 

        World world;
        Hero hero;
        InputHandler inputHandler;


        Dictionary<Point, Monster> monstersList;
        
        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Sprites.setContent(Content);

            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            Window.IsBorderless = true;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            /* FormSetUp */
            var screenScaleY = graphics.PreferredBackBufferHeight / 720.0f;
            var screenScaleX = graphics.PreferredBackBufferWidth / 1024.0f;
            screenXform = Matrix.CreateScale(screenScaleX, screenScaleY, 1.0f);
            screenWidth = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            screenHeight = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            Window.Position = new Point(screenWidth / 2 - windowWidth / 2, screenHeight / 2 - windowHeight / 2);

            
            Sprites.setSpriteBatch(new SpriteBatch(graphics.GraphicsDevice));
            GameItems.SetWeapon();

            world = Loader.LoadWorld(@".\cWorld\level1.world");
            hero = new Hero();
            world.WorldUpdate(hero.position);
            inputHandler = new InputHandler();
            
            monstersList = Common.ReadMonstersList();
            foreach(var monster in monstersList)
                world.WorldUpdate(monster.Value.position);

            base.Initialize();
        }
        
        protected override void LoadContent() { }
        
        protected override void UnloadContent() { }
             
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
          
            Command command = inputHandler.handleInput();
            if (command != null)
            {
                command.execute(hero, world, monstersList);
                if (command.EndTurn)
                {
                    AI.MovevmentsAndAtacks(monstersList, world, hero);
                    Common.DictionaryUpdate(ref monstersList);
                }

                hero.UpdateGUI();
            }

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            Sprites.getSpriteBach().Begin(SpriteSortMode.Deferred, null, null, null, null, null, screenXform);

            Drawing.Draw3D(hero, world, monstersList);
            hero.DrawGUI();

            Sprites.getSpriteBach().End();

            base.Draw(gameTime);
        }
    }
}
