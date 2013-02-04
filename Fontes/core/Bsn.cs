﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bsn.core.busca;
using bsn.core.analise;

namespace bsn.core
{
    public class Bsn
    {
        private Analisador analisador = new Analisador();
        private Buscador buscador = new Buscador();

        /// <summary>
        /// Pede que o buscador se conecte ao Site de Origem e retorna uma
        /// versão atualizada do Conteúdo do Alvo
        /// </summary>
        /// <param name="alvo"></param>
        /// <returns></returns>
        public Alvo GetAlvoAtualizado(Alvo alvo)
        {
            return buscador.GetAlvoAtualizado(alvo);
        }

        /// <summary>
        /// Retorna o Alvo passado com o Anuncio extraido
        /// </summary>
        /// <param name="alvo"></param>
        /// <returns></returns>
        public Alvo GetAnuncioAlvo(Alvo alvo)
        {
            return analisador.Analisar(alvo);
        }

        /// <summary>
        /// Gerar uma lista de Alvos 
        /// </summary>
        /// <param name="site"></param>
        /// <param name="idInicio"></param>
        /// <param name="idFim"></param>
        /// <returns></returns>
        public IList<Alvo> GetAlvos(string site, int idInicio, int idFim)
        {
            var infonet = Site.GetSitePorNome(site);
            var retorno = new List<Alvo>();

            for (int i = idInicio; i <= idFim; i++)
            {
                retorno.Add(new Alvo(infonet, i));
            }

            return retorno;
        }

    }
}