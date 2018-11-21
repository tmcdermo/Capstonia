using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Capstonia.Core;

namespace Capstonia.Systems
{
    /// <summary>
    /// MessageLog Structure:
    /// *Queue container to store our messages
    /// *GameManager object to link our game to the MessageLog
    /// ----
    /// AddMessage Function lets us add a text string to the Queue
    /// Draw Function is more generic it wraps our "print to screen" function stored in our UI_MessageLog
    /// </summary>
    public class MessageLog
    {
        private static int numberMessages = 9;     //change as needed for # of text lines to see
        private readonly Queue<string> messageList;
        private GameManager gameObj;

        public MessageLog(GameManager instance)
        {
            messageList = new Queue<string>();
            gameObj = instance;
        }

        /// <summary>
        /// Add's a text string to our queue of messages
        /// </summary>
        /// <param name="message">Message to be shown on UI</param>
        public void AddMessage(string message)
        {
            messageList.Enqueue(message);
            //check to see if we've exceeded our number of messages limit
            if (messageList.Count > numberMessages)
            {
                messageList.Dequeue();
            }
        }

        /// <summary>
        /// Draw function gets called for every scene refresh;
        /// We "send off the list" to our game object which will use the View namespace to print to the UI
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            int baseLog = 690;
            int offset = 0;

            foreach(string message in messageList)
            {
                if(message.Contains("dodge") || message.Contains("block"))
                {
                    //Print blue
                    spriteBatch.DrawString(gameObj.mainFont, message, new Vector2(20, baseLog + offset), Color.RoyalBlue);
                    offset += 18;
                }
                else if(message.Contains("dmg"))
                {
                    //Print red
                    spriteBatch.DrawString(gameObj.mainFont, message, new Vector2(20, baseLog + offset), Color.Red);
                    offset += 18;
                }
                else if(message.Contains("%") || message.Contains("#") || message.Contains("feel") || 
                        message.Contains("Lose") || message.Contains("Feasted"))
                {
                    //Print green
                    spriteBatch.DrawString(gameObj.mainFont, message, new Vector2(20, baseLog + offset), Color.Green);
                    offset += 18;
                }
                else
                {
                    //Print white
                    spriteBatch.DrawString(gameObj.mainFont, message, new Vector2(20, baseLog + offset), Color.White);
                    offset += 18;
                }
            }
        }
    }
}
