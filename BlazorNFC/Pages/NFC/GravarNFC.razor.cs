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
        protected Color StatusDispositivoColor { get; set; }

        protected bool StatusDispositivo { get; set; }
        
        protected string StatusDispositivoText { get; set; }

        private DotNetObjectReference<GravarNFCBase> ViewRef;

        protected EntidadeJSON Model { get; set; }

        private PoloNFC NFC { get; set; }

        public GravarNFCBase()
        {
            ViewRef = DotNetObjectReference.Create(this);

            Model = new EntidadeJSON();

            NFC = new PoloNFC();

            StatusDispositivoText = "...";

            StatusDispositivoColor = Color.Default;

            Load = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await VerificaDispositivo();
            }
        }

        public async Task VerificaDispositivo()
        {
            Load = true;
            StateHasChanged();

            await Task.Delay(3000);

            await JS.InvokeVoidAsync("VerificaDispositivo", ViewRef);
        }

        [JSInvokable]
        public async void VerificaDispositivo(bool Status)
        {
            StatusDispositivo = Status;

            StatusDispositivoText = Status ? "Válido" : "Não localizado";

            StatusDispositivoColor = Status ? Color.Success : Color.Error;

            Load = false;

            StateHasChanged();
        }

        protected IEnumerable<string> MaxCharacters(string ch)
        {
            if (!string.IsNullOrEmpty(ch) && 25 < ch?.Length)
                yield return "O nome deve ter no máximo 50 characters.";
        }

        public void Dispose() =>
            ViewRef?.Dispose();
    }
}
