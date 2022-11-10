using System;
using System.Linq;
using Cyberbezpieczenstwo.API.Controllers;
using Cyberbezpieczenstwo.API.Models;
using Cyberbezpieczenstwo.API.Modelsp;
using Cyberbezpieczenstwo.Data.Models;
using Microsoft.AspNetCore.Components;

namespace Cyberbezpieczenstwo.Pages;

public partial class Logs
{
    [Inject] private LogHistoryController logHistoryController { get; set; }
    [Inject] private NavigationManager navManager { get; set; }
    private IEnumerable<LogHistory>? LogList { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var response = await logHistoryController.List();
        if (response != null)
        {
            LogList = response;
        }
    }
}

