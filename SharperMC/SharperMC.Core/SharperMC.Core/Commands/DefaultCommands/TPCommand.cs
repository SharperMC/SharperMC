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
using System.Collections.Generic;
using SharperMC.Core.Entity;
using SharperMC.Core.Enums;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Console;
using SharperMC.Core.Utils.Entities.Player;

namespace SharperMC.Core.Commands.DefaultCommands
{
    public class TPCommand : Command
    {
        public TPCommand() : base("tp", new[] {"teleport"}, "tp <x> <y> <z> [yaw] [pitch]", "Teleports you.")
        {
        }

        public override void Execute(ICommandSender sender, string label, string[] args)
        {
            if (sender.IsPlayer())
            {
                var player = (Player) sender;
                if (args.Length != 3 && args.Length != 5)
                {
                    SendUsage(sender, label);
                    return;
                }

                //player.Teleport(new PlayerLocation(player.KnownPosition.X, 80, player.KnownPosition.Z));
                //todo: ~ is relative  then get int from arg and tell player if it's not a number
                var rotation = args.Length == 5;
                var relX = args[0].StartsWith("~");
                var relY = args[1].StartsWith("~");
                var relZ = args[2].StartsWith("~");
                double posX = 0;
                double posY = 0;
                double posZ = 0;
                var yaw = player.KnownPosition.Yaw;
                var pitch = player.KnownPosition.Pitch;
                var msg = "x";
                var attemptConversion = "";
                try
                {
                    if (!relX || args[0].Length > 1)
                        posX = Convert.ToDouble(attemptConversion = relX ? args[0].Substring(1) : args[0]);
                    msg = "y";
                    if (!relY || args[1].Length > 1)
                        posY = Convert.ToDouble(attemptConversion = relY ? args[1].Substring(1) : args[1]);
                    msg = "z";
                    if (!relZ || args[2].Length > 1)
                        posZ = Convert.ToDouble(attemptConversion = relZ ? args[2].Substring(1) : args[2]);
                    if (rotation)
                    {
                        msg = "yaw";
                        yaw = (float) Convert.ToDouble(attemptConversion = args[3]);
                        msg = "pitch";
                        pitch = (float) Convert.ToDouble(attemptConversion = args[4]);
                    }
                }
                catch (FormatException e)
                {
                    sender.SendChat($"Not a number argument {msg}: {attemptConversion}");
                    return;
                }

                player.Teleport(new PlayerLocation(posX += (relX ? player.KnownPosition.X : 0),
                    posY += (relY ? player.KnownPosition.Y : 0),
                    posZ += (relZ ? player.KnownPosition.Z : 0), yaw, pitch));
                sender.SendChat($"Teleported to: {posX}, {posY}, {posZ}" + (rotation ? $", {yaw}, {pitch}" : ""));
            }
            else
            {
                ConsoleFunctions.WriteInfoLine("Cannot teleport console.");
            }
        }

        public override IEnumerable<string> TabComplete(ICommandSender sender, string label, string[] args)
        {
            return new string[0];
        }
    }
}