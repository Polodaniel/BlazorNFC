using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorNFC.Data.NFC
{
    public class EntidadeJSON
    {
        public EntidadeJSON()
        {
            DataNascimento = DateTime.Now;
            Hash = Guid.NewGuid();
        }

        public string Nome { get; set; }
        
        public Guid Hash { get; set; }
        
        public DateTime? DataNascimento { get; set; }
    }
}
