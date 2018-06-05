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

        Matrix screenXform;
        Texture2D window;

        World world;
        Hero hero;
        InputHandler inputHandlerInventory;
        InputHandler inputHandlerMovements;

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
            screenHeight = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            screenWidth = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            Window.Position = new Point(screenWidth / 2 - windowWidth / 2, screenHeight / 2 - windowHeight / 2);
            
            Sprites.setSpriteBatch(new SpriteBatch(graphics.GraphicsDevice));
            GameItems.SetUp();

            world = Loader.LoadWorld(@".\cWorld\level1.world");
            hero = new Hero();
            world.WorldUpdate(hero.Position);

            window = Content.Load<Texture2D>("Hud_Game");

            GameGUI.SetUp(hero);

            inputHandlerInventory = new InputHandler(1);
            inputHandlerMovements = new InputHandler();
            
            List<Monster> list = Loader.LoadKit<Monster>(@".\Content\Monsters.csv").ToList();
            monstersList = Creature.ToDictionary(list);
            foreach(var monster in monstersList.Values)
                world.WorldUpdate(monster.Position);
            base.Initialize();
        }
        
        protected override void LoadContent() { }
        
        protected override void UnloadContent() { }
             
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Command command;
            if (hero.inventoryOpen == true)
            {
                command = inputHandlerInventory.handleInput();
            }
            else command = inputHandlerMovements.handleInput();

            //Command command = inputHandlerInventory.handleInput();
           
            if (command != null)
            {
                command.execute(hero, world, monstersList);
                hero.GetEffects();
                if (command.EndTurn)
                {
                    foreach(var monster in monstersList.Values)
                    if(monster.HP.Actual>0)
                    monster.MakeAMove(world, hero, monstersList);
                }
                AI.DictionaryUpdate(ref monstersList);
                hero.UpdateGUI();
            }

            if (hero.HP.Actual <= 0)
            {
                
            }
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            Sprites.getSpriteBach().Begin(SpriteSortMode.Deferred, null, null, null, null, null, screenXform);

            Drawing.Draw3D(hero, world, monstersList);
            hero.DrawGUI();
            Sprites.getSpriteBach().Draw(window, Vector2.Zero, Color.White);

            Sprites.getSpriteBach().End();

            base.Draw(gameTime);
        }
    }
}
