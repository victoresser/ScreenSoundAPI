using System.Text.RegularExpressions;
using OpenAI_API;
using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Interfaces;
using ScreenSound.Dominio.Interfaces.Armazenadores;
using ScreenSound.Dominio.Interfaces.Repositorios;
using ScreenSound.Dominio.Models.Bandas.Dto;

namespace ScreenSound.Dominio.Models.Bandas.Services;

public class ArmazenadorDeBandas : IArmazenadorBanda
{
    private readonly IBandaRepositorio _bandaRepositorio;
    private readonly IBase64Cleaner _base64Cleaner;
    private static readonly OpenAIAPI Client = new("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");
    private static readonly OpenAI_API.Chat.Conversation? Chat = Client.Chat.CreateConversation();

    public ArmazenadorDeBandas(IBandaRepositorio bandaRepositorio, IBase64Cleaner base64Cleaner)
    {
        _bandaRepositorio = bandaRepositorio;
        _base64Cleaner = base64Cleaner;
    }

    private static string GetRespostaBanda(string nome)
    {
        Chat?.AppendSystemMessage($"Resuma a banda {nome} em 1 parágrafo. Adote uma linguagem informal.");
        return Chat?.GetResponseFromChatbotAsync().GetAwaiter().GetResult() ?? string.Empty;
    }

    private static string GetDataBanda(string nome)
    {
        Chat?.AppendSystemMessage($"Responda apenas com o número do ano: Em qual ano a banda {nome} foi criada?");
        return Chat?.GetResponseFromChatbotAsync().GetAwaiter().GetResult() ?? string.Empty;
    }

    public async Task<string> Armazenar(CreateBandaDto dto)
    {
        var imagemBase64 = Array.Empty<byte>();

        if (dto == null)
            throw new ArgumentNullException(nameof(dto), Resource.BandaInvalida);

        if (string.IsNullOrWhiteSpace(dto.Nome))
            throw new ArgumentException(Resource.NomeBandaInvalido);

        if (dto.Nome.Length > 255)
            throw new ArgumentException(Resource.NomeBandaInvalido);

        if (!string.IsNullOrEmpty(dto.Descricao) && dto.Descricao.Length > 5000)
            throw new ArgumentException(Resource.DescricaoBandaInvalida);

        var bandaSalva = await _bandaRepositorio.ObterPorNome(dto.Nome);

        if (bandaSalva != null)
            throw new ArgumentException(Resource.BandaJaExiste);

        if (!string.IsNullOrEmpty(dto.Imagem))
        {
            // var base64Formatado = Regex.Replace(dto.Imagem, "(data|image|/png|/webpg|/jpg|/jpeg|base64|,|:|;)", string.Empty);
            // imagemBase64 = Convert.FromBase64String(base64Formatado);

            imagemBase64 = _base64Cleaner.ConverterStringBase64ParaBytes(dto.Imagem);
        }

        Banda newBanda = new(dto.Nome, dto.Descricao, imagemBase64);
        await _bandaRepositorio.Adicionar(newBanda);

        return Resource.BandaCriada;
    }

    public async Task<string> Editar(int id, string? nome = null, string? descricao = null)
    {
        var banda = await _bandaRepositorio.ObterPorIdAsync(id);

        if (banda == null) return "Banda não encontrada!";
        if (!string.IsNullOrEmpty(nome))
        {
            banda.AlterarNome(nome);
            await Console.Out.WriteLineAsync("Nome da banda foi alterado!");
        }

        if (string.IsNullOrEmpty(descricao)) return $"O nome da banda {nome} foi editada!";
        banda.AlterarDescricao(descricao);
        await Console.Out.WriteLineAsync("Descrição da banda foi alterada!");

        return $"A banda {nome} foi editada!";
    }
}