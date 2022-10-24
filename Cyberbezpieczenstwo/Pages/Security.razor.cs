using System;
using Cyberbezpieczenstwo.API.Controllers;
using Cyberbezpieczenstwo.API.Modelsp;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cyberbezpieczenstwo.Pages;

public partial class Security
{
    [Inject] private PasswordLimitationController _limitationController { get; set; }
    [Inject] private IJSRuntime js { get; set; }
    private IEnumerable<PasswordLimitationDTO> Limitations { get; set; }

    private class PasswordLimitations
    {
        public bool First { get; set; }
        public bool Second { get; set; }
        public bool Third { get; set; }
    }

    private PasswordLimitations Model = new PasswordLimitations();

    protected override async Task OnInitializedAsync()
    {
        Limitations = await _limitationController.List();
        Model.First = Limitations.Where(x => x.Id == 1).Single().IsActive;
        Model.Second = Limitations.Where(x => x.Id == 2).Single().IsActive;
        Model.Third = Limitations.Where(x => x.Id == 3).Single().IsActive;
    }

    private async Task Submit()
    {
        var firstLimitation = new PasswordLimitationDTO(1, "", Model.First);
        var secondLimitation = new PasswordLimitationDTO(2, "", Model.Second);
        var thirdLimitation = new PasswordLimitationDTO(3, "", Model.Third);
        await _limitationController.Update(firstLimitation);
        await _limitationController.Update(secondLimitation);
        await _limitationController.Update(thirdLimitation);

        await js.InvokeVoidAsync("alert", "Zapisano ustawienia");
        return;
    }
}

