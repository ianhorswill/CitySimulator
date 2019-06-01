using System;
using System.Collections.Generic;

/// <summary>
/// Builder class, uses a table to lookup registered build functions based on type
/// </summary>
public static class Builder
{
    /// <summary>
    /// Quick access table of build functions based on type.
    /// </summary>
    static readonly Dictionary<Type, Func<Action, object>> BuilderTable =
        new Dictionary<Type, Func<Action, object>>();

    /// <summary>
    /// Build the specified type given the action (to pass through to filtering).
    /// </summary>
    /// <returns>The built object</returns>
    /// <param name="t">Type to lookup in build table</param>
    /// <param name="a">The action to pass through to filtering</param>
    public static object Build(Type t, Action a)
    {
        return BuilderTable[t](a);
    }

    /// <summary>
    /// Registers the builder.
    /// </summary>
    /// <param name="t">Type to use as the key in our build dictionary</param>
    /// <param name="buildFunction">Build function that returns an object
    /// of type T (with action passed through to filtering.</param>
    public static void RegisterBuilder(Type t, Func<Action, object> buildFunction)
    {
        BuilderTable[t] = buildFunction;
    }
}