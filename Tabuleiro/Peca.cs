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

    public void IncrementarQteMovimentos()
    {
        QteMovimentos++;
    }
    
    public void DecrementarQteMovimentos()
    {
        QteMovimentos--;
    }

    public bool ExisteMovimentosPossiveis()
    {
        bool[,] mat = movimentosPossiveis();
        for (int i = 0; i < Tab.Linhas; i++)
        {
            for (int j = 0; j < Tab.Colunas; j++)
            {
                if (mat[i, j])
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool PodeMoverPara(Posicao pos)
    {
        return movimentosPossiveis()[pos.Linha, pos.Coluna];
    }

    public virtual bool[,] movimentosPossiveis()
    {
        throw new NotImplementedException();
    }
}