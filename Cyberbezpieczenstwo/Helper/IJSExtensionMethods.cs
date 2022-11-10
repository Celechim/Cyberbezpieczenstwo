using Microsoft.JSInterop;

namespace Cyberbezpieczenstwo.Helper
{
    public static class IJSExtensionMethods
    {
        public static async ValueTask InitializeInactivityTimer<T>
            (this IJSRuntime js,DotNetObjectReference<T> dotNetObjectReference) where T: class
        {
            await js.InvokeVoidAsync("initilizeInactivityTimer",dotNetObjectReference);
        }
    }
}
