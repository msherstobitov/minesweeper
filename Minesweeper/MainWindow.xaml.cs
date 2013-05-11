using System;
using System.Windows;

namespace Minesweeper
{
    partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var game = new Game(16, 30, 99, minefield);

            minefield.MouseLeftButtonDown += (sender, e) => 
                {
                    game.OpenSquare(e.GetPosition(minefield));
                    
                };

            minefield.MouseRightButtonDown += (sender, e) =>
                {
                    game.MarkSquare(e.GetPosition(minefield));

                    lblMinesLeft.Content = game.MinesLeft;
                };

            btnRestart.Click += (sender, e) =>
                {
                    minefield.Children.Clear();

                    game = new Game(16, 30, 99, minefield);

                    lblMinesLeft.Content = game.MinesLeft;
                };
        }
    }
}
