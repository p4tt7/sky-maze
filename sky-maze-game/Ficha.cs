namespace sky_maze_game.GameLogic;
using Spectre.Console;

public class Ficha
{

    public string? Nombre { get; set; }
    public string? Simbolo { get; set; }
    public Position? Posicion { get; set; }
    public int Velocidad { get; set; }
    public int CoolingTime { get; set; }

    public static Random rand = new Random();

    public State Estado { get; set; }
    public int StateDuration { get; set; }
    public int CurrentCoolingTime { get; set; }
    public int CurrentVelocidad { get; set; }


    public enum State
    {
        Normal,
        Mojado,
        Congelado,
        Slower,
        Faster
    }

    public enum HabilidadType
    {
        Rainbow,
        Shadow,
        WindVelocity,
        CloudPath,
        Star,
        Eclipse,
    }



    public HabilidadType Habilidad { get; set; }

    public static List<Ficha> FichasDisponibles = new List<Ficha>{
    new Ficha {Nombre = "Arcoiris" , Simbolo = "🌈" , Velocidad = 4 , CoolingTime = 2 , Habilidad = HabilidadType.Rainbow},
    new Ficha {Nombre = "Luna Nueva" , Simbolo = "🌑" , Velocidad = 3 , CoolingTime = 4 , Habilidad = HabilidadType.Shadow},
    new Ficha {Nombre = "Viento" , Simbolo = "🍃" , Velocidad = 2 , CoolingTime = 3 , Habilidad = HabilidadType.WindVelocity},
    new Ficha {Nombre = "Nube" , Simbolo = "⛅" , Velocidad = 3 , CoolingTime = 5 , Habilidad = HabilidadType.CloudPath},
    new Ficha {Nombre = "Estrella" , Simbolo = "✨" , Velocidad = 3 , CoolingTime = 4 , Habilidad = HabilidadType.Star},
    new Ficha {Nombre = "Eclipse" , Simbolo = "🌘" , Velocidad = 4 , CoolingTime = 5 , Habilidad = HabilidadType.Eclipse},
    };

    public static void FichaInitializer(List<Player> jugadores)
    {
        for (int indexPosition = 0; indexPosition < jugadores.Count; indexPosition++)
        {
            Player jugador = jugadores[indexPosition];

            if (jugador.selectedFicha != null)
            {
                Position pos = Position.InitialPositionFichas[indexPosition];
                jugador.PosicionInicial = new Position(pos.x, pos.y);
                Board.board[pos.x, pos.y] = jugador.selectedFicha.Simbolo;
                jugador.selectedFicha.Posicion = new Position(pos.x, pos.y);
                jugador.selectedFicha.CurrentVelocidad = jugador.selectedFicha.Velocidad;
            }
            jugador.selectedFicha.CurrentCoolingTime = 0;
        }
    }


    public static void WindVelocity(Ficha ficha)
    {
        AnsiConsole.Markup("[green]Ha activado la habilidad\n[/]");

        ficha.CurrentVelocidad = 10;

        Console.WriteLine("Ahora eres diez veces mas rapido.\n");

        ficha.Estado = State.Faster;

    }


    public static void CloudPath(Ficha ficha)
    {
        AnsiConsole.MarkupLine("[grey]Ha activado la habilidad, creando nubes en su camino[/]");
        System.Threading.Thread.Sleep(1500);

        int pasos = ficha.Velocidad;

        for (int i = 0; i < pasos; i++)
        {
            Console.Clear();
            GameUI.GameUI.PrintBoard();
            Program.FichaInfo(ficha, pasos - i, ficha.CoolingTime);

            ConsoleKeyInfo teclaPresionada = Console.ReadKey(true);

            int newX = ficha.Posicion.x;
            int newY = ficha.Posicion.y;

            if (teclaPresionada.Key == ConsoleKey.W || teclaPresionada.Key == ConsoleKey.UpArrow)
            {
                if (newX > 0) newX--;
                else { AnsiConsole.MarkupLine("[red]Movimiento no válido[/]"); System.Threading.Thread.Sleep(1000); continue; }
            }
            else if (teclaPresionada.Key == ConsoleKey.A || teclaPresionada.Key == ConsoleKey.LeftArrow)
            {
                if (newY > 0) newY--;
                else { AnsiConsole.MarkupLine("[red]Movimiento no válido[/]"); System.Threading.Thread.Sleep(1000); continue; }
            }
            else if (teclaPresionada.Key == ConsoleKey.S || teclaPresionada.Key == ConsoleKey.DownArrow)
            {
                if (newX < Board.dimension - 1) newX++;
                else { AnsiConsole.MarkupLine("[red]Movimiento no válido[/]"); System.Threading.Thread.Sleep(1000); continue; }
            }
            else if (teclaPresionada.Key == ConsoleKey.D || teclaPresionada.Key == ConsoleKey.RightArrow)
            {
                if (newY < Board.dimension - 1) newY++;
                else { AnsiConsole.MarkupLine("[red]Movimiento no válido[/]"); System.Threading.Thread.Sleep(1000); continue; }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Tecla no reconocida.[/]");
                System.Threading.Thread.Sleep(1000);
                continue;
            }

            if (newX >= 0 && newX < Board.dimension && newY >= 0 && newY < Board.dimension)
            {

                Board.board[ficha.Posicion.x, ficha.Posicion.y] = "⬜";

                ficha.Posicion.x = newX;
                ficha.Posicion.y = newY;


                Board.board[newX, newY] = ficha.Simbolo;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Movimiento no válido.[/]");
                System.Threading.Thread.Sleep(1000);
                continue;
            }
        }

        AnsiConsole.MarkupLine("[grey]Has terminado de crear caminos nubosos.[/]");
        System.Threading.Thread.Sleep(1500);
    }



