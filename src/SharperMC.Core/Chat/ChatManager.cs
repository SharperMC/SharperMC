// Distrubuted under the MIT license
// ===================================================
// SharperMC uses the permissive MIT license.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the “Software”), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software
// 
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// ©Copyright SharperMC - 2020

using System;
using SharperMC.Core.Entity;
using SharperMC.Core.Enums;
using SharperMC.Core.Utils;

namespace SharperMC.Core.Chat
{
    public class ChatManager
    {
        public char Prefix { get; set; }
        public ChatManager()
        {
            Prefix = '/';
        }
        /// <summary>
        /// Prepares the message for chat.
        /// </summary>
        /// <param name="source">The player that send the message.</param>
        /// <param name="message">The message.</param>
        /// <returns>Formated chat message</returns>
        public virtual string FormatMessage(Player source, string message)
        {
            return String.Format("<{0}> {1}", source.Username, message);
        }
        
        /*
         * TODO: Implement permission manager
         */
        public void HandleCommand(string message, Player player)
        {
            try
            {
                var commandText = message.Split(' ')[0];
                message = message.Replace(commandText, "").Trim();
                commandText = commandText.Replace("/", "").Replace(".", "");

                var arguments = message.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                switch (commandText.ToLower())
                {
                    case "y":
                        player.Teleport(new PlayerLocation(player.KnownPosition.X, 80, player.KnownPosition.Z));
                        break;
                    case "shutdown":
                    case "stop":
                        Globals.StopServer();
                        break;
                    case "gamemode":
                        if (player != null)
                        {
                            if(arguments.Length == 0)
                                player.SendChat("Your current gamemode is [" + player.Gamemode + "]", ChatColor.Gray);
                            else if(arguments.Length >= 1)
                                switch (arguments[0].ToLower())
                                {
                                    case "survival":
                                        player.SetGamemode(Gamemode.Survival);
                                        break;
                                    case "spectator":
                                        player.SetGamemode(Gamemode.Spectator);
                                        break;
                                    case "adventure":
                                        player.SetGamemode(Gamemode.Adventure);
                                        break;
                                    default:
                                        player.SetGamemode(Gamemode.Creative);
                                        break;
                                }
                        }
                        else
                        {
                            ConsoleFunctions.WriteInfoLine("Cannot change gamemode through console.");
                        }
                        break;
                    case "save":
                        foreach (Player allPlayer in Globals.LevelManager.GetAllPlayers())
                        {
                            allPlayer.SavePlayer();
                        }
                        Globals.LevelManager.SaveAllChunks();
                        ConsoleFunctions.WriteInfoLine("World & Player data saved.");
                        if(player != null)
                            player.SendChat("World & Player data saved.", ChatColor.Green);
                        break;
                    default:
                        if (player != null)
                            player.SendChat("Unknown command.", ChatColor.Red);
                        else
                            ConsoleFunctions.WriteInfoLine("Unknown command.", ChatColor.Red);
                        break;
                }
            }
            catch (Exception ex)
            {
                ConsoleFunctions.WriteWarningLine(ex.ToString());
                if(player != null)
                    player.SendChat("An error occured when executing command.", ChatColor.Red);
            }
        }
    }
}