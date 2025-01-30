using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using sky_maze_game.GameUI;
using Spectre.Console;

namespace sky_maze_game.GameLogic;

public class GameLogic
{

    public static void SelectionMenu()
    {
        Console.Clear();

        Panel panel = new Panel("[cyan bold]MENÚ DE SELECCIÓN[/]")
        {
            Border = BoxBorder.Double,
            Padding = new Padding(1, 1),
        };

        AnsiConsole.Write(new Align(panel, HorizontalAlignment.Center));


        AnsiConsole.MarkupLine("[bold cyan]Introduzca la cantidad de jugadores (1-4):[/]");
        if (!int.TryParse(Console.ReadLine(), out int cant_jugadores) || cant_jugadores < 1 || cant_jugadores > 4)
        {
            AnsiConsole.MarkupLine("[bold red]Entrada inválida. Debe ser un número entre 1 y 4.[/]");
            System.Threading.Thread.Sleep(2000);
            Console.Clear();
            SelectionMenu();
            return;
        }

        for (int i = 0; i < cant_jugadores; i++)
        {
            Console.Clear();
            AnsiConsole.MarkupLine($"[bold cyan]Ingresa el nombre del Jugador {i + 1}:[/]");
            string? nombreJugador = Console.ReadLine();

            if (!string.IsNullOrEmpty(nombreJugador))
            {
                Position posicionInicial = Player.posicionesIniciales[i];
                Player.jugadores.Add(new Player(nombreJugador, posicionInicial));
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]Por favor, ingrese un nombre válido.[/]");
                i--;
            }
        }

        System.Threading.Thread.Sleep(2000);
        Console.Clear();


        // fichas
        foreach (Player jugador in Player.jugadores)
        {
            List<Ficha> FichasDisponibles = new List<Ficha>(Ficha.FichasDisponibles);

            Console.Clear();

            Panel header = new Panel($"[cyan]Jugador: {jugador.Nombre}[/]")
            {
                Border = BoxBorder.Double,
            };

            AnsiConsole.Write(header);

            for (int j = 0; j < FichasDisponibles.Count; j++)
            {
                AnsiConsole.MarkupLine($"{j + 1}. [cyan]{FichasDisponibles[j].Nombre}[/]");
            }

            AnsiConsole.MarkupLine("[cyan]Ingresa el número de la ficha que deseas seleccionar:[/]");

            bool seleccionValida = false;
            while (!seleccionValida)
            {
                if (int.TryParse(Console.ReadLine(), out int eleccion) && eleccion > 0 && eleccion <= FichasDisponibles.Count)
                {
                    Ficha fichaSeleccionada = FichasDisponibles[eleccion - 1];
                    jugador.selectedFicha = fichaSeleccionada;
                    AnsiConsole.MarkupLine($"[green bold]Has seleccionado: {fichaSeleccionada.Nombre}[/]");
                    seleccionValida = true;
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red]Selección inválida. Intenta de nuevo.[/]");
                }
            }

            System.Threading.Thread.Sleep(1500);
        }

        System.Threading.Thread.Sleep(2000);
        Console.Clear();

        //nivel de dificultad
        AnsiConsole.Markup($"[cyan]Ingrese el nivel de dificultad por su indice correspondiente:\n[/]");
        Console.WriteLine("1- FACIL\n2- INTERMEDIO\n3- DIFICIL\n");
        int dificultad = int.Parse(Console.ReadLine());
        switch (dificultad)
        {
            case 1:
                Board.dimension = 13;
                Board.center = 13 / 2;
                break;
            case 2:
                Board.dimension = 15;
                Board.center = 15 / 2;
                break;
            case 3:
                Board.dimension = 19;
                Board.center = 19 / 2;
                break;

            default:
                AnsiConsole.MarkupLine("[red]Opción no válida, intenta de nuevo.[/]");
                break;
        }
        Console.Clear();
        Board.BoardInitializer();
    }

}













