using Microsoft.AspNetCore.Mvc;
using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Interfaces.Armazenadores;
using ScreenSound.Dominio.Interfaces.Consultas;
using ScreenSound.Dominio.Interfaces.Repositorios;
using ScreenSound.Dominio.Models.Musicas.Dto;

namespace ScreenSound.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicaController(IWebHostEnvironment hostEnvironment, IMusicaRepositorio musicaRepositorio,
        IConsultaListagemDeMusicas consultaListagem) : ControllerBase
    {
        /// <summary>
        /// GET: api/<MusicaController>/listar
        /// </summary>
        /// <returns>Listagem de músicas existentes</returns>
        [HttpGet("listar")]
        public async Task<IEnumerable<ListagemDeMusicas>> Get(string? nomeMusica = null, [FromQuery] int skip = 0,
            [FromQuery] int take = 10) => await consultaListagem.RetornaListagemDeMusicas(nomeMusica, skip, take);

        /// <summary>
        /// GET: api/<MusicaController>/listarTopFive
        /// </summary>
        /// <returns>Listagem de músicas existentes</returns>
        [HttpGet("listarTopFive")]
        public async Task<IEnumerable<ListagemDeMusicas>> GetTopFive(string? nomeMusica = null, [FromQuery] int skip = 0,
            [FromQuery] int take = 5) => await consultaListagem.RetornaListagemDeMusicas(nomeMusica, skip, take);

        /// <summary>
        /// GET api/<MusicaController>/5
        /// </summary>
        /// <param name="consultaListagemDeMusicas"></param>
        /// <param name="id">Id da música que deseja pesquisar</param>
        /// <returns></returns>
        [HttpGet("listar/{id:int}")]
        public async Task<IActionResult> GetForId(int id)
        {
            var dto = await consultaListagem.RetornaMusicaPorId(id);
            return Ok(await Get(nomeMusica: dto.Nome));
        }

        /// <summary>
        /// POST api/<MusicaController>
        /// </summary>
        /// <param name="dto">Dto com informações da banda criada</param>
        /// <param name="armazenadorMusica">Serviço que serve para Armazenar/Editar uma música</param>
        /// <param name="consultaListagemDeMusicas"></param>
        /// <returns code = "200">Success: Música cadastrada</returns>
        [HttpPost("adicionarMusica")]
        public async Task<IActionResult> Post(CreateMusicaDto dto, [FromServices] IArmazenadorMusica armazenadorMusica)
        {
            await armazenadorMusica.Armazenar(dto);
            return Ok(await Get(nomeMusica: dto.Nome));
        }

        [HttpPost("salvarImagem/{musicaId:int}")]
        public async Task<IActionResult> SalvarImagem(int musicaId, [FromForm] IFormFile imagem)
        {
            try
            {
                // Criar o caminho completo para salvar a imagem no servidor
                var caminho = Path.Combine(hostEnvironment.WebRootPath, "assets", "musicas", $"{imagem.FileName}");

                // Garantir que o diretório existe
                Directory.CreateDirectory(Path.GetDirectoryName(caminho));

                // Salvar a imagem
                using (var stream = new FileStream(caminho, FileMode.Create))
                {
                    imagem.CopyTo(stream);
                }

                return Ok(new { mensagem = "Imagem salva com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = "Erro ao salvar a imagem", detalhes = ex.Message });
            }
        }

        /// <summary>
        /// PUT api/Musica/5
        /// </summary>
        /// <param name="consultaListagemDeMusicas"></param>
        /// <param name="id">ID da música</param>
        /// <param name="nomeMusica">Nome da música que deseja editar</param>
        /// <param name="duracao">Duração da música que deseja editar</param>
        /// <param name="disponibilidade">Disponibilidade da música que deseja editar</param>
        /// <param name="nomeBanda">Nome da Banda da música que deseja editar</param>
        /// <param name="nomeAlbum">Nome do Álbum da música que deseja editar</param>
        /// <param name="dto"></param>
        /// <param name="armazenadorMusica">Serviço que serve para Armazena/Editar uma música</param>
        /// <returns>Música editada!</returns>
        [HttpPut("editar/{id:int}")]
        public async Task<IActionResult> Put(EditMusicaDto dto, [FromServices] IArmazenadorMusica armazenadorMusica,
            int id)
        {
            if (id <= 0) return NotFound(Resource.MusicaInexistente);

            await armazenadorMusica.Editar(dto);
            return Ok(await GetForId(dto.Id));
        }

        /// <summary>
        /// DELETE api/<MusicaController>/5
        /// </summary>
        /// <param name="id">ID da música que deseja deletar</param>
        /// <returns></returns>
        [HttpDelete("excluir/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var musica = await musicaRepositorio.ObterPorId(id);

            if (musica == null) return BadRequest(Resource.MusicaInexistente);
            await musicaRepositorio.Deletar(musica.Id);
            return Ok(Resource.MusicaExcluida);
        }
    }
}