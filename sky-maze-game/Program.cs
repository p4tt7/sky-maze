using sky_maze_game.GameUI;
using sky_maze_game.GameLogic;

class Program
{
    public static void Main()
    {
        GameUI.Start();
        while(true)
        {
            string? state = Console.ReadLine();
            if (string.IsNullOrEmpty(state))
            {  
                Console.WriteLine("Entrada no válida. Por favor, introduce un número.");
            }

            if (state == "1")
            {
                Console.WriteLine("Iniciando juego...");
                System.Threading.Thread.Sleep(1000);
                Console.Clear();
                Game();        
            }

            else if (state == "0")
            {
                Console.WriteLine("Saliendo del juego");
                break;
            }
        } 
        
    }

    public static void Game(){
        GameLogic.SelectionMenu();
        Board.BoardInitializer();
        Board.BoardGenerator();
        Obstacule.ObstaculeGenerator();
        int[,] distancias = Board.DistanceValidator(Board.board, 0, 0);
        Board.ValidatedBoard(Board.board, distancias);
        Trampa.TrampaGenerator();
        Ficha.FichaInitializer(Player.jugadores);
        GameUI.PrintBoard();
        //while(posiciondeljugador !=winning_position){

        //}
    }  
}
