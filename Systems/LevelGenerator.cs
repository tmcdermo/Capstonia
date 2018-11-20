using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using RogueSharp;
using Rectangle = RogueSharp.Rectangle;
using Point = RogueSharp.Point;
using Capstonia;
using Capstonia.Core;
using Capstonia.Monsters;
using Capstonia.Items;
using Capstonia.Items.ArmorTier;
using Capstonia.Items.WeaponTier;
using Capstonia.Items.BookTier;
using System;
using System.Diagnostics;

namespace Capstonia.Systems
{
    public class LevelGenerator
    {
        // columns and rows should remain equal
        private readonly int columns;
        private readonly int rows;
        private readonly int levelWidth;
        private readonly int levelHeight;
        private readonly int roomWidth;
        private readonly int roomHeight;

        private readonly LevelGrid level;
        private readonly GameManager game;

        private List<Rectangle> ExitPath;
        private Rectangle startRoom;
        private Rectangle exitRoom;

        // constructor
        public LevelGenerator(GameManager game, int width, int height, int gameRows, int gameCols, int mapLevel)
        {
            levelWidth = width;
            levelHeight = height;
            columns = gameCols;
            rows = gameRows;
            roomWidth = width / columns;
            roomHeight = height / rows;

            this.game = game;
            level = new LevelGrid(game);
            ExitPath = new List<Rectangle>();
        }

        // CreateLevel()
        // DESC:    Handler for entire process of Level Generation.     
        // PARAMS:  None.
        // RETURNS: level(LevelGrid) - Fully generated level.
        public LevelGrid CreateLevel()
        {
            // Initialize Grid
            // Creates grid that is solid/unwalkable with the given dimensions
            level.Initialize(levelWidth, levelHeight);

            // clear monster and items lists
            game.Monsters.Clear();
            game.Items.Clear();

            int x, y;
            int roomCounter = 1;
            // assign area for rooms
            for(int col = 0; col < columns; col++)
            {
                for(int row = 0; row < rows; row++)
                {
                    x = col * roomWidth;
                    y = row * roomHeight;

                    level.Rooms.Add(AssignRoom(x, y));

                    roomCounter++;
                }
            }

            // create the rooms previously assigned
            foreach(Rectangle room in level.Rooms)
            {
                CreateRoom(room);
               
            }

            // place player start
            PlacePlayerInStartingRoom();

            // place monsters on level
            PlaceMonsters();

            // place items on level
            PlaceItems();

            // place exit
            SelectExitRoom();
            if (game.mapLevel != game.maxLevel)
            {
                PlaceExit();
            }
            else
            {
                PlaceChest();
            }
                

            // place doors between player start and exit
            FindExitPath();
            PlaceDoorsOnPath();

            // randomly place doors
            PlaceRandomDoors();
            
            return level;
        }

        // AssignRoom()
        // DESC:    Creates a Rectangle object based on location and size parameters.             
        // PARAMS:  x(int), y(int) - Represents grid location of top left corner of room
        // RETURNS: room(Rectangle) - Represents area assigned for this room
        public Rectangle AssignRoom(int x, int y)
        {
            var room = new Rectangle(x, y, roomWidth - 1, roomHeight - 1);

            return room;
        }

        // CreateRoom()
        // DESC:    Takes a solid/unwalkable room (Rectangle object) and chisels out the inside.
        //          It leaves a ring around the outside of the room as unwalkable to represent the walls.
        // PARAMS:  room(Rectangle)
        // RETURNS: Nothing.  Modifies the level(LevelGrid) object.
        public void CreateRoom(Rectangle room)
        {
            // loop through each space in room and make it walkable
            for(int x = room.Left + 1; x < room.Right; x++)
            {
                for(int y = room.Top + 1; y < room.Bottom; y++)
                {
                    level.SetCellProperties(x, y, true, true, false);
                }
            }
        }

        // CreateDoor()
        // DESC:    Creates a door at location in room defined by x and y
        // PARAMS:  x(int), y(int)
        // RETURNS: None.
        public void CreateDoor(int x, int y)
        {
            // TODO
        }

