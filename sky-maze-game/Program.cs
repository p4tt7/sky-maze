using sky_maze_game.GameUI;
using sky_maze_game.GameLogic;
using Spectre.Console;
using System.ComponentModel.DataAnnotations;
using System.Data;

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
                        return;
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
                HandlePlayerTurn(jugador, ref playing);

                turno++;
                Console.Clear();
                GameUI.PrintBoard();

                if (!playing)
                {
                    Console.Clear();
                    break;
                }
            }
        }

        foreach (Player jugador in Player.jugadores)
        {
            if (jugador.selectedFicha.CurrentCoolingTime > 0)
            {
                jugador.selectedFicha.CurrentCoolingTime--;
            }
        }




    }

    public static void HandlePlayerTurn(Player jugador, ref bool playing)
    {
        if (jugador.selectedFicha.CurrentCoolingTime > 0)
        {
            jugador.selectedFicha.CurrentCoolingTime--;
        }

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
                int steps = jugador.selectedFicha.CurrentVelocidad;
                while (steps > 0)
                {
                    if (Position.Movement(jugador.selectedFicha))
                    {
                        steps--;
                        FichaInfo(jugador.selectedFicha, steps, jugador.selectedFicha.CurrentCoolingTime);
                        System.Threading.Thread.Sleep(1000);

                        if (Position.IsWinning())
                        {
                            AnsiConsole.MarkupLine($"[green]¡Felicidades, {jugador.Nombre} ha ganado![/]");
                            System.Threading.Thread.Sleep(2000);
                            Console.Clear();
                            GameUI.WinArt();
                            System.Threading.Thread.Sleep(2000);
                            Console.Clear();
                            playing = false;
                            playerTurnOver = true;
                            break;
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
                    AnsiConsole.MarkupLine("[cyan]No puedes usar la habilidad, pero puedes moverte![/]");
                }


            }
        }

        if(jugador.selectedFicha.Estado == Ficha.State.Faster){
            jugador.selectedFicha.CurrentVelocidad = jugador.selectedFicha.Velocidad;
            jugador.selectedFicha.Estado = Ficha.State.Normal;
        }
    }


    public static bool HandleAbility(Player jugador)
    {
        if (jugador.selectedFicha.CurrentCoolingTime == 0)
        {
            Ficha.UseHabilidad(jugador.selectedFicha);
            jugador.selectedFicha.CurrentCoolingTime = jugador.selectedFicha.CoolingTime;
            System.Threading.Thread.Sleep(2000);
            return true;
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]Habilidad en enfriamiento.[/]");
            System.Threading.Thread.Sleep(2000);
            return false;
        }

    }


    public static void FichaInfo(Ficha ficha, int steps, int currentCoolingTime)
    {
        AnsiConsole.MarkupLine($"[cyan]Movimientos restantes:[/] {steps}");
        AnsiConsole.MarkupLine($"[cyan]Cooldown de habilidad:[/] {ficha.CurrentCoolingTime} turnos");
        AnsiConsole.MarkupLine($"[cyan]Estado:[/] {ficha.Estado}");
    }
}