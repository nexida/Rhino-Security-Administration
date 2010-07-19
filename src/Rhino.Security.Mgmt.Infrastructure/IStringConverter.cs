namespace Rhino.Security.Mgmt.Infrastructure
{
    public interface IStringConverter<T>
    {
        string ToString(T obj);
        T FromString(string str);
    }
}