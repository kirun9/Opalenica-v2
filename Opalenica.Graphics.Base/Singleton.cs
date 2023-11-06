namespace Opalenica.Graphic.Base;

public class Singleton<T> : object where T : new()
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            return _instance is not null ? _instance : new T();
        }
        set
        {
            _instance = value;
        }
    }

    static Singleton()
    {
        _instance = new T();
    }

    public Singleton()
    {
    }
}