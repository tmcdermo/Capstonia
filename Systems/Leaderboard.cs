using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Capstonia.Core;

// file IO based on: https://www.tutorialspoint.com/csharp/csharp_binary_files.htm
namespace Capstonia.Systems
{
    public class Leaderboard
    {
        private GameManager game;
        private FileStream file;
        private StreamWriter sw;
        private StreamReader sr;

        public struct Entry
        {
            public string Name;
            public int Glory;
            public int Level;
            public string KilledBy;
            public string Date;

            public Entry(string name, int glory, int level, string killedby, string date)
            {
                Name = name;
                Glory = glory;
                Level = level;
                KilledBy = killedby;
                Date = date;
            }
        }

        private List<Entry> leaderboard;


        public Leaderboard(GameManager game)
        {
            this.game = game;  
            leaderboard = new List<Entry>();

            file = new FileStream("leaderboard.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            sw = new StreamWriter(file);
            sr = new StreamReader(file);

            string line;
            while((line = sr.ReadLine()) != null)
            {
                string[] subStrings = line.Split(',');
                leaderboard.Add(new Entry(subStrings[0], int.Parse(subStrings[1]), int.Parse(subStrings[2]), subStrings[3], subStrings[4]));
            }
        }


        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                game.state = GameState.MainMenu;

        }

        public void AddToLeaderboard(string name, int glory, int level, string killedby, string date)
        {
            if(leaderboard.Count > 0)
            {
                if (glory > leaderboard[leaderboard.Count - 1].Glory || leaderboard.Count < 10)
                {
                    Entry newEntry = new Entry(name, glory, level, killedby, date);
                    leaderboard.Add(newEntry);

                    leaderboard.Sort((b, a) => a.Glory.CompareTo(b.Glory));

                    if (leaderboard.Count >= 10)
                    {
                        leaderboard.RemoveAt(9);
                    }
                }
            }
            else
            {
                Entry newEntry = new Entry(name, glory, level, killedby, date);
                leaderboard.Add(newEntry);

                leaderboard.Sort((b, a) => a.Glory.CompareTo(b.Glory));
            }

            foreach(Entry entry in leaderboard)
            {
                string line = entry.Name + "," + entry.Glory + "," + entry.Level + "," + entry.KilledBy + "," + entry.Date;
                sw.WriteLine(line);
            }

            sw.Close();
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            int xOffset = 100;
            int yOffset = 100;

            spriteBatch.DrawString(game.mainFont, "-LEADERBOARD-", new Vector2(80, 50), Color.White);

            foreach(Entry entry in leaderboard)
            {
                string message1 = "[ " + entry.Glory + " Glory ]";
                string message2 = "|   " + entry.Name + " was killed on level " + entry.Level + " by a " + entry.KilledBy + " on " + entry.Date + "!!";
                spriteBatch.DrawString(game.mainFont, message1, new Vector2(xOffset, yOffset), Color.White);
                spriteBatch.DrawString(game.mainFont, message2, new Vector2(xOffset + 100, yOffset), Color.White);
                yOffset += 18;
            }

            spriteBatch.DrawString(game.mainFont, "Press <SPACE> to go to Main Menu.", new Vector2(xOffset, yOffset + 300), Color.White);
        }
    }
}
