using Microsoft.AspNetCore.Mvc;
using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Models.Musicas;
using ScreenSound.Dominio.Models.Musicas.Dto;
using ScreenSound.Dominio.Services.Armazenadores;
using ScreenSound.Dominio.Services.Repositorios;

namespace ScreenSound.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicaController : ControllerBase
    {
        private readonly IMusicaRepositorio _musicaRepositorio;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MusicaController(IMusicaRepositorio musicaRepositorio, IWebHostEnvironment hostEnvironment)
        {
            _musicaRepositorio = musicaRepositorio;
            _hostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// GET: api/<MusicaController>/listar
        /// </summary>
        /// <returns>Listagem de músicas existentes</returns>
        [HttpGet("listar")]
        public async Task<IEnumerable<ListagemDeMusicas>> Get(string? nomeMusica = null, [FromQuery] int skip = 0,
            [FromQuery] int take = 10)
        {
            IEnumerable<Musica> consulta = await _musicaRepositorio.ConsultarAsync();

            if (!string.IsNullOrWhiteSpace(nomeMusica))
            {
                var dto = consulta.Select(x => new ListagemDeMusicas
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    Imagem = x.Imagem,
                    Duracao = x.Duracao,
                    Banda = x.Banda.Nome,
                    Album = x.Album.Nome,
                    Disponivel = x.Disponivel
                }).Where(x => x.Nome.Contains(nomeMusica)).Skip(skip).Take(take).ToList().OrderBy(x => x.Nome);

                return dto.Any() ? dto : new List<ListagemDeMusicas>();
            }

            var dtos = consulta.Select(x => new ListagemDeMusicas
            {
                Id = x.Id,
                Nome = x.Nome,
                Imagem = x.Imagem,
                Duracao = x.Duracao,
                Banda = x.Banda.Nome,
                Album = x.Album.Nome,
                Disponivel = x.Disponivel
            }).Skip(skip).Take(take).ToList().OrderBy(x => x.Nome);

            return dtos.Any() ? dtos : new List<ListagemDeMusicas>();
        }

        /// <summary>
        /// GET: api/<MusicaController>/listarTopFive
        /// </summary>
        /// <returns>Listagem de músicas existentes</returns>
        [HttpGet("listarTopFive")]
        public async Task<IEnumerable<ListagemDeMusicas>> GetTopFive(string? nomeMusica = null,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 5)
        {
            IEnumerable<Musica> consulta = await _musicaRepositorio.ConsultarAsync();

            if (consulta == null) return new List<ListagemDeMusicas>();
            if (nomeMusica != null)
            {
                var dto = consulta.Select(x => new ListagemDeMusicas
                {
                    Nome = x.Nome,
                    Duracao = x.Duracao,
                    Disponivel = x.Disponivel,
                    Banda = x.Banda.Nome,
                    Album = x.Album.Nome,
                    Imagem = x.Imagem
                }).Where(x => x.Nome.Equals(nomeMusica)).Skip(skip).Take(take).ToList();

                return dto.Any() ? dto : new List<ListagemDeMusicas>();
            }

            var dtos = consulta.Select(x => new ListagemDeMusicas
            {
                Nome = x.Nome,
                Duracao = x.Duracao,
                Disponivel = x.Disponivel,
                Banda = x.Banda.Nome,
                Album = x.Album.Nome,
                Imagem = x.Imagem
            }).Skip(skip).Take(take).ToList();

            return dtos.Any() ? dtos : new List<ListagemDeMusicas>();
        }

        /// <summary>
        /// GET api/<MusicaController>/5
        /// </summary>
        /// <param name="id">Id da música que deseja pesquisar</param>
        /// <returns></returns>
        [HttpGet("listar/{id:int}")]
        public async Task<IActionResult> GetForId(int id)
        {
            var consulta = await _musicaRepositorio.ObterPorId(id);

            if (consulta == null)
            {
                return Ok(new ListagemDeMusicas());
            }

            var dto = new ListagemDeMusicas
            {
                Nome = consulta.Nome,
                Duracao = consulta.Duracao,
                Disponivel = consulta.Disponivel,
                Banda = consulta.Banda.Nome,
                Album = consulta.Album.Nome,
                Imagem = consulta.Imagem
            };

            return Ok(await Get(dto.Nome));
        }

        /// <summary>
        /// POST api/<MusicaController>
        /// </summary>
        /// <param name="dto">Dto com informações da banda criada</param>
        /// <param name="armazenadorMusica">Serviço que serve para Armazenar/Editar uma música</param>
        /// <returns code = "200">Success: Música cadastrada</returns>
        [HttpPost("adicionarMusica")]
        public async Task<IActionResult> Post(CreateMusicaDto dto, [FromServices] IArmazenadorMusica armazenadorMusica)
        {
            await armazenadorMusica.Armazenar(dto);
            return Ok(await Get(dto.Nome));
        }

        [HttpPost("salvarImagem/{musicaId}")]
        public async Task<IActionResult> SalvarImagem(int musicaId, [FromForm] IFormFile imagem)
        {
            try
            {
                // Criar o caminho completo para salvar a imagem no servidor
                var caminho = Path.Combine(_hostEnvironment.WebRootPath, "assets", "musicas", $"{imagem.FileName}");

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
        public async Task<IActionResult> Put(EditMusicaDto dto, [FromServices] IArmazenadorMusica armazenadorMusica, int id)
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
            var musica = await _musicaRepositorio.ObterPorId(id);

            if (musica == null) return BadRequest(Resource.MusicaInexistente);
            await _musicaRepositorio.Deletar(musica.Id);
            return Ok("Música Excluída");

        }
    }
}