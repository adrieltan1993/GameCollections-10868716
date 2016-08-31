using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SudokuUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SudokuPuzzle _puzzle;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonSolve_Click(object sender, RoutedEventArgs e)
        {
            if (_puzzle == null)
            {
                buttonSet_Click(sender, e);
            }
            _puzzle.solve(false);


            printPuzzle();
        }

        private void printPuzzle()
        {
            int[][] puzzleValues = _puzzle.getPuzzle();
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (puzzleValues[r][c] != 0)
                    {
                        var gridElement = gridSudoku.Children.Cast<TextBox>().First(i => Grid.GetColumn(i) == r && Grid.GetRow(i) == c);
                        gridElement.Text = puzzleValues[r][c].ToString();
                    }
                }
            }
        }

        private void buttonSet_Click(object sender, RoutedEventArgs e)
        {
            int[][] inputSudoku = new int[9][];
            for (int r = 0; r < 9; r++)
            {
                inputSudoku[r] = new int[9];
                for (int c = 0; c < 9; c++)
                {
                    var gridElements = gridSudoku.Children.Cast<UIElement>().Where(i => Grid.GetColumn(i) == r && Grid.GetRow(i) == c);
                    var gridElement = gridSudoku.Children.Cast<TextBox>().First(i => Grid.GetColumn(i) == r && Grid.GetRow(i) == c);
                    //var gridElement = gridElements.Where(i => i.GetType() == typeof(TextBox)).Cast<TextBox>().First();
                    if (gridElement.Text != "")
                    {
                        inputSudoku[r][c] = Int32.Parse(gridElement.Text);
                        gridElement.FontWeight = FontWeights.Bold;
                        gridElement.Foreground = Brushes.Blue;
                        gridElement.IsReadOnly = true;
                    }
                    else
                    {
                        inputSudoku[r][c] = 0;
                    }
                }
            }
            _puzzle = new SudokuPuzzle(inputSudoku);
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    var gridElement = gridSudoku.Children.Cast<TextBox>().First(i => Grid.GetColumn(i) == r && Grid.GetRow(i) == c);
                    gridElement.Text = "";
                    gridElement.IsReadOnly = false;
                    gridElement.FontWeight = FontWeights.Normal;
                    gridElement.Foreground = Brushes.Black;
                }
            }
        }

        private void buttonHint_Click(object sender, RoutedEventArgs e)
        {
            _puzzle.solve(true);
            printPuzzle();
        }
    }
}
