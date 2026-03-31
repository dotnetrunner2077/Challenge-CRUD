using Microsoft.JSInterop;
using TestCrudWeb.Enum;
using System.Text.Json;
using TestCrudWeb.Helpers;


namespace TestCrudWeb.SO
{
    public interface ISessionslStorage
    {
        Task<T> GetValue<T>(ValuesKeys key);
        Task SetValue<T>(ValuesKeys key, T value);
        Task RemoveItem(ValuesKeys key);
        Task ClearALL();          
    }

    public class SessionsStorage : ISessionslStorage
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly string _TypeStorage = "sessionStorage.";
        public SessionsStorage(IJSRuntime jSRuntime)
        {
            _jsRuntime = jSRuntime; 
        }
        public async Task ClearALL()
        {
            await _jsRuntime.InvokeVoidAsync($"{_TypeStorage}clear").ConfigureAwait(false);
        }

        public async Task<T> GetValue<T>(ValuesKeys key)
        {
            string data = await _jsRuntime.InvokeAsync<string>($"{_TypeStorage}getItem", key.ToString()).ConfigureAwait(false);
            return StorageHelpers.Check<T>(data);
        }

        public async Task RemoveItem(ValuesKeys key)
        {

            await _jsRuntime.InvokeVoidAsync($"{_TypeStorage}removeItem", key.ToString()).ConfigureAwait(false);
        }

        public async Task SetValue<T>(ValuesKeys key, T value)
        {
            await _jsRuntime.InvokeVoidAsync($"{_TypeStorage}setItem", key.ToString(), JsonSerializer.Serialize(value)).ConfigureAwait(false);
        }
    }
}
