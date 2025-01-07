using sky_maze_game.GameLogic;

namespace sky_maze_game.GameUI;


public class GameUI
{
    public static void Start(){
        Console.WriteLine(
        "        .--.     .--.      \n" +
        "     .-(    ). .-(    ).   .--.\n" +
        "    (___.__)_)(___.__)__) (__.__)\n" +
        "\n" +
        " .--. .-..-..-..-.  .-..-. .--. .----. .--. \n" +
        ": .--': :' ;: :: :  : `' :: .; :`--. :: .--'\n" +
        "`. `. :   ' `.  .'  : .. ::    :  ,',': `;  \n" +
        " _`, :: :.`. .' ;   : :; :: :: :.'.'_ : :__ \n" +
        "`.__.':_;:_;:_,'    :_;:_;:_;:_;:____;`.__.'\n" +
        "\n" +
        "     .--.       .--.      \n" +
        "    (___. ). .-(    ).   .--.\n" +
        "    (___.__) (___.__)__) (__.__)"
        );
            
        Console.WriteLine("1- COMENZAR\n" + "0- SALIR");
    }


    public static void PrintBoard()
    {
        for(int i = 0; i < Board.dimension; i++)
        {
            for(int j = 0; j < Board.dimension; j++)
            {
                if(Board.board[i, j]==0) //camino
                {
                    Console.Write("⬛");
                }

                if(Board.board[i,j]==1) //pared
                {
                    Console.Write("⬜");
                }

                
                if(Board.board[i, j]==-1){ //trampa
                    int indexTrampa = Trampa.random.Next(0, Trampa.Trampas.Count);
                    Console.Write(Trampa.Trampas[indexTrampa].Simbolo);
                }

                if(Board.board[i,j]== 2){
                    int indexObstacule = Obstacule.random.Next(0,Obstacule.Obstaculos.Count);
                    Console.Write(Obstacule.Obstaculos[indexObstacule].Simbolo);
                }
            }
            Console.WriteLine();
        }
    }
}