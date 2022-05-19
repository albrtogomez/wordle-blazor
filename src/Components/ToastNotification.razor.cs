using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using WordleBlazor;
using WordleBlazor.Shared;
using WordleBlazor.Components;
using WordleBlazor.Models;
using WordleBlazor.Models.Enums;
using WordleBlazor.Services;

namespace WordleBlazor.Components
{
    public partial class ToastNotification
    {
        private string Message { get; set; } = "";
        private bool IsVisible { get; set; }

        protected override void OnInitialized()
        {
            ToastNotificationService.OnShow += ShowToast;
            ToastNotificationService.OnHide += HideToast;
        }

        private void ShowToast(string message)
        {
            Message = message;
            IsVisible = true;
            StateHasChanged();
        }

        private void HideToast()
        {
            IsVisible = false;
            StateHasChanged();
        }

        public void Dispose()
        {
            ToastNotificationService.OnShow -= ShowToast;
        }
    }
}