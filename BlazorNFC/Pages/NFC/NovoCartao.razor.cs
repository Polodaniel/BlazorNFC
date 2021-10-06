using BlazorNFC.Data.NFC;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorNFC.Pages.NFC
{
    public class NovoCartaoBase : ComponentBase
    {
        protected bool CriarCartaoView { get; set; }

        protected MudDatePicker _picker;

        protected EntidadeJSON Model { get; set; }

        public NovoCartaoBase()
        {
            CriarCartaoView = true;

            Model = new EntidadeJSON();

            _picker = new MudDatePicker();
        }

        public void GerarCartao() 
        {
            CriarCartaoView = false;
        }

        public void VoltarCriarCartao()
        {
            CriarCartaoView = true;
        }
    }
}
