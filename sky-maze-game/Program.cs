using sky_maze_game.GameUI;
using sky_maze_game.GameLogic;
using Spectre.Console;
using System.ComponentModel.DataAnnotations;

class Program
{
    public static void Main()
    {

        Console.OutputEncoding = System.Text.Encoding.UTF8;
        GameUI.IntroAnimation();
        GameUI.Start();

        while (true)
        {
            string? state = Console.ReadLine();
            if (string.IsNullOrEmpty(state) || (state != "1" && state != "0"))
            {
                Console.WriteLine("Entrada no válida. Por favor, introduce 1 para jugar o 0 para salir.");
                continue;
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
            }
            else if (state == "0")
            {
                Console.WriteLine("Saliendo del juego...");
                break;
            }
        }

    }

    private static void InitializeGame()
    {
        GameLogic.SelectionMenu();
        Console.WriteLine("Cargando...");
        System.Threading.Thread.Sleep(2000);
        Console.Clear();
        Board.BoardGenerator();
        int[,] distancias = Board.DistanceValidator(Board.board, 0, 0);
        Board.ValidatedBoard(Board.board, distancias);
        Trampa.TrampaGenerator();
        Ficha.FichaInitializer(Player.jugadores);
        GameUI.PrintBoard();

    }


    public static void Juego()
    {
        InitializeGame();

        bool playing = true;
        int turno = 0;

        while (playing)
        {
            foreach (Player jugador in Player.jugadores)
            {
                HandlePlayerTurn(jugador);

                turno++;
                Console.Clear();
                GameUI.PrintBoard();
            }
        }
    }

    public static void HandlePlayerTurn(Player jugador)
    {
        AnsiConsole.MarkupLine($"[cyan]Jugador {jugador.Nombre}, es tu turno[/]");
        AnsiConsole.MarkupLine($"[cyan]Presione ENTER para moverse o X para usar su habilidad[/]");

        Ficha.ApplyTrapEffects(jugador.selectedFicha);

        bool playerTurnOver = false;

        while (!playerTurnOver)
        {
            ConsoleKeyInfo teclaPresionada = Console.ReadKey(true);

            if (teclaPresionada.Key == ConsoleKey.Enter)
            {
                int steps = jugador.selectedFicha.Velocidad;
                for (int i = 0; i < steps; i++)
                {
                    if (Position.Movement(jugador.selectedFicha))
                    {
                        if (Position.IsWinning())
                        {
                            AnsiConsole.MarkupLine($"[green]¡Felicidades, {jugador.Nombre} ha ganado![/]");
                            System.Threading.Thread.Sleep(2000);
                            Console.Clear();
                            GameUI.WinArt();
                            System.Threading.Thread.Sleep(2000);
                            Console.Clear();
                            Restart();
                            return;
                        }
                    }

                    else
                    {
                        break;
                    }
                }
                playerTurnOver = true;
            }
            else if (teclaPresionada.Key == ConsoleKey.X)
            {
                HandleAbility(jugador);
                playerTurnOver = true;
            }
        }
    }


    public static void HandleAbility(Player jugador)
    {
        if (jugador.selectedFicha.CoolingTime == 0)
        {
            Ficha.UseHabilidad(jugador.selectedFicha);
            jugador.selectedFicha.CoolingTime = jugador.selectedFicha.CoolingTime;
            AnsiConsole.MarkupLine($"[cyan]Habilidad activada, espera {jugador.selectedFicha.CoolingTime} turnos para usarla nuevamente.[/]");
            System.Threading.Thread.Sleep(2000);
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]Habilidad en enfriamiento, espera {jugador.selectedFicha.CoolingTime} turnos más.[/]");
            System.Threading.Thread.Sleep(2000);
        }
    }


    private static void Restart()
    {
        AnsiConsole.MarkupLine("Deseas volver a jugar?\n1- Si\n0- No");
        if (int.TryParse(Console.ReadLine(), out int reset))
        {
            if (reset == 1)
            {
                Console.Clear();
                AnsiConsole.Markup("[cyan]Reiniciando juego...[/]");
                System.Threading.Thread.Sleep(2000);
                Console.Clear();
                Juego();
            }

        }

    }
}