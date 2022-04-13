// tenker ett grid med verdier 0
// spiller har positiv verdi.
// ved hver tick flytter hodet seg og alle verdier dekrementerer hvis verdier rundt er korrekte

// kanskje grid bare kan vær ein singleton. Ska jo bare vær ein uansett, så då e det tvunget.

using System;
using System.Threading;

namespace clipacman
{
    public class Grid {
        public int[,] GridMatrix { get; set; }
        public bool JustAte { get; set; }
        public Direction CurrDirection { get; set; }
        public int Head_val { get; set; }
        private int head_x_cor;
        private int head_y_cor;
        private int x_corr;
        private int y_corr;

        public Grid(int x_cor, int y_cor) {
            GridMatrix = new int[x_cor,y_cor];
            GridMatrix[0,0] = 1;
            JustAte = false;
            x_corr = x_cor-1;
            y_corr = y_cor-1;
            CurrDirection = Direction.RIGHT;
            head_x_cor = 0;
            head_y_cor = 0;
            Head_val = 1;
        }

        public int FetchNextValue(){
            switch(this.CurrDirection) {
            case Direction.UP:
                try{
                    return this.GridMatrix[head_x_cor-1,head_y_cor];
                }
                catch{
                    return this.GridMatrix[x_corr,head_y_cor];
                }
            case Direction.DOWN:
                try{
                    return this.GridMatrix[head_x_cor+1,head_y_cor];
                    }
                    catch{
                        return this.GridMatrix[0,head_y_cor];
                    }
            case Direction.LEFT:
                try{
                    return this.GridMatrix[head_x_cor,head_y_cor-1];
                }
                catch{
                    return this.GridMatrix[head_x_cor,y_corr];
                }
            case Direction.RIGHT:
                try{
                    return this.GridMatrix[head_x_cor,head_y_cor+1];
                }
                catch{
                    return this.GridMatrix[head_x_cor,0];
                }
            default:
                throw new InvalidOperationException();
            }
        }

        public bool BoardHasDot(){
            for (int i = 0; i <= this.x_corr; i++) {
                for (int j = 0; j <= this.y_corr; j++) {
                    if (this.GridMatrix[i,j] >= 9950) {
                        return true;
                    }
                }
            }
            return false;
        }

        public TupleList<int, int> ListValidDotSpots(){
            var retList = new TupleList<int, int>();
            for (int i = 0; i <= this.x_corr; i++) {
                for (int j = 0; j <= this.y_corr; j++) {
                    if (this.GridMatrix[i,j] <= 0) {
                        retList.Add(i,j);
                    }
                }
            }
            return retList;
        }

        public void PlaceDotFromList(TupleList<int,int> tupleList){
            if (tupleList == null | tupleList.Count <= 2) {
                Console.Clear();
                Console.WriteLine("You won!");
                Thread.Sleep(600);
                System.Environment.Exit(1);
            }
            Random rnd = new Random();
            int index = rnd.Next(0, tupleList.Count-1);
            this.GridMatrix[tupleList[index].Item1, tupleList[index].Item2] = 10000;
        }

        public void PlaceDot(){
            Random rnd = new Random();
            int rand_x = rnd.Next(0, this.x_corr);
            int rand_y = rnd.Next(0, this.y_corr);
            if (this.GridMatrix[rand_x,rand_y] > 0){
                PlaceDot(); // increasingly likely to result in stack overflow error over time
            } else {
                this.GridMatrix[rand_x,rand_y] = 10000;
            }
        }

        public void ClearDot(){
            for (int i = 0; i <= this.x_corr; i++) {
                for (int j = 0; j <= this.y_corr; j++) {
                    if (this.GridMatrix[i,j] == 9949) {
                        this.GridMatrix[i,j] = 0;
                    }
                }
            }
        }

        public bool IsDot(){
            if (this.FetchNextValue() >= 9950){
                return true;
            }
            return false;
        }

