using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using sky_maze_game.GameUI;

namespace sky_maze_game.GameLogic;

public class GameLogic{

    public static void SelectionMenu()
    {
        //JUGADORES
        Console.WriteLine("Introduzca la cantidad de jugadores:");
        if (!int.TryParse(Console.ReadLine(), out int cant_jugadores) || cant_jugadores<1 || cant_jugadores>4) {
            Console.WriteLine("Entrada inválida. Debe ser un número entre 1 y 4.");
            System.Threading.Thread.Sleep(2000);
            Console.Clear();
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
        board[6,6]="c"; 
    }

    public static string[,] BoardGenerator(){ //recursive backtrack
        board[0,0]="c"; //marca el inicio
        board[9,9]="c";
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
                    if(board[i,j]=="w"){
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

            private static void Center(int center){
            int[,] distancias = DistanceValidator(board, 0, 0);
    
            if (distancias[center, center] == -1) {
                ConnectToCenter(center);
            }
    }

    private static void ConnectToCenter(int center) {
        int[] dr = { -1, 1, 0, 0 };
        int[] dc = { 0, 0, -1, 1 };

        for (int d = 0; d < dr.Length; d++) {
            int vr = center + dr[d];
            int vc = center + dc[d];

            if (Range(dimension, vr, vc) && board[vr, vc] == "c") {
                board[center + dr[d] / 2, center + dc[d] / 2] = "c";
                break;
            }
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

public class Position{
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


    public static void Movement(Ficha ficha){

        int pasos = ficha.Velocidad;               
        

        while(pasos>0){
            Console.Clear();
            GameUI.GameUI.PrintBoard();
            Console.WriteLine($"Puedes moverte {pasos} pasos.");
            ConsoleKeyInfo teclaPresionada = Console.ReadKey(true);

            int lastX = ficha.Posicion.x;
            int lastY = ficha.Posicion.y;
            bool moverse = false;

            if (teclaPresionada.Key == ConsoleKey.W || teclaPresionada.Key == ConsoleKey.UpArrow) {
                if (ficha.Posicion.x > 0 && Board.board[ficha.Posicion.x - 1, ficha.Posicion.y] != "w") {
                    ficha.Posicion.x -= 1; 
                    moverse=true;
                } 
            }

            else if (teclaPresionada.Key == ConsoleKey.A || teclaPresionada.Key == ConsoleKey.LeftArrow) {
                if (ficha.Posicion.y > 0 && Board.board[ficha.Posicion.x, ficha.Posicion.y - 1] != "w") {
                    ficha.Posicion.y -= 1; 
                    moverse=true;
                } 
            }
      
            else if (teclaPresionada.Key == ConsoleKey.S || teclaPresionada.Key == ConsoleKey.DownArrow) {
                if (ficha.Posicion.x < Board.dimension-1 && Board.board[ficha.Posicion.x + 1, ficha.Posicion.y] != "w") {
                    ficha.Posicion.x += 1;
                    moverse=true;
                } 
                
                else {
                    Console.WriteLine("Obstáculo encontrado, movimiento detenido.");
                    break; 
                }
            }
       
            else if (teclaPresionada.Key == ConsoleKey.D || teclaPresionada.Key == ConsoleKey.RightArrow) {
                if (ficha.Posicion.y < Board.dimension-1 && Board.board[ficha.Posicion.x, ficha.Posicion.y + 1] != "w") {
                    ficha.Posicion.y += 1; 
                    moverse=true;
                } 
            }
       
            else {
                Console.WriteLine("Tecla no reconocida.");
                continue;
            }

            if (moverse){
                Board.board[lastX, lastY] = "c"; 
                Board.board[ficha.Posicion.x, ficha.Posicion.y] = ficha.Simbolo;

                if (IsTrampa(ficha))
                {
                    Console.WriteLine("¡Trampa activada!");
                    //UseTrampa(trampa);
                }
                pasos--;
            }
        }
    }


    public static bool IsTrampa(Ficha ficha) {
        foreach (var trampa in Trampa.Trampas) {
            if (Board.board[ficha.Posicion.x, ficha.Posicion.y] == trampa.Simbolo) {
                return true; 
            }
        }
        return false; 
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

}

public class Ficha{

    public string Nombre { get; set; }
    public string Simbolo {get; set; }
    public Position Posicion { get; set; }
    public int Velocidad { get; set; }
    public int CoolingTime { get; set; }
    public static Random rand = new Random();

    public enum HabilidadType{
        Rainbow,
        Shadow,
        WindVelocity,
        Fly,
        Star,
        Eclipse,
    }

    public HabilidadType Habilidad { get; set; }

    public static List<Ficha> FichasDisponibles = new List<Ficha>{
    new Ficha {Nombre = "Arcoiris" , Simbolo = "🌈" , Velocidad = 5 , CoolingTime = 15},
    new Ficha {Nombre = "Neblina" , Simbolo = "🌫️" , Velocidad = 5 , CoolingTime = 2},
    new Ficha {Nombre = "Viento" , Simbolo = "🌬️" , Velocidad = 10 , CoolingTime = 3},
    new Ficha {Nombre = "Ala" , Simbolo = "🪽" , Velocidad = 5 , CoolingTime = 3},
    new Ficha {Nombre = "Estrella" , Simbolo = "⭐" , Velocidad = 5 , CoolingTime = 6},
    new Ficha {Nombre = "Eclipse" , Simbolo = "🌑" , Velocidad = 5 , CoolingTime = 10},
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

    public static void WindVelocity(Ficha ficha){
        string[] wind_directions = {"norte", "sur","este","oeste"};
        int index = rand.Next(wind_directions.Length);
        string wind_direction = wind_directions[index];
        Console.WriteLine($"La direccion del viento es: {wind_directions[index]}");

        if(wind_direction == "oeste"){
            ficha.Velocidad += 5;
            
            for (int i = 0; i < ficha.Velocidad; i++){
                if (ficha.Posicion.x > 0 && Board.board[ficha.Posicion.x - 1, ficha.Posicion.y] != "w"){
                    ficha.Posicion.x--; 
                }
                else{
                    Console.WriteLine("Movimiento no válido");
                    break; 
                }
            }
        }

        if(wind_direction == "sur"){
            ficha.Velocidad += 5;
            
            for (int i = 0; i < ficha.Velocidad; i++){
                if (ficha.Posicion.y < Board.dimension - 1 && Board.board[ficha.Posicion.x - 1, ficha.Posicion.y] != "w"){
                    ficha.Posicion.y++; 
                }
                else{
                    Console.WriteLine("Movimiento no válido");
                    break; 
                }
            }
        }

        if(wind_direction == "este"){
            ficha.Velocidad += 5;
            
            for (int i = 0; i < ficha.Velocidad; i++){
                if (ficha.Posicion.x<19 && Board.board[ficha.Posicion.x - 1, ficha.Posicion.y] != "w"){
                    ficha.Posicion.x++; 
                }
                else{
                    Console.WriteLine("Movimiento no válido");
                    break; 
                }
            }
        }

        if(wind_direction == "norte"){
            ficha.Velocidad += 5;
            
            for (int i = 0; i<ficha.Velocidad; i++){
                if (ficha.Posicion.y > 0 && Board.board[ficha.Posicion.x - 1, ficha.Posicion.y] != "w"){
                    ficha.Posicion.y--; 
                }
                else{
                    Console.WriteLine("Movimiento no válido");
                    break; 
                }
            }
        } 

        ficha.Velocidad -= 5;  
    }

    public static void Fly(Ficha ficha){

        int pasos = 3;
        
        for(int i=0;i<pasos;i++){
            Console.Clear();
            GameUI.GameUI.PrintBoard();
            ConsoleKeyInfo teclaPresionada = Console.ReadKey();

            int lastX = ficha.Posicion.x;
            int lastY = ficha.Posicion.y;

            if(teclaPresionada.Key == ConsoleKey.W || teclaPresionada.Key == ConsoleKey.UpArrow){
                if(ficha.Posicion.x>0){
                    ficha.Posicion.x--;
                    i--;
                }
                else{
                    Console.WriteLine("Movimiento no valido");
                }
            }
            else if(teclaPresionada.Key == ConsoleKey.A || teclaPresionada.Key == ConsoleKey.LeftArrow){
                if(ficha.Posicion.y>0){
                    ficha.Posicion.y--;
                    i--;
                }
                else{
                    Console.WriteLine("Movimiento no valido");
                }
            }
            else if(teclaPresionada.Key == ConsoleKey.S || teclaPresionada.Key == ConsoleKey.DownArrow){
                if(ficha.Posicion.x<19){
                    ficha.Posicion.x++;
                    i--;
                }
                else{
                    Console.WriteLine("Movimiento no valido");
                }
            }
            else if(teclaPresionada.Key == ConsoleKey.D || teclaPresionada.Key == ConsoleKey.RightArrow){
                if(ficha.Posicion.y<19){
                    ficha.Posicion.y++;
                    i--;
                }
                else{
                    Console.WriteLine("Movimiento no valido");
                }
            }

            else{
                Console.WriteLine("Tecla no reconocida.");
                i--; 
            }

            Board.board[lastX, lastY] = "c"; 
            Board.board[ficha.Posicion.x, ficha.Posicion.y] = ficha.Simbolo;
        }
    }
  
    public static void Eclipse(Ficha ficha){
        foreach(Player jugador in Player.jugadores){
            foreach(Ficha fichaSeleccionada in jugador.SelectedFichas){
                Console.WriteLine($"{fichaSeleccionada.Nombre} {fichaSeleccionada.Simbolo}");
            }
        }
        string habilidad = Console.ReadLine();

        

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

        //falta
        }
    }

    public static void Shadow(Ficha ficha){

    }

    public static void UseHabilidad(Ficha ficha){
        switch(ficha.Habilidad){
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

public class Player{
    public string Nombre {get; set;}
    public List<Ficha> SelectedFichas { get; set; } = new List<Ficha>();
    public static List<Player> jugadores = new List<Player>();

    public Player(string nombre) {
       Nombre = nombre;
    }    
}
    

public class Trampa{
    public static Random random = new Random();
    public string Nombre { get; set; }
    public string Simbolo { get; set; }
    public Position Posicion { get; set; }

    public enum HabilidadType{
        Copo,
        LLuvia,
        Rayo,
        Agujero,

    }

    public HabilidadType Habilidad { get; set; }

    public static List<Trampa> Trampas = new List<Trampa>{
    new Trampa {Nombre = "Copo de Nieve" , Simbolo = "⛄"},
    new Trampa {Nombre = "LLuvia" , Simbolo = "🌧️"},
    new Trampa {Nombre = "Rayo" , Simbolo = "⚡"},
    new Trampa {Nombre = "Agujero en el Cielo" , Simbolo = "🌀"}
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

   public static void Snowflake(Ficha ficha, Trampa trampa){
        if (ficha.Posicion.x == trampa.Posicion.x && ficha.Posicion.y == trampa.Posicion.y){
            Board.board[trampa.Posicion.x, trampa.Posicion.y] = "c"; 
        }

    } 

    public static void Rain(Ficha ficha, Trampa trampa) {
        if (ficha.Posicion.x == trampa.Posicion.x && ficha.Posicion.y == trampa.Posicion.y) {
            Board.board[trampa.Posicion.x, trampa.Posicion.y] = "c"; 
    
            ficha.Posicion.x = Position.InitialPositionFichas.x;
            ficha.Posicion.y = Position.InitialPositionFichas.y;
        }
    }

    public static void Rayo(Trampa trampa, Ficha ficha){
       if(ficha.Posicion==Trampa.Posicion){
            Board.board[Trampa.Posicion] = "c";
            ficha.Posicion.x = trampa.Simbolo;
        }
       
    }

        public static void Skyhole(Ficha ficha, Trampa[] trampas) {
    
        foreach (Trampa trampa in trampas) {
            if (ficha.Posicion.x == trampa.Posicion.x && ficha.Posicion.y == trampa.Posicion.y && ficha.Simbolo == trampa.Simbolo) {
                foreach (Trampa otroSkyhole in trampas) {
                    if (otroSkyhole.Simbolo == trampa.Simbolo &&
                        (otroSkyhole.Posicion.x != trampa.Posicion.x || otroSkyhole.Posicion.y != trampa.Posicion.y)) {
                        ficha.Posicion = otroSkyhole.Posicion;
                        Console.WriteLine($"Ficha teletransportada al otro Skyhole en posición ({otroSkyhole.Posicion.x}, {otroSkyhole.Posicion.y})");
                        return;
                    }
                }
    
                Console.WriteLine("No existe otro Skyhole, no se puede teletransportar.");
                return;
                }
            }
        }

        public static void UseHabilidad(Ficha ficha){
        switch(ficha.Habilidad){
            case Ficha.HabilidadType.Rainbow:
                Rain(ficha , trampa);
                break;
            case Ficha.HabilidadType.Fly:
                Snowflake(ficha , trampa);
                break;
            case Ficha.HabilidadType.Star:
                Rayo(ficha , trampa);
                break;
            case Ficha.HabilidadType.Eclipse:
                Skyhole(ficha , trampa);
                break;
        }                
    }
    }













