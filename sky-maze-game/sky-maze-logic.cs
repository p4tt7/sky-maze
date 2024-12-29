namespace sky_maze_game.GameLogic;

public class GameLogic{
    public void MainMenu(){
        while(true)
        {
            string input = Console.ReadLine();
            if(!int.TryParse(input, out int state) || (state!=0 && state!=1)){
                Console.WriteLine("Entrada invalida. Ingrese 0 o 1");
                continue;
            }
            if (state == 1)
            {
                Console.WriteLine("Iniciando juego...");
                System.Threading.Thread.Sleep(1000);
                Console.Clear();
                Game();             
            }
            else if (state == 0)
            {
                Console.WriteLine("Saliendo del juego");
                break;
            }
        }
    }

    public void Game()
    {     
        int rows = 5;  
        int columns = 5;  
        Board board = new Board(rows, columns); 
        board.BoardGenerator(0, 0);  
        board.PrintBoard();
    }
}

public class Board
{
    int[,] board;
    public enum WallorWalk{
        wall = 1, walk = 0

    }

    int row, column;
    int[] directions;
    Random rand = new Random();

    public  Board(int row, int column)
    {
        this.row = row;
        this.column = column;
        board = new int[row, column];
        directions = [1,-1,0,0];
        for(int i=0; i<row;i++){
            for(int j=0; j<column; j++){
                if(i%2==0){
                    if(j==0 || j==column-1){
                        board[i,j] = 0;
                    }
                    else{
                        board[i,j]=1;
                    }
                    
                }
                else{
                    board[i,j]=1;
                }
            }
        }

    }

    public void BoardGenerator(int x, int y)
    {
        for(int i=0; i<row;i++){
            for(int j=0; j<column; j++){
                if(board[i,j]==0){
                    WalkGenerator(x,y);
                }
            }
        }     
    }

    public void WalkGenerator(int first_row, int first_column){
        List<int[]> directions = new List<int[]>() {
            new int[] { 0, 1 },  // derecha
            new int[] { 1, 0 },  // abajo
            new int[] { 0, -1 }, // izquierda
            new int[] { -1, 0 }  // arriba
        };

        Random rand = new Random();
        int x = first_row;
        int y = first_column;

         for (int step = 0; step < 50; step++) 
        {
            var direction = directions[rand.Next(directions.Count)]; 
            int newX = x + direction[0];
            int newY = y + direction[1];

            if (ValidMove(newX, newY))
            {
                board[newX, newY] = 0; 
                x = newX;  
                y = newY;
            }
        }
    }

    public bool ValidMove(int x, int y)
    {
        return x >= 0 && x < row && y >= 0 && y < column && board[x, y] == 1;
    }

        public void PrintBoard()
    {  
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                Console.Write(board[i, j] == 1 ? "0" : " "); 
            }
            Console.WriteLine();

        }
    }

    //esto esta mal cambialo todo


 //    static int[,] IsRecheable(int[,] board, int first_row, int first_column){ //algoritmo de lee
 //        int rows = board.GetLength(0); //marcar cantidad de filas y columnas 
 //        int columns = board.GetLength(1);
 //        int[,] distance = new int[rows,columns]; //nuevo array para añadir las distancias
 //        distance[first_row , first_column] = 1; //celda inicial
 //
 //    }

}



//public class Fichas{
//    string 
//    string 
//    string 
//    string 
//    string 
//    string 
//    string 

//
//}
//
//public class Player{



//}

//public class Obstacules{



//}

//public class Tramps{


//}