    public static void Eclipse(Ficha ficha)
    {
        Console.WriteLine("Selecciona una ficha por su índice para copiar su habilidad:");
        System.Threading.Thread.Sleep(1500);

        int index = 1;
        foreach (Player jugador in Player.jugadores)
        {
            if (jugador.selectedFicha != ficha)
            {
                Console.WriteLine($"{index}. {jugador.selectedFicha.Nombre} ({jugador.selectedFicha.Simbolo}) Habilidad: {jugador.selectedFicha.Habilidad}");
                index++;
            }

            if (Player.jugadores.Count == 1)
            {
                Console.WriteLine("No hay habilidades que copiar.");
                return;
            }
        }


        int indice;
        
        while (!int.TryParse(Console.ReadLine(), out indice) || indice < 1 || indice >= index)
        {
            Console.WriteLine("Selección no válida. Por favor, intenta de nuevo.");
        }

        Ficha fichaSeleccionada = Player.jugadores[indice - 1].selectedFicha!;
        Console.WriteLine($"Has seleccionado la habilidad de {fichaSeleccionada.Nombre}: {fichaSeleccionada.Habilidad}");

        HabilidadType habilidadOriginal = ficha.Habilidad;
        ficha.Habilidad = fichaSeleccionada.Habilidad;
        UseHabilidad(ficha);
        ficha.Habilidad = habilidadOriginal;
    }



    public static void Rainbow(Ficha ficha)
    {
        Console.WriteLine("Ha activado la habilidad de Rainbow.");
        System.Threading.Thread.Sleep(1500);

        if (ficha.Estado != State.Normal)
        {
            ficha.Estado = State.Normal;
            ficha.CurrentVelocidad = ficha.Velocidad;
            Console.WriteLine("Te has curado de todos los efectos de trampas.");
        }
        else
        {
            Console.WriteLine("No tienes efectos de trampas que curar.");
        }
    }

