using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorNFC.Data.NFC
{
    public class EntidadeJSON
    {
        public EntidadeJSON(bool DadosIniciais)
        {
            if (DadosIniciais)
            {
                DataValidade = DateTime.Now.AddYears(1);
                Data = DataValidade?.ToString("MM/yy");
                ChaveHash = GerarChaveHash();
                Chave = GerarChave();
            }
        }

        public string Nome { get; set; }

        public string ChaveHash { get; set; }

        public int Chave { get; set; }

        private DateTime? _dataValidade;
        public DateTime? DataValidade 
        {
            get => _dataValidade;
            set 
            {
                _dataValidade = value;

                Data = _dataValidade?.ToString("MM/yy");
            }
        }

        public string Data { get; set; }

        public string GerarChaveHash()
        {
            Random randNum = new Random();
            return string.Concat(randNum.Next(0, 9999).ToString().PadLeft(4, '0'), " ",
                                 randNum.Next(0, 9999).ToString().PadLeft(4, '0'), " ",
                                 randNum.Next(0, 9999).ToString().PadLeft(4, '0'), " ",
                                 randNum.Next(0, 9999).ToString().PadLeft(4, '0'));
        }

        public int GerarChave()
        {
            Random randNum = new Random();
            return randNum.Next(1, 999);
        }

        //public string GerarChave()
        //{
        //    int Tamanho = 8;
        //    string senha = string.Empty;
        //    for (int i = 0; i < Tamanho; i++)
        //    {
        //        Random random = new Random();
        //        int codigo = Convert.ToInt32(random.Next(48, 122).ToString());

        //        if ((codigo >= 48 && codigo <= 57) || (codigo >= 97 && codigo <= 122))
        //        {
        //            string _char = ((char)codigo).ToString();

        //            if (!senha.Contains(_char))
        //                senha += _char;
        //            else
        //                i--;
        //        }
        //        else
        //        {
        //            i--;
        //        }
        //    }
        //    return senha;
        //}
    }
}
