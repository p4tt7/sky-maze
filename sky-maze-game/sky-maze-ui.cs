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
                
                if (i == 9 && j == 9 && Board.board[i,j]=="c"){ //posicion ganadora y es camino
                    Console.Write("🟨"); 
                }

                else if(Board.board[i, j]=="c") //camino
                {
                    Console.Write("⬛");
                }

                else if(Board.board[i,j]=="w") //pared
                {
                    Console.Write("⬜");
                }
    

                else{
                    Console.Write(Board.board[i,j]); //ficha, trampa y obstaculo
                }
            }
            Console.WriteLine();
        }
    }
}