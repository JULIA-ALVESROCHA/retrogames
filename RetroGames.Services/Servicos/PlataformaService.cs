using RetroGames.Model.Model;
using RetroGames.Repository.Base;
using RetroGames.Repository.Interfaces;
using RetroGames.Services.Erros;
using RetroGames.Services.Interfaces;

namespace RetroGames.Services.Servicos;

/// <summary>
/// Regras de negócio das plataformas. Recebe só o IRepositorySet:
/// não conhece SQL nem HTTP. Toda regra violada lança um erro do BusinessErrors.
/// </summary>
public class PlataformaService : IPlataformaService
{
    private readonly IRepositorySet repositorio;

    public PlataformaService(IRepositorySet repositorio)
    {
        this.repositorio = repositorio;
    }

    public async Task<List<Plataforma>> ListarAsync()
    {
        return await repositorio.Plataforma.ListarAsync();
    }

    public async Task<Plataforma> BuscarPorIdAsync(int id)
    {
        var plataforma = await repositorio.Plataforma.BuscarPorIdAsync(id);
        if (plataforma is null)
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA);
        }

        return plataforma;
    }

    public async Task<Plataforma> InserirAsync(Plataforma plataforma)
    {
        plataforma.Nome = plataforma.Nome.Trim();

        // RN01: não pode haver duas plataformas com o mesmo nome.
        if (await repositorio.Plataforma.ExisteComNomeAsync(plataforma.Nome))
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NOME_DUPLICADO);
        }

        try
        {
            plataforma.Id = await repositorio.Plataforma.InserirAsync(plataforma);
        }
        catch (RegistroDuplicadoException)
        {
            // Duas gravações ao mesmo tempo: a consulta acima passou, mas o UNIQUE do banco recusou.
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NOME_DUPLICADO);
        }

        return plataforma;
    }

    public async Task AlterarAsync(Plataforma plataforma)
    {
        // Confere se existe (senão, PLATAFORMA_NAO_ENCONTRADA).
        await BuscarPorIdAsync(plataforma.Id);

        plataforma.Nome = plataforma.Nome.Trim();

        // RN01, ignorando a própria plataforma: manter o próprio nome não é duplicidade.
        if (await repositorio.Plataforma.ExisteComNomeAsync(plataforma.Nome, plataforma.Id))
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NOME_DUPLICADO);
        }

        try
        {
            await repositorio.Plataforma.AlterarAsync(plataforma);
        }
        catch (RegistroDuplicadoException)
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NOME_DUPLICADO);
        }
    }

    public async Task ExcluirAsync(int id)
    {
        await BuscarPorIdAsync(id);

        // RN02: plataforma com jogos não pode ser excluída.
        if (await repositorio.Plataforma.PossuiJogosAsync(id))
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_POSSUI_JOGOS);
        }

        await repositorio.Plataforma.ExcluirAsync(id);
    }
}
