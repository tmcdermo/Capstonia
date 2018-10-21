//using Capstonia.Core;

//namespace Capstonia.Systems
//{
//    // InputKeyboard Class
//    // DESC:  Contains attributes and methods for dealing with input provided
//    //        by the user via the keyboard.
//    // NOTES: Heavily inspired by: https://github.com/Olivexe/RogueSharpTutorialUnityPort
//    public class UserInputKeyboard : MonoBehaviour
//    {
//        private UserInputCommands  userInput;

//        // Will return the last key pressed by user and then
//        // set userInput to None.
//        public  UserInputCommands Command
//        {
//            get
//            {
//                // set last user key press
//                UserInputCommands lastUserKeyPress = userInput;
//                // clear user input
//                userInput = UserInputCommands.None;

//                return lastUserKeyPress;
//            }
//        }

//        // Update()
//        // DESC:    Update user input based on most recent key press.
//        // PARAMS:  None.
//        // RETURNS: None.
//        private void Update()
//        {
//            userInput = GetKeyboardValue();
//        }

//        // GetKeyboardValue()
//        // DESC:    Get user's keyboard input.  Note that one must use the
//        //          keyboard keypad to move diagonally.
//        // PARAMS:  None.
//        // RETURNS: None.
//        private UserInputCommands GetKeyboardValue()
//        {
//            if (Input.GetKeyUp(KeyCode.Keypad7))
//            {
//                return UserInputCommands.UpLeft;
//            }
//            else if (Input.GetKeyUp(KeyCode.Keypad8) || Input.GetKeyUp(KeyCode.UpArrow))
//            {
//                return UserInputCommands.Up;
//            }
//            else if (Input.GetKeyUp(KeyCode.Keypad9))
//            {
//                return UserInputCommands.UpRight;
//            }
//            else if (Input.GetKeyUp(KeyCode.Keypad4) || Input.GetKeyUp(KeyCode.LeftArrow))
//            {
//                return UserInputCommands.Left;
//            }
//            else if (Input.GetKeyUp(KeyCode.Keypad6) || Input.GetKeyUp(KeyCode.RightArrow))
//            {
//                return UserInputCommands.Right;
//            }
//            else if (Input.GetKeyUp(KeyCode.Keypad1))
//            {
//                return UserInputCommands.DownLeft;
//            }
//            else if (Input.GetKeyUp(KeyCode.Keypad2) || Input.GetKeyUp(KeyCode.DownArrow))
//            {
//                return UserInputCommands.Down;
//            }
//            else if (Input.GetKeyUp(KeyCode.Keypad3))
//            {
//                return UserInputCommands.DownRight;
//            }
//            else if (Input.GetKeyUp(KeyCode.Return))
//            {
//                return UserInputCommands.ChangeLevel;
//            }
//            else if (Input.GetKeyUp(KeyCode.Escape))
//            {
//                return UserInputCommands.CloseGame;
//            }

//            return UserInputCommands.None;
//        }
//    }
//}
