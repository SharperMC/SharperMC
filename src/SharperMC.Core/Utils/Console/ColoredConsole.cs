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
using SharperMC.Core.Commands;

namespace SharperMC.Core.Utils.Console
{
    public class ColoredConsole
    {
        private readonly ConsoleColor _color = ConsoleColor.White;
        private readonly ConsoleColor _hintColor = ConsoleColor.Gray;
        private readonly ConsoleColor _errorColor = ConsoleColor.Red;
        private List<string> _history = new List<string>();
        private int _historyIndex;
        private string _savedInput = "";
        private string _hintedArg = "";
        private string _currentInputStrip = "";
        private string _currentInput = "";
        private string _currentArg = "";
        private string _leftInput = "";
        private string _rightInput = "";
        private int _cursor;
        private bool _gotHints;
        private List<string> _hints;
        private int _hintsIndex;

        public string GetNextString()
        {
            Reset();
            while (true)
            {
                var input = System.Console.ReadKey();

                switch (input.Key)
                {
                    case ConsoleKey.Tab:
                        if (_gotHints)
                        {
                            // TODO: Better this stuff
                            _hintsIndex++;
                            if (_hintsIndex >= _hints.Count) _hintsIndex = 0;
                            _hintedArg = _hints[_hintsIndex];
                        }
                        else
                        {
                            _hints = CommandManager.ParseTab(Globals.ConsoleSender, _currentInputStrip);
                            _gotHints = true;
                            _hintsIndex = 0;
                            if (_hints.Count == 1) _hintedArg = _hints[0];
                        }

                        break;
                    case ConsoleKey.Enter:
                        _history.Add(_currentInput);
                        return _currentInput;
                    case ConsoleKey.Backspace:
                        if (_currentInput.Length > 0 && _cursor > 0)
                        {
                            _leftInput = _leftInput.Substring(0, _leftInput.Length - 1);
                            SetInput();
                            OffsetCursor(-1);
                        }

                        break;
                    case ConsoleKey.Delete:
                        if (_currentInput.Length > 0 && _cursor < _currentInput.Length)
                        {
                            _rightInput = _rightInput.Substring(1, _rightInput.Length - 1);
                            SetInput();
                            OffsetCursor(0);
                        }

                        break;
                    case ConsoleKey.LeftArrow:
                        OffsetCursor(-1);
                        break;
                    case ConsoleKey.RightArrow:
                        OffsetCursor(1);
                        break;
                    case ConsoleKey.UpArrow:
                        SetHistory(1);
                        break;
                    case ConsoleKey.DownArrow:
                        SetHistory(-1);
                        break;
                    case ConsoleKey.Escape:
                        break;
                    case ConsoleKey.Spacebar:
                        SpaceReset();
                        goto SHENANIGAN;
                    default:
                        SHENANIGAN:
                        if (input.KeyChar == 0) break;
                        _leftInput += input.KeyChar;
                        _currentInput = _leftInput + _rightInput;
                        OffsetCursor(1);
                        _leftInput = _currentInput.Substring(0, _cursor);
                        _rightInput = _currentInput.Substring(_cursor, _currentInput.Length - _cursor);
                        break;
                }

                ConsoleFunctions.ClearCurrentConsoleLine();
                System.Console.Write(_currentInput);// + "|" + _leftInput + ":" + _rightInput); // debug
                System.Console.CursorLeft = _cursor;
            }
        }

        private void SetInput()
        {
            var input = _leftInput + _rightInput;
            _currentInput = input;
            var cisS = input.LastIndexOf(' ');
            _currentInputStrip = input.Substring(0, cisS == -1 ? input.Length : cisS).Trim();
            var leftLio = _leftInput.LastIndexOf(' ');
            var rightLio = _rightInput.LastIndexOf(' ');
            _currentArg = _leftInput.Substring(leftLio + 1, _leftInput.Length - leftLio - 1) +
                          _rightInput.Substring(rightLio >= 0 ? rightLio : 0,
                              rightLio >= 0 ? (_rightInput.Length - rightLio) : 0);
        }

        private void UpdateInputCursor()
        {
            _leftInput = _currentInput.Substring(0, _cursor);
            _rightInput = _currentInput.Substring(_cursor, _currentInput.Length - _cursor);
        }

        private void SpaceReset()
        {
            _gotHints = false;
            _hints = new List<string>();
            _hintedArg = "";
        }

        private void OffsetCursor(int offset)
        {
            _cursor += offset;
            if (_cursor < 0) _cursor = 0;
            else if (_cursor > _currentInput.Length) _cursor = _currentInput.Length;
            if (_cursor < _currentInput.Length && _currentInput[_cursor] == ' ') SpaceReset();
            UpdateInputCursor();
        }

        private void SetHistory(int offset)
        {
            if (_historyIndex == 0) _savedInput = _currentInput;
            _historyIndex += offset;
            if (_historyIndex < 0) _historyIndex = 0;
            else if (_historyIndex > _history.Count) _historyIndex = _history.Count;
            _currentInput = _historyIndex == 0 ? _savedInput : _history[_historyIndex - 1];

            _cursor = _currentInput.Length;
        }

        private void Reset()
        {
            _leftInput = "";
            _rightInput = "";
            _currentInput = "";
            _historyIndex = 0;
            _cursor = 0;
            _hints = new List<string>();
            _gotHints = false;
        }
    }
}