        // SelectRandomRoom()
        // DESC:    Chooses a random room from level.Rooms
        // PARAMS:  None
        // RETURNS: random room(rectangle)
        public Rectangle SelectRandomRoom()
        {
            return level.Rooms[GameManager.Random.Next(level.Rooms.Count - 1)];
        }

        // PlacePlayerInStartingRoom()
        // DESC:    Places the Player in a random starting room for the Level.
        //          Instantiates Player object if this is the first Level.
        // PARAMS:  None
        // RETURNS: None.  Modifies level(LevelGrid) object.
        public void PlacePlayerInStartingRoom()
        {
            // if this is the first Level, Player is not yet instantiated
            // so here we check to see if it exists, if not, we create the object
            //if(game.Player == null)
            //{
            //    Player player = new Player(game);
            //    player.Sprite = game.Content.Load<Texture2D>("dknight_1");
            //    game.Player = player;
            //}

            // get random room as starting room
            startRoom = SelectRandomRoom();

            // give player position in center of room
            game.Player.X = startRoom.Center.X;
            game.Player.Y = startRoom.Center.Y;

            // add player to that room
            level.AddPlayer(game.Player);
        }

        // PlaceMonsters()
        // DESC:    Place monsters throughout entire level
        // PARAMS:  None
        // RETURNS: None.
        public void PlaceMonsters()
        {
            // startingRoomTemp = new Rectangle();
            Point randomPoint;
        
            foreach (var room in level.Rooms)
            {
                var numberOfMonsters = GameManager.Random.Next(0,3);
                for (int i = 0; i < numberOfMonsters; i++)
                {
                    randomPoint = GetRandomPointInRoom(room);

                    //Ensures that the selected tile is walkable (i.e. not a wall or door)
                    while (!level.IsWalkable(randomPoint.X, randomPoint.Y))
                    {
                        randomPoint = GetRandomPointInRoom(room);
                    }
                   
                    int monsterLevel = GetMonsterLevel();
                    MonsterType monsterIndex;
                    Monster monster;
                    do {
                        monsterIndex = (MonsterType)GameManager.Random.Next(0, Enum.GetNames(typeof(MonsterType)).Length - 1);
                        monster = GetMonster(monsterIndex);
                    } while (monster.Level != monsterLevel);                    

                    monster.X = randomPoint.X;
                    monster.Y = randomPoint.Y;

                    level.AddMonster(monster);
                }
            }
        }


        // PlaceItems()
        // DESC:    Place items throughout entire level
        // PARAMS:  None
        // RETURNS: None.
        public void PlaceItems()
        {            
            Point randomPoint;
           
            //var numberOfItems = GameManager.Random.Next(0,2);
            foreach (var room in level.Rooms)
            {
                //Add random number of items between 0-2 into each room
                var numberOfItems = GameManager.Random.Next(0,2);
                for (int i = 0; i < numberOfItems; i++)
                {
                    randomPoint = GetRandomPointInRoom(room);

                    //Ensures that the selected tile is walkable (i.e. not a wall or door)
                    while (!level.IsWalkable(randomPoint.X, randomPoint.Y))
                    {
                        randomPoint = GetRandomPointInRoom(room);
                    }

                    // choose random item to spawn
                    ItemType itemIndex;
                    Item item;

                    itemIndex = (ItemType)GameManager.Random.Next(0, Enum.GetNames(typeof(ItemType)).Length - 1);
                    item = GetItem(itemIndex);
                    
                    item.X = randomPoint.X;
                    item.Y = randomPoint.Y;

                    level.AddItem(item);
                }
            }
        }

        // SelectExitRoom()
        // DESC:    Selects a random room that is not the starting room as the exit room.
        // PARAMS:  None
        // RETURNS: None. Creates an exit room.
        public void SelectExitRoom()
        {
            //Select random room for exit room
            exitRoom = SelectRandomRoom();

            //Ensures the exit room does not choose the same room that the player starts in
            while(game.Player.X >= exitRoom.Left && game.Player.X <= exitRoom.Right && game.Player.Y >= exitRoom.Top && game.Player.Y <= exitRoom.Bottom)
            {
                exitRoom = SelectRandomRoom();
            }            
        }


