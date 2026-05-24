namespace SalvageCore.Interface;

public interface IRedisCacheService
{
    T? GetData<T>(string key);
    void SetData<T>(string key, T value);
    Task RemoveData(string key);
}