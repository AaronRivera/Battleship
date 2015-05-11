using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            //adjust the console screen
            Console.WindowWidth = 50;
            Console.BufferWidth = 50;
            Console.WindowHeight = 30;
            Console.BufferHeight = 30;

            //cretes a new game
            Grid grid = new Grid();
            //calls the game 
            grid.PlayGame();

            //after the game ends
            Console.WriteLine("");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }

    /// <summary>
    /// Represents a single cell of the grid, its responsible for holding the status of the point
    /// </summary>
    public class Point
    {
        //X coordinate
        public int X { get; set; }
        //Y coordinate
        public int Y { get; set; }
        //Status of the point that can be empty, ship, hit, or miss
        public PointStatus Status { get; set; }
        //enumeration for the different status that a cell can have
        public enum PointStatus { Empty, Ship, Hit, Miss }

        /// <summary>
        /// creates a point and defines the status in the grid
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="p"></param>
        public Point(int x, int y, PointStatus p)
        {
            //creates a point wit the coordinates and status in the grid
            this.X = x;
            this.Y = y;
            this.Status = p;
        }
    }



    /// <summary>
    /// class ship
    /// </summary>
    public class Ship
    {
        //enumeration for types of ships and their values
        public enum ShipType { Carrier = 5, Battleship = 4, Cruiser = 3, Submarine = 3, Minesweeper = 2 }
        //defines the ship type
        public ShipType Type { get; set; }
        //defines the occupied points by the ship
        public List<Point> OccupiedPoints { get; set; }
        //the cells that the ships is going to occupy
        public int Lenght { get; set; }
        //returns true if all the points have been hit
        public bool IsDestroyed { get { return OccupiedPoints.All(x => x.Status == Point.PointStatus.Hit); } }

        /// <summary>
        /// Contructor 
        /// </summary>
        /// <param name="typeOfShip"></param>
        public Ship(ShipType typeOfShip)
        {
            //contructs the type of ship dicteted by the parameter
            //creates a new list that holds the cells that the ship is going to occupy
            this.OccupiedPoints = new List<Point>();
            //defines the type of ship
            this.Type = typeOfShip;
            //defines the lenght of the ship by the type of enumeration
            this.Lenght = (int)typeOfShip;
        }

    }
    public class Grid
    {
        //enumeration to keep track of the ship direction
        public enum PlaceShipDirection { Horizontal, Vertical }
        //the point in the ocean
        public Point[,] Ocean { get; set; }
        //list of all the ships held by the grid "ocean"
        public List<Ship> ListOfShips { get; set; }
        //keeps track if all the ships have been destroyed
        public bool AllShipsDestroyed
        {
            get
            {
                //flag to return
                bool allShipsSunk = false;
                //check every ship in the list of ships 
                foreach (Ship ship in ListOfShips)
                {
                    //if the current ship is destroyed
                    if (ship.IsDestroyed)
                        //the flag changes to true if all the ships are destroyed
                        allShipsSunk = true;
                    //if not destroys it means that there are ships left
                    else
                    {
                        //changes the flag to false and breaks the forech loop bc theres no need to check more
                        //since one ship hasnt been destroyed
                        allShipsSunk = false;
                        break;
                    }
                }
                // returns the value of the flag
                return allShipsSunk;
            }
        }

        //Holds what round is taking place
        public int CombatRound { get; set; }

        /// <summary>
        /// lays ou the grid aka ocean
        /// </summary>
        public Grid()
        {
            //the grid is going to be 10X10
            this.Ocean = new Point[10, 10];
            //for each point in the grid assigns empty value
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                    this.Ocean[x, y] = new Point(x, y, Point.PointStatus.Empty);
            }

            //List that holds the ships
            this.ListOfShips = new List<Ship>();
            //create ships
            this.ListOfShips.Add(new Ship(Ship.ShipType.Carrier));
            this.ListOfShips.Add(new Ship(Ship.ShipType.Battleship));
            this.ListOfShips.Add(new Ship(Ship.ShipType.Cruiser));
            this.ListOfShips.Add(new Ship(Ship.ShipType.Submarine));
            this.ListOfShips.Add(new Ship(Ship.ShipType.Minesweeper));


            //declares a random varable to choose a random spot in the grid
            Random rnd = new Random();
            //for each ship in the list of ships
            foreach (Ship shipToPlace in ListOfShips)
            {
                //placement flag
                bool placementFlag = false;
                //get the direction from a random then cast the result to Horizontal or Vertical
                PlaceShipDirection direction = (PlaceShipDirection)rnd.Next(0, 2);
                //coordinates for x
                int startX = 0;
                //coordinates for y
                int startY = 0;
                //coordinates x to check
                int xToCheck = 0;
                //coordinates y to check
                int yToCheck = 0;
                //checks 
                int check = 0;

                //while the ship hasnt been placed in the grid
                while (!placementFlag)
                {
                    //gets the random direction
                    direction = (PlaceShipDirection)rnd.Next(0, 2);
                    //chooses a number from 0 to 9 for X
                    startX = rnd.Next(0, 10);
                    //chooses a number from 0 to 9 for Y
                    startY = rnd.Next(0, 10);
                    //assigns these values to x,y to check to see if its a valid point to place the ship
                    xToCheck = startX;
                    yToCheck = startY;
                    //switch the direction

                    switch (direction)
                    {
                        //compare if the horizontal grid and size of the ship fit the grid
                        case PlaceShipDirection.Horizontal:
                            //do until the ship fits the coordinates
                            while (shipToPlace.Lenght > (9 - startX))
                            {
                                startX = rnd.Next(0, 10);
                                xToCheck = startX;
                            }
                            break;
                        //compare if the vertical grid and size of the ship fit the grid

                        case PlaceShipDirection.Vertical:
                            //do until the ship lenght fits the vertical grid
                            while (shipToPlace.Lenght > (9 - startY))
                            {
                                startY = rnd.Next(0, 10);
                                yToCheck = startY;
                            }
                            break;
                        default:
                            break;
                    }
                    //while the Grid status equals empty and check less than the ship to place size
                    while (Ocean[xToCheck, yToCheck].Status == Point.PointStatus.Empty && check < shipToPlace.Lenght)
                    {
                        switch (direction)
                        {
                            //if direction horizontal then increase xToCheck and check ++
                            case PlaceShipDirection.Horizontal:
                                xToCheck++;
                                check++;
                                break;
                            //if direction vertical then increase yTocheck and check ++
                            case PlaceShipDirection.Vertical:
                                yToCheck++;
                                check++;
                                break;
                            default:
                                break;
                        }
                    }
                    //if the points generated are empmty
                    if (Ocean[xToCheck, yToCheck].Status == Point.PointStatus.Empty)
                    {
                        //then place the ship
                        PlaceShip(shipToPlace, direction, startX, startY);
                        //changes the flag to true
                        placementFlag = true;
                    }
                }
            }
        }


        /// <summary>
        /// Places the ships in the gris
        /// </summary>
        /// <param name="shipToPlace"></param>
        /// <param name="direction"></param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        public void PlaceShip(Ship shipToPlace, PlaceShipDirection direction, int startX, int startY)
        {
            //loop for the size of the ship
            for (int i = 0; i < shipToPlace.Lenght; i++)
            {
                //changes the point from empty to occupied by a ship
                this.Ocean[startX, startY].Status = Point.PointStatus.Ship;
                //adds the ship coordinates to the list
                shipToPlace.OccupiedPoints.Add(this.Ocean[startX, startY]);

                switch (direction)
                {
                    //if the direction is horizontal
                    case PlaceShipDirection.Horizontal:
                        //increases start X
                        startX++;
                        break;
                    //if the direction is vertical
                    case PlaceShipDirection.Vertical:
                        //increases start Y
                        startY++;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Displays the grid and updates the grid
        /// </summary>
        public void DisplayOcean()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("              B A T T L E S H I P");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("");
            Console.ResetColor();

            //displays the grid
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("          0  1  2  3  4  5  6  7  8  9");
            Console.ResetColor();
            //draws the grid 
            for (int y = 0; y < 10; y++)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("       {0} ", y);
                Console.ResetColor();
                for (int x = 0; x < 10; x++)
                {
                    //depending on the cell status it will:
                    switch (Ocean[x, y].Status)
                    {
                        //draw a empty cell if its empty
                        case Point.PointStatus.Empty:
                            Console.Write("[ ]");
                            break;
                        //draw empty cell if its a ship hidden
                        case Point.PointStatus.Ship:
                            Console.Write("[ ]");
                            break;
                        //draw an X if the cell status is hit
                        case Point.PointStatus.Hit:
                            Console.Write("[");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("X");
                            Console.ResetColor();
                            Console.Write("]");
                            break;
                        //draw a 0 if the cell status is miss
                        case Point.PointStatus.Miss:
                            Console.Write("[");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write("0");
                            Console.ResetColor();
                            Console.Write("]");
                            break;
                        default:
                            break;
                    }
                }

                Console.WriteLine();
            }



            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("");
            Console.WriteLine("-------------------------------------------");
            Console.ResetColor();
            Console.WriteLine("Number of rounds: " + CombatRound);
            Console.WriteLine("Number of  ships: " + ListOfShips.Count());
            Console.WriteLine("Destroyed: " + ListOfShips.Count(x => x.IsDestroyed));
            Console.WriteLine("Remaining: " + (ListOfShips.Count() - ListOfShips.Count(x => x.IsDestroyed)));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-------------------------------------------");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("      Enter the coordinates below");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            Console.WriteLine("         For HELP type in help");
            Console.WriteLine("");
            Console.ResetColor();
        }

        /// <summary>
        /// displays the hidden ships
        /// </summary>
        public void DisplayShips()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("          H I D E  N    S H I P S");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("");
            Console.ResetColor();

            //displays the grid
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("          0  1  2  3  4  5  6  7  8  9");
            Console.ResetColor();
            //draws the grid 
            for (int y = 0; y < 10; y++)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("       {0} ", y);
                Console.ResetColor();
                for (int x = 0; x < 10; x++)
                {
                    //depending on the cell status it will:
                    switch (Ocean[x, y].Status)
                    {
                        //draw a empty cell if its empty
                        case Point.PointStatus.Empty:
                            Console.Write("[ ]");
                            break;
                        //draw empty cell if its a ship hidden
                        case Point.PointStatus.Ship:
                            Console.Write("[");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("X");
                            Console.ResetColor();
                            Console.Write("]");
                            break;
                        //draw an X if the cell status is hit
                        case Point.PointStatus.Hit:
                            Console.Write("[");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("X");
                            Console.ResetColor();
                            Console.Write("]");
                            break;
                        //draw a 0 if the cell status is miss
                        case Point.PointStatus.Miss:
                            Console.Write("[");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write("0");
                            Console.ResetColor();
                            Console.Write("]");
                            break;
                        default:
                            break;
                    }
                }

                Console.WriteLine();
            }
        }

        
        /// <summary>
        /// game instructions
        /// </summary>
        public void Help()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("           H O W   T O   P L A Y");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Battleship is primarily a guessing game,");
            Console.WriteLine("with you trying to figure out the ");
            Console.WriteLine("the location of the computer warships and");
            Console.WriteLine("shoot them");
            Console.WriteLine("You need to specify the coordinates that");
            Console.WriteLine("you want to shoot. If theres a ship hiden");
            Console.WriteLine("and its hit then it will display an X and");
            Console.WriteLine("it will show an 0");
            Console.WriteLine("For coordinates enter X and Y");
            Console.WriteLine("");
            Console.ResetColor();
        }

        /// <summary>
        /// Handles the logic for determining hits, or misses 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Target(int x, int y)
        {
            //calculates the number of ships destroyes
            int numberOfShipsDestroyed = ListOfShips.Where(z => z.IsDestroyed).Count();
            //if the grid coordinate equals a ship then the user hit the ship
            if (Ocean[x, y].Status == Point.PointStatus.Ship)
            {
                //Marks the coordinate as hit and shows the message that it hit the ship
                Ocean[x, y].Status = Point.PointStatus.Hit;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("");
                Console.WriteLine("You hit the ship");
                Console.ResetColor();
                System.Threading.Thread.Sleep(1000);

            }
            //if the coordinate its empty it shows that the user missed
            else if (Ocean[x, y].Status == Point.PointStatus.Empty)
            {
                Ocean[x, y].Status = Point.PointStatus.Miss;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("");
                Console.WriteLine("You missed");
                Console.ResetColor();
                System.Threading.Thread.Sleep(1000);
            }
            else
            {
                //if not that means that the coordinate was already attacked
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("");
                Console.WriteLine("Already attacked this coordinate");
                Console.ResetColor();
                System.Threading.Thread.Sleep(1000);
            }

            //updates the list of ships destroyed
            int numberOfShipsDestroyedNow = ListOfShips.Where(z => z.IsDestroyed).Count();

            //if the number of ships destroyed at this point is greater than the number of ship destroyed 
            if (numberOfShipsDestroyedNow > numberOfShipsDestroyed)
                //return true
                return true;
            else
                return false;
        }

        /// <summary>
        /// plays game
        /// </summary>
        public void PlayGame()
        {
            //coordinates to get from the gamer
            int x = 0;
            int y = 0;

            //play while all ships arent destroyed
            while (!AllShipsDestroyed)
            {
                bool showDetais = false;
                bool validInput = false;
                Console.Clear();

                DisplayOcean();
                //while the user has typed in a valid  x coordinate
                while (!validInput)
                {

                    Console.Write("X: ");



                    //tryes to perse the coordinate given by the user to an int
                    Console.ForegroundColor = ConsoleColor.Green;
                    string input = Console.ReadLine();
                    if (input.ToLower() == "show ships")
                    {
                        DisplayShips();
                        System.Threading.Thread.Sleep(2000);
                        showDetais = true;
                    }

                    if (input.ToLower() == "help")
                    {
                        Help();
                        Console.WriteLine("");
                        Console.WriteLine("Press any key to continue ");
                        Console.ReadKey();
                        showDetais = true;
                    }

                    bool xIsNumber = int.TryParse(input, out x);
                    Console.ResetColor();
                    if (!xIsNumber)
                    {
                        //if invalid displays error
                        Console.WriteLine("");
                        if (!showDetais)
                            Console.WriteLine("invalid input!");
                        System.Threading.Thread.Sleep(900);
                        Console.Clear();
                        DisplayOcean();

                    }
                    //checks that the number captured fits the range of the grid
                    else if (x < 0 || x > 9)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("The coordinate should be between 0 and 9");
                        System.Threading.Thread.Sleep(900);
                        Console.Clear();
                        DisplayOcean();


                    }
                    else
                        validInput = true;
                }
                validInput = false;
                //while the user types in a valid Y coordinate
                while (!validInput)
                {

                    Console.Write("Y: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    bool yIsNumber = int.TryParse(Console.ReadLine(), out y);
                    Console.ResetColor();
                    //if its invalid shows error
                    if (!yIsNumber)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("invalid input!");
                        System.Threading.Thread.Sleep(900);
                        Console.Clear();
                    }
                    //if its out of range then shows error
                    else if (y < 0 || y > 9)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("The coordinate should be between 0 and 9");
                        System.Threading.Thread.Sleep(900);
                        Console.Clear();
                    }
                    else
                        validInput = true;
                }

                //after the input was validates calls target
                Target(x, y);
                //increases combat round
                CombatRound++;
            }

            Console.Clear();

            DisplayOcean();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("             Y O U   W O N ! ! !");
            Console.WriteLine("-------------------------------------------");
            Console.ResetColor();

            System.Threading.Thread.Sleep(1000);
        }




    }


}
