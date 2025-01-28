namespace sky_maze_game.GameLogic;

public class Board
{
    public static int dimension;
    public static int center;

    public static string[,] board;
    public static int[,] direcciones = { { 0, -1 }, { 0, 1 }, { -1, 0 }, { 1, 0 } };


    public static void BoardInitializer()
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

    public static string[,] BoardGenerator() //recursive backtrack
    {
        board[0, 0] = "c";
        board[center, center] = "c";

        foreach (Position Posicion in Position.InitialPositionFichas)
        {
            board[Posicion.x, Posicion.y] = "c";
        }

        GenerateMaze(board, 0, 0);
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

        int[,] distancias = new int[dimension, dimension];

        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                if (board[i, j] != "w")
                {
                    distancias[i, j] = -1;

                }
            }
        }

        distancias[firstRow, firstColumn] = 1;

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

                    for (int d = 0; d < dr.Length; d++)
                    {
                        int vr = i + dr[d];
                        int vc = j + dc[d];

                        if (Range(dimension, vr, vc) && distancias[vr, vc] == -1 && board[vr, vc] == "c")
                        {
                            distancias[vr, vc] = distancias[i, j] + 1;
                            change = true;
                        }
                    }
                }
            }

        } while (change);

        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                Console.Write(distancias[i, j]);

            }
            Console.WriteLine();


        }

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
        int dimension = board.GetLength(0); // Suponiendo que board es cuadrado
        int[] dr = { -1, 1, 0, 0 }; // arriba, abajo, izquierda, derecha
        int[] dc = { 0, 0, -1, 1 };

        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                if (distancias[i, j] == -1)
                {
                    for (int d = 0; d < dr.Length; d++) // Revisa todas las direcciones
                    {
                        int vr = i;
                        int vc = j;

                        while (Range(dimension, vr + dr[d], vc + dc[d]))
                        {
                            vr += dr[d];
                            vc += dc[d];

                            if (board[vr, vc] != "w")
                            {
                               
                                int markR = i;
                                int markC = j;
                                while (markR != vr || markC != vc)
                                {
                                    board[markR, markC] = "c";
                                    distancias[markR, markC] = 0;
                                    markR += dr[d];
                                    markC += dc[d];
                                }
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
}














