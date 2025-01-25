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
            SelectionMenu();
            return;
        }

        // jugadores
        for (int i = 1; i <= cant_jugadores; i++)
        {
            Console.Clear();
            AnsiConsole.MarkupLine($"[bold cyan]Ingresa el nombre del Jugador {i}:[/]");
            string? nombreJugador = Console.ReadLine();

            if (!string.IsNullOrEmpty(nombreJugador))
            {
                Player.jugadores.Add(new Player(nombreJugador));
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
                Board.dimension = 10;
                break;
            case 2:
                Board.dimension = 16;
                break;
            case 3:
                Board.dimension = 20;
                break;
        }
        Board.BoardInitializer();
    }

}


public class Board
{
    public static int dimension;
    public static int center;
    public static string[,] board;
    public static int[,] direcciones = { { 0, -1 }, { 0, 1 }, { -1, 0 }, { 1, 0 } };


    public static void BoardInitializer()
    {
        board = new string[dimension, dimension];
        center = dimension / 2;
        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                board[i, j] = "w";
            }
        }
    }

    public static string[,] BoardGenerator() //recursive backtrack
    {
        board[0, 0] = "c"; //marca el inicio

        GenerateMaze(board, 0, 0); //genera el laberunto
        return board;
    }

    private static void GenerateMaze(string[,] board, int currentRow, int currentCol)
    {
        int[] indices = { 0, 1, 2, 3 };
        ShuffleDirections(indices);

        for (int i = 0; i < 4; i++)
        {
            int directionIndex = indices[i];
            int newRow = currentRow + direcciones[directionIndex, 0] * 2;
            int newCol = currentCol + direcciones[directionIndex, 1] * 2;

            if (newRow >= 0 && newRow < board.GetLength(0) && newCol >= 0 && newCol < board.GetLength(1) && board[newRow, newCol] == "w")
            {

                board[currentRow + direcciones[directionIndex, 0], currentCol + direcciones[directionIndex, 1]] = "c";
                board[newRow, newCol] = "c";

                GenerateMaze(board, newRow, newCol);
            }
        }
    }

    private static void ShuffleDirections(int[] indices)
    {
        int n = indices.Length;
        Random rand = new Random();

        for (int i = n - 1; i > 0; i--)
        {

            int j = rand.Next(0, i + 1);

            int temp = indices[i];
            indices[i] = indices[j];
            indices[j] = temp;
        }
    }


    public static int[,] DistanceValidator(string[,] board, int firstRow, int firstColumn) //algoritmo de lee
    {
        board[center, center] = "c";
        foreach (Position Posicion in Position.InitialPositionFichas)
        {
            board[Posicion.x, Posicion.y] = "c";
        }
        int[,] distancias = new int[dimension, dimension];
        distancias[firstRow, firstColumn] = 1;
        int[] dr = { -1, 1, 0, 0, -1, 1, -1, 1 };
        int[] dc = { 0, 0, 1, -1, -1, -1, 1, 1 };
        bool change;

        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                distancias[i, j] = -1;
            }
        }

        do
        {
            change = false;

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    if (distancias[i, j] == 0)
                    {
                        continue;
                    }
                    if (board[i, j] == "w")
                    {
                        continue;
                    }
                    for (int d = 0; d < dr.Length; d++)
                    {
                        int vr = i + dr[d];
                        int vc = j + dc[d];

                        if (Range(dimension, vr, vc) && distancias[vr, vc] == 0 && board[vr, vc] == "c")
                        {
                            distancias[vr, vc] = distancias[i, j] + 1;
                            change = true;
                        }
                    }
                }
            }
        } while (change);

        return distancias;
    }

    public static bool Range(int dimension, int row, int column)
    {
        if (row >= 0 && row < dimension && column >= 0 && column < dimension)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void ValidatedBoard(string[,] board, int[,] distancias)
    {

        int[] dr = { -1, 1, 0, 0, -1, 1, -1, 1 };
        int[] dc = { 0, 0, 1, -1, -1, -1, 1, 1 };

        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                if (distancias[i, j] == -1)
                {

                    for (int d = 0; d < dr.Length; d++)
                    {
                        int vr = i + dr[d];
                        int vc = j + dc[d];

                        if (Range(dimension, vr, vc) && distancias[vr, vc] != -1)
                        {
                            board[i, j] = "c";
                            distancias[i, j] = distancias[vr, vc] + 1;
                            break;
                        }
                    }

                }
            }
        }
    }
}

