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

namespace BlazorNFC.Shared.Dialog
{
    public class DialogNFCBase : BaseComponent, IDisposable
    {
        #region Parametros
        [CascadingParameter]
        public MudDialogInstance MudDialog { get; set; }

        [Parameter]
        public StatusNFC Status { get; set; }
        #endregion

        #region Propriedades
        protected string TextoOperacao { get; set; }

        protected EntidadeJSON Model { get; set; }

        public DotNetObjectReference<DialogNFCBase> ViewRef;
        #endregion

        #region Construtor
        public DialogNFCBase()
        {
            TextoOperacao = string.Empty;
            ViewRef = DotNetObjectReference.Create(this);
        }
        #endregion

        #region Eventos
        protected async override Task OnInitializedAsync()
        {
            AjustarStatus(StatusNFC.Iniciando, "Iniciando o sistema!");

            await Task.Delay(2000);

            AjustarStatus(StatusNFC.Buscando, "Aproxime o cartão próximo ao sensor!");

            await JS.InvokeVoidAsync("GravarJsonNFC", ViewRef, Model);
        }

        private void AjustarStatus(StatusNFC StatusNovo, string Menssagem)
        {
            Status = StatusNovo;
            TextoOperacao = Menssagem;
            StateHasChanged();
        }

        protected void Close() =>
            MudDialog.Cancel();

        public void Dispose() =>
            ViewRef?.Dispose();

        #region Evento JS
        [JSInvokable]
        public void GravadoNFC(string msg)
        {
            AjustarStatus(StatusNFC.Concluido, $"NFC gravado com sucesso!");
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
