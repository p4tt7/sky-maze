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

    public int Dimension(){
        Console.WriteLine("Ingrese las dimensiones deseadas:");
        int n = int.Parse(Console.ReadLine());
        if(n<0){
            throw new Exception("Dimension invalida. Ingrese un numero mayor que 0");
        }
        return n;
    }

    public void Game()
    {     
        int n = Dimension();
        Board board = new Board(n); 
        board.BoardGenerator(); 
        board.PrintBoard();
    }
}

public class Board
{
    int[,] board;
    int n;


    public Board(int n)
    {
        this.n=n;
        board = new int[n, n];
    }

    public void BoardGenerator()
    {
       for(int i=0; i<n;i++){
            for(int j=0;j<n;j++){
                board[i,j] = 0;
            }
        }
    }

    public void PrintBoard()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write(board[i, j] + " "); 
            }
            Console.WriteLine();
        }
    }
}
