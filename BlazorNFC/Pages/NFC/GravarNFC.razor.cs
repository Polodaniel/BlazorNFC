using BlazorNFC.Data;
using BlazorNFC.Data.NFC;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorNFC.Pages.NFC
{
    public class GravarNFCBase : BaseComponent, IDisposable
    {
        public MudMessageBox mbox { get; set; }

        protected Color StatusDispositivoColor { get; set; }

        protected bool StatusDispositivo { get; set; }

        protected string StatusDispositivoText { get; set; }

        protected string OperacaoText { get; set; }
        protected Color OperacaoColor { get; set; }

        private DotNetObjectReference<GravarNFCBase> ViewRef;

        protected EntidadeJSON Model { get; set; }

        public GravarNFCBase()
        {
            ViewRef = DotNetObjectReference.Create(this);

            Model = new EntidadeJSON();

            StatusDispositivoText = "...";
            MudarTextoOperacao("Aguardando operação");

            StatusDispositivoColor = Color.Default;
            OperacaoColor = Color.Default;

            Load = false;

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
                await VerificaDispositivo();
        }

        public async Task VerificaDispositivo()
        {
            Load = true;
            StateHasChanged();

            await Task.Delay(3000);

            await JS.InvokeVoidAsync("VerificaDispositivo", ViewRef);
        }

        [JSInvokable]
        public void VerificaDispositivo(bool Status)
        {
            StatusDispositivo = Status;

            StatusDispositivoText = Status ? "Válido" : "Não localizado";
            StatusDispositivoColor = Status ? Color.Success : Color.Error;

            Load = false;

            StateHasChanged();
        }

        public bool ValidaInformacoes()
        {
            if (string.IsNullOrEmpty(Model.Nome))
            {
                Snackbar.Add("Oops! O Nome é obrigatório!", Severity.Error);
                return false;
            }

            return true;
        }

        public async Task GravarNFC()
        {
            if (ValidaInformacoes())
            {
                Load = true;
                MudarTextoOperacao("Gravando NFC");

                StateHasChanged();

                await Task.Delay(3000);

                await JS.InvokeVoidAsync("GravarJsonNFC", ViewRef, Model);

            }
        }

        [JSInvokable]
        public void GravarNFC(bool result)
        {
            Load = false;

            MudarTextoOperacao("Aguardando operação");

            if (result)
                Snackbar.Add("NFC grava com sucesso!", Severity.Success);
            else
                Snackbar.Add("Oops! Ocorreu um erro ao gravar o NFC!", Severity.Error);

            StateHasChanged();
        }

        [JSInvokable]
        public void ErroNFC(string msg)
        {
            Load = false;

            MudarTextoOperacao("Aguardando operação");

            Snackbar.Add(string.Concat("Oops! ", msg), Severity.Error);

            StateHasChanged();
        }

        private void MudarTextoOperacao(string Text) =>
            OperacaoText = Text;


        protected IEnumerable<string> MaxCharacters(string ch)
        {
            if (!string.IsNullOrEmpty(ch) && 25 < ch?.Length)
                yield return "O nome deve ter no máximo 50 characters.";
        }

        public void Dispose() =>
            ViewRef?.Dispose();
    }
}
