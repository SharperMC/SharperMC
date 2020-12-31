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

using SharperMC.Core.Entity;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Console;

namespace SharperMC.Core.Commands.DefaultCommands
{
    public class SaveCommand : Command
    {

        public SaveCommand() : base("save", "save", "Saves the world.")
        {
        }
        
        public override void Execute(ICommandSender sender, string label, string[] args)
        {
            foreach (Player allPlayer in Globals.LevelManager.GetAllPlayers())
            {
                allPlayer.SavePlayer();
            }
            Globals.LevelManager.SaveAllChunks();
            ConsoleFunctions.WriteInfoLine("World & Player data saved.");
        }

        public override string[] TabComplete(ICommandSender sender, string label, string[] args)
        {
            return new string[0];
        }
    }
}