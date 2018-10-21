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
        private static int numberMessages = 7;     //change as needed for # of text lines to see
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
            //check to see if we've exceepded our number of messages limit//
            if (messageList.Count > numberMessages)
            {
                messageList.Dequeue();
            }
        }

        /// <summary>
        /// Draw function gets called for every scene refresh;
        /// We "send off the list" to our game object which will use the View namespace to print to the UI
        /// </summary>
        public void Draw()
        {
            gameObj.messageDeliver(messageList);
        }
    }
}
