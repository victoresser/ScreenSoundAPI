using Microsoft.AspNetCore.Mvc;
using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Models.Albuns.Dto;
using ScreenSound.Dominio.Models.Bandas;
using ScreenSound.Dominio.Services.Armazenadores;
using ScreenSound.Dominio.Services.Conversores;
using ScreenSound.Dominio.Services.Repositorios;

namespace ScreenSound.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumRepositorio _albumRepositorio;
        private readonly IBandaRepositorio _bandaRepositorio;
        private readonly IConversorMusicasDoAlbum _conversor;

        public AlbumController(IAlbumRepositorio albumRepositorio, IBandaRepositorio bandaRepositorio,
            IConversorMusicasDoAlbum conversorMusicasDoAlbum)
        {
            _albumRepositorio = albumRepositorio;
            _bandaRepositorio = bandaRepositorio;
            _conversor = conversorMusicasDoAlbum;
        }

        /// <summary>
        /// GET: api/AlbumController/listar
        /// </summary>
        /// <param name="conversor"></param>
        /// <param name="skip">Quantos registros pular? Padrão: 0</param>
        /// <param name="take">Quantos registros pegar? Padrão: 10</param>
        /// <param name="nome">Insira o nome da banda que deseja pesquisar</param>
        /// <returns>Listagem de bandas registradas</returns>
        [HttpGet("listar")]
        public async Task<IEnumerable<ListagemDeAlbuns>> Get(
            [FromServices] IConversorMusicasDoAlbum conversor,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 10,
            string? nome = null)
        {
            var albuns = await _albumRepositorio.Consultar();

            if (albuns != null && nome != null)
            {
                var dto = albuns.Select(x => new ListagemDeAlbuns
                {
                    Nome = x.Nome,
                    Banda = x.Banda.Nome,
                    Musicas = conversor.ConverterParaListagemDeMusicas(x.MusicasDoAlbum),
                    Imagem = x.Imagem
                }).Where(x => x.Nome.Equals(nome)).ToList();

                return dto.Any() ? dto : new List<ListagemDeAlbuns>();
            }

            if (albuns != null)
            {
                var dto = albuns.Select(x => new ListagemDeAlbuns
                {
                    Nome = x.Nome,
                    Banda = x.Banda.Nome,
                    Musicas = conversor.ConverterParaListagemDeMusicas(x.MusicasDoAlbum),
                    Imagem = x.Imagem
                }).ToList();

                return dto.Any() ? dto : new List<ListagemDeAlbuns>();
            }

            return new List<ListagemDeAlbuns>();
        }

        /// <summary>
        /// GET api/<AlbumController>/listarTopFive
        /// </summary>
        /// <param name="conversor"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="nome"></param>
        /// <returns></returns>
        [HttpGet("listarTopFive")]
        public async Task<IEnumerable<ListagemDeAlbuns>> GetTopFive(
            [FromServices] IConversorMusicasDoAlbum conversor,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 5,
            string? nome = null)
        {
            var albuns = await _albumRepositorio.Consultar();

            if (albuns != null && nome != null)
            {
                var dto = albuns.Select(x => new ListagemDeAlbuns
                {
                    Nome = x.Nome,
                    Banda = x.Banda.Nome,
                    Musicas = conversor.ConverterParaListagemDeMusicas(x.MusicasDoAlbum),
                    Imagem = x.Imagem
                }).Where(x => x.Nome.Equals(nome)).ToList();

                return dto.Any() ? dto : new List<ListagemDeAlbuns>();
            }

            if (albuns == null) return new List<ListagemDeAlbuns>();
            {
                var dto = albuns.Select(x => new ListagemDeAlbuns
                {
                    Nome = x.Nome,
                    Banda = x.Banda.Nome,
                    Musicas = conversor.ConverterParaListagemDeMusicas(x.MusicasDoAlbum),
                    Imagem = x.Imagem
                }).ToList();

                return dto.Any() ? dto : new List<ListagemDeAlbuns>();
            }
        }

        /// <summary>
        /// GET api/AlbumController/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("listar/{id}")]
        public async Task<IActionResult> GetForId(int id)
        {
            var album = await _albumRepositorio.ObterPorId(id);

            if (album == null) return NotFound(new ListagemDeAlbuns());
            var dto = new ListagemDeAlbuns
            {
                Nome = album.Nome,
                Banda = album.Banda.Nome,
                Musicas = _conversor.ConverterParaListagemDeMusicas(album.MusicasDoAlbum)
            };

            return Ok(dto);
        }

        [HttpGet("imagem/{id}")]
        public IActionResult GetImage(int id)
        {
            var path = $"../../../../assets/CapasDeMusica/{id}";

            return Ok(path);
        }

        // POST api/<AlbumController>
        [HttpPost("adicionarAlbum")]
        public async Task<IActionResult> Post(CreateAlbumDto dto, [FromServices] IArmazenadorAlbum armazenadorAlbum)
        {
            var banda = await _bandaRepositorio.ObterPorNome(dto.NomeBanda);

            if (banda == null) return NotFound(Resource.BandaInexistente);
            await armazenadorAlbum.Armazenar(dto.Nome, banda);
            return Ok(await Get(_conversor, nome: dto.Nome));
        }

        // PUT api/<AlbumController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromQuery] string? nomeAlbum = null,
            [FromQuery] string? artista = null)
        {
            var album = await _albumRepositorio.ObterPorId(id);
            var banda = await _bandaRepositorio.ObterPorNome(artista);

            if (album == null) return NotFound();
            
            if (nomeAlbum != null && album != null)
            {
                album.AlterarNome(nomeAlbum);
            }

            if (artista != null && album != null)
            {
                album.AlterarArtista(banda);
            }

            return Ok(await Get(_conversor, nome: nomeAlbum));
        }

        // DELETE api/<AlbumController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var album = await _albumRepositorio.ObterPorId(id);

            if (album != null)
            {
                await _albumRepositorio.Deletar(album.Id);
                return NoContent();
            }

            return NotFound();
        }
    }
}