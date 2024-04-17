using tabuleiro;

namespace xadrez;

public class Torre : Peca
{
    public Torre(Tabuleiro tab, Cor cor) : base(cor, tab)
    {
    }

    public override string ToString()
    {
        return "T";
    }
    
    private bool podeMover(Posicao pos)
    {
        Peca p = Tab.peca(pos);
        return p == null || p.Cor != Cor;
    }
    
    public override bool[,] movimentosPossiveis()
    {
        bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];

        Posicao pos = new Posicao(0, 0);

        // acima 
        pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
        while (Tab.PosicaoValida(pos) && podeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
            if (Tab.peca(pos) != null && Tab.peca(pos).Cor != Cor)
            {
                break;
            }

            pos.Linha = pos.Linha - 1;
        }
        
        // abaixo 
        pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);
        while (Tab.PosicaoValida(pos) && podeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
            if (Tab.peca(pos) != null && Tab.peca(pos).Cor != Cor)
            {
                break;
            }

            pos.Linha = pos.Linha + 1;
        }
        
        // direita
        pos.DefinirValores(Posicao.Linha, Posicao.Coluna + 1);
        while (Tab.PosicaoValida(pos) && podeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
            if (Tab.peca(pos) != null && Tab.peca(pos).Cor != Cor)
            {
                break;
            }

            pos.Coluna = pos.Coluna + 1;
        }
        
        // esquerda 
        pos.DefinirValores(Posicao.Linha, Posicao.Coluna - 1);
        while (Tab.PosicaoValida(pos) && podeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
            if (Tab.peca(pos) != null && Tab.peca(pos).Cor != Cor)
            {
                break;
            }

            pos.Coluna = pos.Coluna - 1;
        }

        return mat;
    }
}