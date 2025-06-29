using Application.Services;
using Microsoft.JSInterop;

namespace Infrastructure.Services;

public sealed class CookieService(IJSRuntime js) : ICookieService
{
    public async Task<string> GetCookieAsync(string key, CancellationToken ct = default) =>
        await js.InvokeAsync<string>("window.getCookie", ct, key);

    public async Task SetCookieAsync(string key, string value, CancellationToken ct = default) =>
        await js.InvokeVoidAsync("window.setCookie", ct, key, value);

    public async Task DeleteCookieAsync(string key, CancellationToken ct = default) =>
        await js.InvokeVoidAsync("window.delCookie", ct, key);
}