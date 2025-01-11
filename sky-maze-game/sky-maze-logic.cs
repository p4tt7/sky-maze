using System.Runtime.CompilerServices;
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

        for (int i=1;i<=cant_jugadores;i++){
            Console.WriteLine($"Ingresa el nombre del Jugador {i}:");
            string nombreJugador = Console.ReadLine();
            Player.jugadores.Add(new Player(nombreJugador));  
        }
        
        System.Threading.Thread.Sleep(2000);
        Console.Clear();

        //FICHAS
        foreach (Player jugador in Player.jugadores){

            List<Ficha> FichasDisponibles = new List<Ficha>(Ficha.FichasDisponibles);

            for (int i=0;i<3;i++)
            {
                if(FichasDisponibles.Count == 0){
                    Console.WriteLine("No hay más fichas disponibles.");
                    break;
                }

                Console.WriteLine("Elige una ficha:");
                for (int j = 0; j < FichasDisponibles.Count; j++){
                    Console.WriteLine($"{j + 1}. {FichasDisponibles[j].Nombre}");
                }

                Console.Write("Ingresa el numero de ficha para seleccionarla: ");
                int eleccion = int.Parse(Console.ReadLine());
                if(eleccion>0 && eleccion <= FichasDisponibles.Count){
                    Ficha fichaSeleccionada = FichasDisponibles[eleccion - 1];
                    jugador.SelectedFichas.Add(fichaSeleccionada);

                    FichasDisponibles.RemoveAt(eleccion - 1);
                    Console.WriteLine($"Has seleccionado: {fichaSeleccionada.Nombre}");

                }

                else
                {
                    Console.WriteLine("Selección inválida. Intenta de nuevo");
                    i--;
                }
            }
            System.Threading.Thread.Sleep(2000);
            Console.Clear();
        }  
    }
}

public class Board{
    public static int dimension = 20;
    public static string[,] board = new string[dimension,dimension];
    public static string winning_position = board[10,10];
    public static int[,] direcciones = {{0,-1},{0,1},{-1,0},{1,0}};


    public static void BoardInitializer(){
        for(int i=0;i<dimension;i++){
            for(int j=0;j<dimension;j++){
                board[i,j]="w";
            }
        }  
    }

    public static string[,] BoardGenerator() //binary tree
    {
        board[0,0]="c"; //marca el inicio
        GenerateMaze(board, 0, 0); //genera el laberunto
        return board;
    }

    private static void GenerateMaze(string[,] board, int currentRow, int currentCol){
        int[] indices = { 0, 1, 2, 3 };
        ShuffleDirections(indices);

        for (int i = 0; i < 4; i++){
            int directionIndex = indices[i];
            int newRow = currentRow + direcciones[directionIndex, 0] * 2;
            int newCol = currentCol + direcciones[directionIndex, 1] * 2;

            if (newRow >= 0 && newRow < board.GetLength(0) && newCol >= 0 && newCol < board.GetLength(1) && board[newRow, newCol] == "w"){
                
                board[currentRow + direcciones[directionIndex, 0], currentCol + direcciones[directionIndex, 1]] = "c";
                board[newRow, newCol] = "c";

                GenerateMaze(board, newRow, newCol); 
            }
        }
    }

    private static void ShuffleDirections(int[] indices){
        int n = indices.Length;
        Random rand = new Random();

        for (int i = n - 1; i > 0; i--){
    
            int j = rand.Next(0, i + 1);

            int temp = indices[i];
            indices[i] = indices[j];
            indices[j] = temp;
        }
    }

