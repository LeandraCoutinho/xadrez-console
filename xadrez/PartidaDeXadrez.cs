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
    public bool Xeque { get; private set; }


    public PartidaDeXadrez()
    {
        tab = new Tabuleiro(8, 8);
        turno = 1;
        JogadorAtual = Cor.Branca;
        Terminada = false;
        Xeque = false;
        pecas = new HashSet<Peca>();
        capturadas = new HashSet<Peca>();
        colocarPecas();
    }

    public Peca ExecutaMovimento(Posicao origem, Posicao destino)
    {
        Peca p = tab.RetirarPeca(origem);
        p.IncrementarQteMovimentos();
        Peca pecaCapturada = tab.RetirarPeca(destino);
        tab.ColocarPeca(p, destino);
        if (pecaCapturada != null)
        {
            capturadas.Add(pecaCapturada);
        }
        
        // #jogadaespecial roque pequeno
        if (p is Rei && destino.Coluna == origem.Coluna + 2) {
            Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
            Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
            Peca T = tab.RetirarPeca(origemT);
            T.IncrementarQteMovimentos();
            tab.ColocarPeca(T, destinoT);
        }

        // #jogadaespecial roque grande
        if (p is Rei && destino.Coluna == origem.Coluna - 2) {
            Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
            Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
            Peca T = tab.RetirarPeca(origemT);
            T.IncrementarQteMovimentos();
            tab.ColocarPeca(T, destinoT);
        }

        return pecaCapturada;
    }

    public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
    {
        Peca p = tab.RetirarPeca(destino);
        p.DecrementarQteMovimentos();
        if (pecaCapturada != null)
        {
            tab.ColocarPeca(pecaCapturada, destino);
            capturadas.Remove(pecaCapturada);
        }

        tab.ColocarPeca(p, origem);
        
        // #jogadaespecial roque pequeno
        if (p is Rei && destino.Coluna == origem.Coluna + 2) {
            Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
            Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
            Peca T = tab.RetirarPeca(destinoT);
            T.DecrementarQteMovimentos();
            tab.ColocarPeca(T, origemT);
        }

        // #jogadaespecial roque grande
        if (p is Rei && destino.Coluna == origem.Coluna - 2) {
            Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
            Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
            Peca T = tab.RetirarPeca(destinoT);
            T.DecrementarQteMovimentos();
            tab.ColocarPeca(T, origemT);
        }
    }

    public void RealizaJogada(Posicao origem, Posicao destino)
    {
        Peca pecaCacpurada = ExecutaMovimento(origem, destino);
        if (EstaEmXeque(JogadorAtual))
        {
            DesfazMovimento(origem, destino, pecaCacpurada);
            throw new TabuleiroException("Você não pode se colocar em xeque!");
        }

        if (EstaEmXeque(Adversaria(JogadorAtual)))
        {
            Xeque = true;
        }
        else
        {
            Xeque = false;
        }

        if (TesteXequeMate(Adversaria(JogadorAtual)))
        {
            Terminada = true;
        }
        else
        {
            turno++;
            MudaJogador();
        }
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
        if (!tab.peca(origem).MovimentoPossivel(destino))
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

    private Cor Adversaria(Cor cor)
    {
        if (cor == Cor.Branca)
        {
            return Cor.Preta;
        }
        else
        {
            return Cor.Branca;
        }
    }

    private Peca Rei(Cor cor)
    {
        foreach (Peca x in PecasEmJogo(cor))
        {
            if (x is Rei)
            {
                return x;
            }
        }

        return null;
    }

    public bool EstaEmXeque(Cor cor)
    {
        Peca R = Rei(cor);
        if (R == null)
        {
            throw new TabuleiroException("Não tem rei da cor" + cor + "no tabuleiro");
        }

        foreach (Peca x in PecasEmJogo(Adversaria(cor)))
        {
            bool[,] mat = x.movimentosPossiveis();
            if (mat[R.Posicao.Linha, R.Posicao.Coluna])
            {
                return true;
            }
        }

        return false;
    }

    public bool TesteXequeMate(Cor cor)
    {
        if (!EstaEmXeque(cor))
        {
            return false;
        }

        foreach (Peca x in PecasEmJogo(cor))
        {
            bool[,] mat = x.movimentosPossiveis();
            for (int i = 0; i < tab.Linhas; i++)
            {
                for (int j = 0; j < tab.Colunas; j++)
                {
                    if (mat[i, j])
                    {
                        Posicao origem = x.Posicao;
                        Posicao destino = new Posicao(i, j);
                        Peca pecaCapturada = ExecutaMovimento(origem, destino);
                        bool testeXeque = EstaEmXeque(cor);
                        DesfazMovimento(origem, destino, pecaCapturada);
                        if (!testeXeque)
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    public void ColocarNovaPeca(char Coluna, int Linha, Peca peca)
    {
        tab.ColocarPeca(peca, new PosicaoXadrez(Coluna, Linha).ToPosicao());
        pecas.Add(peca);
    }

    private void colocarPecas() {
            ColocarNovaPeca('a', 1, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('b', 1, new Cavalo(tab, Cor.Branca));
            ColocarNovaPeca('c', 1, new Bispo(tab, Cor.Branca));
            ColocarNovaPeca('d', 1, new Dama(tab, Cor.Branca));
            ColocarNovaPeca('e', 1, new Rei(tab, Cor.Branca, this));
            ColocarNovaPeca('f', 1, new Bispo(tab, Cor.Branca));
            ColocarNovaPeca('g', 1, new Cavalo(tab, Cor.Branca));
            ColocarNovaPeca('h', 1, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('a', 2, new Peao(tab, Cor.Branca));
            ColocarNovaPeca('b', 2, new Peao(tab, Cor.Branca));
            ColocarNovaPeca('c', 2, new Peao(tab, Cor.Branca));
            ColocarNovaPeca('d', 2, new Peao(tab, Cor.Branca));
            ColocarNovaPeca('e', 2, new Peao(tab, Cor.Branca));
            ColocarNovaPeca('f', 2, new Peao(tab, Cor.Branca));
            ColocarNovaPeca('g', 2, new Peao(tab, Cor.Branca));
            ColocarNovaPeca('h', 2, new Peao(tab, Cor.Branca));

            ColocarNovaPeca('a', 8, new Torre(tab, Cor.Preta));
            ColocarNovaPeca('b', 8, new Cavalo(tab, Cor.Preta));
            ColocarNovaPeca('c', 8, new Bispo(tab, Cor.Preta));
            ColocarNovaPeca('d', 8, new Dama(tab, Cor.Preta));
            ColocarNovaPeca('e', 8, new Rei(tab, Cor.Preta, this));
            ColocarNovaPeca('f', 8, new Bispo(tab, Cor.Preta));
            ColocarNovaPeca('g', 8, new Cavalo(tab, Cor.Preta));
            ColocarNovaPeca('h', 8, new Torre(tab, Cor.Preta));
            ColocarNovaPeca('a', 7, new Peao(tab, Cor.Preta));
            ColocarNovaPeca('b', 7, new Peao(tab, Cor.Preta));
            ColocarNovaPeca('c', 7, new Peao(tab, Cor.Preta));
            ColocarNovaPeca('d', 7, new Peao(tab, Cor.Preta));
            ColocarNovaPeca('e', 7, new Peao(tab, Cor.Preta));
            ColocarNovaPeca('f', 7, new Peao(tab, Cor.Preta));
            ColocarNovaPeca('g', 7, new Peao(tab, Cor.Preta));
            ColocarNovaPeca('h', 7, new Peao(tab, Cor.Preta));
        }
}