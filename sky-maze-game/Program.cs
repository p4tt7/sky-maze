using sky_maze_game.GameUI;
using sky_maze_game.GameLogic;
using Spectre.Console;
using System.ComponentModel.DataAnnotations;

class Program
{

    public static bool isSolitaire = false;

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

                while (true) 
                {
                    ConsoleKeyInfo tecla = Console.ReadKey(true);
                    if (tecla.Key == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        Juego();
                        break; 
                    }
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
        //Board.board = new string[Board.dimension,Board.dimension];
        Board.BoardGenerator();
        int[,] distancias = Board.DistanceValidator(Board.board, 0, 0);
        Board.ValidatedBoard(Board.board, distancias);
        Trampa.TrampaGenerator();
        Ficha.FichaInitializer(Player.jugadores);
        GameUI.PrintBoard();

        if (Player.jugadores.Count == 1)
        {
            isSolitaire = true;
        }
        else
        {
            isSolitaire = false;
        }

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

        foreach (Player jugador in Player.jugadores)
        {
            if (jugador.selectedFicha.CoolingTime > 0)
            {
                jugador.selectedFicha.CoolingTime--;
            }
        }


    }

    public static void HandlePlayerTurn(Player jugador)
    {

        if (!isSolitaire)
        {
            AnsiConsole.MarkupLine($"[cyan]Jugador {jugador.Nombre}, es tu turno[/]");
        }

        AnsiConsole.MarkupLine($"[cyan]Presione ENTER para moverse o X para usar su habilidad[/]");

        Ficha.ApplyTrapEffects(jugador.selectedFicha);

        bool playerTurnOver = false;

        while (!playerTurnOver)
        {
            ConsoleKeyInfo teclaPresionada = Console.ReadKey(true);

            if (teclaPresionada.Key == ConsoleKey.Enter)
            {
                int steps = jugador.selectedFicha.Velocidad;
                while (steps > 0)
                {
                    if (Position.Movement(jugador.selectedFicha))
                    {
                        steps--;
                        FichaInfo(jugador.selectedFicha , steps, jugador.selectedFicha.CoolingTime);
                        System.Threading.Thread.Sleep(2000);

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
                        continue;
                    }

                }
                playerTurnOver = true;
            }
            else if (teclaPresionada.Key == ConsoleKey.X)
            {
                bool abilityUsed = HandleAbility(jugador);

                if (!abilityUsed)
                {
                    AnsiConsole.MarkupLine("[red]No puedes usar la habilidad, pero puedes moverte.[/]");
                }


            }
        }

        if (jugador.selectedFicha.CoolingTime > 0)
        {
            jugador.selectedFicha.CoolingTime--;
        }
    }


    public static bool HandleAbility(Player jugador)
    {
        if (jugador.selectedFicha.CoolingTime == 0)
        {
            Ficha.UseHabilidad(jugador.selectedFicha);

            Ficha originalFicha = Ficha.FichasDisponibles.FirstOrDefault(f => f.Habilidad == jugador.selectedFicha.Habilidad);
            jugador.selectedFicha.CoolingTime = originalFicha.CoolingTime;


            AnsiConsole.MarkupLine($"[cyan]Habilidad activada, espera {jugador.selectedFicha.CoolingTime} turnos para usarla nuevamente.[/]");
            System.Threading.Thread.Sleep(2000);
            return true;
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]Habilidad en enfriamiento, espera {jugador.selectedFicha.CoolingTime} turnos más.[/]");
            System.Threading.Thread.Sleep(2000);
            return false;
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

    public static void FichaInfo(Ficha ficha , int steps , int currentCoolingTime)
    {
        AnsiConsole.MarkupLine($"[cyan]Movimientos restante:[/] {ficha.Velocidad - steps}");
        AnsiConsole.MarkupLine($"[cyan]Cooldown de habilidad:[/] {ficha.CoolingTime} turnos");
        AnsiConsole.MarkupLine($"[cyan]Estado:[/] {ficha.Estado}");
    }
}