using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorNFC.Data
{
    public class BaseComponent : ComponentBase
    {
        [Inject]
        protected ISnackbar Snackbar { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }

        public bool Load { get; set; }
    }
}
