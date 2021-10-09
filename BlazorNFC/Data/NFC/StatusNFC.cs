using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorNFC.Data.NFC
{
    public enum StatusNFC
    {
        /// <summary>
        /// Status de Iniciando o Sistema
        /// </summary>
        Iniciando = 0,

        /// <summary>
        /// Buscando Dispositivos NFC
        /// </summary>
        Buscando = 1,

        /// <summary>
        /// Erro ao Buscar Dispositivos NFC
        /// </summary>
        Error = 2,

        /// <summary>
        /// Concluido a operação NFC
        /// </summary>
        Concluido = 3
    }
}
