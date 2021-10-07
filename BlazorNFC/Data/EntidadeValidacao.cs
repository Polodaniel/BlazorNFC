using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorNFC.Data
{
    public class EntidadeValidacao
    {
        public EntidadeValidacao() { }

        public EntidadeValidacao(int ID, string Operacao, bool? Status)
        {
            this.ID = ID;
            this.Operacao = Operacao;
            this.Status = Status;
        }

        public int ID { get; set; }
        public string Operacao { get; set; }
        public bool? Status { get; set; }
    }
}