    public static void Star(Ficha ficha)
    {
        AnsiConsole.MarkupLine("[yellow]Ha activado la habilidad de Star.[/]");
        System.Threading.Thread.Sleep(1500);

        int pasos = ficha.CurrentVelocidad;
        Position originalPosition = new Position(ficha.Posicion.x, ficha.Posicion.y);

        for (int i = 0; i < pasos; i++)
        {
            Console.Clear();
            GameUI.GameUI.PrintBoard();
            ConsoleKeyInfo teclaPresionada = Console.ReadKey();

            int lastX = ficha.Posicion.x;
            int lastY = ficha.Posicion.y;

            int newX = ficha.Posicion.x;
            int newY = ficha.Posicion.y;

            if (teclaPresionada.Key == ConsoleKey.W || teclaPresionada.Key == ConsoleKey.UpArrow)
            {
                if (ficha.Posicion.x > 0) newX--;
                else { Console.WriteLine("Movimiento no válido"); System.Threading.Thread.Sleep(1000); continue; }
            }
            else if (teclaPresionada.Key == ConsoleKey.A || teclaPresionada.Key == ConsoleKey.LeftArrow)
            {
                if (ficha.Posicion.y > 0) newY--;
                else { Console.WriteLine("Movimiento no válido"); System.Threading.Thread.Sleep(1000); continue; }
            }
            else if (teclaPresionada.Key == ConsoleKey.S || teclaPresionada.Key == ConsoleKey.DownArrow)
            {
                if (ficha.Posicion.x < Board.dimension - 1) newX++;
                else { Console.WriteLine("Movimiento no válido"); System.Threading.Thread.Sleep(1000); continue; }
            }
            else if (teclaPresionada.Key == ConsoleKey.D || teclaPresionada.Key == ConsoleKey.RightArrow)
            {
                if (ficha.Posicion.y < Board.dimension - 1) newY++;
                else { Console.WriteLine("Movimiento no válido"); System.Threading.Thread.Sleep(1000); continue; }
            }
            else
            {
                Console.WriteLine("Tecla no reconocida.");
                System.Threading.Thread.Sleep(1000);
                continue;
            }

            if (Board.Range(Board.dimension, newX, newY))
            {
                Board.board[lastX, lastY] = "c";

                if (Position.IsFicha(newX, newY))
                {
                    string tempSimbolo = Board.board[newX, newY];
                    Board.board[newX, newY] = ficha.Simbolo;
                    Board.board[lastX, lastY] = tempSimbolo;
                    ficha.Posicion.x = newX;
                    ficha.Posicion.y = newY;
                }

                else
                {
                    Board.board[newX, newY] = "c"; 
                    ficha.Posicion.x = newX;
                    ficha.Posicion.y = newY;
                    Board.board[newX, newY] = ficha.Simbolo; 

                }

            }

            else
            {
                Console.WriteLine("Movimiento no válido.");
                System.Threading.Thread.Sleep(1000);
            }
        }
        AnsiConsole.MarkupLine("[yellow]La habilidad de Star ha terminado.[/]");

        Board.board[originalPosition.x, originalPosition.y] = "c";
    }



    public static void Shadow(Ficha ficha)
    {
        Console.WriteLine("Ha activado la habilidad de Shadow.");
        System.Threading.Thread.Sleep(1500);

        for (int i = 0; i < 3; i++)
        {
            int x, y;
            do
            {
                x = rand.Next(Board.dimension);
                y = rand.Next(Board.dimension);
            } while (Board.board[x, y] != "c");

            Board.board[x, y] = "🕸️";
            System.Threading.Thread.Sleep(1500);
        }
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
            case Ficha.HabilidadType.CloudPath:
                CloudPath(ficha);
                break;
            case Ficha.HabilidadType.Star:
                Star(ficha);
                break;
            case Ficha.HabilidadType.Eclipse:
                Eclipse(ficha);
                break;
        }
    }

    public static void ApplyTrapEffects(Ficha ficha)
    {
        if (ficha.Estado == State.Congelado)
        {
            AnsiConsole.MarkupLine("[blue]¡Estás congelado![/]\n");
            System.Threading.Thread.Sleep(2000);
            ficha.CurrentVelocidad = 0;
            ficha.StateDuration--;

            if (ficha.StateDuration <= 0)
            {
                ficha.Estado = State.Normal;
                ficha.CurrentVelocidad = ficha.Velocidad;
                AnsiConsole.MarkupLine("[green]¡Te has descongelado y puedes moverte de nuevo![/]\n");
                System.Threading.Thread.Sleep(2000);
            }
        }

        else if (ficha.Estado == State.Mojado)
        {
            AnsiConsole.MarkupLine("[blue]Estás mojado, cuidado con resbalar.[/]\n");
            System.Threading.Thread.Sleep(1000);


            if (Ficha.rand.Next(0, 2) == 0)
            {
                AnsiConsole.MarkupLine("[blue]¡Resbalaste por la lluvia y te has movido inesperadamente![/]");
                System.Threading.Thread.Sleep(2000);

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
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[blue]La lluvia te mojó, pero mantuviste el equilibrio.[/]");
            }

            ficha.StateDuration--;

            if (ficha.StateDuration <= 0)
            {
                ficha.Estado = State.Normal;
                AnsiConsole.MarkupLine("[green]¡Te has secado y ya no estás mojado![/]\n");
            }
        }

        else if (ficha.Estado == State.Slower)
        {
            AnsiConsole.MarkupLine("[grey]¡Estás ralentizado por una telaraña![/]\n");
            System.Threading.Thread.Sleep(2000);

            if (ficha.Velocidad > 1)
            {
                ficha.CurrentVelocidad = Math.Max(1, ficha.Velocidad - 2);
            }

            ficha.StateDuration--;

            if (ficha.StateDuration <= 0)
            {
                ficha.Estado = State.Normal;
                ficha.CurrentVelocidad = ficha.Velocidad;

                AnsiConsole.MarkupLine("[green]¡Te has liberado de la telaraña y recuperas tu velocidad![/]\n");
            }
        }
    }


}





