public class Position
{
    public int x { get; set; }
    public int y { get; set; }

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static List<Position> InitialPositionFichas = new List<Position>{
        new Position(0, 0),
        new Position(0, Board.dimension - 1),
        new Position(Board.dimension - 1, 0),
        new Position(Board.dimension - 1, Board.dimension - 1)
    };


    public static void Movement(Ficha ficha)
    {

        Console.Clear();
        GameUI.GameUI.PrintBoard();
        ConsoleKeyInfo teclaPresionada = Console.ReadKey(true);

        int lastX = ficha.Posicion.x;
        int lastY = ficha.Posicion.y;
        bool moverse = false;

        if (teclaPresionada.Key == ConsoleKey.W || teclaPresionada.Key == ConsoleKey.UpArrow)
        {
            if (ficha.Posicion.x > 0 && !IsWall(ficha, ficha.Posicion.x - 1, ficha.Posicion.y))
            {
                ficha.Posicion.x -= 1;
                moverse = true;
            }

        }

        else if (teclaPresionada.Key == ConsoleKey.A || teclaPresionada.Key == ConsoleKey.LeftArrow)
        {
            if (ficha.Posicion.y > 0 && !IsWall(ficha, ficha.Posicion.x, ficha.Posicion.y - 1))
            {
                ficha.Posicion.y -= 1;
                moverse = true;
            }
        }


        else if (teclaPresionada.Key == ConsoleKey.S || teclaPresionada.Key == ConsoleKey.DownArrow)
        {
            if (ficha.Posicion.x < Board.dimension - 1 && !IsWall(ficha, ficha.Posicion.x + 1, ficha.Posicion.y))
            {
                ficha.Posicion.x += 1;
                moverse = true;
            }
        }

        else if (teclaPresionada.Key == ConsoleKey.D || teclaPresionada.Key == ConsoleKey.RightArrow)
        {
            if (ficha.Posicion.y < Board.dimension - 1 && !IsWall(ficha, ficha.Posicion.x, ficha.Posicion.y + 1))
            {
                ficha.Posicion.y += 1;
                moverse = true;
            }
        }

        if (moverse)
        {
            Board.board[ficha.Posicion.x, ficha.Posicion.y] = ficha.Simbolo;
            Board.board[lastX, lastY] = "c";
        }
    }



    public static bool IsTrampa(Ficha ficha, int newX, int newY)
    {
        foreach (Position posicion in Trampa.PosicionesTrampas)
        {
            if (posicion.x == newX && posicion.y == newY)
            {
                return true;
            }
        }
        return false;
    }



    public static bool IsWall(Ficha ficha, int newX, int newY)
    {
        if (Board.board[newX, newY] == "w")
        {
            return true;
        }
        return false;
    }


    public static bool IsWinning()
    {

        foreach (Player jugador in Player.jugadores)
        {
            if (Board.board[Board.center, Board.center] == jugador.selectedFicha.Simbolo)
            {
                return true;
            }

        }
        return false;
    }

}

public class Ficha
{

    public string? Nombre { get; set; }
    public string? Simbolo { get; set; }
    public Position? Posicion { get; set; }
    public int Velocidad { get; set; }
    public int CoolingTime { get; set; }
    public static Random rand = new Random();
    public State Estado { get; set; }

    public enum State
    {
        Normal,
        Mojado,
        Congelado,
        Invisible
    }

    public enum HabilidadType
    {
        Rainbow,
        Shadow,
        WindVelocity,
        Fly,
        Star,
        Eclipse,
    }

    public HabilidadType Habilidad { get; set; }

