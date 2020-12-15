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
        public bool Printed;
        private const ConsoleColor HintColor = ConsoleColor.Gray;
        private const ConsoleColor SelectedHintColor = ConsoleColor.DarkGray;
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
        private List<string> _hints = new List<string>();
        private List<string> _rawHints = new List<string>();
        private int _hintsIndex;
        private ConsoleKeyInfo _input;

        public string GetNextString()
        {
            Reset();
            PrintInput();
            while (true)
            {
                _input = System.Console.ReadKey();

                switch (_input.Key)
                {
                    case ConsoleKey.Tab:
                        if (_gotHints)
                        {
                            if (_hints.Count == 0) break;
                            if (_hints.Count == 1) SetHint();
                            _hintsIndex++;
                            if (_hintsIndex >= _hints.Count) _hintsIndex = 0;
                            _hintedArg = _hints[_hintsIndex];
                        }
                        else
                        {
                            _hints = _rawHints = CommandManager.ParseTab(Globals.ConsoleSender, _currentInputStrip);
                            _gotHints = true;
                            _hintsIndex = 0;
                            UpdateHints();
                            if (_hints.Count == 1) _hintedArg = _hints[0];
                        }

                        break;
                    case ConsoleKey.Enter:
                        if (string.IsNullOrEmpty(_hintedArg))
                        {
                            _history.Add(_currentInput); 
                            return _currentInput;
                        }

                        System.Console.CursorTop--;
                        SetHint();
                        break;
                    case ConsoleKey.Backspace:
                        if (_currentInput.Length > 0 && _cursor > 0)
                        {
                            if (_cursor > 0 && _currentInput[_cursor - 1] == ' ') ResetHints();
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
                    case ConsoleKey.Escape:
                        ResetHints();
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
                    case ConsoleKey.Spacebar:
                        ResetHints();
                        goto SHENANIGAN;
                    default:
                        SHENANIGAN:
                        if (_input.KeyChar == 0) break;
                        _leftInput += _input.KeyChar;
                        SetInput();
                        OffsetCursor(1);
                        _leftInput = _currentInput.Substring(0, _cursor);
                        _rightInput = _currentInput.Substring(_cursor, _currentInput.Length - _cursor);
                        break;
                }

                PrintInput();
            }
        }

        private void PrintInput()
        {
            var color = System.Console.ForegroundColor;
            if (Printed)
            {
                System.Console.WriteLine();
                Printed = false;
            }

            ConsoleFunctions.ClearCurrentConsoleLine();
            System.Console.CursorTop--;
            ConsoleFunctions.ClearCurrentConsoleLine();
            System.Console.ResetColor();
            for (var i = 0; i < _hints.Count; i++)
            {
                var hint = _hints[i];
                System.Console.ResetColor();
                if (i == _hintsIndex) System.Console.ForegroundColor = SelectedHintColor;
                System.Console.Write(hint);
                System.Console.ResetColor();
                if (i < _hints.Count - 1) System.Console.Write(", ");
            }

            System.Console.Write("\n");
            System.Console.Write(_leftInput);
            System.Console.ForegroundColor = HintColor;
            if (!string.IsNullOrEmpty(_hintedArg))
                System.Console.Write(_hintedArg.Substring(_currentArg.Length) + " ");
            System.Console.ResetColor();
            System.Console.Write(_rightInput);
            System.Console.CursorLeft = _cursor;
            System.Console.ForegroundColor = color;
        }

        private void SetInput()
        {
            SetInput(_leftInput + _rightInput);
        }

        private void SetInput(string input)
        {
            _currentInput = input;
            var cisS = input.LastIndexOf(' ');
            _currentInputStrip = input.Substring(0, cisS == -1 ? 0 : cisS).Trim();
            var leftLio = _leftInput.LastIndexOf(' ');
            // var rightLio = _rightInput.LastIndexOf(' ');
            _currentArg = _leftInput.Substring(leftLio + 1, _leftInput.Length - leftLio - 1)/* +
                          _rightInput.Substring(rightLio >= 0 ? rightLio : 0,
                              rightLio >= 0 ? (_rightInput.Length - rightLio) : 0)*/;
            UpdateHints();
        }

        private void UpdateInputCursor()
        {
            _leftInput = _currentInput.Substring(0, _cursor);
            _rightInput = _currentInput.Substring(_cursor, _currentInput.Length - _cursor);
        }

        private void UpdateHints()
        {
            var lowerArg = _currentArg.ToLower();
            _hints = _rawHints.FindAll(s => s.ToLower().StartsWith(lowerArg));
            if (_hintsIndex >= _hints.Count) _hintsIndex = 0;
            if (_hints.Count == 0) _hintedArg = "";
        }

        private void ResetHints()
        {
            _gotHints = false;
            _hints = new List<string>();
            _rawHints = new List<string>();
            _hintedArg = "";
        }

        private void SetHint()
        {
            var addTo = _hintedArg.Substring(_currentArg.Length) + " ";
            _leftInput += addTo;
            SetInput();
            OffsetCursor(addTo.Length);

            ResetHints();
        }

        private void OffsetCursor(int offset)
        {
            _cursor += offset;
            if (_cursor < 0) _cursor = 0;
            else if (_cursor > _currentInput.Length) _cursor = _currentInput.Length;
            if (_cursor < _currentInput.Length && _currentInput[_cursor] == ' ') ResetHints();
            UpdateInputCursor();
            SetInput();
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
            _rawHints = new List<string>();
            _gotHints = false;
        }
    }
}