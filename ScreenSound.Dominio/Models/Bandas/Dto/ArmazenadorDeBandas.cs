using OpenAI_API;
using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Services.Armazenadores;
using ScreenSound.Dominio.Services.Repositorios;

namespace ScreenSound.Dominio.Models.Bandas.Dto;

public class ArmazenadorDeBandas : IArmazenadorBanda
{
    private readonly IBandaRepositorio _bandaRepositorio;
    private static readonly OpenAIAPI Client = new("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");
    private static readonly OpenAI_API.Chat.Conversation? Chat = Client.Chat.CreateConversation();

    public ArmazenadorDeBandas(IBandaRepositorio bandaRepositorio)
    {
        _bandaRepositorio = bandaRepositorio;
    }

    private static string GetRespostaBanda(string nome)
    {
        Chat!.AppendSystemMessage($"Resuma a banda {nome} em 1 parágrafo. Adote uma linguagem informal.");
        return Chat.GetResponseFromChatbotAsync().GetAwaiter().GetResult();
    }

    private static string GetDataBanda(string nome)
    {
        Chat!.AppendSystemMessage($"Responda apenas com o número do ano: Em qual ano a banda {nome} foi criada?");
        return Chat.GetResponseFromChatbotAsync().GetAwaiter().GetResult();
    }

    public async Task<string> Armazenar(Banda bandaDto)
    {
        Banda? bandaSalva = await _bandaRepositorio.ObterPorNome(bandaDto.Nome);


        if (bandaSalva != null && bandaSalva.Nome == bandaDto.Nome)
        {
            throw new ArgumentException(Resource.ArtistaExistente);
        }

        if (bandaDto.Nome != null && bandaDto.Nome.Length > 255)
        {
            throw new ArgumentException(Resource.NomeInvalido);
        }

        if (bandaDto != null && bandaDto.Descricao.Length > 5000)
        {
            throw new ArgumentException(Resource.DescricaoBandaInvalida);
        }

        Banda newBanda = new(bandaDto.Nome, GetRespostaBanda(bandaDto.Nome));
        await _bandaRepositorio.Adicionar(newBanda);
        return "Banda registrada!";
    }

    public async Task<string> Editar(int id, string? nome = null, string? descricao = null)
    {
        var banda = await _bandaRepositorio.ObterPorId(id);

        if (banda != null)
        {
            if (!string.IsNullOrEmpty(nome))
            {
                banda.AlterarNome(nome);
                await Console.Out.WriteLineAsync("Nome da banda foi alterado!");
            }

            if (!string.IsNullOrEmpty(descricao))
            {
                banda.AlterarDescricao(descricao);
                await Console.Out.WriteLineAsync("Descrição da banda foi alterada!");
            }

            return $"A banda {nome} foi editada!";
        }

        return "Banda não encontrada!";
    }
}