        // PlaceExit()
        // DESC:    Creates an exit at a random point within the exit room.
        // PARAMS:  None.
        // RETURNS: None.  Modifies exit room.
        public void PlaceExit()
        {
            //Select random point within exit room
            Point randomPoint = GetRandomPointInRoom(exitRoom);

            //Ensures that the selected tile is walkable (i.e. not a wall or door)
            while(!level.IsWalkable(randomPoint.X, randomPoint.Y))
            {
                randomPoint = GetRandomPointInRoom(exitRoom);
            }

            //Creates LevelExit object and places the exit at the selected location
            level.LevelExit = new Exit(game);
            level.LevelExit.X = randomPoint.X;
            level.LevelExit.Y = randomPoint.Y;
        }

        //PlaceChest()
        // DESC:    Adds the final chest to the game where the exit would normally go
        // PARAMS:  None
        // Returns: None.  Modifies exit room.
        public void PlaceChest()
        {
            Point randomPoint = GetRandomPointInRoom(exitRoom);

            //Ensures that the selected tile is walkable (i.e. not a wall or door)
            while (!level.IsWalkable(randomPoint.X, randomPoint.Y))
            {
                randomPoint = GetRandomPointInRoom(exitRoom);
            }

            Item item = new Chest(game)
            {
                X = randomPoint.X,
                Y = randomPoint.Y
            };

            level.AddItem(item);
        }


        // GetRandomPointInRoom()
        // DESC:    Gets and returns a random coordinate within a given room.
        // PARAMS:  room(Rectangle)
        // RETURNS: Point with the x and y coordinates of random point within the room.
        public Point GetRandomPointInRoom(Rectangle room)
        {
            return new Point(GameManager.Random.Next(room.Left, room.Right), 
                             GameManager.Random.Next(room.Top, room.Bottom));
        }

        // FindExitPath()
        // DESC:    Finds a path from start room to exit room and stores the path in a exit path object.
        // PARAMS:  None.
        // RETURNS: None.  Modifies ExitPath by storing room objects inside the exitPath List.
        public void FindExitPath()
        {
            //Create variables 
            Rectangle currentRoom = startRoom;
            int roomIndex = GetCurrentRoomIndex(currentRoom);
            int randomDirection;

            ExitPath.Add(startRoom);

            //Loop until we find the exit room
            while(currentRoom != exitRoom)
            {
                //Gives 50/50 chance of checking x or y axis for a path to the exit
                randomDirection = GameManager.Random.Next(0, 1);

                if (randomDirection == 0) // check X
                {
                    if(currentRoom.Center.X < exitRoom.Center.X) // currentRoom is to left of exitRoom
                    {
                        roomIndex += columns;                       
                    }
                    else if(currentRoom.Center.X > exitRoom.Center.X) // currentRoom is to right of exitRoom
                    {
                        roomIndex -= columns;
                    }                    
                }
                else // check Y
                {
                    if(currentRoom.Center.Y < exitRoom.Center.Y) // currentRoom is above exitRoom
                    {
                        roomIndex += 1;
                    }
                    else if(currentRoom.Center.Y > exitRoom.Center.Y) // currentRoom is below exitRoom
                    {
                        roomIndex -= 1;
                    }
                }

                //Assign current room to the current room index and adds to the Exit path
                currentRoom = level.Rooms[roomIndex];
                ExitPath.Add(currentRoom);                
            }            
        }

        // GetCurrentRoomIndex()
        // DESC:    Loops through the list of rooms and returns the index of the selected room.
        // PARAMS:  room(Rectangle)
        // RETURNS: x(int) - the index of the room within the Room list
        public int GetCurrentRoomIndex(Rectangle room)
        {
            int x;

            //Loop through the list of rooms and check if current room index matches the room passed in
            for(x = 0; x < level.Rooms.Count; x++)
            {
                //Once room is found, break out of loop and return the index
                if (level.Rooms[x] == room)
                    break;
            }

            return x;
        }

