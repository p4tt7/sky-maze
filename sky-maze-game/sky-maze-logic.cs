using System.Security.Cryptography.X509Certificates;

namespace sky_maze_game.GameLogic;

public class GameLogic{

    public static void SelectionMenu()
    {
        //JUGADORES
        Console.WriteLine("Introduzca la cantidad de jugadores:");
        if (!int.TryParse(Console.ReadLine(), out int cant_jugadores) || cant_jugadores<1 || cant_jugadores>4) {
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

    public static Ficha SeleccionarFicha(List<Ficha> fichasDisponibles)
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

public class Board{
    public static int dimension = 20;
    public static int[,] board = new int[dimension,dimension];
    Random rand = new Random();


    public static void BoardInitializer(){
        for(int i=0;i<dimension;i++){
            for(int j=0;j<dimension;j++){
                board[i,j]=1;
            }
        }  
    }

    public static void BoardGenerator() //binary tree
    {
        Random rand = new Random();
        
        for(int i=0; i<dimension; i++){
            for(int j=0; j<dimension; j++){
                List<int[]> validDirections = new List<int[]>();
            
                if(i>0){
                    validDirections.Add(new int[] { -1, 0 }); // norte
                }
                if(j<dimension-1){ 
                    validDirections.Add(new int[] { 0, 1 }); // este
                }

                if (validDirections.Count>0 && rand.NextDouble()<0.7){ 
                    int[] chosenDirection = validDirections[rand.Next(validDirections.Count)];
                    int newRow = i+chosenDirection[0];
                    int newCol = j+chosenDirection[1];
    
                    board[newRow, newCol] = 0;
                }    
            }    
        }   
    }


    public static bool BoardValidator() //algoritmo de lee
    {

        
        return true;
  
    }
}

public class Ficha{
    Random rand = new Random();
    public string Nombre { get; set; }
    public string Simbolo {get; set; }

    public static List<Ficha> FichasDisponibles = new List<Ficha>{
    new Ficha {Nombre = "Tornado" , Simbolo = "🌪️"},
    new Ficha {Nombre = "Neblina" , Simbolo = "🌫️"},
    new Ficha {Nombre = "Viento" , Simbolo = "🌬️"},
    new Ficha {Nombre = "Ala" , Simbolo = "🪽"},
    new Ficha {Nombre = "Estrella" , Simbolo = "⭐"},
    new Ficha {Nombre = "Eclipse" , Simbolo = "🌑"},
    };

    public static void FichaInitializer(){

    }
}

public class Player{
    public string Nombre {get; set;}
    public List<Ficha> Fichas { get; set; } = new List<Ficha>();

    public Player(string nombre) {
       Nombre = nombre;
    }    
}


public class Obstacule{
    public static Random random = new Random();
    public string Nombre { get; set; }
    public string Simbolo { get; set; }

    public static List<Obstacule> Obstaculos = new List<Obstacule>{
        new Obstacule {Nombre = "Agujero en el Cielo" , Simbolo = "🌀"},
        new Obstacule {Nombre = "Zona de Tormenta" , Simbolo = "🌩️"}
    };

    public static void ObstaculeGenerator(){
        for(int i=0;i<Board.dimension;i++){
            for(int j=0;j<Board.dimension;j++){
                if(Board.board[i,j]==0 && random.NextDouble()<0.1){
                    Board.board[i,j]=2;
                }
            }
        }
    }


}


public class Trampa{
    public static Random random = new Random();
    public string Nombre { get; set; }
    public string Simbolo { get; set; }

    public static List<Trampa> Trampas = new List<Trampa>{
    new Trampa {Nombre = "Copo de Nieve" , Simbolo = "⛄"},
    new Trampa {Nombre = "LLuvia" , Simbolo = "🌧️"},
    new Trampa {Nombre = "Rayo" , Simbolo = "⚡"},
    };

    public static void TrampaGenerator(){
        for(int i=0;i<Board.dimension;i++){
            for(int j=0;j<Board.dimension;j++){
                if(Board.board[i,j]==0 && random.NextDouble()<0.1){
                    Board.board[i,j]=-1;
                }
            }
        }
    }   
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





