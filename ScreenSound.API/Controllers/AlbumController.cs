using Microsoft.AspNetCore.Mvc;
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

        public AlbumController(IAlbumRepositorio albumRepositorio, IBandaRepositorio bandaRepositorio, IConversorMusicasDoAlbum conversorMusicasDoAlbum)
        {
            _albumRepositorio = albumRepositorio;
            _bandaRepositorio = bandaRepositorio;
            _conversor = conversorMusicasDoAlbum;
        }

        // GET: api/<AlbumController>
        [HttpGet("pesquisar")]
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
                    Musicas = conversor.ConverterParaListagemDeMusicas(x.MusicasDoAlbum)
                }).Where(x => x.Nome.Equals(nome)).ToList();

                return dto.Any() ? dto : new List<ListagemDeAlbuns> { };
            }

            if (albuns != null)
            {
                var dto = albuns.Select(x => new ListagemDeAlbuns
                {
                    Nome = x.Nome,
                    Banda = x.Banda.Nome,
                    Musicas = conversor.ConverterParaListagemDeMusicas(x.MusicasDoAlbum)
                }).ToList();

                return dto.Any() ? dto : new List<ListagemDeAlbuns> { };
            }

            return new List<ListagemDeAlbuns> { };
        }

        // GET api/<AlbumController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetForId(int id)
        {
            var album = await _albumRepositorio.ObterPorId(id);

            if (album != null)
            {
                var dto = new ListagemDeAlbuns
                {
                    Nome = album.Nome,
                    Banda = album.Banda.Nome,
                    Musicas = _conversor.ConverterParaListagemDeMusicas(album.MusicasDoAlbum)
                };

                return Ok(dto);
            }

            return NotFound(new ListagemDeAlbuns { });
        }

        [HttpGet("imagem/{id}")]
        public IActionResult GetImage(int id)
        {
            var path = $"../../../../assets/CapasDeMusica/{id}";

            return Ok(path);
        }

        // POST api/<AlbumController>
        [HttpPost]
        public async Task<IActionResult> Post(string nomeAlbum, string nomeBanda, [FromServices] IArmazenadorAlbum _armazenadorAlbum)
        {
            Banda? banda = await _bandaRepositorio.ObterPorNome(nomeBanda);

            if (banda != null)
            {
                await _armazenadorAlbum.Armazenar(nomeAlbum, banda);
                return Ok(await Get(_conversor, nome: nomeAlbum));
            }

            return NotFound("Álbum não encontrada!");
        }

        // PUT api/<AlbumController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromQuery] string? nomeAlbum = null, [FromQuery] string? artista = null)
        {
            var album = await _albumRepositorio.ObterPorId(id);
            var banda = await _bandaRepositorio.ObterPorNome(artista);

            if (album != null)
            {
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

            return NotFound();
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
