using System;
using System.Collections.Generic;

// We are only using a generic type here to allow for any type in the list
//  -> the list will be used in Linq queries to get individual objects.
public abstract class RoleType<T> : RoleTypeBase
{
    public abstract List<T> list { get; }

    public abstract Boolean attribute(T self, params object[] args);
}