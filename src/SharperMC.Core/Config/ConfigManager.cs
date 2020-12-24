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
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using SharperMC.Core.Enums;
using SharperMC.Core.Utils.Console;

namespace SharperMC.Core.Config
{
	public class ConfigManager
	{
		public string ConfigName { get; }
		private string[] Config { get; set; }
		
		public ConfigManager(string name)
		{
			ConfigName = name;
			if (!ConfigExists())
				WriteDefault();
			else
				Config = ReadConfig();
		}

		private void WriteDefault()
		{
			if(File.Exists(ConfigName))
				File.WriteAllText(ConfigName, String.Empty);
			foreach (FieldInfo field in Server.ServerSettings.GetType().GetFields())
			{
				File.AppendAllText(ConfigName, field.Name + "=" + field.GetValue(Server.ServerSettings) + Environment.NewLine);
			}
		}

		private bool CheckAuthenticity()
		{
			foreach (FieldInfo field in Server.ServerSettings.GetType().GetFields())
			{
				if (File.ReadAllText(ConfigName).Contains(field.Name))
					continue;
				ConsoleFunctions.WriteInfoLine(field.Name + " " + field.GetValue(Server.ServerSettings));
				WriteDefault();
				return false;
			}
			return true;
		}

		public bool ConfigExists()
		{
			if (!String.IsNullOrEmpty(ConfigName) && File.Exists(ConfigName))
				return true;
			return false;
		}

		public dynamic GetProperty(string property, dynamic defaultValue)
		{
			foreach (string line in File.ReadAllLines(ConfigName))
			{
				string[] split = line.Split('=');
				if (split.Length >= 2)
				{
					if (!String.IsNullOrEmpty(split[0]) && split[0].Equals(property, StringComparison.InvariantCultureIgnoreCase))
					{
						return TypeDescriptor.GetConverter(defaultValue.GetType()).ConvertFromString(split[1]);
					}
				}
			}
			ConsoleFunctions.WriteDebugLine("Property value couldn't be found (Property: {0}, Value: {1})", true, property, defaultValue);
			return defaultValue;
		}

		private string[] ReadConfig()
		{
			CheckAuthenticity();
			if (ConfigExists())
				return File.ReadAllLines(ConfigName);
			return null;
		}

		public void ReloadConfig()
		{
			Server.LoadServerSettingsFromFile();
		}

		public void SaveConfig()
		{
			Config = ReadConfig();
		}
	}
}