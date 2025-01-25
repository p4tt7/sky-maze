using sky_maze_game.GameUI;
using sky_maze_game.GameLogic;
using Spectre.Console;
using System.ComponentModel.DataAnnotations;

class Program
{
    private static int turnosPorJugador = 0;

    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        GameUI.IntroAnimation();
        GameUI.Start();

        while (true)
        {
            string? state = Console.ReadLine();
            if (string.IsNullOrEmpty(state))
            {
                Console.WriteLine("Entrada no válida. Por favor, introduce un número.");
            }

            if (state == "1")
            {
                Console.Clear();
                Console.WriteLine("Iniciando juego...");
                System.Threading.Thread.Sleep(2000);
                Console.Clear();

                GameUI.Presentation();
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    Juego();
                }
                else if (state == "0")
                {
                    Console.WriteLine("Saliendo del juego");
                    break;
                }
                break;
            }
        }
    }

    public static void Juego()
    {
        GameLogic.SelectionMenu();
        Console.WriteLine("Cargando...");
        Console.Clear();
        System.Threading.Thread.Sleep(2000);
        Board.BoardGenerator();
        int[,] distancias = Board.DistanceValidator(Board.board, 0, 0);
        Board.ValidatedBoard(Board.board, distancias);
        Trampa.TrampaGenerator();
        Ficha.FichaInitializer(Player.jugadores);
        GameUI.PrintBoard();

        int turno = 0;

        while (!Position.IsWinning())
        {

            foreach (Player jugador in Player.jugadores)
            {
                AnsiConsole.Markup($"[cyan]Jugador {jugador.Nombre}, es tu turno\n[/]");
                AnsiConsole.Markup($"[cyan]Presione ENTER para comenzar, o X para activar su habilidad\n[/]");

                int cooldownRestante = 0;

                while (true)
                {
                    ConsoleKeyInfo teclaPresionada = Console.ReadKey(true);

                    if (teclaPresionada.Key == ConsoleKey.Enter)
                    {
                        int steps = jugador.selectedFicha.Velocidad;

                        while (steps > 0)
                        {
                            Position.Movement(jugador.selectedFicha);
                            Position position = jugador.selectedFicha.Posicion;

                            if (Position.IsTrampa(jugador.selectedFicha, position.x, position.y))
                            {
                                for (int t = 0; t < Trampa.PosicionesTrampas.Count; t++)
                                {
                                    if (Trampa.PosicionesTrampas[t].x == position.x && Trampa.PosicionesTrampas[t].y == position.y)
                                    {
                                        Trampa.ActivateTrampa(jugador.selectedFicha, Trampa.Trampas[t]);
                                        AnsiConsole.Markup("\n[red]¡Ha caído en una trampa![/]\n");
                                        break;
                                    }
                                }
                            }

                            steps--;
                        }

                        break;  
                    }

                    if (teclaPresionada.Key == ConsoleKey.X)
                    {
                        if (cooldownRestante == 0)
                        {
                            Ficha.UseHabilidad(jugador.selectedFicha);
                            cooldownRestante = jugador.selectedFicha.CoolingTime;
                        }
                        else
                        {
                            AnsiConsole.Markup($"[red]Habilidad en enfriamiento, espera {cooldownRestante} turnos más.[/]\n");
                        }
                    }

                    if (cooldownRestante > 0)
                    {
                        cooldownRestante--;
                    }
                }

                turno++;  
                Console.Clear();  
                GameUI.PrintBoard();
            }

            GameUI.WinArt(); 
        }

    }
}