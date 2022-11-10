using System;
using Cyberbezpieczenstwo.API.Controllers;
using Cyberbezpieczenstwo.API.Models;
using Cyberbezpieczenstwo.API.Modelsp;
using Cyberbezpieczenstwo.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cyberbezpieczenstwo.Pages;

public partial class Security
{
    [Inject] private PasswordLimitationController _limitationController { get; set; }
    [Inject] private IJSRuntime js { get; set; }
    private IEnumerable<PasswordLimitationDTO> Limitations { get; set; }
    private List<SecuritySettings> Settings { get; set; }
    [Inject] private SecuritySettingsController _securitySettingsController { get; set; }
    [Inject] private LogHistoryController _logHistoryController { get; set; }


    private class PasswordLimitations
    {
        public bool First { get; set; }
        public bool Second { get; set; }
        public bool Third { get; set; }
    }
    private class LoginLimitation
    {
        public bool IsActive { get; set; }
        public int MaxNumber { get; set; }
    }
    private class AutoLogout
    {
        public bool IsSetAutoLogout { get; set; }
        public int SecToAutoLogout { get; set; }
    }

    private PasswordLimitations Model = new PasswordLimitations();
    private LoginLimitation Model2 = new LoginLimitation();
    private AutoLogout Model3 = new AutoLogout();

    protected override async Task OnInitializedAsync()
    {
        Limitations = await _limitationController.List();
        var settingController = await _securitySettingsController.Get();
        Settings = settingController.Value;
        Model.First = Limitations.Where(x => x.Id == 1).Single().IsActive;
        Model.Second = Limitations.Where(x => x.Id == 2).Single().IsActive;
        Model.Third = Limitations.Where(x => x.Id == 3).Single().IsActive;
        Model2.IsActive = Settings.First().IsSetLimitOfMaxFailedLoginAttemps;
        Model2.MaxNumber = Settings.First().MaxNumbersOfFailedLoginAttemps is null ? 0 : (int)Settings.First().MaxNumbersOfFailedLoginAttemps;
        Model3.IsSetAutoLogout = Settings.First().IsSetAutoLogout;
        Model3.SecToAutoLogout = Settings.First().SecToAutoLogout is null ? 0 : (int)Settings.First().SecToAutoLogout;
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
    private async Task Submit2()
    {   
        var newSec = new SecuritySettingsDTO
        {
            Id = 0,
            IsSetLimitOfMaxFailedLoginAttemps = Model2.IsActive,
            MaxNumbersOfFailedLoginAttemps = Model2.MaxNumber
        };

        await _securitySettingsController.Update(newSec);
        await js.InvokeVoidAsync("alert", "Zapisano ustawienia");
        return;
    }
    private async Task Submit3()
    {   
        var newSec = new SecuritySettingsDTO
        {
            Id = 0,
            IsSetAutoLogout = Model3.IsSetAutoLogout,
            SecToAutoLogout = Model3.SecToAutoLogout
        };

        await _securitySettingsController.Update(newSec);
        await js.InvokeVoidAsync("alert", "Zapisano ustawienia automatycznego wylogowania");
        return;
    }
    public async Task SendLog(string login, string desc, bool status)
    {
        var dto = new LogHistoryDTO();
        dto.UserName = login;
        dto.Desc = desc;
        dto.Status = status;
        await _logHistoryController.Post(dto);
    }
}

