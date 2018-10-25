using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueSharp;
using RogueSharp.Random;
using System;
using System.Collections.Generic;
using Capstonia.Systems;
using Capstonia.Core;
using Capstonia.Items;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Capstonia
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameManager : Game
    {
        // MonoGame Specific Declarations
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Texture2D floor;
        public Texture2D wall;
        public SpriteFont mainFont;
		
		// container to hold the monsters
        public List<Monster> Monsters;

        // RogueSharp Specific Declarations
        public static IRandom Random { get; private set; }
        public Player Player { get; set; }
        public LevelGrid Level { get; private set; }
        public MessageLog Messages { get; set; }
        public CommandSystem CommandSystem;


        // Game Variable Declarations
        public readonly int levelWidth = 70;
        public readonly int levelHeight = 70;
        public readonly int levelRows = 5;
        public readonly int levelCols = 5;
        public int mapLevel = 1;
        public readonly int tileSize = 48;
        public float scale = 1.0f;
        
        //Inventory Testing//
        public InventorySystem Inventory;
        public Rectangle inventoryScreen;
        public Texture2D emptyTexture; //used to fill a blank rectangle (i.e., inventoryScreen)
        public Texture2D armor;
        public Texture2D food;
        public Texture2D weapon;
        public Texture2D potion;
        public Texture2D book;
        public Texture2D gem;
        public Texture2D Outline;
        ///////////////////////

        private bool renderRequired = true;
       

        // track keyboard state (i.e. capture key presses)
        public KeyboardState currentKeyboardState;
        public KeyboardState previousKeyboardState;

        public GameManager() : base()
        {
            // MonoGame Graphic/Content setup
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 830;
            Content.RootDirectory = "Content";

            //link the messageLog and game instance
            Messages = new MessageLog(this);

            // Player provided commands
            CommandSystem = new CommandSystem(this);

            //Create Inventory for player
            Inventory = new InventorySystem(this);
            inventoryScreen = new Rectangle(200, 100, 1200, 0);   //Height, Width, X, Y

        }

        private void testInventSystem()
        {
            //Drawing black screen for inventory inspired by: https://stackoverflow.com/questions/5751732/draw-rectangle-in-xna-using-spritebatch
            //inventoryScreen = new Texture2D(graphics.GraphicsDevice, 200, 100);
            //inventoryScreen.SetData(new[] { Color.White });

            //Testing manual inventory
            Armor leather = new Armor(this);
            Armor leather2 = new Armor(this);
            Food drummy = new Food(this);
            Food drummy2 = new Food(this);
            Food drummy3 = new Food(this);
            Weapon thingy = new Weapon(this);
            Weapon thingy2 = new Weapon(this);
            Book read = new Book(this);
            Potion drink = new Potion(this);
            Potion drink2 = new Potion(this);
            Potion drink3 = new Potion(this);
            Potion drink4 = new Potion(this);
            Inventory.AddItem(leather);
            Inventory.AddItem(leather2);
            Inventory.AddItem(drink);
            Inventory.AddItem(drink2);
            Inventory.AddItem(drummy);
            Inventory.AddItem(thingy);
            Inventory.AddItem(drink3);
            Inventory.AddItem(drink4);
            Inventory.AddItem(drummy2);
            Inventory.AddItem(read);
            Inventory.AddItem(drummy3);
            Inventory.AddItem(thingy2);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Monsters = new List<Monster>();
            // get seed based on current time and set up RogueSharp Random instance
            int seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);
            //https://stackoverflow.com/questions/22535699/mouse-cursor-is-not-showing-in-windows-store-game-developing-using-monogame
            this.IsMouseVisible = true;
            GenerateLevel();
            testInventSystem();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            floor = Content.Load<Texture2D>("floor_extra_12");
            wall = Content.Load<Texture2D>("wall_stone_11");

            mainFont = Content.Load<SpriteFont>("MainFont");

            //Drawing black screen for inventory inspired by: https://stackoverflow.com/questions/5751732/draw-rectangle-in-xna-using-spritebatch
            emptyTexture = new Texture2D(GraphicsDevice, 1, 1);
            emptyTexture.SetData(new[] { Color.White });

            armor = Content.Load<Texture2D>("armor");
            food = Content.Load<Texture2D>("drumstick");
            weapon = Content.Load<Texture2D>("weapon");
            potion = Content.Load<Texture2D>("potion") ;
            book = Content.Load<Texture2D>("book"); ;
            Outline = Content.Load<Texture2D>("inventory_gui");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Handle keyboard input
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // move player
            Player.Move();

            // update game state
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // making the spriteBatch.begin(...) change below should fix the
            // rendering issues where layers would randomly render out of order
            // spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.Begin();

            Inventory.Draw(spriteBatch);
            Messages.Draw(spriteBatch);
            Level.Draw(spriteBatch);
			
			// draw all of the monsters in the list
            foreach (var monster in Monsters)
            {
                monster.Draw(spriteBatch);
            }

            Player.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }


        // GenerateLevel()
        // DESC:    Generates the entire level grid in which individual rooms will be placed.     
        // PARAMS:  None.
        // RETURNS: None.
        private void GenerateLevel()
        {
            LevelGenerator levelGenerator = new LevelGenerator(this, levelWidth, levelHeight, levelRows, levelCols, mapLevel);
            Level = levelGenerator.CreateLevel();
        }

        //SetLevelCell()
        // DESC:    Takes data from object and passed data to UserInterface for level update
        // PARAMS:  x(int), y(int), type(ObjectType), isExplored(bool)
        // RETURNS: None.
        public void SetLevelCell(int x, int y, ObjectType type, bool isExplored)
        {
            //masterConsole.UpdateLevelCell(x, y, type, isExplored);
        }

        public bool IsInRoomWithPlayer(int x, int y)
        {
            // get room player is in
            int RoomIndex;
            for (RoomIndex = 0; RoomIndex < Level.Rooms.Count; RoomIndex++)
            {
                if(Player.X >= Level.Rooms[RoomIndex].Left && Player.X <= Level.Rooms[RoomIndex].Right && Player.Y >= Level.Rooms[RoomIndex].Top && Player.Y <= Level.Rooms[RoomIndex].Bottom)
                {
                    // found player room, now check if passed in coordinates exist within it
                    if(x >= Level.Rooms[RoomIndex].Left && x <= Level.Rooms[RoomIndex].Right && y >= Level.Rooms[RoomIndex].Top && y <= Level.Rooms[RoomIndex].Bottom)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            // player should always be located in the list of Rooms so we should never reach this point
            return false;
        }
    }
}
