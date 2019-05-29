using System;
using System.Collections.Generic;
using System.Linq;
using Codes.Institution;

// We are using a generic type here to allow for any type in the Collection
//  -> the collection is be used in Linq queries to get individual objects.
// The T that you use must have a default constructor for build to work.
public class RoleType<T> : RoleTypeBase
{
    /// <summary>
    /// Procedure to bind role to values
    /// </summary>
    private readonly Func<Action, T> binder;
    /// <summary>
    /// Collection from which to draw values when binding, when binder not specified
    /// </summary>
    private readonly List<T> collection;
    /// <summary>
    /// Filter to apply to collection.
    /// </summary>
    private readonly Func<T, Action, bool> filter;

    /// <summary>
    /// Make n object representing a new kind of role that selects bindings using the specified collection and filter predicate
    /// </summary>
    /// <param name="name">Name of the role</param>
    /// <param name="collection">Set of objects to which the action might potentially be bound</param>
    /// <param name="filter">Additional predicate to use to test whether a member of collection should be considered for this role.</param>
    public RoleType(string name, List<T> collection, Func<T, Action, bool> filter)
    {
        Name = name;
        this.collection = collection;
        this.filter = filter;
    }

    /// <summary>
    /// Make an object representing a new kind of role that selects bindings using the default collection and the specified filter predicate
    /// Bindings are selected from the default collection, which depends on the type parameter of RoleType.  For type Person, it is the set
    /// of living persons.
    /// </summary>
    /// <param name="name">Name of the role</param>
    /// <param name="filter">Additional predicate to use to test whether a member of collection should be considered for this role.</param>
    public RoleType(string name, Func<T, Action, bool> filter)
        : this(name, DefaultCollection(), filter)
    { }

    /// <summary>
    /// Make an object representing a new kind of role that selects bindings using the default collection
    /// Bindings are selected from the default collection, which depends on the type parameter of RoleType.  For type Person, it is the set
    /// of living persons.
    /// </summary>
    /// <param name="name">Name of the role</param>
    public RoleType(string name)
        : this(name, DefaultCollection(), (t, l) => true)
    { }

    /// <summary>
    /// Make n object representing a new kind of role that selects bindings using the specified collection
    /// </summary>
    /// <param name="name">Name of the role</param>
    /// <param name="collection">Set of objects to which the action might potentially be bound</param>
    public RoleType(string name, List<T> collection)
        : this(name, collection, (t, l) => true)
    { }

    /// <summary>
    /// Make an object representing a new kind of role that selects bindings using the specified binding procedure.
    /// </summary>
    /// <param name="name">Name of the role</param>
    /// <param name="binder">Procedure to find values for this role.</param>
    public RoleType(string name, Func<Action, T> binder)
    {
        Name = name;
        this.binder = binder;
    }

    private static List<T> DefaultCollection()
    {
        var t = typeof(T);
        if (t == typeof(Person))
            return ForceToT(PersonTown.Singleton.aliveResidents);
        if (t == typeof(Institution))
            return ForceToT(InstitutionManager.institutionList);
        throw new InvalidOperationException($"No default collection defined for type {t.Name}");
    }

    private static List<T> ForceToT(object collection)
    {
        return (List<T>)collection;
    }

    // This standardizes the task of finding and filling in a role. Uses:
    //  - Filter to check the entities (current and past)
    //  - Name to return a Role<T> with the right object
    //  - CAN use Collection as the source/target in a Linq query
    public Role<T> GetRole(Action a)
    {
        if (binder != null)
        {
            var value = binder(a);
            return value != null ? new Role<T>(Name, value) : null;
        }
        IEnumerable<T> candidates = from entity in collection
                                    where filter(entity, a)
                                    select entity;
        List<T> candidateList = candidates.ToList();
        if (candidateList.Any())
        {
            return new Role<T>(Name, candidateList.RandomElement());
        }
        return null;
    }

    // Passes a new Role<T> back up to the RoleBase, and returns that role in the
    // form of a RoleBase... Uses the custom GetRole to fill in and (if set) uses:
    //  - BuildFlag to create a new object of the right type in the right collection.
    public override RoleBase FillRoleUntyped(Action a)
    {
        if (BuildFlag)
        {
            Type typeParameterType = typeof(T);
            object newObject = Builder.Build(typeParameterType, a);
            if (newObject != null)
            {
                T newTypedObject = (T) newObject;
                collection.Add(newTypedObject);
                return new Role<T>(Name, newTypedObject);
            }
            return null;
        }
        return GetRole(a);
    }

    // Relies on the proper object type being passed to FillRoleWith...
    // NOT TYPE SAFE
    public override RoleBase FillRoleWith(object desiredValue, Action a)
    {
        if (filter((T) desiredValue, a))
        {
            return new Role<T>(Name, (T)desiredValue);
        }
        return null;
    }
}