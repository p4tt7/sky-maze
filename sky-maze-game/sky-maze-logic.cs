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

        //Seleccion de cantidad de Jugadores

        int cant_jugadores;
        while (true)
        {
            AnsiConsole.MarkupLine("[bold cyan]Introduzca la cantidad de jugadores (1-4):[/]");
            if (int.TryParse(Console.ReadLine(), out cant_jugadores) || cant_jugadores >= 1 || cant_jugadores <= 4)
            {
                break;
            }
            AnsiConsole.MarkupLine("[red]Entrada invalida. Solo pueden haber de 1 a 4 jugadores.[/]");

            System.Threading.Thread.Sleep(2000);
            Console.Clear();
        }

        //Introduccion de nombres de jugadores

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


        List<Ficha> FichasDisponibles = new List<Ficha>(Ficha.FichasDisponibles);

        // Eleccion de fichas
        foreach (Player jugador in Player.jugadores)
        {

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
                    FichasDisponibles.RemoveAt(eleccion - 1);
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


        //Nivel de Dificultad
        AnsiConsole.Markup($"[cyan]Ingrese el nivel de dificultad por su indice correspondiente:\n[/]");
        Console.WriteLine("1- Facil\n2- Normal\n3- Dificil\n");

        int dificultad;

        while (true)
        {

            string? input = Console.ReadLine();

            if (int.TryParse(input, out dificultad) && dificultad >= 1 && dificultad <= 3)
            {
                break;
            }

            AnsiConsole.MarkupLine("[red]Opción no válida, intenta de nuevo.[/]");
        }
        
        switch (dificultad)
        {
            case 1:
                Board.dimension = 9;
                Board.center = 9 / 2;
                break;
            case 2:
                Board.dimension = 11;
                Board.center = 11 / 2;
                break;
            case 3:
                Board.dimension = 13;
                Board.center = 13 / 2;
                break;
        }
        Console.Clear();
        Board.BoardInitializer();
    }

}













