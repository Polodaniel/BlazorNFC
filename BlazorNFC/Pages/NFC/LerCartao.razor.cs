﻿using BlazorNFC.Data;
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

            Model = new EntidadeJSON(false);

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
            AjustarStatus(StatusNFC.Iniciando, "Inicializando o sistema.");
            await Task.Delay(5000);

            AjustarStatus(StatusNFC.Buscando, "Aproxime o Catão para realizar a busca");

            await JS.InvokeVoidAsync("LerNFC", ViewRef);
        }

        public void Dispose()
        {

        }


        #region Evento JS
        [JSInvokable]
        public void LerNFC(string Nome, string ChaveHash, string Chave, string Data)
        {
            Model.Nome = Nome;
            Model.ChaveHash = ChaveHash;
            Model.Chave = !string.IsNullOrEmpty(Chave) ? Convert.ToInt32(Chave) : 0;

            var DataCartao = string.Empty;

            if (!string.IsNullOrEmpty(Data))
            {
                DataCartao = Convert.ToDateTime(Data).ToString("dd/MM/yyyy");

                Model.DataValidade = Convert.ToDateTime(DataCartao);
            }

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
