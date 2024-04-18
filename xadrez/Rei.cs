using tabuleiro;

namespace xadrez;

public class Rei : Peca
{
    private PartidaDeXadrez Partida;
    public Rei(Tabuleiro tab, Cor Cor, PartidaDeXadrez partida) : base(Cor, tab)
    {
        Partida = partida;
    }

    public override string ToString()
    {
        return "R";
    }

    private bool podeMover(Posicao pos)
    {
        Peca p = Tab.peca(pos);
        return p == null || p.Cor != Cor;
    }
    
    private bool testeTorreParaRoque(Posicao pos) {
        Peca p = Tab.peca(pos);
        return p != null && p is Torre && p.Cor == Cor && p.QteMovimentos == 0;
    }

    public override bool[,] movimentosPossiveis()
    {
        bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];

        Posicao pos = new Posicao(0, 0);

        // acima 
        pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
        if (Tab.PosicaoValida(pos) && podeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
        }

        // ne 
        pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
        if (Tab.PosicaoValida(pos) && podeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
        }

        // direita 
        pos.DefinirValores(Posicao.Linha, Posicao.Coluna + 1);
        if (Tab.PosicaoValida(pos) && podeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
        }

        // se 
        pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
        if (Tab.PosicaoValida(pos) && podeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
        }

        // abaixo 
        pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
        if (Tab.PosicaoValida(pos) && podeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
        }

        // so 
        pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
        if (Tab.PosicaoValida(pos) && podeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
        }

        // esquerda 
        pos.DefinirValores(Posicao.Linha, Posicao.Coluna - 1);
        if (Tab.PosicaoValida(pos) && podeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
        }

        // no 
        pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
        if (Tab.PosicaoValida(pos) && podeMover(pos))
        {
            mat[pos.Linha, pos.Coluna] = true;
        }
        
        // #jogadaespecial roque
        if (QteMovimentos==0 && !Partida.Xeque) {
            // #jogadaespecial roque pequeno
            Posicao posT1 = new Posicao(Posicao.Linha, Posicao.Coluna + 3);
            if (testeTorreParaRoque(posT1)) {
                Posicao p1 = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                Posicao p2 = new Posicao(Posicao.Linha, Posicao.Coluna + 2);
                if (Tab.peca(p1)==null && Tab.peca(p2)==null) {
                    mat[Posicao.Linha, Posicao.Coluna + 2] = true;
                }
            }
            // #jogadaespecial roque grande
            Posicao posT2 = new Posicao(Posicao.Linha, Posicao.Coluna - 4);
            if (testeTorreParaRoque(posT2)) {
                Posicao p1 = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                Posicao p2 = new Posicao(Posicao.Linha, Posicao.Coluna - 2);
                Posicao p3 = new Posicao(Posicao.Linha, Posicao.Coluna - 3);
                if (Tab.peca(p1) == null && Tab.peca(p2) == null && Tab.peca(p3) == null) {
                    mat[Posicao.Linha, Posicao.Coluna - 2] = true;
                }
            }
        }

        return mat;
    }
}