        // PlaceDoorsOnPath()
        // DESC:    Takes the exitPath and places appropriate doors along the path from start to exit.
        // PARAMS:  None.
        // RETURNS: None.  Modifies level to create doors.
        public void PlaceDoorsOnPath() 
        {
            for(int x = 0; x < ExitPath.Count - 1; x++)
            {
                // determine proper wall to place door
                // make door spot walkable
                // add door logic here later if necessary
                if(ExitPath[x+1].Center.X > ExitPath[x].Center.X)       // next room is to the right
                {
                    // open center of right wall in this room and center of left wall in next room
                    level.SetIsWalkable(ExitPath[x].Right, ExitPath[x].Center.Y, true);
                    level.SetIsWalkable(ExitPath[x+1].Left, ExitPath[x+1].Center.Y, true);

                }
                else if(ExitPath[x+1].Center.X < ExitPath[x].Center.X)  // next room is to the left
                {
                    // open center of left wall in this room and center of right wall in next room
                    level.SetIsWalkable(ExitPath[x].Left, ExitPath[x].Center.Y, true);
                    level.SetIsWalkable(ExitPath[x+1].Right, ExitPath[x+1].Center.Y, true);
                }
                else if(ExitPath[x+1].Center.Y > ExitPath[x].Center.Y)  // next room is below
                {
                    // open center of bottom wall in this room and center of top wall in next room
                    level.SetIsWalkable(ExitPath[x].Center.X, ExitPath[x].Bottom, true);
                    level.SetIsWalkable(ExitPath[x+1].Center.X, ExitPath[x+1].Top, true);
                } 
                else if(ExitPath[x+1].Center.Y < ExitPath[x].Center.Y)  // next room is above
                {
                    // open center of top wall in this room and center of bottom wall in next room
                    level.SetIsWalkable(ExitPath[x].Center.X, ExitPath[x].Top, true);
                    level.SetIsWalkable(ExitPath[x+1].Center.X, ExitPath[x+1].Bottom, true);
                }

            }
        }

        // DESC:    Loops through all of the rooms and places doors on random walls.    
        // PARAMS:  None.
        // RETURNS: None. Modifies level to add doors.
        public void PlaceRandomDoors()
        {
            int door;

            // loop through all rooms in level
            for(int x = 0; x < level.Rooms.Count - 1; x++)
            {
                // check each wall, ensure it is not an exterior level wall, and then 50% chance of placing a doorway
                // since each possible doorway gets tested twice (once from each room), each check is 25%
                if(level.Rooms[x].Top != 0)
                {
                    door = GameManager.Random.Next(0, 3);
                    if (door == 3)
                    {
                        // open center of top wall in this room and center of bottom wall in next room
                        level.SetIsWalkable(level.Rooms[x].Center.X, level.Rooms[x].Top, true);
                        level.SetIsWalkable(level.Rooms[x - 1].Center.X, level.Rooms[x - 1].Bottom, true);
                    }
                }

                if (level.Rooms[x].Bottom != levelHeight - 1)
                {
                    door = GameManager.Random.Next(0, 3);
                    if (door == 3)
                    {
                        // open center of bottom wall in this room and center of top wall in next room
                        level.SetIsWalkable(level.Rooms[x].Center.X, level.Rooms[x].Bottom, true);
                        level.SetIsWalkable(level.Rooms[x + 1].Center.X, level.Rooms[x + 1].Top, true);
                    }
                }

                if (level.Rooms[x].Left != 0)
                {
                    door = GameManager.Random.Next(0, 3);
                    if (door == 3)
                    {
                        // open center of left wall in this room and center of right wall in next room
                        level.SetIsWalkable(level.Rooms[x].Left, level.Rooms[x].Center.Y, true);
                        level.SetIsWalkable(level.Rooms[x - rows].Right, level.Rooms[x - rows].Center.Y, true);
                    }
                }

                if (level.Rooms[x].Right != levelWidth - 1)
                {
                    door = GameManager.Random.Next(0, 3);
                    if (door == 3)
                    {
                        // open center of right wall in this room and center of left wall in next room
                        level.SetIsWalkable(level.Rooms[x].Right, level.Rooms[x].Center.Y, true);
                        level.SetIsWalkable(level.Rooms[x + rows].Left, level.Rooms[x + rows].Center.Y, true);
                    }
                }                

            }
        }

