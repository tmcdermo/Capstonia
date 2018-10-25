using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueSharp;
using RogueSharp.Random;
using System;
using System.Collections.Generic;
using Capstonia.Systems;
using Capstonia.Core;

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

            GenerateLevel();

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

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            
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


        private ICell GetRandomEmptyCell()
        { 
            while (true)
            {
                int x = Random.Next(7);
                int y = Random.Next(7);
                if (Level.IsWalkable(x, y))
                {
                    return Level.GetCell(x, y);
                }
            }
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
