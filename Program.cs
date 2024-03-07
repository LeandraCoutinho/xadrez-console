using tabuleiro;
using xadrez;

namespace xadrez_console;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            PartidaDeXadrez partida = new PartidaDeXadrez();

            while (!partida.Terminada)
            {
                Console.Clear();
                Tela.ImprimirTabuleiro(partida.tab);
                
                Console.WriteLine();
                Console.Write("Origem: ");
                Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();
                Console.Write("Destino: ");
                Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();
                
                partida.ExecutaMovimento(origem, destino);
            }
            
            Tela.ImprimirTabuleiro(partida.tab);
        }

        catch (TabuleiroException e)
        {
            Console.WriteLine(e.Message);
        }
        
    }
}
