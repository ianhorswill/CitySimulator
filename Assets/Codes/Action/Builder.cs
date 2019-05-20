using System;
using System.Collections.Generic;

public static class Builder
{
    static readonly Dictionary<Type, Func<List<RoleBase>, object>> BuilderTable =
        new Dictionary<Type, Func<List<RoleBase>, object>>();

    public static object Build(Type t, List<RoleBase> roles)
    {
        return BuilderTable[t](roles);
    }

    public static void RegisterBuilder(Type t, Func<List<RoleBase>, object> build_function)
    {
        BuilderTable[t] = build_function;
    }
}
