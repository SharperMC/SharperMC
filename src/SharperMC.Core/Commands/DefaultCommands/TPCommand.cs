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

namespace SharperMC.Core.Commands.DefaultCommands
{
    public class TPCommand : Command
    {
        public TPCommand() : base("tp", new[] {"teleport"}, "/tp <x> <y> <z> [yaw] [pitch]", "Teleports you.")
        {
        }

        public override void Execute(ICommandSender sender, string label, string[] args)
        {
            if (sender.IsPlayer())
            {
                var player = (Player) sender;
                if (args.Length < 3 || args.Length == 4 || args.Length > 5)
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
                double posX;
                double posY;
                double posZ;
                var yaw = player.KnownPosition.Yaw;
                var pitch = player.KnownPosition.Pitch;
                var msg = "x";
                try
                {
                    posX = Convert.ToDouble(relX ? args[0].Substring(1) : args[0]);
                    msg = "y";
                    posY = Convert.ToDouble(relY ? args[1].Substring(1) : args[1]);
                    msg = "z";
                    posZ = Convert.ToDouble(relZ ? args[2].Substring(1) : args[2]);
                    if (rotation)
                    {
                        msg = "yaw";
                        yaw = (float) Convert.ToDouble(args[3]);
                        msg = "pitch";
                        pitch = (float) Convert.ToDouble(args[4]);
                    }
                }
                catch (FormatException e)
                {
                    sender.SendChat($"Not a number: {msg}");
                    return;
                }

                player.Teleport(new PlayerLocation(posX += (relX ? player.KnownPosition.X : 0),
                    posY += (relY ? player.KnownPosition.Y : 0),
                    posZ += (relZ ? player.KnownPosition.Z : 0), yaw, pitch));
                sender.SendChat($"Teleported to: {posX}, {posY}, {posZ}" + (rotation ? "" : $", {yaw}, {pitch}"));
            }
            else
            {
                ConsoleFunctions.WriteInfoLine("Cannot teleport console.");
            }
        }

        public override string[] TabComplete(ICommandSender sender, string label, string[] args)
        {
            return new string[0];
        }
    }
}