    public static List<Ficha> FichasDisponibles = new List<Ficha>{
    new Ficha {Nombre = "Arcoiris" , Simbolo = "🌈" , Velocidad = 4 , CoolingTime = 15 , Habilidad = HabilidadType.Rainbow},
    new Ficha {Nombre = "Luna Nueva" , Simbolo = "🌑" , Velocidad = 3 , CoolingTime = 2 , Habilidad = HabilidadType.Shadow},
    new Ficha {Nombre = "Viento" , Simbolo = "🍃" , Velocidad = 2 , CoolingTime = 3 , Habilidad = HabilidadType.WindVelocity},
    new Ficha {Nombre = "Nube" , Simbolo = "⛅" , Velocidad = 3 , CoolingTime = 3 , Habilidad = HabilidadType.Fly},
    new Ficha {Nombre = "Estrella" , Simbolo = "✨" , Velocidad = 3 , CoolingTime = 6 , Habilidad = HabilidadType.Star},
    new Ficha {Nombre = "Eclipse" , Simbolo = "🌘" , Velocidad = 4 , CoolingTime = 10 , Habilidad = HabilidadType.Eclipse},
    };

    public static void FichaInitializer(List<Player> jugadores)
    {
        int indexPosition = 0;

        foreach (Player jugador in jugadores)
        {
            if (jugador.selectedFicha != null)
            {
                Position pos = Position.InitialPositionFichas[indexPosition];
                Board.board[pos.x, pos.y] = jugador.selectedFicha.Simbolo;
                jugador.selectedFicha.Posicion = pos;
                indexPosition++;
            }
        }
    }


    public static void WindVelocity(Ficha ficha)
    {
        Console.WriteLine("Ha activado la habilidad\n");
        string[] wind_directions = { "norte", "sur", "este", "oeste" };
        int index = rand.Next(wind_directions.Length);
        string wind_direction = wind_directions[index];
        Console.WriteLine($"La direccion del viento es: {wind_directions[index]}");

        if (wind_direction == "oeste")
        {
            ficha.Velocidad += 10;

            for (int i = 0; i < ficha.Velocidad; i++)
            {
                if (ficha.Posicion.x > 0 && Board.board[ficha.Posicion.x - 1, ficha.Posicion.y] != "w")
                {
                    ficha.Posicion.x--;
                }
                else
                {
                    Console.WriteLine("Movimiento no válido");
                    break;
                }
            }
        }

        if (wind_direction == "sur")
        {
            ficha.Velocidad += 10;

            for (int i = 0; i < ficha.Velocidad; i++)
            {
                if (ficha.Posicion.y < Board.dimension - 1 && Board.board[ficha.Posicion.x - 1, ficha.Posicion.y] != "w")
                {
                    ficha.Posicion.y++;
                }
                else
                {
                    Console.WriteLine("Movimiento no válido");
                    break;
                }
            }
        }

        if (wind_direction == "este")
        {
            ficha.Velocidad += 10;

            for (int i = 0; i < ficha.Velocidad; i++)
            {
                if (ficha.Posicion.x < Board.dimension - 1 && Board.board[ficha.Posicion.x - 1, ficha.Posicion.y] != "w")
                {
                    ficha.Posicion.x++;
                }
                else
                {
                    Console.WriteLine("Movimiento no válido");
                    break;
                }
            }
        }

        if (wind_direction == "norte")
        {
            ficha.Velocidad += 10;

            for (int i = 0; i < ficha.Velocidad; i++)
            {
                if (ficha.Posicion.y > 0 && Board.board[ficha.Posicion.x - 1, ficha.Posicion.y] != "w")
                {
                    ficha.Posicion.y--;
                }
                else
                {
                    Console.WriteLine("Movimiento no válido");
                    break;
                }
            }
        }

        ficha.Velocidad -= 10;
    }

