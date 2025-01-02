namespace sky_maze_game.GameLogic;

public class GameLogic{
    public void MainMenu()
    {
        while(true)
        {
            string input = Console.ReadLine();
            if(!int.TryParse(input, out int state) || (state!=0 && state!=1)){
                Console.WriteLine("Entrada invalida. Ingrese 0 o 1");
                continue;
            }
            if (state == 1)
            {
                Console.WriteLine("Iniciando juego...");
                System.Threading.Thread.Sleep(1000);
                Console.Clear();
                SelectionMenu();        
            }
            else if (state == 0)
            {
                Console.WriteLine("Saliendo del juego");
                break;
            }
        }
    }

    public void SelectionMenu()
    {

        //JUGADORES
        Console.WriteLine("Introduzca la cantidad de jugadores:");
        if (!int.TryParse(Console.ReadLine(), out int cant_jugadores) || cant_jugadores < 1 || cant_jugadores > 4) {
            Console.WriteLine("Entrada inválida. Debe ser un número entre 1 y 4.");
            SelectionMenu();
        }

        List<Player> jugadores = new List<Player>();
        for (int i=1;i<=cant_jugadores;i++){
            Console.WriteLine($"Ingresa el nombre del Jugador {i}:");
            string nombreJugador = Console.ReadLine();
            jugadores.Add(new Player(nombreJugador));  
        }

        System.Threading.Thread.Sleep(2000);
        Console.Clear();

        //FICHAS
        foreach (Player jugador in jugadores)
        {
            Console.WriteLine($"Jugador {jugador.Nombre}, ingrese las 3 fichas a utilizar:");
            List<Ficha> fichasDisponiblesParaJugador = new List<Ficha>(Ficha.FichasDisponibles);
            for (int i = 0; i < 3; i++)
            {
                if (fichasDisponiblesParaJugador.Count == 0)
                {
                    Console.WriteLine("No hay más fichas disponibles.");
                    break;
                }

                Ficha fichaSeleccionada = SeleccionarFicha(fichasDisponiblesParaJugador);

                if (fichaSeleccionada != null)
                {
                    jugador.Fichas.Add(fichaSeleccionada);
                }
                else
                {
                    Console.WriteLine("Selección inválida.");
                    i--;
                }
            }
            System.Threading.Thread.Sleep(2000);
            Console.Clear();
        }   
    }

    public Ficha SeleccionarFicha(List<Ficha> fichasDisponibles)
    {
        Console.WriteLine("\nFichas disponibles:");
        for(int i=0;i<Ficha.FichasDisponibles.Count;i++){
            Console.WriteLine($"{i+1}. {Ficha.FichasDisponibles[i].Nombre}");
        }

        Console.WriteLine("Elija una ficha ingresando su numero.");
        int selection;

        while(!int.TryParse(Console.ReadLine(), out selection) || selection < 1 || selection > Ficha.FichasDisponibles.Count) {
        Console.WriteLine("Entrada inválida. Por favor, selecciona un número válido.");
        }

        Ficha fichaSeleccionada = fichasDisponibles[selection - 1];
        fichasDisponibles.RemoveAt(selection - 1);
        Console.WriteLine($"Has seleccionado la ficha: {fichaSeleccionada.Nombre}");

        return fichaSeleccionada;
    }

    public void Game()
    {  
        Console.WriteLine("Generando el laberinto...");
        System.Threading.Thread.Sleep(1000);
        Console.Clear();
        Console.WriteLine("1- REINICIAR\n" + "0- SALIR");
    }
}


class Board {
    int dimension;
    int[,] directions;
    int[,] board;
    Random rand; 

    public static void BoardInitializer(){
        int dimension = 10;
        int[,] board = new int[dimension,dimension];
        for(int i=0;i<dimension;i++){
            for(int j=0;j<dimension;j++){
                Console.Write(0 + " \n");
            }
        }  
    }

    public static void BoardGenerator()
    {
        Random rand = new Random(); //elige valores aleatoriamente 
    }   

    public static void BoardValidator()
    {

    }

        
    public static void ImprimirLaberinto()
    {

    }

}


public class Ficha{
    public string Nombre { get; set; }

    public static List<Ficha> FichasDisponibles = new List<Ficha> {
    new Ficha { Nombre = "Tornado" },
    new Ficha { Nombre = "Neblina" },
    new Ficha { Nombre = "Rayo" },
    new Ficha { Nombre = "Ala" },
    new Ficha { Nombre = "Estrella" },
    new Ficha { Nombre = "Eclipse" },
    };

}

public class Player{
    public string Nombre {get; set;}
    public List<Ficha> Fichas { get; set; } = new List<Ficha>();

    public Player(string nombre) {
        Nombre = nombre;
    }    
}


public class Obstacules{
}


public class Tramps{
}

public class Habilidades{
// copo de nieve (trampa): congelar por 3 turnos
// lluvia (trampa): resbalar y retroceder 5 casillas
// rayo (trampa): paralizar un area cercana
// viento: resbala 3/5 casillas en direccion del viento
// ala: rebasa 1 obstaculo
// estrella: rompe 1 obstaculo
// neblina: nubla un area
// eclipse: toma control de la habilidad de la ficha enemiga 
// tormenta: crea un paralizante temporal
}





