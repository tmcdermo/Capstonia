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
        // GameState Controller (Scene Control)
        public GameState state;
        public MainMenu MainMenu { get; set; }        
        public Instructions Instructions { get; set; }
        public Leaderboard Leaderboard { get; set; }
        public Credits Credits { get; set; }


        // Game Variable Declarations
        public readonly int levelWidth = 70;
        public readonly int levelHeight = 70;
        public readonly int levelRows = 5;
        public readonly int levelCols = 5;
        public int mapLevel = 1;
        public readonly int tileSize = 48;
        public float scale = 1.0f;

        // Game Actor Constants
        public readonly int BaseStrength = 10;
        public readonly int BaseDexterity = 10;
        public readonly int BaseConstitution = 10;
        //public readonly float BaseStrength = 10.0f;
        //public readonly float BaseDexterity = 10.0f;
        //public readonly float BaseConstitution = 10.0f;

        // RogueSharp Specific Declarations
        public static IRandom Random { get; private set; }
        public Player Player { get; set; }
        public MapLevel MapLevelDisplay { get; set; }
        public Monster Monster { get; set; }
        public LevelGrid Level { get; private set; }
        public MessageLog Messages { get; set; }
        public Score ScoreDisplay { get; set; }
        public CommandSystem CommandSystem;
        public PathFinder GlobalPositionSystem;

        // MonoGame Specific Declarations
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Texture2D floor;
        public Texture2D wall;
        public Texture2D exit;
        public SpriteFont mainFont;

        // Items - Gameboard
        public Texture2D armor;
        public Texture2D armor_leather_chest;
        public Texture2D armor_steel_chest;
        public Texture2D armor_gold_chest;
        public Texture2D armor_emerald_chest;
        public Texture2D armor_diamond_chest;
        public Texture2D armor_blood_chest;
        public Texture2D food;
        public Texture2D weapon_club;
        public Texture2D potion;
        public Texture2D book;
        public Texture2D gem;
        public Texture2D chest;

        // Items - Player Stats
        public Texture2D constitution;
        public Texture2D dexterity;
        public Texture2D experience;
        public Texture2D health;
        public Texture2D level;
        public Texture2D strength;

        // Monsters
        public Texture2D banshee;
        public Texture2D barbarian;
        public Texture2D bat;
        public Texture2D beholder;
        public Texture2D demon;
        public Texture2D dragon;
        public Texture2D drowelf;
        public Texture2D fireelemental;
        public Texture2D goblin;
        public Texture2D lich;
        public Texture2D lizardman;
        public Texture2D minotaur;
        public Texture2D mummy;
        public Texture2D ogre;
        public Texture2D rat;
        public Texture2D skeleton;
        public Texture2D slime;
        public Texture2D snake;
        public Texture2D spider;
        public Texture2D spirit;
        public Texture2D stonegolem;
        public Texture2D troll;
        public Texture2D valkyrie;
        public Texture2D vampire;
        public Texture2D wolf;
        public Texture2D wraith;
        public Texture2D zombie;

        // containers
        public List<Monster> Monsters;
        public List<Item> Items;
        
        //Inventory
        public InventorySystem Inventory;
        public Rectangle inventoryScreen;
        public Texture2D emptyTexture; //used to fill a blank rectangle (i.e., inventoryScreen)
        public Texture2D Outline;

        //Equipment
        public Equipment Equip;
        
        // Player Stats and Equipment
        public Texture2D PlayerStatsOutline;
        public Texture2D PlayerEquipmentOutline;

        // Monster Stats
        public Texture2D MonsterStatsOutline;

        // track keyboard state (i.e. capture key presses)
        public KeyboardState currentKeyboardState;
        public KeyboardState previousKeyboardState;

        public GameManager() : base()
        {
            state = GameState.MainMenu;

            // Scenes other than GameManager itself
            MainMenu = new MainMenu(this);
            Instructions = new Instructions(this);
            Leaderboard = new Leaderboard(this);
            Credits = new Credits(this);            

            // MonoGame Graphic/Content setup
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 858;
            Content.RootDirectory = "Content";

            // add player instance
            Player = new Player(this);

            // add monster instance
            Monster = new Monster(this);

            //link the messageLog and game instance
            Messages = new MessageLog(this);

            // display glory/score
            ScoreDisplay = new Score(this);

            // display map level player is on
            MapLevelDisplay = new MapLevel(this);

            // Player provided commands
            CommandSystem = new CommandSystem(this);

            //Create Inventory for player
            Inventory = new InventorySystem(this);
            inventoryScreen = new Rectangle(200, 100, 1200, 0);   //Height, Width, X, Y

            //Equipment
            Equip = new Equipment(this);

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // initialize lists
            Monsters = new List<Monster>();
            Items = new List<Item>();

            // get seed based on current time and set up RogueSharp Random instance
            int seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);

            //https://stackoverflow.com/questions/22535699/mouse-cursor-is-not-showing-in-windows-store-game-developing-using-monogame
            this.IsMouseVisible = true;

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

            // load level textures
            floor = Content.Load<Texture2D>("floor_extra_12");
            wall = Content.Load<Texture2D>("wall_stone_11");
            exit = Content.Load<Texture2D>("floor_set_grey_8");

            // load item textures - gameboard
            armor = Content.Load<Texture2D>("armor");
            armor_leather_chest = Content.Load<Texture2D>("armor_leather_chest");
            armor_steel_chest = Content.Load<Texture2D>("armor_steel_chest");
            armor_gold_chest = Content.Load<Texture2D>("armor_gold_chest");
            armor_emerald_chest = Content.Load<Texture2D>("armor_emerald_chest");
            armor_diamond_chest = Content.Load<Texture2D>("armor_diamond_chest");
            armor_blood_chest = Content.Load<Texture2D>("armor_blood_chest");
            food = Content.Load<Texture2D>("drumstick");
            weapon_club = Content.Load<Texture2D>("weapon_club");
            potion = Content.Load<Texture2D>("potion");
            book = Content.Load<Texture2D>("book");
            chest = Content.Load<Texture2D>("chest_gold_open");

            // load item textures - player stats
            constitution = Content.Load<Texture2D>("constitution");
            dexterity = Content.Load<Texture2D>("dexterity");
            experience = Content.Load<Texture2D>("experience");
            health = Content.Load<Texture2D>("health");
            level = Content.Load<Texture2D>("level");
            strength = Content.Load<Texture2D>("strength");

            // load gui textures
            Outline = Content.Load<Texture2D>("inventory_gui");
            PlayerStatsOutline = Content.Load<Texture2D>("player_stats_gui");
            PlayerEquipmentOutline = Content.Load<Texture2D>("player_equipment_gui");
            MonsterStatsOutline = Content.Load<Texture2D>("monster_stats_gui");

            // load actor textures
            Player.Sprite = Content.Load<Texture2D>("dknight_1");
            beholder = Content.Load<Texture2D>("beholder_deep_1");
            banshee = Content.Load<Texture2D>("banshee_1");
            barbarian = Content.Load<Texture2D>("barbarian_f_1");
            bat = Content.Load<Texture2D>("bat_giant_1");
            demon = Content.Load<Texture2D>("demon_red_1");
            dragon = Content.Load<Texture2D>("dragon_green_1");
            drowelf = Content.Load<Texture2D>("drow_1");
            fireelemental = Content.Load<Texture2D>("elemental_fire_1");
            goblin = Content.Load<Texture2D>("goblin_1");
            stonegolem = Content.Load<Texture2D>("golem_stone_1");
            lich = Content.Load<Texture2D>("lich_1");
            lizardman = Content.Load<Texture2D>("lizardman_blue_1");
            minotaur = Content.Load<Texture2D>("minotaur_1");
            mummy = Content.Load<Texture2D>("mummy_1");
            ogre = Content.Load<Texture2D>("ogre_1");
            rat = Content.Load<Texture2D>("rat_giant_1");
            skeleton = Content.Load<Texture2D>("skeleton_1");
            slime = Content.Load<Texture2D>("slime_purple_1");
            snake = Content.Load<Texture2D>("snake_giant_1");
            spider = Content.Load<Texture2D>("spider_black_giant_1");
            spirit = Content.Load<Texture2D>("spirit_1");
            troll = Content.Load<Texture2D>("troll_1");
            valkyrie = Content.Load<Texture2D>("valkyrie_b_1");
            vampire = Content.Load<Texture2D>("vampire_lord_1");
            wolf = Content.Load<Texture2D>("wolf_black_1");
            wraith = Content.Load<Texture2D>("wraith_a_1");
            zombie = Content.Load<Texture2D>("zombie_a_1");

            // load fonts
            mainFont = Content.Load<SpriteFont>("MainFont");

            //Drawing black screen for inventory inspired by: https://stackoverflow.com/questions/5751732/draw-rectangle-in-xna-using-spritebatch
            emptyTexture = new Texture2D(GraphicsDevice, 1, 1);
            emptyTexture.SetData(new[] { Color.White });

            GenerateLevel();

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
            switch (state)
            {
                case GameState.MainMenu:
                    MainMenu.Update();
                    break;
                case GameState.Instructions:
                    Instructions.Update();
                    break;
                case GameState.Leaderboard:
                    Leaderboard.Update();
                    break;
                case GameState.Credits:
                    Credits.Update();
                    break;
                case GameState.GamePlay:
                    UpdateGamePlay();
                    break;
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// This is the GamePlay Update Controller
        /// </summary>
        protected void UpdateGamePlay()
        {
            //testing hunger timings//
            bool turnComplete = false;
            bool playerHasMoved = false;
            bool monstersHaveMoved = false;

            if (Player.CurrHealth > 0)
            {
                while (turnComplete == false)
                {
                    // Handle keyboard input
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                        Exit();

                    // move player
                    if (playerHasMoved == false)
                    {
                        Player.Move();
                    }
                    playerHasMoved = true;

                    if (playerHasMoved)
                    {
                        //move Monsters
                        foreach (Monster enemy in Monsters)
                        {
                            enemy.Move();
                        }
                        monstersHaveMoved = true;
                    }

                    if (playerHasMoved && monstersHaveMoved)
                    {
                        turnComplete = true;
                    }
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
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

            switch (state)
            {
                case GameState.MainMenu:
                    MainMenu.Draw(spriteBatch);
                    break;
                case GameState.Instructions:
                    Instructions.Draw(spriteBatch);
                    break;
                case GameState.Leaderboard:
                    Leaderboard.Draw(spriteBatch);
                    break;
                case GameState.Credits:
                    Credits.Draw(spriteBatch);
                    break;
                case GameState.GamePlay:
                    DrawGamePlay(spriteBatch);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Draw method for GamePlay
        /// </summary>
        protected void DrawGamePlay(SpriteBatch spriteBatch)
        {
            Inventory.Draw(spriteBatch);
            Messages.Draw(spriteBatch);
            ScoreDisplay.Draw(spriteBatch);
            MapLevelDisplay.Draw(spriteBatch);
            Level.Draw(spriteBatch);

            // draw all of the monsters in the list
            foreach (var monster in Monsters)
            {
                monster.Draw(spriteBatch);
            }

            // draw all of the items in the list
            foreach (var item in Items)
            {
                item.Draw(spriteBatch);
            }

            // draw player sprite
            Player.Draw(spriteBatch);

            // draw stats grid for player
            Player.DrawStats(spriteBatch);

            // draw stats grid for monsters
            Monster.DrawStats(spriteBatch);

            // draw equipment grid for player
            Player.DrawEquipment(spriteBatch);
        }

        // GenerateLevel()
        // DESC:    Generates the entire level grid in which individual rooms will be placed.     
        // PARAMS:  None.
        // RETURNS: None.
        public void GenerateLevel()
        {
            LevelGenerator levelGenerator = new LevelGenerator(this, levelWidth, levelHeight, levelRows, levelCols, mapLevel);
            Level = levelGenerator.CreateLevel();
        }

        // SetLevelCell()
        // DESC:    Takes data from object and passed data to UserInterface for level update
        // PARAMS:  x(int), y(int), type(ObjectType), isExplored(bool)
        // RETURNS: None.
        public void SetLevelCell(int x, int y, ObjectType type, bool isExplored)
        {
            //masterConsole.UpdateLevelCell(x, y, type, isExplored);
        }

        // IsInRoomWithPlayer()
        // DESC:    Determines if coordinates are in the same room as the player
        // PARAMS:  x(int), y(int)
        // RETURNS: Boolean (true if in same room, false otherwise)
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

        // HandleDeath()
        // DESC:    Handle monster death
        // PARAMS:  Monster
        // RETURNS: None
        public void HandleMonsterDeath(Monster monster)
        {
            int addGlory = Random.Next(monster.MinGlory, monster.MaxGlory);
            Player.Glory += addGlory;

            int addExperience = monster.GetMonsterExperience();
            Player.Experience += addExperience;

            Messages.AddMessage("You have slaughtered the " + monster.Name + "!!!");            
            Messages.AddMessage("You have earned " + addGlory + " Glory worth of gold and bones!!!");
            Messages.AddMessage("You gained " + addExperience + " Experience Points!!!");

            Player.CheckLevelUp();
            
            Level.SetIsWalkable(monster.X, monster.Y, true);
            Monsters.Remove(monster);            
        }

        // HandlePlayerDeath()
        // DESC:    Handle player death
        // PARAMS:  None
        // RETURNS: None
        public void HandlePlayerDeath()
        {
            Messages.AddMessage("You have DIED!  Game Over!");
            Messages.AddMessage("Press <ESC> to Exit Game.");

        }
    }
}
