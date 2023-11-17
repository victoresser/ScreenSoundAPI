﻿using Microsoft.AspNetCore.Mvc;
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
        /// GET: api/<BandaController>
        /// </summary>
        /// <param name="skip">Variável que determina a quantidade de registros que serão ignorados</param>
        /// <param name="take">Variável que determina a quantidade de registros que serão exibidos</param>
        /// <param name="nomeBanda">Nome da banda que se deseja pesquisar</param>
        /// <returns></returns>
        [HttpGet("pesquisar")]
        public async Task<IEnumerable<ListagemDeBandas>> Get(
            [FromServices] IConversorAlbunsDaBanda _conversor,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 10,
            string? nomeBanda = null
            )
        {
            var consulta = await _bandaRepositorio.Consultar();

            if (nomeBanda == null)
            {
                var dto = consulta.Select(x => new ListagemDeBandas
                {
                    Nome = x.Nome,
                    Descricao = x.Descricao,
                    Albuns = _conversor.ConverterParaListagemDeAlbuns(x.AlbunsDaBanda)
                }).Skip(0).Take(10).ToList();

                return dto.Any() ? dto : new List<ListagemDeBandas> { };
            }

            var dtos = consulta.Select(x => new ListagemDeBandas
            {
                Nome = x.Nome,
                Descricao = x.Descricao,
                Albuns = _conversor.ConverterParaListagemDeAlbuns(x.AlbunsDaBanda),
            }).Where(x => x.Nome.Equals(nomeBanda)).Skip(skip).Take(take).ToList();

            return dtos.Any() ? dtos : new List<ListagemDeBandas> { };

        }

        /// <summary>
        /// GET api/<BandaController>/5
        /// </summary>
        /// <param name="id">Id da Banda</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetForId(int id, [FromServices] IConversorAlbunsDaBanda _conversor)
        {
            var consulta = await _bandaRepositorio.ObterPorId(id);

            if (consulta == null)
            {
                return NotFound(new ListagemDeBandas { });
            }

            var dto = new ListagemDeBandas
            {
                Nome = consulta.Nome,
                Descricao = consulta.Descricao,
                Albuns = _conversor.ConverterParaListagemDeAlbuns(consulta.AlbunsDaBanda),
            };

            return Ok(dto);
        }

        // POST api/<BandaController>
        [HttpPost]
        public async Task<IActionResult> Post(string nomeDaBanda, [FromServices] IArmazenadorBanda _armazenadorBanda)
        {
            var dto = new Banda(nomeDaBanda);
            await _armazenadorBanda.Armazenar(dto);
            return Ok(await Get(nomeBanda: nomeDaBanda, _conversor: _conversor));
        }

        // PUT api/<BandaController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, string? nome, string? descricao, [FromServices] IArmazenadorBanda _armazenadorBanda)
        {
            await _armazenadorBanda.Editar(id, nome, descricao);
            return Ok(await GetForId(id, _conversor));
        }

        // DELETE api/<BandaController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _bandaRepositorio.ObterPorId(id);
            await _bandaRepositorio.Deletar(dto.Id);
            return NoContent();
        }
    }
}
