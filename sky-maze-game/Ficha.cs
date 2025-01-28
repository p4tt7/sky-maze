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

    public enum State
    {
        Normal,
        Mojado,
        Congelado,
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
        


        ficha.Velocidad += 10;

        Console.WriteLine($"Ahora eres diez veces mas rapido.");
    }


    public static void Fly(Ficha ficha)
    {
        Console.WriteLine("Ha activado la habilidad, ahora puede volar sobre los obstaculos");
        System.Threading.Thread.Sleep(1500);

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
        System.Threading.Thread.Sleep(1500);

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
        Console.WriteLine("Ha activado la habilidad de Rainbow.");
        System.Threading.Thread.Sleep(1500);

        if (ficha.Estado != State.Normal)
        {
            ficha.Estado = State.Normal;
            Console.WriteLine("Te has curado de todos los efectos de trampas.");
        }
        else
        {
            Console.WriteLine("No tienes efectos de trampas que curar.");
        }
    }

    public static void Star(Ficha ficha)
    {
        Console.WriteLine("Ha activado la habilidad de Star.");
        System.Threading.Thread.Sleep(1500);
        Position currentPosition = ficha.Posicion;

        int[] dx = { -1, 1, 0, 0, -1, 1, -1, 1 };
        int[] dy = { 0, 0, -1, 1, -1, -1, 1, 1 };

        for (int i = 0; i < dx.Length; i++)
        {
            int newX = currentPosition.x + dx[i];
            int newY = currentPosition.y + dy[i];

            if (newX >= 0 && newX < Board.dimension && newY >= 0 && newY < Board.dimension)
            {
                if (Board.board[newX, newY] == "w" && !Position.IsFicha(newX, newY))
                {
                    Board.board[newX, newY] = "c";
                    Console.WriteLine($"Convirtiendo ({newX}, {newY}) en camino.");
                }
            }
        }
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
            Console.WriteLine($"Creada una casilla de desaceleración en ({x}, {y}).");
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

    public static void ApplyTrapEffects(Ficha ficha)
    {
        if(ficha.Estado == State.Congelado){
            AnsiConsole.Markup("[Blue]Estas congelado! No puedes moverte por este turno[/]");
            ficha.Velocidad++;
        }
        if(ficha.Estado == State.Mojado){
            int wetTurns = 5;
            while(wetTurns>0){
                Trampa.Rain(ficha);
                wetTurns--;
            }     
        }
    }
}

    
        
    
        
    















