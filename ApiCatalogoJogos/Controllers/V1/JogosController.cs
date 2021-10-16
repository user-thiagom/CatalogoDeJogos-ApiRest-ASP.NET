using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Services;
using ApiCatalogoJogos.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly IJogoService _jogoservice;

        public JogosController(IJogoService jogoservice)
        {
            _jogoservice = jogoservice;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1,50)] int quantidade = 5)
        {
            var jogos = await _jogoservice.Obter(1, 5);
            if (jogos.Count() == 0)
                return NoContent();
            return Ok(jogos);
        }

        [HttpGet("{idJogo:guid}")]
        public async Task<ActionResult<List<JogoViewModel>>> Obter([FromRoute]Guid idJogo)
        {
            var jogo = _jogoservice.Obter(idJogo);
            if (jogo == null)
                return NoContent();
            return Ok(jogo);
        }

        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> InserirJogo([FromBody]JogoInputModel jogoInputModel)
        {
            try
            {
                var jogo = await _jogoservice.Inserir(jogoInputModel);
                return Ok(jogo);
            }
            catch (JogoCadastradoException ex)
            {
                return UnprocessableEntity("Já existe um jogo com este nome para esta produtora");
            }
        }

        [HttpPut("{idJogo:guid}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute]Guid idJogo, [FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                await _jogoservice.Atualizar(idJogo, jogoInputModel);
                return Ok();
            }
            catch (Exception)
            {
                return NotFound("Não Existe este Jogo");
            }
        }

        [HttpPatch("{idJogo:guid}/preco/{preco:double}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid idJogo, [FromRoute] double preco)
        {
            try
            {
                await _jogoservice.Atualizar(idJogo, preco);
                return Ok();
            }
            catch (Exception)
            {
                return NotFound("Não Existe este Jogo");
            }
        }

        [HttpDelete("{idJogo:guid}")]
        public async Task<ActionResult> ApagarJogo( [FromRoute] Guid idJogo)
        {
            try
            {
                await _jogoservice.Remover(idJogo);
                return Ok();
            }
            catch (Exception)
            {
                return NotFound("Não Existe Este Jogo");
            }
        }
    }
}
