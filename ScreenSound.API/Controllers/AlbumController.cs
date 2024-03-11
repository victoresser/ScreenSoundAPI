using Microsoft.AspNetCore.Mvc;
using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Interfaces.Armazenadores;
using ScreenSound.Dominio.Interfaces.Repositorios;
using ScreenSound.Dominio.Models.Albuns.Dto;
using ScreenSound.Dominio.Models.Albuns.Services;

namespace ScreenSound.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly ConsultaListagemDeAlbuns _consultaListagemDeAlbuns;
        private readonly IAlbumRepositorio _albumRepositorio;

        public AlbumController(ConsultaListagemDeAlbuns consultaListagemDeAlbuns, IAlbumRepositorio albumRepositorio)
        {
            _consultaListagemDeAlbuns = consultaListagemDeAlbuns;
            _albumRepositorio = albumRepositorio;
        }

        /// <summary>
        /// GET: api/AlbumController/listar
        /// </summary>
        /// <param name="conversor"></param>
        /// <param name="skip">Quantos registros pular? Padrão: 0</param>
        /// <param name="take">Quantos registros pegar? Padrão: 10</param>
        /// <param name="nomeAlbum">Insira o nome da banda que deseja pesquisar</param>
        /// <returns>Listagem de bandas registradas</returns>
        [HttpGet("listar")]
        public async Task<IEnumerable<ListagemDeAlbuns>> Get([FromQuery] int skip = 0, [FromQuery] int take = 10,
            string? nomeAlbum = null) => await _consultaListagemDeAlbuns.RetornaListagemDeAlbuns(nomeAlbum, skip, take);

        /// <summary>
        /// GET api/<AlbumController>/listarTopFive
        /// </summary>
        /// <param name="conversor"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="nome"></param>
        /// <returns></returns>
        [HttpGet("listarTopFive")]
        public async Task<IEnumerable<ListagemDeAlbuns>> GetTopFive([FromQuery] int skip = 0, [FromQuery] int take = 5,
            string? nome = null) => await _consultaListagemDeAlbuns.RetornaListagemDeAlbuns(nome, skip: skip, take: take);

        /// <summary>
        /// GET api/AlbumController/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("listar/{id:int}")]
        public async Task<IActionResult> GetForId(int id)
        {
            var dto = await _consultaListagemDeAlbuns.RetornaListagemDeAlbunsPorId(id);
            return Ok(dto);
        }
        
        /// <summary>
        /// POST api/<AlbumController>
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="armazenadorAlbum"></param>
        /// <returns></returns>
        [HttpPost("adicionarAlbum")]
        public async Task<IActionResult> Post(CreateAlbumDto dto, [FromServices] IArmazenadorAlbum armazenadorAlbum)
        {
            await armazenadorAlbum.Armazenar(dto);
            return Ok(await Get(nomeAlbum: dto.Nome));
        }
        
        /// <summary>
        /// PUT api/<AlbumController>/5
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="armazenadorAlbum"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("editar/{id:int}")]
        public async Task<IActionResult> Put(EditAlbumDto dto, [FromServices] IArmazenadorAlbum armazenadorAlbum , int id)
        {
            if (id <= 0) return NotFound(Resource.AlbumInexistente);

            await armazenadorAlbum.Editar(dto);

            return Ok(await GetForId(id));
        }
        
        /// <summary>
        /// DELETE api/<AlbumController>/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("excluir/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var album = await _albumRepositorio.ObterPorId(id);

            if (album == null) return NotFound(Resource.AlbumInexistente);
            
            await _albumRepositorio.Deletar(album.Id);
            return Ok(Resource.AlbumExcluido);
        }
    }
}