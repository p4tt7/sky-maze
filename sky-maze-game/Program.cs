using sky_maze_game.GameUI;
using sky_maze_game.GameLogic;
using Spectre.Console;

class Program
{
    private static int turnosPorJugador = 0;

    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
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
        int[,] distancias = Board.DistanceValidator(Board.board, 0, 0);
        Board.ValidatedBoard(Board.board, distancias);
        Trampa.TrampaGenerator();
        Ficha.FichaInitializer(Player.jugadores);
        GameUI.PrintBoard();
        

        bool seguirJugando = true;

    while (seguirJugando && !Position.IsWinning())
    {
        foreach (Player jugadorActual in Player.jugadores)
        {
            Console.WriteLine($"{jugadorActual.Nombre}, es tu turno. Selecciona el número de la ficha correspondiente");

            for (int j = 0; j < jugadorActual.SelectedFichas.Count; j++)
            {
                Ficha ficha = jugadorActual.SelectedFichas[j];
                Console.WriteLine($"{j + 1}- {ficha.Nombre} ({ficha.Simbolo})");
            }

            Ficha playFicha = null;

            while (playFicha == null)
            {
                string input = Console.ReadLine();
                if (input == "salir")
                {
                    Console.WriteLine("Saliendo del juego. ¡Gracias por jugar!");
                    seguirJugando = false;
                    break;
                }
                else if (input == "reiniciar")
                {
                    Console.WriteLine("Reiniciando el juego...");
                    return; 
                }
                else if (int.TryParse(input, out int index) && index > 0 && index <= jugadorActual.SelectedFichas.Count)
                {
                    playFicha = jugadorActual.SelectedFichas[index - 1];
                    Console.WriteLine($"Ha elegido {playFicha.Nombre}");
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Intenta nuevamente");
                    continue;
                }

                bool movimientoCompletado = false;

                while (!movimientoCompletado)
                {
                    Console.WriteLine("Ahora muévase en la dirección deseada usando W (Arriba), A (Izquierda), S (Abajo), D (Derecha). Presiona X para usar la habilidad de la ficha o 'salir' para salir del juego.");
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.X)
                    {
                        Ficha.UseHabilidad(playFicha);
                        GameUI.PrintBoard();
                    }
                
                    else
                    {
                        Position.Movement(playFicha);
                        GameUI.PrintBoard();
                        movimientoCompletado = true;
                    }
                }
            }
        }
    }

    if (Position.IsWinning())
    {
        Console.Clear();
        Console.WriteLine($"¡Felicidades! Has ganado el juego.");
    }
    Program.turnosPorJugador = 0;  

    }
}
