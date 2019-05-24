using System;
using System.Collections.Generic;
using System.Linq;

// We are using a generic type here to allow for any type in the Collection
//  -> the collection is be used in Linq queries to get individual objects.
// The T that you use must have a default constructor for build to work.
public class RoleType<T> : RoleTypeBase
{
    public new string Name;
    public List<T> Collection;

    public Func<T, List<RoleBase>, bool> Filter;

    public RoleType(string name, List<T> collection, Func<T, List<RoleBase>, bool> filter)
    {
        Name = name;
        Collection = collection;
        Filter = filter;
    }

    public RoleType(string name, Func<T, List<RoleBase>, bool> filter)
        : this(name, DefaultCollection(), filter)
    { }

    public RoleType(string name)
        : this(name, DefaultCollection(), (t, l) => true)
    { }

    private static List<T> DefaultCollection()
    {
        var t = typeof(T);
        if (t == typeof(Person))
            return ForceToT(PersonTown.Singleton.aliveResidents);
        throw new InvalidOperationException($"No default collection defined for type {t.Name}");
    }

    private static List<T> ForceToT(object collection)
    {
        return (List<T>)collection;
    }

    // This standardizes the task of finding and filling in a role. Uses:
    //  - Filter to check the entites (current and past)
    //  - Name to return a Role<T> with the right object
    //  - CAN use Collection as the source/target in a Linq query
    public Role<T> GetRole(List<RoleBase> filled_roles)
    {
        IEnumerable<T> candidates = from entity in Collection
                                    where Filter(entity, filled_roles)
                                    select entity;
        List<T> candidate_List = candidates.ToList();
        if (candidate_List.Any())
        {
            return new Role<T>(this.Name, candidate_List.RandomElement());
        }
        return null;
    }

    // Passes a new Role<T> back up to the RoleBase, and returns that role in the
    // form of a RoleBase... Uses the custom GetRole to fill in and (if set) uses:
    //  - BuildFlag to create a new object of the right type in the right collection.
    public override RoleBase FillRoleUntyped(List<RoleBase> roleBindings)
    {
        if (this.BuildFlag)
        {
            Type typeParameterType = typeof(T);
            object newObject = Builder.Build(typeParameterType, roleBindings);
            if (newObject != null)
            {
                T newTypedObject = (T) newObject;
                Collection.Add(newTypedObject);
                return new Role<T>(this.Name, newTypedObject);
            }
            return null;
        }
        return GetRole(roleBindings);
    }

    // Relies on the proper object type being passed to FillRoleWith...
    // NOT TYPESAFE
    public override RoleBase FillRoleWith(object desiredValue, List<RoleBase> roleBindings)
    {
        if (Filter((T) desiredValue, roleBindings))
        {
            return new Role<T>(this.Name, (T)desiredValue);
        }
        return null;
    }
}