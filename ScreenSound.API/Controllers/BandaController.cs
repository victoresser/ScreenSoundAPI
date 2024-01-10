using Microsoft.AspNetCore.Mvc;
using ScreenSound.Dominio.Models.Bandas;
using ScreenSound.Dominio.Models.Bandas.Dto;
using ScreenSound.Dominio.Services.Armazenadores;
using ScreenSound.Dominio.Services.Conversores;
using ScreenSound.Dominio.Services.Repositorios;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ScreenSound.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BandaController : ControllerBase
    {
        private readonly IBandaRepositorio _bandaRepositorio;
        private readonly IConversorAlbunsDaBanda _conversor;

        public BandaController(IBandaRepositorio bandaRepositorio, IConversorAlbunsDaBanda conversor)
        {
            _bandaRepositorio = bandaRepositorio;
            _conversor = conversor;
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
        public async Task<IEnumerable<ListagemDeBandas>> Get(
            [FromServices] IConversorAlbunsDaBanda conversor,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 10,
            string? nomeBanda = null
            )
        {
            var consulta = await _bandaRepositorio.ConsultarAsync();

            if (nomeBanda == null)
            {
                var dto = consulta.Select(x => new ListagemDeBandas
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    Descricao = x.Descricao,
                    Albuns = conversor.ConverterParaListagemDeAlbuns(x.AlbunsDaBanda),
                    Imagem = x.Imagem
                }).Skip(skip).Take(take).ToList();

                return dto.Any() ? dto : new List<ListagemDeBandas>();
            }

            var dtos = consulta.Select(x => new ListagemDeBandas
            {
                Id = x.Id,
                Nome = x.Nome,
                Descricao = x.Descricao,
                Albuns = conversor.ConverterParaListagemDeAlbuns(x.AlbunsDaBanda),
                Imagem = x.Imagem
            }).Where(x => x.Nome.Contains(nomeBanda)).Skip(skip).Take(take).ToList();

            return dtos.Any() ? dtos : new List<ListagemDeBandas>();

        }
        
        /// <summary>
        /// GET api/BandaController/listarTopFive
        /// </summary>
        /// <param name="conversor"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="nomeBanda"></param>
        /// <returns></returns>
        [HttpGet("listarTopFive")]
        public async Task<IEnumerable<ListagemDeBandas>> GetTopFive(
            [FromServices] IConversorAlbunsDaBanda conversor,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 5,
            string? nomeBanda = null
            )
        {
            var consulta = await _bandaRepositorio.ConsultarAsync();

            if (nomeBanda == null)
            {
                var dto = consulta.Select(x => new ListagemDeBandas
                {
                    Nome = x.Nome,
                    Descricao = x.Descricao,
                    Albuns = conversor.ConverterParaListagemDeAlbuns(x.AlbunsDaBanda),
                    Imagem = x.Imagem
                }).Skip(skip).Take(take).ToList();

                return dto.Any() ? dto : new List<ListagemDeBandas> { };
            }

            var dtos = consulta.Select(x => new ListagemDeBandas
            {
                Nome = x.Nome,
                Descricao = x.Descricao,
                Albuns = conversor.ConverterParaListagemDeAlbuns(x.AlbunsDaBanda),
                Imagem = x.Imagem
            }).Where(x => x.Nome.Equals(nomeBanda)).Skip(skip).Take(take).ToList();

            return dtos.Any() ? dtos : new List<ListagemDeBandas> { };

        }

        /// <summary>
        /// GET api/<BandaController>/listar/5
        /// </summary>
        /// <param name="id">Id da Banda</param>
        /// <param name="conversor"></param>
        /// <returns></returns>
        [HttpGet("listar/{id}")]
        public async Task<IActionResult> GetForId(int id, [FromServices] IConversorAlbunsDaBanda conversor)
        {
            var consulta = await _bandaRepositorio.ObterPorIdAsync(id);

            if (consulta == null)
            {
                return NotFound(new ListagemDeBandas { });
            }

            var dto = new ListagemDeBandas
            {
                Nome = consulta.Nome,
                Descricao = consulta.Descricao,
                Albuns = conversor.ConverterParaListagemDeAlbuns(consulta.AlbunsDaBanda),
                Imagem = consulta.Imagem
            };

            return Ok(dto);
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
        public async Task<IActionResult> Post(CreateBandaDto dto, [FromServices] IArmazenadorBanda armazenadorBanda)
        {
            await armazenadorBanda.Armazenar(dto);
            return Ok(await Get(nomeBanda: dto.Nome, conversor: _conversor));
        }

        // PUT api/<BandaController>/5
        [HttpPut("editar/{id:int}")]
        public async Task<IActionResult> Put(int id, EditBandaDto dto,[FromServices] IArmazenadorBanda armazenadorBanda)
        {
            await armazenadorBanda.Editar(id, dto.Nome, dto.Descricao);
            return Ok(await GetForId(id, _conversor));
        }

        // DELETE api/<BandaController>/5
        [HttpDelete("excluir/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _bandaRepositorio.ObterPorIdAsync(id);
            await _bandaRepositorio.Deletar(dto.Id);
            return NoContent();
        }
    }
}
