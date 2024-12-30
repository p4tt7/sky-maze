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
        Board board = new Board(10, 10);   
        board.InitializeBoard();
        board.BoardGenerator();
        board.PrintBoard();
    }
}

public class Board
{
    int[,] board;

    int row, column;
    int[] directions;
    Random rand = new Random();


    public Board(int row, int column)
    {
        this.row = row;
        this.column = column;
        board = new int[row, column];
    }

    public void InitializeBoard()
    {
        board[0,0]=-1;
        board[row-1, column-1]=-1;
        for(int i = 0; i<row; i++)
        {
            for(int j=0; j<column; j++)
            { 
                if(board[i,j]!=-1){
                    board[i,j]=1;
                }
            }    
        }
    }

    public void BoardGenerator()
    {
        List<int[]> directions = new List<int[]>() {
            new int[] { 0, 1 },  // derecha
            new int[] { 1, 0 },  // abajo
            new int[] { 0, -1 }, // izquierda
            new int[] { -1, 0 }  // arriba
        };

        Random rand = new Random();
        List<int[]> walls = new List<int[]>();
        walls.Add(new int[] { 1, 1 });

        while (walls.Count > 0)
        {
            // pared aleatoria
            int randomIndex = rand.Next(walls.Count);
            int[] wall = walls[randomIndex];
            walls.RemoveAt(randomIndex);

            int x = wall[0];
            int y = wall[1];

            // Verificar las celdas vecinas y si están vacías (0) o no tienen ficha (-1)
            foreach (var direction in directions)
            {
                int newX = x + direction[0];
                int newY = y + direction[1];

                if (ValidMove(newX, newY))
                {
                    board[x, y] = 0; // Convertimos la pared en camino

                    // Añadimos las celdas vecinas como paredes si están vacías
                    if (IsInsideBounds(newX, newY) && board[newX, newY] == 1)
                    {
                        walls.Add(new int[] { newX, newY });
                    }
                }
            }
        }
    }


    public bool ValidMove(int x, int y)
    {
        return x >= 0 && x < row && y >= 0 && y < column && board[x, y] != -1; // no puede ser ficha
    }

    
    public bool IsInsideBounds(int x, int y)    // dentro de limites
    {
        return x >= 0 && x < row && y >= 0 && y < column;
    }

    // Clase UI

    public void PrintBoard()
    {
        for (int i = 0; i < row; i++) 
        {
            for (int j = 0; j < column; j++) 
            {
                if (board[i, j] == -1)
                    Console.Write("❄️");  // Ficha
                else if (board[i, j] == 1)
                    Console.Write("⬜");  
                else
                    Console.Write("☁️");
            }
            Console.WriteLine();
        }
    }
}

 //    static int[,] IsRecheable(int[,] board, int first_row, int first_column){ //algoritmo de lee
 //        int rows = board.GetLength(0); //marcar cantidad de filas y columnas 
 //        int columns = board.GetLength(1);
 //        int[,] distance = new int[rows,columns]; //nuevo array para añadir las distancias
 //        distance[first_row , first_column] = 1; //celda inicial
 //
 //    }





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