        Monster GetMonster(MonsterType monsterType)
        {
            switch (monsterType)
            {
                case MonsterType.Banshee:
                    return new Banshee(game);
                case MonsterType.Barbarian:
                    return new Barbarian(game);
                case MonsterType.Bat:
                    return new Bat(game);
                case MonsterType.Beholder:
                    return new Beholder(game);
                case MonsterType.Demon:
                    return new Demon(game);
                case MonsterType.Dragon:
                    return new Dragon(game);
                case MonsterType.DrowElf:
                    return new DrowElf(game);
                case MonsterType.FireElemental:
                    return new FireElemental(game);
                case MonsterType.Goblin:
                    return new Goblin(game);
                case MonsterType.Lich:
                    return new Lich(game);
                case MonsterType.Lizardman:
                    return new Lizardman(game);
                case MonsterType.Minotaur:
                    return new Minotaur(game);
                case MonsterType.Mummy:
                    return new Mummy(game);
                case MonsterType.Ogre:
                    return new Ogre(game);
                case MonsterType.Rat:
                    return new Rat(game);
                case MonsterType.Skeleton:
                    return new Skeleton(game);
                case MonsterType.Slime:
                    return new Slime(game);
                case MonsterType.Snake:
                    return new Snake(game);
                case MonsterType.Spider:
                    return new Spider(game);
                case MonsterType.Spirit:
                    return new Spirit(game);
                case MonsterType.StoneGolem:
                    return new StoneGolem(game);
                case MonsterType.Troll:
                    return new Troll(game);
                case MonsterType.Valkyrie:
                    return new Valkyrie(game);
                case MonsterType.Vampire:
                    return new Vampire(game);
                case MonsterType.Wolf:
                    return new Wolf(game);
                case MonsterType.Wraith:
                    return new Wraith(game);
                case MonsterType.Zombie:
                    return new Zombie(game);
            }

            // should never hit this
            return null;
        }

        Item GetItem(ItemType itemType)
        {
            switch (itemType)
            {
                //case ItemType.Armor:
                //    return new Armor(game);
                case ItemType.LeatherChest:
                    return new LeatherChest(game);
                case ItemType.SteelChest:
                    return new SteelChest(game);
                case ItemType.GoldChest:
                    return new GoldChest(game);
                case ItemType.EmeraldChest:
                    return new EmeraldChest(game);
                case ItemType.DiamondChest:
                    return new DiamondChest(game);
                case ItemType.BloodChest:
                    return new BloodChest(game);
                //case ItemType.Bone:
                //    return new Bone(game);
                //case ItemType.Book:
                 //   return new Book(game);
                //case ItemType.Chest:
                //    return new Chest(game);
                case ItemType.Food:
                    return new Food(game);
                //case ItemType.Gem:
                //    return new Gem(game);
                case ItemType.Potion:
                    return new Potion(game);
                //case ItemType.Ring:
                //    return new Ring(game);
                //case ItemType.Skull:
                //    return new Skull(game);
                case ItemType.Weapon:
                    return new Weapon(game);
                case ItemType.AxeGold:
                    return new AxeGold(game);
                case ItemType.LanceVorpal:
                    return new LanceVorpal(game);
                case ItemType.SwordAcid:
                    return new SwordAcid(game);
                case ItemType.SwordDiamond:
                    return new SwordDiamond(game);
                case ItemType.SwordHellfire:
                    return new SwordHellfire(game);
                case ItemType.SwordLightning:
                    return new SwordLightning(game);
                case ItemType.StrengthBook:
                    return new StrengthBook(game);
                case ItemType.DexterityBook:
                    return new DexterityBook(game);
                case ItemType.ConstitutionBook:
                    return new ConstitutionBook(game);
                case ItemType.EvilBook:
                    return new EvilBook(game);


            }

            // should never hit this
            return null;
        }

