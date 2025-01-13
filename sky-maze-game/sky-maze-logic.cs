using System.ComponentModel;
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
            int newRow = currentRow+direcciones[directionIndex, 0]*2;
            int newCol = currentCol+direcciones[directionIndex, 1]*2;

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

        for (int i=n-1; i>0;i--){
    
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
    public int Velocidad { get; set; }
    public int CoolingTime { get; set; }
    public static Random rand = new Random();

    public static List<Ficha> FichasDisponibles = new List<Ficha>{
    new Ficha {Nombre = "Arcoiris" , Simbolo = "🌈" , Velocidad = 2 , CoolingTime = 3},
    new Ficha {Nombre = "Neblina" , Simbolo = "🌫️" , Velocidad = 1 , CoolingTime = 2},
    new Ficha {Nombre = "Viento" , Simbolo = "🌬️" , Velocidad = 1 , CoolingTime = 3},
    new Ficha {Nombre = "Ala" , Simbolo = "🪽" , Velocidad = 2 , CoolingTime = 3},
    new Ficha {Nombre = "Estrella" , Simbolo = "⭐" , Velocidad = 3 , CoolingTime = 6},
    new Ficha {Nombre = "Eclipse" , Simbolo = "🌑" , Velocidad = 3 , CoolingTime = 10},
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

    public static void Movement(Ficha ficha){

        int pasos = ficha.Velocidad;

        for(int i=0;i<pasos;i++){
            ConsoleKeyInfo teclaPresionada = Console.ReadKey();
            Board.board[ficha.Posicion.x, ficha.Posicion.y] = "c";

            if(teclaPresionada.Key == ConsoleKey.W || teclaPresionada.Key == ConsoleKey.UpArrow){
                if(ficha.Posicion.x>0 && Board.board[ficha.Posicion.x - 1, ficha.Posicion.y] != "w"){
                    ficha.Posicion.x--;
                }
                else{
                    Console.WriteLine("Movimiento no valido");
                    continue;
                }
            }
                
            else if(teclaPresionada.Key == ConsoleKey.A || teclaPresionada.Key == ConsoleKey.LeftArrow){
                if(ficha.Posicion.y>0 && Board.board[ficha.Posicion.x, ficha.Posicion.y-1] != "w"){
                    ficha.Posicion.y--;
                }
                else{
                    Console.WriteLine("Movimiento no valido");
                    continue;
                }
            }
            else if(teclaPresionada.Key == ConsoleKey.S || teclaPresionada.Key == ConsoleKey.DownArrow){
                if(ficha.Posicion.x<19 && Board.board[ficha.Posicion.x + 1, ficha.Posicion.y] != "w"){
                    ficha.Posicion.x++;
                }
                else{
                    Console.WriteLine("Movimiento no valido");
                    continue;
                }
            }
            else if(teclaPresionada.Key == ConsoleKey.D || teclaPresionada.Key == ConsoleKey.RightArrow){
                if(ficha.Posicion.y<19 && Board.board[ficha.Posicion.x, ficha.Posicion.y+1] != "w"){
                    ficha.Posicion.y++;
                }
                else{
                    Console.WriteLine("Movimiento no valido");
                    continue;
                }
            }
            else{
                Console.WriteLine("Tecla no reconocida");
            }
 
            Board.board[ficha.Posicion.x, ficha.Posicion.y] = ficha.Simbolo;
        }
    }

    public static bool IsWinning(){
        foreach(Player jugador in Player.jugadores){
            foreach(Ficha fichaSeleccionada in jugador.SelectedFichas){
                if(fichaSeleccionada.Posicion.x == 9 && fichaSeleccionada.Posicion.y==9){
                    return true;
                }
            }
        }
        return false;
    }

    public static void WindVelocity(Ficha ficha){
        string[] wind_directions = {"norte", "sur","este","oeste"};
        int index = rand.Next(wind_directions.Length);
        string wind_direction = wind_directions[index];

        if(wind_direction == "oeste"){
            ficha.Velocidad += 2;
            
            for (int i = 0; i < ficha.Velocidad; i++){
                if (ficha.Posicion.x > 0 && Board.board[ficha.Posicion.x - 1, ficha.Posicion.y] != "p"){
                    ficha.Posicion.x--; 
                }
                else{
                    Console.WriteLine("Movimiento no válido");
                    break; 
                }
            }
        }

        if(wind_direction == "sur"){
            ficha.Velocidad += 2;
            
            for (int i = 0; i < ficha.Velocidad; i++){
                if (ficha.Posicion.y < 19 && Board.board[ficha.Posicion.x - 1, ficha.Posicion.y] != "p"){
                    ficha.Posicion.y++; 
                }
                else{
                    Console.WriteLine("Movimiento no válido");
                    break; 
                }
            }
        }

        if(wind_direction == "este"){
            ficha.Velocidad += 2;
            
            for (int i = 0; i < ficha.Velocidad; i++){
                if (ficha.Posicion.x<19 && Board.board[ficha.Posicion.x - 1, ficha.Posicion.y] != "p"){
                    ficha.Posicion.x++; 
                }
                else{
                    Console.WriteLine("Movimiento no válido");
                    break; 
                }
            }
        }

        if(wind_direction == "norte"){
            ficha.Velocidad += 2;
            
            for (int i = 0; i<ficha.Velocidad; i++){
                if (ficha.Posicion.y > 0 && Board.board[ficha.Posicion.x - 1, ficha.Posicion.y] != "p"){
                    ficha.Posicion.y--; 
                }
                else{
                    Console.WriteLine("Movimiento no válido");
                    break; 
                }
            }
        }   
    }

    public static void Fly(Ficha ficha){

        int pasos = 2;
        ConsoleKeyInfo teclaPresionada = Console.ReadKey();

        for(int i=0;i<pasos;i++){
            if(teclaPresionada.Key == ConsoleKey.W || teclaPresionada.Key == ConsoleKey.UpArrow){
                if(ficha.Posicion.x>0){
                    ficha.Posicion.x--;
                    break;
                }
                else{
                    Console.WriteLine("Movimiento no valido");
                }
            }
            if(teclaPresionada.Key == ConsoleKey.A || teclaPresionada.Key == ConsoleKey.LeftArrow){
                if(ficha.Posicion.y>0){
                    ficha.Posicion.y--;
                    break;
                }
                else{
                    Console.WriteLine("Movimiento no valido");
                }
            }
            if(teclaPresionada.Key == ConsoleKey.S || teclaPresionada.Key == ConsoleKey.DownArrow){
                if(ficha.Posicion.x<19){
                    ficha.Posicion.x++;
                    break;
                }
                else{
                    Console.WriteLine("Movimiento no valido");
                }
            }
            if(teclaPresionada.Key == ConsoleKey.D || teclaPresionada.Key == ConsoleKey.RightArrow){
                if(ficha.Posicion.y<19){
                    ficha.Posicion.y++;
                    break;
                }
                else{
                    Console.WriteLine("Movimiento no valido");
                }
            }
        }
    }
  
    public static void Eclipse(Ficha ficha){
        Console.WriteLine("Introduzca el nombre de la habilidad a copiar");

        foreach(Player jugador in Player.jugadores){
            foreach(Ficha fichaSeleccionada in jugador.SelectedFichas){
                Console.WriteLine($"{fichaSeleccionada.Nombre} {fichaSeleccionada.Simbolo}");
            }
        }
        string habilidad = Console.ReadLine();


        if(habilidad=="Arcoiris"){
            Rainbow(ficha);

        }

        if(habilidad=="Viento"){
            WindVelocity(ficha);

        }

        if(habilidad=="Ala"){
            Fly(ficha);

        }

        if(habilidad=="Estrella"){
            Star(ficha);

        }

        if(habilidad=="Eclipse"){
            Console.WriteLine("Habilidad ya adquirida! Intente con otra!");

        }

        else{
            Console.WriteLine("Nombre incorrecto. Vueva a intentarlo");
        }

    }

    
    public static void Rainbow(Ficha ficha){
        int[] dr = { -1, 1, 0, 0 };
        int[] dc = { 0, 0, -1, 1 };
    }


    public static void Star(Ficha ficha){
        int pasos = ficha.Velocidad; 
        int x = ficha.Posicion.x;
        int y = ficha.Posicion.y;

        for (int i=0; i<pasos; i++){

            if (x > 0 && Board.board[x-1,y] == "w"){ 
                Board.board[x-1,y] = "c"; 
                x--;
            }

            else if (x < 19 && Board.board[x+1,y] == "w"){
                Board.board[x+1,y] = "c"; 
                x++;
            }

            else if (y > 0 && Board.board[x,y-1] == "w"){
                Board.board[x,y-1] = "c"; 
                y--; 
            }

            else if (y < 19 && Board.board[x,y+1] == "w"){
                Board.board[x,y+1] = "c"; 
                y++;
            }

            else{
                break; 
            }

        //ficha.Posicion = new Posicion.x , Posicion.y;
        }
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
                    int indexObstacule = Obstacule.random.Next(0,Obstacule.Obstaculos.Count);
                    Board.board[i,j] = Obstacule.Obstaculos[indexObstacule].Simbolo;
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
                    int indexTrampa = Trampa.random.Next(0, Trampa.Trampas.Count);
                    Board.board[i,j] = Trampa.Trampas[indexTrampa].Simbolo;
                }
            }
        }
    }  

    public static void Snowflake(){
        // copo de nieve (trampa): congelar por 3 turnos

    } 

    public static void Rain(){
        // lluvia (trampa): resbalar y retroceder 5 casillas

    }

    public static void Rayo(){
        // rayo (trampa): paralizar un area cercana

    }

}












