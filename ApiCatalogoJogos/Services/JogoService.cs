using ApiCatalogoJogos.Entities;
using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Repositories;
using ApiCatalogoJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Services
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogoRepository;

        public JogoService(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        public async Task Atualizar(Guid id, JogoInputModel jogo)
        {
            var jogoBuscar = await _jogoRepository.Obter(id);
            if (jogoBuscar == null)
            {
                throw new JogoNaoCadastradoException();
            }
            jogoBuscar.Nome = jogo.Nome;
            jogoBuscar.Produtora = jogo.Produtora;
            jogoBuscar.Preco = jogo.Preco;
            await _jogoRepository.Atualizar(jogoBuscar);
        }

        public async Task Atualizar(Guid id, double preco)
        {
            var jogoBuscar = await _jogoRepository.Obter(id);
            if (jogoBuscar == null)
            {
                throw new JogoNaoCadastradoException();
            }
            jogoBuscar.Preco = preco;
            await _jogoRepository.Atualizar(jogoBuscar);
        }

        public void Dispose()
        {
            _jogoRepository?.Dispose();
        }

        public async Task<JogoViewModel> Inserir(JogoInputModel jogo)
        {
            var entidadeJogo = await _jogoRepository.Obter(jogo.Nome, jogo.Produtora);
            if (entidadeJogo.Count > 0)
            {
                throw new JogoCadastradoException();
            }

            var jogoInsert = new Jogo
            {
                Id = Guid.NewGuid(),
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };

            await _jogoRepository.Inserir(jogoInsert);

            return new JogoViewModel
            {
                Id = jogoInsert.Id,
                Nome = jogoInsert.Nome,
                Produtora = jogoInsert.Produtora,
                Preco = jogoInsert.Preco
            };
            
        }

        public async Task<List<JogoViewModel>> Obter(int pagina, int quantidade)
        {
            var jogos = await _jogoRepository.Obter(pagina, quantidade);
            return jogos.Select(jogo => new JogoViewModel
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            }).ToList();
        }

        public async Task<JogoViewModel> Obter(Guid id)
        {
            var jogo = await _jogoRepository.Obter(id);
            if (jogo == null)
            {
                return null;
            }
            return new JogoViewModel
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };
          
        }

        public async Task Remover(Guid id)
        {
            var jogo = _jogoRepository.Obter(id);
            if (jogo == null)
            {
                throw new JogoNaoCadastradoException();
            }
            await _jogoRepository.Remover(id);
        }
    }
}