        // GetMonsterLevel()
        // DESC:    Return Random Monster Level Dependent on Dungeon level
        // PARAMS:  None
        // RETURNS: Monster Level (int)
        public int GetMonsterLevel()
        {
            int monsterLevelChance = GameManager.Random.Next(1, 100);

            switch (game.mapLevel)
            {
                case 1:
                    if (monsterLevelChance <= 85)
                    {
                        return 1;
                    }
                    else if (monsterLevelChance <= 94)
                    {
                        return 2;
                    }
                    else if (monsterLevelChance <= 99)
                    {
                        return 3;
                    }
                    return 4;
                case 2:
                    if(monsterLevelChance <= 60)
                    {
                        return 2;
                    }
                    else if (monsterLevelChance <= 85)
                    {
                        return 1;
                    }
                    else if (monsterLevelChance <= 94)
                    {
                        return 3;
                    }
                    else if (monsterLevelChance <= 99)
                    {
                        return 4;
                    }
                    return 5;
                case 3:
                    if(monsterLevelChance <= 45)
                    {
                        return 3;
                    }
                    else if (monsterLevelChance <= 70)
                    {
                        return 2;
                    }
                    else if (monsterLevelChance <= 85)
                    {
                        return 1;
                    }
                    else if (monsterLevelChance <= 94)
                    {
                        return 4;
                    }
                    else if (monsterLevelChance <= 99)
                    {
                        return 5;
                    }
                    return 6;
                case 4:
                    if (monsterLevelChance <= 40)
                    {
                        return 4;
                    }
                    else if (monsterLevelChance <= 65)
                    {
                        return 3;
                    }
                    else if (monsterLevelChance <= 80)
                    {
                        return 2;
                    }
                    else if (monsterLevelChance <= 85)
                    {
                        return 1;
                    }
                    else if (monsterLevelChance <= 94)
                    {
                        return 5;
                    }
                    else if (monsterLevelChance <= 99)
                    {
                        return 6;
                    }
                    return 7;
                case 5:
                    if (monsterLevelChance <= 40)
                    {
                        return 5;
                    }
                    else if (monsterLevelChance <= 65)
                    {
                        return 4;
                    }
                    else if (monsterLevelChance <= 80)
                    {
                        return 3;
                    }
                    else if (monsterLevelChance <= 85)
                    {
                        return 2;
                    }
                    else if (monsterLevelChance <= 94)
                    {
                        return 6;
                    }
                    else if (monsterLevelChance <= 99)
                    {
                        return 7;
                    }
                    return 8;
                case 6:
                    if (monsterLevelChance <= 40)
                    {
                        return 6;
                    }
                    else if (monsterLevelChance <= 65)
                    {
                        return 5;
                    }
                    else if (monsterLevelChance <= 80)
                    {
                        return 4;
                    }
                    else if (monsterLevelChance <= 85)
                    {
                        return 3;
                    }
                    else if (monsterLevelChance <= 94)
                    {
                        return 7;
                    }
                    else if (monsterLevelChance <= 99)
                    {
                        return 8;
                    }
                    return 9;
                case 7:
                    if (monsterLevelChance <= 40)
                    {
                        return 7;
                    }
                    else if (monsterLevelChance <= 65)
                    {
                        return 6;
                    }
                    else if (monsterLevelChance <= 80)
                    {
                        return 5;
                    }
                    else if (monsterLevelChance <= 85)
                    {
                        return 4;
                    }
                    else if (monsterLevelChance <= 94)
                    {
                        return 8;
                    }
                    else if (monsterLevelChance <= 99)
                    {
                        return 9;
                    }
                    return 10;
                case 8:
                    if (monsterLevelChance <= 41)
                    {
                        return 8;
                    }
                    else if (monsterLevelChance <= 66)
                    {
                        return 7;
                    }
                    else if (monsterLevelChance <= 81)
                    {
                        return 6;
                    }
                    else if (monsterLevelChance <= 86)
                    {
                        return 5;
                    }
                    else if (monsterLevelChance <= 95)
                    {
                        return 9;
                    }
                    return 10;
                case 9:
                    if (monsterLevelChance <= 41)
                    {
                        return 9;
                    }
                    else if (monsterLevelChance <= 66)
                    {
                        return 8;
                    }
                    else if (monsterLevelChance <= 81)
                    {
                        return 7;
                    }
                    else if (monsterLevelChance <= 86)
                    {
                        return 6;
                    }
                    return 10;
                case 10:
                    if (monsterLevelChance <= 55)
                    {
                        return 10;
                    }
                    else if (monsterLevelChance <= 80)
                    {
                        return 9;
                    }
                    else if (monsterLevelChance <= 95)
                    {
                        return 8;
                    }
                    return 7;
            }

            // should never get here
            return -1;
        }

    }
}

