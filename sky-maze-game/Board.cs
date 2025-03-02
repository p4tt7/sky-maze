namespace sky_maze_game.GameLogic;

public class Board
{
    public static int dimension;
    public static int center;

    public static string[,] board;
    public static int[,] direcciones = { { 0, -1 }, { 0, 1 }, { -1, 0 }, { 1, 0 } };


    public static void BoardInitializer() //Inicializa todo en pared
    {
        board = new string[dimension, dimension];
        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                board[i, j] = "w";
            }
        }
    }

    public static string[,] BoardGenerator() //Recursive Backtrack
    {
        board[0, 0] = "c"; //Ajusta la primera casilla a camino
        board[center, center] = "c"; //Ajusta el centro a camino

        foreach (Position Posicion in Position.InitialPositionFichas)
        {
            board[Posicion.x, Posicion.y] = "c"; //Ajusta las casillas de las fichas a camino
        }

        GenerateMaze(board, 0, 0); //Comienza a partir del inicio
        return board;
    }

    private static void GenerateMaze(string[,] board, int currentRow, int currentCol)
    {
        int[] indices = { 0, 1, 2, 3 }; //Indice de direcciones
        ShuffleDirections(indices); //Randomea las direcciones (Fisher Yates)

        for (int i = 0; i < 4; i++)
        {
            int directionIndex = indices[i]; //Por cada direccion, toma una random
            int newRow = currentRow + direcciones[directionIndex, 0] * 2;
            int newCol = currentCol + direcciones[directionIndex, 1] * 2;
            //Genera las nuevas columnas y filas usando la direccion random, *2 para dejar una casilla de por 
            //medio y evitar paredes dobles

            if (Range(board.GetLength(0), newRow, newCol) && board[newRow, newCol] == "w")
            { 
                //si esta en el rango, y es pared (es decir que no ha sido visitada)

                board[currentRow + direcciones[directionIndex, 0], currentCol + direcciones[directionIndex, 1]] = "c";
                board[newRow, newCol] = "c";
                //vuelve camino la celda que encontraste, y la otra a dos casillas para hacer camino

                GenerateMaze(board, newRow, newCol); //llamado recursivo desde la ultima casilla que se creo
            }
        }
    }

    private static void ShuffleDirections(int[] indices) //randomizador de direcciones (Fisher Yates)
    {
        int n = indices.Length; //Cantidad de direcciones
        Random rand = new Random(); //Instancia de la clase random

        for (int i = n - 1; i > 0; i--) //Por cada indice
        {

            int j = rand.Next(0, i + 1); //Genera un numero aleatorio entre 0 e i

            int temp = indices[i]; //Guardar  el valor del indice i
            indices[i] = indices[j]; //Intercambiar con el valor del indice j
            indices[j] = temp; //Guardar j como el anterior valor de i
        }
    }


    public static int[,] DistanceValidator(string[,] board, int firstRow, int firstColumn) //algoritmo de lee
    {

        int[,] distancias = new int[dimension, dimension]; 

        for (int i = 0; i < dimension; i++) //Inicializa todo el camino en -1
        {
            for (int j = 0; j < dimension; j++)
            {
                if (board[i, j] != "w")
                {
                    distancias[i, j] = -1;

                }
            }
        }

        distancias[firstRow, firstColumn] = 1; //La primera casilla se vuelve 1

        int[] dr = { -1, 1, 0, 0, -1, 1, -1, 1 };
        int[] dc = { 0, 0, 1, -1, -1, -1, 1, 1 };
        bool change;

        do
        {
            change = false;

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    if (distancias[i, j] > 0)
                    {
                        continue;
                    }
                    if (board[i, j] == "w")
                    {
                        continue;
                    }

                    for (int d = 0; d < dr.Length; d++) //Recorre todas las direcciones posibles
                    {
                        int vr = i + dr[d];
                        int vc = j + dc[d];

                        if (Range(dimension, vr, vc) && distancias[vr, vc] == -1 && board[vr, vc] == "c") //Si esta en el rango, la nueva casilla es -1, y es camino
                        {
                            distancias[vr, vc] = distancias[i, j] + 1; //Actualiza la distancia en la nueva posicion
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
        int[] dr = { -1, 1, 0, 0 }; // arriba, abajo, izquierda, derecha
        int[] dc = { 0, 0, -1, 1 };

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

                        if (Range(dimension, vr, vc))
                        {

                            board[i, j] = "c";
                            distancias[i, j] = 0;
                            board[vr, vc] = "c";
                            distancias[vr, vc] = 0;
                            break;

                        }
                    }
                }
            }
        }
    }
}














