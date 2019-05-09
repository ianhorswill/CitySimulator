using System;
using System.Collections.Generic;

public abstract class RoleType<T>
{
    public abstract string name { get; }
    public abstract string tag { get; }
    public abstract List<T> list { get; }

    public abstract Boolean attribute(object self, params object[] args);

    public Type getType()
    {
        return typeof(T);
    }
}