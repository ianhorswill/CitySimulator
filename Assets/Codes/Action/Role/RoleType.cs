using System;
using System.Collections.Generic;

public abstract class RoleType<T> : RoleTypeBase
{
    public abstract List<T> list { get; }

    public abstract Boolean attribute(T self, params object[] args);

    public Type getType()
    {
        return typeof(T);
    }
}