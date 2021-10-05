using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorNFC.Data
{
    public class BaseComponent : ComponentBase
    {
        [Inject]
        public IJSRuntime JS { get; set; }

        public bool Load { get; set; }
    }
}
