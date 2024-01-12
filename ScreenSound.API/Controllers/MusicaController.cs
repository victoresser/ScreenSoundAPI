using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Infrastructure;
using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Models.Albuns;
using ScreenSound.Dominio.Models.Bandas;
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
        private readonly IBandaRepositorio _bandaRepositorio;
        private readonly IAlbumRepositorio _albumRepositorio;

        public MusicaController(IMusicaRepositorio musicaRepositorio, IBandaRepositorio bandaRepositorio,
            IAlbumRepositorio albumRepositorio)
        {
            _musicaRepositorio = musicaRepositorio;
            _bandaRepositorio = bandaRepositorio;
            _albumRepositorio = albumRepositorio;
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

            if (nomeMusica != null)
            {
                var dto = consulta.Select(x => new ListagemDeMusicas
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    Duracao = x.Duracao,
                    Disponivel = x.Disponivel,
                    Banda = x.Banda.Nome,
                    Album = x.Album.Nome,
                    Imagem = x.Imagem
                }).Where(x => x.Nome.Contains(nomeMusica)).Skip(skip).Take(take).ToList();

                return dto.Any() ? dto : new List<ListagemDeMusicas>();
            }

            var dtos = consulta.Select(x => new ListagemDeMusicas
            {
                Id = x.Id,
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
            var banda = await _bandaRepositorio.ObterPorNome(dto.NomeBanda);
            var album = await _albumRepositorio.ObterPorNome(dto.NomeAlbum);

            if (string.IsNullOrWhiteSpace(dto.NomeMusica)) return BadRequest(Resource.NomeMusicaInvalido);
            if (banda == null) return BadRequest(Resource.BandaInvalida);
            if (album == null) return BadRequest(Resource.AlbumInvalido);

            await armazenadorMusica.Armazenar(dto, album, banda);
            return Ok(await Get(dto.NomeMusica));
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
        public async Task<IActionResult> Put(EditMusicaDto dto, [FromServices] IArmazenadorMusica armazenadorMusica)
        {
            if (dto.Id <= 0) return NotFound(Resource.MusicaInexistente);

            await armazenadorMusica.Editar(dto);
            return Ok(await GetForId(dto.Id));
        }

        /// <summary>
        /// DELETE api/<MusicaController>/5
        /// </summary>
        /// <param name="id">ID da música que deseja deletar</param>
        /// <returns></returns>
        [HttpDelete("excluir/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var musica = await _musicaRepositorio.ObterPorId(id);

            if (musica == null) return BadRequest(Resource.MusicaInexistente);
            await _musicaRepositorio.Deletar(musica.Id);
            return Ok("Música Excluída");

        }
    }
}