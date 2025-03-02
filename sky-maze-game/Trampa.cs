using Spectre.Console;

namespace sky_maze_game.GameLogic;

public class Trampa
{
    public static Random random = new Random();
    public string? Nombre { get; set; }

    public HabilidadType Habilidad { get; set; }

    public string? Simbolo { get; set; }

    public enum HabilidadType
    {
        Copo,
        Lluvia,
        Rayo,
        Agujero,

    }


    public static List<Trampa> Trampas = new List<Trampa>{
    new Trampa {Nombre = "Copo de Nieve" , Simbolo = "⛄" , Habilidad = HabilidadType.Copo},
    new Trampa {Nombre = "LLuvia" , Simbolo = "☔" , Habilidad = HabilidadType.Lluvia},
    new Trampa {Nombre = "Rayo" , Simbolo = "⚡" , Habilidad = HabilidadType.Rayo},
    new Trampa {Nombre = "Skyhole" , Simbolo = "🌀" , Habilidad = HabilidadType.Agujero},
    };


    public static void TrampaGenerator()
    {
        Random random = new Random();
        double probabilidadTrampa = 0.0;

        switch (Board.dimension)
        {
            case 9: // FACIL
                probabilidadTrampa = 0.07;
                break;
            case 11: // INTERMEDIO
                probabilidadTrampa = 0.08;
                break;
            case 13: // DIFICIL
                probabilidadTrampa = 0.09;
                break;
        }

        for (int i = 0; i < Board.dimension; i++)
        {
            for (int j = 0; j < Board.dimension; j++)
            {
                if (Board.board[i, j] == "c" && !(i == Board.dimension / 2 && j == Board.dimension / 2))
                {
                    if (random.NextDouble() < probabilidadTrampa)
                    {
                        int indexTrampa = Trampa.random.Next(0, Trampa.Trampas.Count);
                        Board.board[i, j] = Trampa.Trampas[indexTrampa].Simbolo;
                    }
                }
            }
        }
    }

    public static void DetectarTrampa(Ficha ficha)
    {
        foreach (var trampa in Trampas)
        {
            if (Board.board[ficha.Posicion.x, ficha.Posicion.y] == trampa.Simbolo)
            {
                ActivateTrampa(trampa, ficha);
                break;
            }
        }
    }

    public static void ActivateTrampa(Trampa trampa, Ficha ficha)
    {

        int trapX = ficha.Posicion.x;
        int trapY = ficha.Posicion.y;

        switch (trampa.Habilidad)
        {
            case HabilidadType.Copo:
                Snowflake(ficha);
                break;
            case HabilidadType.Lluvia:
                Rain(ficha);
                break;
            case HabilidadType.Rayo:
                Rayo(ficha);
                break;
            case HabilidadType.Agujero:
                Skyhole(ficha);
                break;
        }

        Board.board[trapX, trapY] = "c";
    }

    public static void Snowflake(Ficha ficha)
    {
        AnsiConsole.MarkupLine("[cyan]Estás congelado a partir del proximo turno, no puedras moverte por tres turnos.\n[/]");
        System.Threading.Thread.Sleep(2000);
        ficha.Estado = Ficha.State.Congelado;
        ficha.StateDuration = 3;
        ficha.CurrentVelocidad = 0;
    }

    public static void Rain(Ficha ficha)
    {
        ficha.Estado = Ficha.State.Mojado;
        ficha.StateDuration = 5;
        AnsiConsole.MarkupLine("[blue]Estás mojado, cuidado con resbalar.[/]\n");
        System.Threading.Thread.Sleep(2000);
    }


    public static Position Skyhole(Ficha ficha)
    {
        int newX, newY;

        do
        {
            newX = random.Next(0, Board.dimension);
            newY = random.Next(0, Board.dimension);
        } while (Board.board[newX, newY] != "c" && newX == Board.center && newY == Board.center);

        AnsiConsole.MarkupLine("[magenta]¡Te has teletransportado a otro lugar debido al Skyhole![/]");
        System.Threading.Thread.Sleep(2000);

        return new Position(newX, newY);
    }



    public static Position Rayo(Ficha ficha)
    {
        Player? jugador = null;

        foreach (Player j in Player.jugadores)
        {
            if (j.selectedFicha == ficha)
            {
                jugador = j;
                break;
            }
        }

        Position posicionInicial = jugador.PosicionInicial;

        AnsiConsole.MarkupLine("[yellow]¡Has sido golpeado por un rayo y vuelves al inicio![/]");
        System.Threading.Thread.Sleep(2000);

        return posicionInicial;
    }

    public static void Slow(Ficha ficha)
    {
        ficha.Estado = Ficha.State.Slower;
        ficha.StateDuration = 3;
        ficha.CurrentVelocidad = Math.Max(1, ficha.Velocidad - 2);
        AnsiConsole.MarkupLine("[red]¡Has sido ralentizado por una telaraña![/]");
    }

}