    public static void Fly(Ficha ficha)
    {

        int pasos = 3;

        for (int i = 0; i < pasos; i++)
        {
            Console.Clear();
            GameUI.GameUI.PrintBoard();
            ConsoleKeyInfo teclaPresionada = Console.ReadKey();

            int lastX = ficha.Posicion.x;
            int lastY = ficha.Posicion.y;

            string originalCell = Board.board[ficha.Posicion.x, ficha.Posicion.y];

            if (teclaPresionada.Key == ConsoleKey.W || teclaPresionada.Key == ConsoleKey.UpArrow)
            {
                if (ficha.Posicion.x > 0)
                {
                    ficha.Posicion.x--;
                    i--;
                }
                else
                {
                    Console.WriteLine("Movimiento no valido");
                }
            }
            else if (teclaPresionada.Key == ConsoleKey.A || teclaPresionada.Key == ConsoleKey.LeftArrow)
            {
                if (ficha.Posicion.y > 0)
                {
                    ficha.Posicion.y--;
                    i--;
                }
                else
                {
                    Console.WriteLine("Movimiento no valido");
                }
            }
            else if (teclaPresionada.Key == ConsoleKey.S || teclaPresionada.Key == ConsoleKey.DownArrow)
            {
                if (ficha.Posicion.x < Board.dimension - 1)
                {
                    ficha.Posicion.x++;
                    i--;
                }
                else
                {
                    Console.WriteLine("Movimiento no valido");
                }
            }
            else if (teclaPresionada.Key == ConsoleKey.D || teclaPresionada.Key == ConsoleKey.RightArrow)
            {
                if (ficha.Posicion.y < Board.dimension - 1)
                {
                    ficha.Posicion.y++;
                    i--;
                }
                else
                {
                    Console.WriteLine("Movimiento no valido");
                }
            }

            else
            {
                Console.WriteLine("Tecla no reconocida.");
                i--;
            }

            if (originalCell != ficha.Simbolo)
            {
                Board.board[lastX, lastY] = originalCell;
            }

            Board.board[ficha.Posicion.x, ficha.Posicion.y] = ficha.Simbolo;

        }
    }

    public static void Eclipse(Ficha ficha)
    {
        Console.WriteLine("Selecciona una ficha por su índice para copiar su habilidad:");

        int index = 1;
        foreach (Player jugador in Player.jugadores)
        {
            Console.WriteLine($"{index}. {jugador.selectedFicha.Nombre} ({jugador.selectedFicha.Simbolo}) Habilidad: {jugador.selectedFicha.Habilidad}");
            index++;
        }

        int indice;
        while (!int.TryParse(Console.ReadLine(), out indice) || indice < 1 || indice >= index)
        {
            Console.WriteLine("Selección no válida. Por favor, intenta de nuevo.");
        }

        Ficha fichaSeleccionada = Player.jugadores[indice - 1].selectedFicha!;
        Console.WriteLine($"Has seleccionado la habilidad de {fichaSeleccionada.Nombre}: {fichaSeleccionada.Habilidad}");

        UseHabilidad(fichaSeleccionada);
    }



    public static void Rainbow(Ficha ficha)
    {


    }

    public static void Star(Ficha ficha)
    {


    }



    public static void Shadow(Ficha ficha)
    {

    }


    public static void UseHabilidad(Ficha ficha)
    {
        switch (ficha.Habilidad)
        {
            case Ficha.HabilidadType.Rainbow:
                Rainbow(ficha);
                break;
            case Ficha.HabilidadType.Shadow:
                Shadow(ficha);
                break;
            case Ficha.HabilidadType.WindVelocity:
                WindVelocity(ficha);
                break;
            case Ficha.HabilidadType.Fly:
                Fly(ficha);
                break;
            case Ficha.HabilidadType.Star:
                Star(ficha);
                break;
            case Ficha.HabilidadType.Eclipse:
                Eclipse(ficha);
                break;
        }
    }

}

public class Player
{
    public string? Nombre { get; set; }
    public Ficha? selectedFicha { get; set; }
    public static List<Player> jugadores = new List<Player>();

    public Player(string nombre)
    {
        Nombre = nombre;
    }
}

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
                        return false; // No coloque trampas si hay una adyacente
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
        ficha.Simbolo = "🟦";
        Board.board[ficha.Posicion.x, ficha.Posicion.y] = ficha.Simbolo;
    }

    public static void Rain(Ficha ficha)
    {
        ficha.Estado = Ficha.State.Mojado;
        AnsiConsole.Markup("[blue]Estás mojado, tu movimiento es impredecible.[/]");

        int backwardMove = Ficha.rand.Next(1, 3);
        if (Ficha.rand.Next(0, 2) == 0)
        {
            int oldY = ficha.Posicion.y;
            ficha.Posicion.y = Math.Max(0, ficha.Posicion.y - backwardMove);
            AnsiConsole.Markup($"[blue]La ficha se movió {backwardMove} espacios hacia atrás debido a la lluvia.[/]");

            Board.board[oldY, ficha.Posicion.x] = "c";
            Board.board[ficha.Posicion.y, ficha.Posicion.x] = ficha.Simbolo;
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













