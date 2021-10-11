using BlazorNFC.Data;
using BlazorNFC.Data.NFC;
using BlazorNFC.Shared.Dialog;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorNFC.Pages.NFC
{
    public class LerCartaoBase : BaseComponent, IDisposable
    {
        #region Propriedades
        protected EntidadeJSON Model { get; set; }

        public DotNetObjectReference<LerCartaoBase> ViewRef;
        #endregion

        #region Constrtor
        public LerCartaoBase()
        {
            Status = StatusNFC.Iniciando;

            TextoOperacao = string.Empty;
            ViewRef = DotNetObjectReference.Create(this);
        }
        #endregion

        #region Eventos
        protected async override Task OnInitializedAsync()
        {
            AjustarStatus(StatusNFC.Iniciando, "Sistema parado.");
        }

        protected async void LerCartaoNFC() 
        {
            AjustarStatus(StatusNFC.Iniciando,"Inicializando o sistema.");
            await Task.Delay(5000);

            AjustarStatus(StatusNFC.Buscando,"Aproxime o Catão para realizar a busca");
            
            await JS.InvokeVoidAsync("LerNFC", ViewRef);
        }

        public void Dispose()
        {
            
        }


        #region Evento JS
        [JSInvokable]
        public void LerNFC(string Nome, string ChaveHash, string Chave, string Data)
        {
            AjustarStatus(StatusNFC.Error, string.Concat("Nome: ", Nome));
            StateHasChanged();
        }


        [JSInvokable]
        public void ErroNFC(string msg)
        {
            AjustarStatus(StatusNFC.Error, $"Ops! Erro {msg}!");
            StateHasChanged();
        }
        #endregion

        #endregion
    }
}
