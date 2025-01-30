using Spectre.Console;

namespace sky_maze_game.GameLogic;

public class Trampa
{
    public static Random random = new Random();
    public string? Nombre { get; set; }
    public string? Simbolo { get; set; }

    public enum HabilidadType
    {
        Copo,
        Lluvia,
        Rayo,
        Agujero,

    }

    public HabilidadType Habilidad { get; set; }

    public static List<Trampa> Trampas = new List<Trampa>{
    new Trampa {Nombre = "Copo de Nieve" , Simbolo = "⛄" , Habilidad = HabilidadType.Copo},
    new Trampa {Nombre = "LLuvia" , Simbolo = "☔" , Habilidad = HabilidadType.Lluvia},
    new Trampa {Nombre = "Rayo" , Simbolo = "⚡" , Habilidad = HabilidadType.Rayo},
    new Trampa {Nombre = "Skyhole" , Simbolo = "🌀" , Habilidad = HabilidadType.Agujero}
    };

    public static List<Trampa> TrampasActivas = new List<Trampa>();

    public static void TrampaGenerator()
    {
        Random random = new Random();
        double probabilidadTrampa = 0.0;

        switch (Board.dimension)
        {
            case 13: // FACIL
                probabilidadTrampa = 0.06;
                break;
            case 15: // INTERMEDIO
                probabilidadTrampa = 0.069;
                break;
            case 19: // DIFICIL
                probabilidadTrampa = 0.07;
                break;
        }

        for (int i = 0; i < Board.dimension; i++)
        {
            for (int j = 0; j < Board.dimension; j++)
            {
                if (Board.board[i, j] == "c" && PuedeColocarTrampa(i, j))
                {
                    if (random.NextDouble() < probabilidadTrampa && Trampa.Trampas.Count > 0)
                    {
                        int indexTrampa = Trampa.random.Next(0, Trampa.Trampas.Count);
                        Board.board[i, j] = Trampa.Trampas[indexTrampa].Simbolo;
                    }
                }
            }
        }
    }

    private static bool PuedeColocarTrampa(int x, int y)
    {
        int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
        int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };

        for (int i = 0; i < 8; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];
            if (nx >= 0 && nx < Board.dimension && ny >= 0 && ny < Board.dimension)
            {
                foreach (var trampa in TrampasActivas)
                {
                    if (Board.board[nx, ny] == trampa.Simbolo)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
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
        AnsiConsole.MarkupLine("[cyan]Estás congelado, no puedes moverte por 3 turnos.\n[/]");
        System.Threading.Thread.Sleep(2000);
        ficha.Estado = Ficha.State.Congelado;
        ficha.StateDuration = 1;
        ficha.Velocidad = -3;
    }

    public static void Rain(Ficha ficha)
    {
        ficha.Estado = Ficha.State.Mojado;
        ficha.StateDuration = 5;
        AnsiConsole.MarkupLine("[blue]Estás mojado, cuidado con resbalar.[/]\n");
        System.Threading.Thread.Sleep(2000);
        

        if (Ficha.rand.Next(0, 2) == 0)
        {
            int[] dx = { 0, 1, 0, -1 };
            int[] dy = { -1, 0, 1, 0 };
            int direction = Ficha.rand.Next(0, 4);

            int newX = ficha.Posicion.x + dx[direction];
            int newY = ficha.Posicion.y + dy[direction];

            if (newX >= 0 && newX < Board.dimension && newY >= 0 && newY < Board.dimension && Board.board[newX, newY] == "c")
            {
                Board.board[ficha.Posicion.x, ficha.Posicion.y] = "c";
                ficha.Posicion = new Position(newX, newY);
                Board.board[newX, newY] = ficha.Simbolo;

                AnsiConsole.MarkupLine("[blue]¡Resbalaste por la lluvia![/]");
                System.Threading.Thread.Sleep(2000);
            }

        }
        else
        {
            AnsiConsole.MarkupLine("[blue]La lluvia te mojó, pero mantuviste el equilibrio.[/]");
        }
    }


    public static void Skyhole(Ficha ficha)
    {
        Board.board[ficha.Posicion.x, ficha.Posicion.y] = "c";


        int newX = random.Next(0, Board.dimension);
        int newY = random.Next(0, Board.dimension);


        ficha.Posicion = new Position(newX, newY);
        Board.board[newX, newY] = ficha.Simbolo;

        AnsiConsole.MarkupLine("[magenta]¡Te has teletransportado a otro lugar gracias al Skyhole![/]");
        System.Threading.Thread.Sleep(2000);
    }

    public static void Rayo(Ficha ficha)
    {

        Player? jugador = Player.jugadores.FirstOrDefault(j => j.selectedFicha == ficha);

        Position posicionInicial = jugador.PosicionInicial;

        int oldX = ficha.Posicion.x;
        int oldY = ficha.Posicion.y;

        Board.board[oldX, oldY] = "c";

        ficha.Posicion = posicionInicial;

        Board.board[ficha.Posicion.x, ficha.Posicion.y] = ficha.Simbolo;

        AnsiConsole.Markup("[yellow]¡Has sido golpeado por un rayo y vuelves al inicio![/]");
        System.Threading.Thread.Sleep(2000);

        
    }

}













