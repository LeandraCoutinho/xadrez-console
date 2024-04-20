using tabuleiro;

namespace xadrez;

class Bispo : Peca
{
    public Bispo(Tabuleiro tab, Cor cor) : base(cor, tab)
    {
    }

    public override string ToString()
    {
        return "B";
    }

    private bool PodeMover(Posicao pos)
    {
        var p = Tab.Peca(pos);
        return p == null || p.Cor != Cor;
    }

    public override bool[,] MovimentosPossiveis()
    {
        var mat = new bool[Tab.Linhas, Tab.Colunas];

        var pos = new Posicao(0, 0);

        // NO
        pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
        while (Tab.PosicaoValida(pos) && PodeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
            if (Tab.Peca(pos) != null && Tab.Peca(pos).Cor != Cor) break;
            pos.DefinirValores(pos.Linha - 1, pos.Coluna - 1);
        }

        // NE
        pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
        while (Tab.PosicaoValida(pos) && PodeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
            if (Tab.Peca(pos) != null && Tab.Peca(pos).Cor != Cor) break;
            pos.DefinirValores(pos.Linha - 1, pos.Coluna + 1);
        }

        // SE
        pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
        while (Tab.PosicaoValida(pos) && PodeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
            if (Tab.Peca(pos) != null && Tab.Peca(pos).Cor != Cor) break;
            pos.DefinirValores(pos.Linha + 1, pos.Coluna + 1);
        }

        // SO
        pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
        while (Tab.PosicaoValida(pos) && PodeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
            if (Tab.Peca(pos) != null && Tab.Peca(pos).Cor != Cor) break;
            pos.DefinirValores(pos.Linha + 1, pos.Coluna - 1);
        }

        return mat;
    }
}