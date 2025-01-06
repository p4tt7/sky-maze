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
}


public class Board {
    static int dimension = 10;
    int[,] directions = {{-1,0},{0,1}}; //Norte y Este respectivamente
    int[,] board = new int[dimension,dimension];
    Random rand = new Random(); 

    public void BoardInitializer(){
        for(int i=0;i<dimension;i++){
            for(int j=0;j<dimension;j++){
                board[i,j]=1;
            }
        }  
    }

    public void BoardGenerator() //binary tree
    {
        int column=dimension;
        int row=dimension;
        Random rand = new Random(); //elige valores aleatoriamente 
        
        for(int i = 0; i < dimension; i++){
            for(int j = 0; j < dimension; j++){
                int valid_directions = 0; // cuenta direcciones válidas
        
                //verificar si hay direcciones validas
                if(Range(i - 1, j) && board[i - 1, j] != 0){
                    valid_directions++; // al norte
                } 
                if(Range(i, j + 1) && board[i, j + 1] != 0){
                    valid_directions++; // al este
                } 

                if(valid_directions > 0){ // si hay al menos una dirección válida
                    int random_direction = rand.Next(0, valid_directions);

                    //mover al norte
                    if(random_direction == 0 && Range(i - 1, j) && board[i - 1, j] != 0){
                        board[i, j] = 0; // Marcar la celda actual como parte del camino
                        board[i - 1, j] = 0; // Marcar la celda hacia el norte como parte del camino
                    }
                        //sino pues el este
                    if(random_direction == 1 && Range(i, j + 1) && board[i, j + 1] != 0){
                        board[i, j] = 0; // Marcar la celda actual como parte del camino
                        board[i, j + 1] = 0; // Marcar la celda hacia el este como parte del camino
                    }
                }    
            }    
        }   
    }

    public bool Range(int row, int column){
        if(row>=0 && row<dimension && column>=0 && column<dimension){
            return true;

        }
        else{
            return false;
        }
    }

    public static bool BoardValidator() //lee algoritmo
    {
        return true;
  
    }

        
    public void PrintBoard()
    {
        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                if (board[i, j] == 0)
                {
                    Console.Write(" ");
                }
                else
                {
                    Console.Write("#");
                }
            }
            Console.WriteLine();
        }
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


public class Trampa{
//    public static List<Trampa> Trampas = new List<Trampa> {
//    new Trampa {Nombre = "Copo de Nieve"},
//    new Trampa {Nombre = "LLuvia"},
//    new Trampa {Nombre = "Rayo"},
//    };
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





