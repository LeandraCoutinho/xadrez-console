using tabuleiro;

namespace xadrez;

public class PartidaDeXadrez
{
    public Tabuleiro tab { get; private set; }
    public int turno { get; private set; }
    public Cor JogadorAtual { get; private set; }
    public bool Terminada { get; private set; }
    private HashSet<Peca> pecas;
    private HashSet<Peca> capturadas;
    

    public PartidaDeXadrez()
    {
        tab = new Tabuleiro(8, 8);
        turno = 1;
        JogadorAtual = Cor.Branca;
        Terminada = false;
        pecas = new HashSet<Peca>();
        capturadas = new HashSet<Peca>();
        ColocarPecas();
    }

    public void ExecutaMovimento(Posicao origem, Posicao destino)
    {
        Peca p = tab.RetirarPeca(origem);
        p.IncrementarQteMovimentos();
        Peca pecaCapturada = tab.RetirarPeca(destino);
        tab.ColocarPeca(p, destino);
        if (pecaCapturada != null)
        {
            capturadas.Add(pecaCapturada);
        }
    }

    public void RealizaJogada(Posicao origem, Posicao destino)
    {
        ExecutaMovimento(origem, destino);
        turno++;
        MudaJogador();
    }

    public void ValidarPosicaoDeOrigem(Posicao pos)
    {
        if (tab.peca(pos) == null)
        {
            throw new TabuleiroException("Não existe peça na posição escolhida!");
        }

        if (JogadorAtual != tab.peca(pos).Cor)
        {
            throw new TabuleiroException("A peça de origem escolhida não é sua!");
        }

        if (!tab.peca(pos).ExisteMovimentosPossiveis())
        {
            throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
        }
    }

    public void ValidarPosicaoDeDestino(Posicao origem, Posicao destino)
    {
        if (!tab.peca(origem).PodeMoverPara(destino))
        {
            throw new TabuleiroException("Posição de destino inválida!");
        }
    }

    private void MudaJogador()
    {
        if (JogadorAtual == Cor.Branca)
        {
            JogadorAtual = Cor.Preta;
        }
        else
        {
            JogadorAtual = Cor.Branca;
        }
    }
    
    // método pra separar um conjunto das peças capturadas por uma cor específica
    public HashSet<Peca> PecasCapturadas(Cor cor)
    {
        HashSet<Peca> aux = new HashSet<Peca>();
        foreach (Peca x in capturadas)
        {
            if (x.Cor == cor)
            {
                aux.Add(x);
            }
        }
        return aux;
    }

    public HashSet<Peca> PecasEmJogo(Cor cor)
    {
        HashSet<Peca> aux = new HashSet<Peca>();
        foreach (Peca x in pecas)
        {
            if (x.Cor == cor)
            {
                aux.Add(x);
            }
        }
        aux.ExceptWith(PecasCapturadas(cor));
        return aux;
    }

    public void ColocarNovaPeca(char coluna, int linha, Peca peca)
    {
        tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
        pecas.Add(peca);
    }
    
    private void ColocarPecas()
    {
        ColocarNovaPeca('c', 1, new Torre(tab, Cor.Branca));
        ColocarNovaPeca('c', 2, new Torre(tab, Cor.Branca));
        ColocarNovaPeca('d', 2, new Torre(tab, Cor.Branca));
        ColocarNovaPeca('e', 2, new Torre(tab, Cor.Branca));
        ColocarNovaPeca('e', 1, new Torre(tab, Cor.Branca));
        ColocarNovaPeca('d', 1, new Rei(tab, Cor.Branca));

        ColocarNovaPeca('c', 7, new Torre(tab, Cor.Preta));
        ColocarNovaPeca('c', 8, new Torre(tab, Cor.Preta));
        ColocarNovaPeca('d', 7, new Torre(tab, Cor.Preta));
        ColocarNovaPeca('e', 7, new Torre(tab, Cor.Preta));
        ColocarNovaPeca('e', 8, new Torre(tab, Cor.Preta));
        ColocarNovaPeca('d', 8, new Rei(tab, Cor.Preta));
    }
}