    public static int[,] DistanceValidator(string[,] board, int firstRow, int firstColumn) //algoritmo de lee
    {
        int[,] distancias = new int[dimension,dimension];
        distancias[firstRow,firstColumn] = 1;
        int[] dr = {-1,1,0,0,-1,1,-1,1};
        int[] dc = {0,0,1,-1,-1,-1,1,1};
        bool change;

        for(int i = 0; i < dimension; i++){
            for(int j = 0; j < dimension; j++){
                distancias[i, j] = -1;
            }
        }

        do{
            change = false;

            for(int i=0;i<dimension;i++){
                for(int j=0;j<dimension;j++){
                    if(distancias[i,j]==0){
                        continue;
                    }
                    if(board[i,j]=="w" || board[i,j]=="o"){
                        continue;
                    }
                    for(int d=0;d<dr.Length;d++){
                        int vr= i+dr[d];
                        int vc= j+dc[d];

                        if(Range(dimension, vr, vc) && distancias[vr,vc]==0 && board[vr,vc]=="c"){
                            distancias[vr,vc]=distancias[i,j]+1;
                            change=true;
                        } 
                    }  
                }
            }
        } while(change);
        
        return distancias;
    }

    public static bool Range(int dimension, int row, int column){
        if(row>=0 && row<dimension && column >=0 && column<dimension){
            return true;
        }
        else{
            return false;
        }

    }

    public static void ValidatedBoard(string[,] board, int[,] distancias){

        int[] dr = {-1,1,0,0,-1,1,-1,1};
        int[] dc = {0,0,1,-1,-1,-1,1,1};

        for(int i=0;i<dimension;i++){
            for(int j=0; j<dimension;j++){
                if(distancias[i,j]==-1){

                    for(int d=0;d<dr.Length;d++){
                        int vr= i+dr[d];
                        int vc= j+dc[d];

                        if(Range(dimension,vr,vc) && distancias[vr,vc]!=-1){
                            board[i,j]="c";
                            distancias[i,j]=distancias[vr,vc]+1;
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

    public static List<Position> InitialPositionFichas = new List<Position>{
        new Position {x=0 , y=0},
        new Position {x=0 , y=1},
        new Position {x=1 , y=0},

        new Position {x=0 , y=19},
        new Position {x=0 , y=18},
        new Position {x=1 , y=19},

        new Position {x=19 , y=0},
        new Position {x=18 , y=0},
        new Position {x=19 , y=1},

        new Position {x=19 , y=19},
        new Position {x=19 , y=18},
        new Position {x=18 , y=19}
    };
}

public class Ficha{
    public string Nombre { get; set; }
    public string Simbolo {get; set; }
    public Position Posicion { get; set; }

    public static List<Ficha> FichasDisponibles = new List<Ficha>{
    new Ficha {Nombre = "Tornado" , Simbolo = "🌪️"},
    new Ficha {Nombre = "Neblina" , Simbolo = "🌫️"},
    new Ficha {Nombre = "Viento" , Simbolo = "🌬️"},
    new Ficha {Nombre = "Ala" , Simbolo = "🪽"},
    new Ficha {Nombre = "Estrella" , Simbolo = "⭐"},
    new Ficha {Nombre = "Eclipse" , Simbolo = "🌑"},
    };

    public static void FichaInitializer(List<Player> jugadores){
        int indexPosition = 0;

        foreach(Player jugador in jugadores){
            foreach(Ficha fichaSeleccionada in jugador.SelectedFichas){
                Position pos = Position.InitialPositionFichas[indexPosition];
                Board.board[pos.x,pos.y]= fichaSeleccionada.Simbolo;
                fichaSeleccionada.Posicion = pos;
                indexPosition++;
            }
        }
            
    }

    public static void Movement(){

    }

    public static bool IsWinning(){
        return true;
    }


}

public class Player{
    public string Nombre {get; set;}
    public List<Ficha> SelectedFichas { get; set; } = new List<Ficha>();
    public static List<Player> jugadores = new List<Player>();

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
                if(Board.board[i,j]=="c" && random.NextDouble()<0.05){
                    Board.board[i,j]="o";
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
                if(Board.board[i,j]=="c" && random.NextDouble()<0.05){
                    Board.board[i,j]="t";
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





