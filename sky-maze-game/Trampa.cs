using Spectre.Console;

namespace sky_maze_game.GameLogic;

public class Trampa
{
    public static Random random = new Random();
    public string? Nombre { get; set; }
    public string? Simbolo { get; set; }
    public Position? Posicion { get; set; }
    public static List<Position> PosicionesTrampas = new List<Position>();


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

    public static void TrampaGenerator()
    {
        Random random = new Random();
        double probabilidadTrampa = 0.0;


        switch (Board.dimension)
        {
            case 10: // FACIL
                probabilidadTrampa = 0.02;
                break;
            case 16: // INTERMEDIO
                probabilidadTrampa = 0.05;
                break;
            case 20: // DIFICIL
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
                        Trampa.PosicionesTrampas.Add(new Position(i, j));
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
                foreach (var trampa in Trampa.Trampas)
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


    public static void Snowflake(Ficha ficha)
    {
        ficha.Estado = Ficha.State.Congelado;
        ficha.Velocidad = -3;
        AnsiConsole.Markup("[cyan]Estás congelado, no puedes moverte por 3 turnos.[/]");
    }

    public static void Rain(Ficha ficha)
    {
        ficha.Estado = Ficha.State.Mojado;
        AnsiConsole.Markup("[blue]Estás mojado, cuidado con resbalar.[/]");

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

                AnsiConsole.Markup($"[blue]¡Resbalaste {GetDirectionString(direction)} por la lluvia![/]");
            }

        }
        else
        {
            AnsiConsole.Markup("[blue]La lluvia te mojó, pero mantuviste el equilibrio.[/]");
        }
        
        string GetDirectionString(int direction)
        {
            switch (direction)
            {
                case 0: return "hacia arriba";
                case 1: return "hacia la derecha";
                case 2: return "hacia abajo";
                case 3: return "hacia la izquierda";
                default: return "desconocido";
            }
        }
    }

    public static void Rayo(Ficha ficha)
    {
        int index = Ficha.FichasDisponibles.IndexOf(ficha);
        if (index != -1)
        {
            int oldX = ficha.Posicion.x, oldY = ficha.Posicion.y;
            ficha.Posicion = Position.InitialPositionFichas[index];
            AnsiConsole.Markup("[yellow]¡Has sido golpeado por un rayo y vuelves al inicio![/]");

            Board.board[oldX, oldY] = "c";
            Board.board[ficha.Posicion.x, ficha.Posicion.y] = ficha.Simbolo;
        }
    }

    public static void Skyhole(Ficha ficha, Trampa trampa)
    {
        var skyholes = new List<Position>();
        for (int i = 0; i < Board.dimension; i++)
        {
            for (int j = 0; j < Board.dimension; j++)
            {
                if (Board.board[i, j] == trampa.Simbolo)
                {
                    skyholes.Add(new Position(i, j));
                }
            }
        }

        if (skyholes.Count > 0)
        {
            int oldX = ficha.Posicion.x, oldY = ficha.Posicion.y;
            var randomPos = skyholes[Ficha.rand.Next(skyholes.Count)];
            ficha.Posicion.x = randomPos.x;
            ficha.Posicion.y = randomPos.y;

            Board.board[oldX, oldY] = "c";
            Board.board[ficha.Posicion.x, ficha.Posicion.y] = ficha.Simbolo;
        }
    }



    public static void ActivateTrampa(Ficha ficha, Trampa trampa)
    {
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
                Skyhole(ficha, trampa);
                break;
        }
    }
}













