public interface IOpenable
{
    bool IsOpen { get; }
    void Open();
    void Close();
    void Toggle();
}