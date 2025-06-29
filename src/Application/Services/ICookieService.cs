namespace Application.Services;

public interface ICookieService
{
    public Task<string> GetCookieAsync(string key, CancellationToken ct = default);

    public Task SetCookieAsync(string key, string value, CancellationToken ct = default);

    public Task DeleteCookieAsync(string key, CancellationToken ct = default);
}