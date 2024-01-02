using Microsoft.AspNetCore.Mvc;
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

        public MusicaController(IMusicaRepositorio musicaRepositorio, IBandaRepositorio bandaRepositorio, IAlbumRepositorio albumRepositorio)
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
        public async Task<IEnumerable<ListagemDeMusicas>> Get(string? nomeMusica = null, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            IEnumerable<Musica> consulta = await _musicaRepositorio.Consultar();

            if (consulta != null)
            {
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

                    return dto.Any() ? dto : new List<ListagemDeMusicas> { };
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

                return dtos.Any() ? dtos : new List<ListagemDeMusicas> { };
            }

            return new List<ListagemDeMusicas> { };
        }

        /// <summary>
        /// GET: api/<MusicaController>/listarTopFive
        /// </summary>
        /// <returns>Listagem de músicas existentes</returns>
        [HttpGet("listarTopFive")]
        public async Task<IEnumerable<ListagemDeMusicas>> GetTopFive(string? nomeMusica = null, [FromQuery] int skip = 0, [FromQuery] int take = 5)
        {
            IEnumerable<Musica> consulta = await _musicaRepositorio.Consultar();

            if (consulta != null)
            {
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

                    return dto.Any() ? dto : new List<ListagemDeMusicas> { };
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

                return dtos.Any() ? dtos : new List<ListagemDeMusicas> { };
            }

            return new List<ListagemDeMusicas> { };
        }

        /// <summary>
        /// GET api/<MusicaController>/5
        /// </summary>
        /// <param name="id">Id da música que deseja pesquisar</param>
        /// <returns></returns>
        [HttpGet("listar/{id}")]
        public async Task<IActionResult> GetForId(int id)
        {
            var consulta = await _musicaRepositorio.ObterPorId(id);

            if (consulta == null)
            {
                return Ok(new ListagemDeMusicas { });
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
        /// <param name="nomeMusica">Nome da música que deseja registrar</param>
        /// <param name="duracaoMusica">Duração da Música que deseja registrar</param>
        /// <param name="disponibilidadeMusica">Disponibilidade da música que deseja registrar</param>
        /// <param name="nomeBanda">Nome da banda à qual pertence esta música</param>
        /// <param name="nomeAlbum">Nome do álbum ao qual pertence esta música</param>
        /// <param name="_armazenadorMusica">Serviço que serve para Armazena/Editar uma música</param>
        /// <returns code = "200">Success</returns>
        [HttpPost("adicionarMusica")]

        public async Task<IActionResult> Post(string nomeMusica,
                                              short duracaoMusica,
                                              bool disponibilidadeMusica,
                                              string nomeBanda,
                                              string nomeAlbum,
                                              [FromServices] IArmazenadorMusica _armazenadorMusica)
        {
            var banda = await _bandaRepositorio.ObterPorNome(nomeBanda);
            var album = await _albumRepositorio.ObterPorNome(nomeAlbum);

            if (string.IsNullOrWhiteSpace(nomeMusica)) return BadRequest(Resource.NomeMusicaInvalido);
            if (banda == null || album == null) return BadRequest(Resource.ArtistaInvalido);
            await _armazenadorMusica.Armazenar(nomeMusica, duracaoMusica, banda, album, disponibilidadeMusica);
            return Ok(await Get(nomeMusica));

        }

        /// <summary>
        /// PUT api/<MusicaController>/5
        /// </summary>
        /// <param name="id">ID da música</param>
        /// <param name="nomeMusica">Nome da música que deseja editar</param>
        /// <param name="duracao">Duração da música que deseja editar</param>
        /// <param name="disponibilidade">Disponibilidade da música que deseja editar</param>
        /// <param name="nomeBanda">Nome da Banda da música que deseja editar</param>
        /// <param name="nomeAlbum">Nome do Álbum da música que deseja editar</param>
        /// <param name="_armazenadorMusica">Serviço que serve para Armazena/Editar uma música</param>
        /// <returns>Música editada!</returns>
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Put(int id,
                                             string? nomeMusica,
                                             short? duracao,
                                             bool disponibilidade,
                                             string? nomeBanda,
                                             string? nomeAlbum,
                                             string? imagem,
                                             [FromServices] IArmazenadorMusica _armazenadorMusica)
        {
            if (id <= 0) return NotFound();


            await _armazenadorMusica.Editar(id, nomeMusica, nomeBanda, nomeAlbum, duracao, disponibilidade, imagem);
            return Ok(await GetForId(id));
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

            if (musica != null)
            {
                await _musicaRepositorio.Deletar(musica.Id);
                return Ok("Música Excluída");
            }

            return BadRequest();
        }
    }
}
