using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Minesweeper
{
    class Game
    {
        private const int lengthOfSquare = 20;
        private Canvas _minefield;
        private TextBlock[,] _content;
        private Rectangle[,] _squares;
        private bool[,] _isMarked;

        public int MinesLeft { get; private set; }


        public Game(int heightInSquares, int widthInSquares, int numberOfMines, Canvas minefield)
        {
            _minefield = minefield;
            _minefield.Height = heightInSquares * lengthOfSquare;
            _minefield.Width = widthInSquares * lengthOfSquare;

            _squares = new Rectangle[heightInSquares, widthInSquares];
            _content = new TextBlock[heightInSquares, widthInSquares];
            _isMarked = new bool[heightInSquares, widthInSquares];

            MinesLeft = numberOfMines;

            RenderGrid();
            AddMines();
            AddNumbers();
        }

        public void OpenSquare(Point point)
        {
            int row = (int)point.Y / lengthOfSquare;
            int col = (int)point.X / lengthOfSquare;

            if (!_isMarked[row, col])
            {
                if (_content[row, col].IsVisible)
                {
                    Chord(row, col);
                }

                _squares[row, col].Fill = Brushes.LightCyan;
                _content[row, col].Visibility = Visibility.Visible;

                if (_content[row, col].Text == string.Empty)
                {
                    OpenEmptySquares(row, col);
                }
                else if (_content[row, col].Text == "*")
                {
                    GameOver();
                }
            }
        }

        public void MarkSquare(Point point)
        {
            int row = (int)point.Y / lengthOfSquare;
            int col = (int)point.X / lengthOfSquare;

            if (!_content[row, col].IsVisible)
            {
                if (_squares[row, col].Fill == Brushes.LightBlue)
                {
                    _squares[row, col].Fill = Brushes.OrangeRed;
                    _squares[row, col].StrokeThickness = 5;
                    _squares[row, col].Stroke = Brushes.LightBlue;

                    MinesLeft--;
                    _isMarked[row, col] = true;
                }
                else
                {
                    _squares[row, col].Fill = Brushes.LightBlue;
                    _squares[row, col].StrokeThickness = 1;
                    _squares[row, col].Stroke = Brushes.DarkCyan;

                    MinesLeft++;
                    _isMarked[row, col] = false;
                }
            }
        }


        private void RenderGrid()
        {
            double leftMargin = 0;
            double topMargin = 0;

            for (int row = 0; row < _squares.GetLength(0); row++)
            {
                for (int col = 0; col < _squares.GetLength(1); col++)
                {
                    _squares[row, col] = new Rectangle
                    {
                        Width = lengthOfSquare,
                        Height = lengthOfSquare,
                        StrokeThickness = 1,
                        Stroke = Brushes.DarkCyan,
                        Fill = Brushes.LightBlue,
                        Margin = new Thickness(leftMargin, topMargin, 0, 0)
                    };

                    _content[row, col] = new TextBlock
                    {
                        FontSize = 16,
                        FontFamily = new FontFamily("Comic Sans MS"),
                        Visibility = Visibility.Hidden,
                        Margin = new Thickness(leftMargin + 5, topMargin - 2, 0, 0)
                    };

                    _minefield.Children.Add(_squares[row, col]);
                    _minefield.Children.Add(_content[row, col]);

                    leftMargin += lengthOfSquare;
                }

                leftMargin = 0;
                topMargin += lengthOfSquare;
            }
        }

        private void AddMines()
        {
            Random rnd = new Random();
            int random;
            int minesCounter = MinesLeft;

            while (minesCounter > 0)
            {
                for (int row = 0; row < _squares.GetLength(0); row++)
                {
                    for (int col = 0; col < _squares.GetLength(1); col++)
                    {
                        random = rnd.Next(5);

                        if (random == 0 && minesCounter > 0 && _content[row, col].Text != "*")
                        {
                            _content[row, col].Text = "*";
                            _content[row, col].Foreground = Brushes.Black;
                            _content[row, col].FontSize = 23;

                            minesCounter--;
                        }
                    }
                }
            }
        }

        private void AddNumbers()
        {
            int minesAround = 0;

            for (int row = 0; row < _squares.GetLength(0); row++)
            {
                for (int col = 0; col < _squares.GetLength(1); col++)
                {
                    if (_content[row, col].Text == "*")
                        continue;

                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            if (row + j >= 0 && row + j < _squares.GetLength(0) &&
                                col + i >= 0 && col + i < _squares.GetLength(1) &&
                                _content[row + j, col + i].Text == "*")
                            {
                                minesAround++;
                            }
                        }
                    }

                    if (minesAround > 0)
                    {
                        _content[row, col].Text = minesAround.ToString();

                        switch (minesAround)
                        {
                            case 1:
                                _content[row, col].Foreground = Brushes.Blue;
                                break;
                            case 2:
                                _content[row, col].Foreground = Brushes.Green;
                                break;
                            case 3:
                                _content[row, col].Foreground = Brushes.Red;
                                break;
                            case 4:
                                _content[row, col].Foreground = Brushes.DarkBlue;
                                break;
                            case 5:
                                _content[row, col].Foreground = Brushes.DarkRed;
                                break;
                            case 6:
                                _content[row, col].Foreground = Brushes.DarkOliveGreen;
                                break;
                            case 7:
                                _content[row, col].Foreground = Brushes.DarkViolet;
                                break;
                            case 8:
                                _content[row, col].Foreground = Brushes.DarkOrange;
                                break;
                        }

                        minesAround = 0;
                    }
                }
            }
        }

        private void OpenEmptySquares(int row, int col)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (row + j >= 0 && row + j < _squares.GetLength(0) &&
                        col + i >= 0 && col + i < _squares.GetLength(1) &&
                        _content[row + j, col + i].Text == string.Empty &&
                        !_content[row + j, col + i].IsVisible && !_isMarked[row + j, col + i])
                    {
                        _squares[row + j, col + i].Fill = Brushes.LightCyan;
                        _content[row + j, col + i].Visibility = Visibility.Visible;

                        OpenEmptySquares(row + j, col + i);
                    }
                }
            }

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (row + j >= 0 && row + j < _squares.GetLength(0) &&
                        col + i >= 0 && col + i < _squares.GetLength(1) &&
                            !_isMarked[row + j, col + i])
                    {
                        _squares[row + j, col + i].Fill = Brushes.LightCyan;
                        _content[row + j, col + i].Visibility = Visibility.Visible;

                        if (_content[row + j, col + i].Text == "*")
                            GameOver();
                    }
                }
            }
        }

        private void Chord(int row, int col)
        {
            int markedAround = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (row + j >= 0 && row + j < _squares.GetLength(0) &&
                        col + i >= 0 && col + i < _squares.GetLength(1) &&
                        _isMarked[row + j, col + i])
                    {
                        markedAround++;
                    }
                }
            }

            if (markedAround.ToString() == _content[row, col].Text)
            {
                OpenEmptySquares(row, col);
            }
        }

        private void GameOver()
        {
            for (int row = 0; row < _squares.GetLength(0); row++)
            {
                for (int col = 0; col < _squares.GetLength(1); col++)
                {
                    _content[row, col].Visibility = Visibility.Visible;
                }
            }
        }

    }
}