        public bool IsDead(){
            if (this.FetchNextValue() > 0) {
                return true;
            }
            return false;
        }

        public void DecrementAll(){
            for (int i = 0; i <= this.x_corr; i++) {
                for (int j = 0; j <= this.y_corr; j++) {
                    this.GridMatrix[i,j] -= 1; 
                }
            }
        }

        public void PerformMove(){
            // this is the big one.
            //It needs to check check that the move is legal and update all relevant fields.
            
            // Check if dead or dot
            if (this.IsDot()){
                this.JustAte = true;
            }
            else {
                if (this.IsDead()){
                    Console.Clear();
                    Console.WriteLine("You died");
                    Thread.Sleep(600);
                    System.Environment.Exit(1);
                }
            }

            this.DecrementAll();
            if (this.JustAte){
                this.Head_val += 1;
                this.JustAte = false;
            }

            this.head_x_cor = getNewX();
            this.head_y_cor = getNewY();

            this.GridMatrix[this.head_x_cor, this.head_y_cor] = this.Head_val;

            if (!this.BoardHasDot()){
                this.ClearDot();
                this.PlaceDotFromList(this.ListValidDotSpots());
//                this.PlaceDot();
            }

            this.PrintGrid();
            // this.PrintGridString(); // did not improve upon the old one
            // this.DebugPrintGrid();
        }

        public int getNewX(){
            switch(this.CurrDirection) {
                case Direction.LEFT:
                    return this.head_x_cor;
                case Direction.RIGHT:
                    return this.head_x_cor;
                case Direction.UP:
                    if (this.head_x_cor == 0) return this.x_corr;
                    return this.head_x_cor-1;
                case Direction.DOWN:
                    if (this.head_x_cor == this.x_corr) return 0;
                    return this.head_x_cor+1;
                default:
                    throw new InvalidOperationException();
            }
        }

        public int getNewY(){
            switch(this.CurrDirection) {
                case Direction.UP:
                    return this.head_y_cor;
                case Direction.DOWN:
                    return this.head_y_cor;
                case Direction.LEFT:
                    if (this.head_y_cor == 0) return this.y_corr;
                    return this.head_y_cor-1;
                case Direction.RIGHT:
                    if (this.head_y_cor == this.y_corr) return 0;
                    return this.head_y_cor+1;
                default:
                    throw new InvalidOperationException();
            }

        }

        public void PrintGrid(){
            Console.Clear();
            for (int i = 0; i <= this.x_corr; i++) {
                for (int j = 0; j <= this.y_corr; j++) {
                    if (i == head_x_cor && j == head_y_cor) {
                        Console.Write("S");
                    } else {
                        if (this.GridMatrix[i,j] >= 9950) {
                            Console.Write("X");
                        } else {
                            if (this.GridMatrix[i,j] > 0) {
                                Console.Write("V");
                            } else {
                                Console.Write(" ");
                            }
                        }
                    }
                }
                Console.Write("\n");
            }
        }

        public void PrintGridString(){
            string printString = "";
            for (int i = 0; i <= this.x_corr; i++) {
                for (int j = 0; j <= this.y_corr; j++) {
                    if (i == head_x_cor && j == head_y_cor) {
                        printString += "S";
                    } else {
                        if (this.GridMatrix[i,j] >= 9950) {
                            printString += "X";
                        } else {
                            if (this.GridMatrix[i,j] > 0) {
                                printString += "V";
                            } else {
                                printString += ".";
                            }
                        }
                    }
                }
                printString += "\n";
            }
            Console.Clear();
            Console.Write(printString);
        }
        public void DebugPrintGrid(){
            Console.Clear();
            for (int i = 0; i <= this.x_corr; i++) {
                for (int j = 0; j <= this.y_corr; j++) {
                    Console.Write(this.GridMatrix[i,j]);
                }
                Console.Write("\n");
            }
        }
    }
}