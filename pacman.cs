using System;
using System.Diagnostics;
// using System.Threading.Tasks; // hvis det må benyttes threading

namespace clipacman
{
    class Program {
        static void Main(string[] args) {
            // Grid gameGrid = new Grid(4,4);
            // Console.WriteLine(gameGrid.GridMatrix[0,0]);
            // Console.WriteLine(gameGrid.FetchNextValue());
            // gameGrid.DecrementAll();
            // Console.WriteLine(gameGrid.IsDead());
            // Console.WriteLine(gameGrid.GridMatrix[0,0]);
            // Console.WriteLine(gameGrid.FetchNextValue());
            // gameGrid.PrintGrid();
            Console.WriteLine("Welcome!");
            Console.WriteLine("This is snake. Use W,A,S,D to play.");
            Console.WriteLine("Do note that this was written quick and dirty.");
            Console.WriteLine("As a result, the larger the grid, the worse it runs.");
            Console.WriteLine("A grid size of 8 by 8 is recommended.");
            Console.WriteLine("Use larger grids at your own risk.");
            Console.WriteLine("Enter x: (int)");
            string val = Console.ReadLine();
            int grid_x = Convert.ToInt32(val);
            Console.WriteLine("Enter y: (int)");
            val = Console.ReadLine();
            int grid_y = Convert.ToInt32(val);
            Grid gameGrid = new Grid(grid_x,grid_y);
            gameGrid.PrintGrid();
            InputGameLoop(gameGrid);
        }

        static void gameLoop(Grid gamegrid){
            Stopwatch timer = new Stopwatch();
            timer.Start();
            while(timer.Elapsed.TotalSeconds < 1)
            {
                Console.WriteLine("Waiting for keypress");
                string inputKey = Console.ReadKey(true).Key.ToString();
                Console.WriteLine("Keypress recieved");
                Console.WriteLine(inputKey);
                switch (inputKey) {
                case "W":
                    gamegrid.CurrDirection = Direction.UP;
                    continue;
                default:
                    continue;
                // do something here
                }
            }
            gamegrid.PerformMove();
            gameLoop(gamegrid);
        }
        static void InputGameLoop(Grid gamegrid){
            Stopwatch timer = new Stopwatch();
            timer.Start();
            while(timer.Elapsed.TotalSeconds < 0.5)
            {
                if (Console.KeyAvailable)
                {
                    string inputKey = Console.ReadKey(true).Key.ToString();
                    switch (inputKey)
                    {
                        case "W":
                            gamegrid.CurrDirection = Direction.UP;
                            continue;
                        case "S":
                            gamegrid.CurrDirection = Direction.DOWN;
                            continue;
                        case "A":
                            gamegrid.CurrDirection = Direction.LEFT;
                            continue;
                        case "D":
                            gamegrid.CurrDirection = Direction.RIGHT;
                            continue;
                        default:
                            continue;
                    }
                }
            }
            gamegrid.PerformMove();
            InputGameLoop(gamegrid);
        }
    }
}