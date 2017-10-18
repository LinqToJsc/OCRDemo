namespace TDF.Core.Configuration
{
    public interface ISettings
    {
        string ConfigMode { get; }

        string GetValue(string key);

        void SetValue(string key, string value);
    }
}
