using sky_maze_game.GameUI;
using sky_maze_game.GameLogic;

class Program
{
    private static int turnosPorJugador = 0;

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
        

        while(!Ficha.IsWinning()){

            foreach(Player jugadorActual in Player.jugadores){
                Console.WriteLine($"{jugadorActual.Nombre} es tu turno. Selecciona el numero de la ficha correspondiente");

                for (int j = 0; j < jugadorActual.SelectedFichas.Count; j++){
                    Ficha ficha = jugadorActual.SelectedFichas[j];
                    Console.WriteLine($"{j + 1}- {ficha.Nombre} ({ficha.Simbolo})");
                }

                Ficha playFicha = null;
                while(playFicha==null){
                    int index = int.Parse(Console.ReadLine());
                    if(index>0 && index <= jugadorActual.SelectedFichas.Count){
                        playFicha = jugadorActual.SelectedFichas[index - 1];
                        Console.WriteLine($"Ha elegido {playFicha.Nombre}");
                    }
                    else{
                        Console.WriteLine("Ficha invalida. Intente nuevamente");
                    }
                    Console.WriteLine("Ahora muevase en la direccion deseada usando los controles");
                    Ficha.Movement(playFicha);
                    Console.Clear();
                    GameUI.PrintBoard();
                    Program.turnosPorJugador++;
                }
            }
                
        }
        Program.turnosPorJugador = 0;   
        Console.Clear();
        //Console.WriteLine($"Felicidades! Ha ganado {Player.jugadores[i]}");
    }
}
