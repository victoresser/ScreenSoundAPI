using Microsoft.AspNetCore.Mvc;
using ScreenSound.Dominio._Base;
using ScreenSound.Dominio.Interfaces.Armazenadores;
using ScreenSound.Dominio.Interfaces.Repositorios;
using ScreenSound.Dominio.Models.Bandas.Dto;
using ScreenSound.Dominio.Models.Bandas.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ScreenSound.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BandaController : ControllerBase
    {
        private readonly IBandaRepositorio _bandaRepositorio;
        private readonly ConsultaListagemDeBandas _consultaListagemDeBandas;

        public BandaController(IBandaRepositorio bandaRepositorio, ConsultaListagemDeBandas consultaListagemDeBandas)
        {
            _bandaRepositorio = bandaRepositorio;
            _consultaListagemDeBandas = consultaListagemDeBandas;
        }

        /// <summary>
        /// GET: api/<BandaController>/listar
        /// </summary>
        /// <param name="conversor">Conversor de albuns da banda</param>
        /// <param name="skip">Variável que determina a quantidade de registros que serão ignorados</param>
        /// <param name="take">Variável que determina a quantidade de registros que serão exibidos</param>
        /// <param name="nomeBanda">Nome da banda que se deseja listar</param>
        /// <returns></returns>
        [HttpGet("listar")]
        public async Task<IEnumerable<ListagemDeBandas>> Get([FromQuery] int skip = 0, [FromQuery] int take = 10,
            string? nomeBanda = null) => await _consultaListagemDeBandas.RetornaListagemDeBandas(nomeBanda, skip, take);
        
        /// <summary>
        /// GET api/BandaController/listarTopFive
        /// </summary>
        /// <param name="conversor"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="nomeBanda"></param>
        /// <returns></returns>
        [HttpGet("listarTopFive")]
        public async Task<IEnumerable<ListagemDeBandas>> GetTopFive([FromQuery] int skip = 0, [FromQuery] int take = 5,
            string? nomeBanda = null) => await _consultaListagemDeBandas.RetornaListagemDeBandas(nomeBanda, skip, take);

        /// <summary>
        /// GET api/<BandaController>/listar/5
        /// </summary>
        /// <param name="id">Id da Banda</param>
        /// <param name="conversor"></param>
        /// <returns></returns>
        [HttpGet("listar/{id:int}")]
        public async Task<IActionResult> GetForId(int id)
        {
            var consulta = await _consultaListagemDeBandas.RetornarBandaPorId(id);
            return Ok(consulta);
        }

        /// <summary>
        /// POST api/<BandaController>/adicionarBanda
        /// </summary>
        /// <param name="nomeDaBanda"></param>
        /// <param name="dto">DTO com informações necessárias para criação de uma nova banda</param>
        /// <param name="armazenadorBanda">Serviço que armazena as informações da Banda</param>
        /// <param name="imagem">URL da imagem de capa da banda</param>
        /// <returns></returns>
        [HttpPost("adicionarBanda")]
        public async Task<IActionResult> Post([FromBody] CreateBandaDto dto, [FromServices] IArmazenadorBanda armazenadorBanda)
        {
            await armazenadorBanda.Armazenar(dto);
            return Ok(await Get(nomeBanda: dto.Nome));
        }
        
        /// <summary>
        /// PUT api/<BandaController>/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="armazenadorBanda"></param>
        /// <returns></returns>
        [HttpPut("editar/{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] EditBandaDto dto,[FromServices] IArmazenadorBanda armazenadorBanda)
        {
            await armazenadorBanda.Editar(id, dto.Nome, dto.Descricao);
            return Ok(await GetForId(id));
        }
        
        /// <summary>
        /// DELETE api/<BandaController>/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("excluir/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _bandaRepositorio.ObterPorIdAsync(id);
            
            await _bandaRepositorio.Deletar(dto.Id);
            return Ok(Resource.BandaExcluida);
        }
    }
}
