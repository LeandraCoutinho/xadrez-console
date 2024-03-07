namespace tabuleiro;

public class Peca
{
    public Posicao Posicao { get; set; }
    public Cor Cor { get; protected set; }
    public int QteMovimentos { get; protected set; }
    public Tabuleiro Tab { get; set; }

    public Peca(Cor cor, Tabuleiro tab)
    {
        Posicao = null;
        Cor = cor;
        Tab = tab;
        QteMovimentos = 0;
    }
}