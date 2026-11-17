using System.Transactions;
using RetroGames.Model.Comum;
using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;
using RetroGames.Services.Erros;
using RetroGames.Services.Interfaces;
using RetroGames.Services.ServiceModels;

namespace RetroGames.Services.Servicos;

/// <summary>
/// Regras de negócio dos jogos. O Repository devolve jogos só com os ids de plataforma
/// e condição; este Service busca as outras tabelas e combina tudo com LINQ.
/// Recebe só o IRepositorySet: não conhece SQL nem HTTP.
/// </summary>
public class JogoService : IJogoService
{
    private readonly IRepositorySet repositorio;

    public JogoService(IRepositorySet repositorio)
    {
        this.repositorio = repositorio;
    }

    public async Task<ResultadoPaginado<JogoDetalheServiceModel>> ListarAsync(FiltroJogo filtro)
    {
        if (filtro.IdGenero is not null)
        {
            // Filtro por gênero: seleciona com LINQ os ids dos jogos do gênero.
            // O Repository recebe a lista e continua filtrando e paginando no SQL.
            var vinculos = await repositorio.JogoGenero.ListarTodosAsync();
            filtro.IdsJogos = vinculos
                .Where(v => v.IdGenero == filtro.IdGenero)
                .Select(v => v.IdJogo)
                .Distinct()
                .ToList();
        }

        var pagina = await repositorio.Jogo.ListarAsync(filtro);

        return new ResultadoPaginado<JogoDetalheServiceModel>
        {
            Itens = await MontarDetalhesAsync(pagina.Itens),
            Total = pagina.Total,
        };
    }

    public async Task<JogoDetalheServiceModel> BuscarPorIdAsync(int id)
    {
        var jogo = await BuscarJogoAsync(id);
        var detalhes = await MontarDetalhesAsync([jogo]);
        return detalhes[0];
    }

    public async Task<JogoDetalheServiceModel> InserirAsync(Jogo jogo, List<int> idsGeneros)
    {
        var generos = idsGeneros.Distinct().ToList();
        await ConferirReferenciasAsync(jogo, generos);

        jogo.Nome = jogo.Nome.Trim();
        jogo.Ativo = true;
        jogo.DataCadastro = DateTime.UtcNow;

        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout,
        };
        using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                   transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
        {
            jogo.Id = await repositorio.Jogo.InserirAsync(jogo);
            await repositorio.JogoGenero.InserirAsync(jogo.Id, generos);

            // Só depois da última gravação: ou grava tudo, ou nada.
            transactionScope.Complete();
        }

        return await BuscarPorIdAsync(jogo.Id);
    }

    public async Task AlterarAsync(Jogo jogo, List<int> idsGeneros)
    {
        await BuscarJogoAsync(jogo.Id);

        var generos = idsGeneros.Distinct().ToList();
        await ConferirReferenciasAsync(jogo, generos);

        jogo.Nome = jogo.Nome.Trim();

        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout,
        };
        using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                   transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
        {
            await repositorio.Jogo.AlterarAsync(jogo);
            await repositorio.JogoGenero.RemoverPorJogoAsync(jogo.Id);
            await repositorio.JogoGenero.InserirAsync(jogo.Id, generos);

            transactionScope.Complete();
        }
    }

    public async Task DesativarAsync(int id)
    {
        await BuscarJogoAsync(id);

        // RN03: o jogo nunca é apagado, só marcado como inativo.
        await repositorio.Jogo.DesativarAsync(id);
    }

    /// <summary>O jogo do id. Se não existir, lança JOGO_NAO_ENCONTRADO.</summary>
    private async Task<Jogo> BuscarJogoAsync(int id)
    {
        var jogo = await repositorio.Jogo.BuscarPorIdAsync(id);
        if (jogo is null)
        {
            throw new BusinessRuleException(BusinessErrors.JOGO_NAO_ENCONTRADO);
        }

        return jogo;
    }

    /// <summary>RN05: plataforma, condição e gêneros do jogo precisam existir.</summary>
    private async Task ConferirReferenciasAsync(Jogo jogo, List<int> idsGeneros)
    {
        if (await repositorio.Plataforma.BuscarPorIdAsync(jogo.IdPlataforma) is null)
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA);
        }

        var condicoes = await repositorio.CondicaoJogo.ListarAsync();
        if (!condicoes.Any(c => c.Id == jogo.IdCondicaoJogo))
        {
            throw new BusinessRuleException(BusinessErrors.CONDICAO_NAO_ENCONTRADA);
        }

        var idsExistentes = (await repositorio.Genero.ListarAsync()).Select(g => g.Id).ToHashSet();
        if (idsGeneros.Any(id => !idsExistentes.Contains(id)))
        {
            throw new BusinessRuleException(BusinessErrors.GENERO_NAO_ENCONTRADO);
        }
    }

    /// <summary>
    /// Monta o detalhe de uma lista de jogos com LINQ. Faz sempre as mesmas 4 consultas
    /// (plataformas, condições, gêneros e vínculos dos jogos da lista), qualquer que seja
    /// o tamanho da página: nunca uma consulta por jogo.
    /// </summary>
    private async Task<List<JogoDetalheServiceModel>> MontarDetalhesAsync(List<Jogo> jogos)
    {
        if (jogos.Count == 0)
        {
            return [];
        }

        var plataformas = (await repositorio.Plataforma.ListarAsync()).ToDictionary(p => p.Id);
        var condicoes = (await repositorio.CondicaoJogo.ListarAsync()).ToDictionary(c => c.Id);
        var generos = (await repositorio.Genero.ListarAsync()).ToDictionary(g => g.Id);
        var vinculosPorJogo = (await repositorio.JogoGenero.ListarPorJogosAsync(jogos.Select(j => j.Id)))
            .ToLookup(v => v.IdJogo, v => v.IdGenero);

        return jogos
            .Select(jogo => new JogoDetalheServiceModel
            {
                Jogo = jogo,
                NomePlataforma = plataformas.TryGetValue(jogo.IdPlataforma, out var plataforma)
                    ? plataforma.Nome
                    : string.Empty,
                DescricaoCondicaoJogo = condicoes.TryGetValue(jogo.IdCondicaoJogo, out var condicao)
                    ? condicao.Descricao
                    : string.Empty,
                Generos = vinculosPorJogo[jogo.Id]
                    .Where(generos.ContainsKey)
                    .Select(idGenero => generos[idGenero])
                    .OrderBy(g => g.Nome)
                    .ToList(),
            })
            .ToList();
    }
}
