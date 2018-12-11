using System;
using System.IO;
using System.Collections.Generic;
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
        private string path;

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

            path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Dungeons of Capstonia\\";

            Directory.CreateDirectory(path);

            file = new FileStream(path + "leaderboard.txt", FileMode.OpenOrCreate, FileAccess.Read);
            
            sr = new StreamReader(file);

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] subStrings = line.Split(',');
                leaderboard.Add(new Entry(subStrings[0], int.Parse(subStrings[1]), int.Parse(subStrings[2]), subStrings[3], subStrings[4]));
            }

            sr.Close();
        }


        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                game.MenuDown.Play();
                game.state = GameState.MainMenu;
                if (game.PlayerDead || game.PlayerWin)
                    game.Reinitialize();
            }

        }

        public void AddToLeaderboard(string name, int glory, int level, string killedby, string date)
        {
            if(leaderboard.Count > 0)
            {
                if (glory > leaderboard[leaderboard.Count - 1].Glory || leaderboard.Count <= 10)
                {
                    leaderboard.Add(new Entry(name, glory, level, killedby, date));

                    leaderboard.Sort((b, a) => a.Glory.CompareTo(b.Glory));

                    if (leaderboard.Count > 10)
                    {
                        leaderboard.RemoveAt(9);
                    }
                }
            }
            else
            {
                leaderboard.Add(new Entry(name, glory, level, killedby, date));
            }     
        }

        public void CloseFile()
        {
            file = new FileStream(path + "leaderboard.txt", FileMode.Truncate, FileAccess.Write);
            sw = new StreamWriter(file);

            foreach (Entry entry in leaderboard)
            {
                string line = entry.Name + "," + entry.Glory + "," + entry.Level + "," + entry.KilledBy + "," + entry.Date;
                sw.WriteLine(line);
            }

            sw.Close();
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            string message1, message2;

            // draw title
            string title = "LEADER BOARD";
            int yOffset = 40;
            int centerOffset = 500;
            int numChars = title.Length;
            spriteBatch.DrawString(game.pressStart2PFont, title, new Vector2((int)(centerOffset - (10.5 * numChars)), yOffset), Color.White);


            // draw high scores
            yOffset = 120;
            int xOffset = 20;
            foreach(Entry entry in leaderboard)
            {
                message1 = "[ " + entry.Glory + " Glory ]  ";
                if(entry.KilledBy != null)
                {
                    message2 = entry.Name + " was killed on level " + entry.Level + " by a " + entry.KilledBy + " on " + entry.Date;
                }
                else
                {
                    message2 = entry.Name + " found the lost treasure on " + entry.Date;
                }

                spriteBatch.DrawString(game.pressStart2PSmallFont, message1 + message2, new Vector2(xOffset, yOffset), Color.White);
                yOffset += 18;
            }

            // draw continue message
            string continueMsg = "Press <SPACE> to go to Main Menu.";
            yOffset = 800;
            numChars = continueMsg.Length;
            spriteBatch.DrawString(game.pressStart2PFont, continueMsg, new Vector2((int)(centerOffset - (10.5 * numChars)), yOffset), Color.White);
        }
    }
}



