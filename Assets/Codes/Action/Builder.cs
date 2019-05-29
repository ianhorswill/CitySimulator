using System;
using System.Collections.Generic;

public static class Builder
{
    static readonly Dictionary<Type, Func<Action, object>> BuilderTable =
        new Dictionary<Type, Func<Action, object>>();

    public static object Build(Type t, Action a)
    {
        return BuilderTable[t](a);
    }

    public static void RegisterBuilder(Type t, Func<Action, object> buildFunction)
    {
        BuilderTable[t] = buildFunction;
    }
}