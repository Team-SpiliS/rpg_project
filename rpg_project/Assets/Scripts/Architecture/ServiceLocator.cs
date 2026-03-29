using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

    public static void Register<T>(T service)
    {
        var type = typeof(T);
        if (!_services.ContainsKey(type))
        {
            _services[type] = service;
        }
        else
        {
            throw new Exception($"Сервис {type.Name} уже зарегистрирован!");
        }
    }

    public static T Get<T>()
    {
        var type = typeof(T);
        if (_services.TryGetValue(type, out object service))
        {
            return (T)service;
        }
        throw new Exception($"Сервис {type.Name} не найден в ServiceLocator!");
    }

    public static void Clear() => _services.Clear();
}