using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightsOutUWP {
    class LightsOutModel {
        internal class LightsOutGame {
            private int gridSize = 3;
            private bool[,] gridArray;
            private Random rand;
            public const int MaxGridSize = 7;
            public const int MinGridSize = 3;

            public int GridSize {
                get {
                    return gridSize;
                }

                set {
                    if (value >= MinGridSize && value <= MaxGridSize) {
                        if (GridSize != value) {
                            gridSize = value;
                            gridArray = new bool[gridSize, gridSize];
                            NewGame();
                        }
                    }
                }
            }

            public string Grid {
                get {
                    return ToString();
                }

                set {
                    StringToGrid(value);
                }
            }

            public override string ToString() {
                string gridValues = "";
                for (int r = 0; r < gridSize; r++) {
                    for (int c = 0; c < gridSize; c++) {
                        if (gridArray[r,c] == true) {
                            gridValues += "T";
                        } else {
                            gridValues += "F";
                        }
                    }
                }

                return gridValues;
            }

            private void StringToGrid(string value) {
                int r = 0;
                int c = 0;

                foreach (char character in value) {
                    if (character == 'T') {
                        gridArray[r, c] = true;
                    } else {
                        gridArray[r, c] = false;
                    }

                    if (c < GridSize-1) {
                        c++;
                    } else {
                        c = 0;
                    }

                    if (c == 0) {
                        r++;
                    }
                }
            }

            public LightsOutGame() {
                rand = new Random();
                GridSize = MinGridSize;
                gridArray = new bool[GridSize,GridSize];
            }

            public bool GetGridValue(int row, int col) {
                return gridArray[row, col];
            }

            public void NewGame() {
                for (int r = 0; r < gridSize; r++) {
                    for (int c = 0; c < gridSize; c++) {
                        gridArray[r, c] = rand.Next(2) == 1;
                    }
                }
            }

            public void Move(int row, int col) {
                if (row < 0 || row >= gridSize || col < 0 || col >= gridSize) {
                    throw new ArgumentException("Row or colum is outisde the legal range of 0 to " + (gridSize - 1));
                }

                for (int i = row - 1; i <= row + 1; i++) {
                    for (int j = col - 1; j <= col + 1; j++) {
                        if (i >= 0 && i < gridSize && j >= 0 && j < gridSize) {
                            gridArray[i, j] = !gridArray[i, j];
                        }
                    }
                }
            }

            public bool IsGameOver() {
                for (int r = 0; r < gridSize; r++) {
                    for (int c = 0; c < gridSize; c++) {
                        if (gridArray[r, c]) {
                            return false;
                        }
                    }
                }

                return true;
            }
        }
    }
}
