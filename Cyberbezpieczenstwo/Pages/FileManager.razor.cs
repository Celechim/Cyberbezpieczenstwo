using System;
using Cyberbezpieczenstwo.API.Controllers;
using Cyberbezpieczenstwo.API.Models;
using Cyberbezpieczenstwo.Data.Services;
using Cyberbezpieczenstwo.API.Modelsp;
using Cyberbezpieczenstwo.Authentication;
using Cyberbezpieczenstwo.Data.Interfaces;
using Cyberbezpieczenstwo.Data.Models;
using Cyberbezpieczenstwo.Data.Specifications;
using Cyberbezpieczenstwo.Shared;
using Cyberbezpieczenstwo.SharedKernel.Enums;
using Cyberbezpieczenstwo.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.ComponentModel;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Cyberbezpieczenstwo.Pages;

public partial class FileManager
{
	[Inject] private IUserService _userService { get; set; }
	[Inject] private AuthenticationStateProvider authStateProvider { get; set; }
	[Inject] private NavigationManager navManager { get; set; }
	[Inject] private IJSRuntime js { get; set; }
	[Inject] private UserController _userController { get; set; }
    [Inject] private PasswordLimitationController _limitationController { get; set; }
    [Inject] private LogHistoryController _logHistoryController { get; set; }
    [Inject] private SecuritySettingsController _settingsController { get; set; }
    [Inject] private GooglereCaptchaService _googlereCaptchaService { get; set; }
    [Inject] private UserController userController { get; set; }
    [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
    [Parameter] public int Id { get; set; }
    private UserDTO User { get; set; }
    private string View { get; set; } = "details";
    private bool showInvalidPasswords { get; set; } = false;
    private bool showPasswordAlreadyUsed { get; set; } = false;
    private UserDTO? userToUpdate { get; set; }
    private string passwordToConfirm { get; set; }
    private int passwordExpiresIn { get; set; }
    private LoginValidator _loginValidator;
    private AuthenticationState authState { get; set; }
    private List<IBrowserFile> loadedFiles { get; set; } = new();
    public long maxFileSize { get; set; } = 1024 * 5000;
    private bool isLoading { get; set; }
    private bool generateError { get; set; } = false;
    private bool filesLoaded { get; set; } = true;
    FileInfo[] fileEntries { get; set; }
    private bool demowere { get; set; }
    private string licenseKey { get; set; }
    private string errorMessage { get; set; } = String.Empty;

    protected override async Task OnInitializedAsync()
	{

        authState = await authenticationState;
        User = await userController.GetByLogin(authState.User.Identity?.Name);
        demowere = await userController.CheckLicense(User.Login);
    }

    

    private async Task LoadFiles(InputFileChangeEventArgs e)
    {
        demowere = await userController.CheckLicense(User.Login);
        if (demowere)
            maxFileSize = 1024 * 100;
        else
            maxFileSize = 1024 * 5000;
        isLoading = true;
        loadedFiles.Clear();

        foreach (var file in e.GetMultipleFiles(e.FileCount))
        {
            if (file.Size >= maxFileSize)
            {
                generateError = true;
                return;
            }
            generateError = false;

            loadedFiles.Add(file);

            var trustedFileNameForFileStorage = Path.GetRandomFileName();
            var path = Path.Combine(Directory.GetCurrentDirectory(),
            "Development", "unsafe_uploads", file.Name);

            await using FileStream fs = new(path, FileMode.Create);
            await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
        }

        isLoading = false;
    }
    private async Task GetFiles()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(),
        "Development", "unsafe_uploads");
        DirectoryInfo d = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(),
        "Development", "unsafe_uploads"));
        fileEntries = d.GetFiles("*.*");
        filesLoaded = false;
    }
    private async Task UpdateLicense()
    {
        if (licenseKey == null)
        {
            errorMessage = "Enter License Code First!";
            return;
        }

        string output = string.Empty;

        foreach (char ch in User.Login)
            output += cipher(ch, 3);
        output.ToLower();
        licenseKey.ToLower();
        if (output == licenseKey)
        {
            await userController.UpdateLicense(User.Login);
            demowere = false;
            maxFileSize = 1024 * 5000;
        }
        else
        {
            errorMessage = "Wrong License Code!";
        }

    }
    public static char cipher(char ch, int key)
    {
        if (!char.IsLetter(ch))
        {

            return ch;
        }

        char d = char.IsUpper(ch) ? 'A' : 'a';
        return (char)((((ch + key) - d) % 26) + d);
    }
    public static string Decipher(string input, int key)
    {
        return cipher(input, 26 - key);
    }
}

