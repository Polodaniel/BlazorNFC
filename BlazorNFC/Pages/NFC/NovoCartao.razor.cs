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
    public class NovoCartaoBase : BaseComponent, IDisposable
    {
        #region Propriedades
        protected EntidadeJSON Model { get; set; }

        protected bool CriarCartaoView { get; set; }

        protected MudDatePicker _picker { get; set; }

        protected List<EntidadeValidacao> ListaValidacoes { get; set; }

        public DotNetObjectReference<NovoCartaoBase> ViewRef;
        #endregion

        #region Construtor
        public NovoCartaoBase()
        {
            CriarCartaoView = true;

            Model = new EntidadeJSON();

            _picker = new MudDatePicker();

            ViewRef = DotNetObjectReference.Create(this);

            ListaValidacoes = new List<EntidadeValidacao>();
        }
        #endregion

        #region Eventos

        #region Eventos Tela
        protected override void OnInitialized()
        {
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;

            MontarListaValidacao();
        }

        private void MontarListaValidacao()
        {
            ListaValidacoes.Add(new EntidadeValidacao(1, "Informação validas", true));
            ListaValidacoes.Add(new EntidadeValidacao(2, "Navegador", null));
            ListaValidacoes.Add(new EntidadeValidacao(3, "Hardware NFC", null));
        }

        protected async void ValidarInformacoes()
        {
            if (string.IsNullOrEmpty(Model.Nome))
            {
                Menssagem("Ops! O nome deve-se ser informado.", Severity.Error);
                return;
            }

            CriarCartaoView = false;

            await RealizaValidacoes();
        }

        private async Task RealizaValidacoes()
        {
            ListaValidacoes.ForEach(async x =>
            {
                if (x.ID == 2)
                    await VerificaDispositivo();
                else if (x.ID == 3)
                    await VerificaHardware();
            });
        }

        protected void VoltarCriarCartao() =>
            CriarCartaoView = true;

        protected async Task<bool> GerarCartao() 
        {
            var Parameters = new DialogParameters();
            Parameters.Add("Model", Model);

            var Options = new DialogOptions()
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Medium
            };

            var result = await DialogService.Show<DialogNFC>("Gerar NFC").Result;

            if (!result.Cancelled)
            {
                //license_accepted = (bool)(result.Data ?? false);
            }

            return true;
        }

        public void Dispose()
        {

        }
        #endregion

        #region Eventos JS
        public async Task VerificaDispositivo() =>
            await JS.InvokeVoidAsync("VerificaDispositivo", ViewRef, 2);

        public async Task VerificaHardware() =>
            await JS.InvokeVoidAsync("VerificaHardware", ViewRef, 3);

        [JSInvokable]
        public void RetornoVerificacoes(int ID, bool Status)
        {
            var Item = ListaValidacoes.Where(x => x.ID == ID).FirstOrDefault();

            if (!Equals(Item, null))
                Item.Status = Status;

            StateHasChanged();
        }
        #endregion

        #endregion
    }
}
