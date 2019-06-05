using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a role, independent of the value it's bound to in a particular action.
/// Contains the information needed to choose objects to fill the role.
/// We are using a generic type here to allow for any type in the Collection,
/// the collection is be used in Linq queries to get individual objects.
/// The T that you use needs to use RegisterBuilder(Type, Func<Action, object>)
/// to have the build code (and flag) to work.
/// </summary>
public class RoleType<T> : RoleTypeBase
{
    /// <summary>
    /// Procedure to bind role directly to values, bypasses the regular list filtering
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
    /// Make n object representing a new kind of role that selects bindings using the specified collection
    /// </summary>
    /// <param name="name">Name of the role</param>
    /// <param name="collection">Set of objects to which the action might potentially be bound</param>
    public RoleType(string name, List<T> collection)
        : this(name, collection, (t, l) => true)
    { }

    /// <summary>
    /// Make an object representing a new kind of role that selects bindings
    /// using the default collection. Bindings are selected from the default
    /// collection, which depends on the type parameter of RoleType.  For type
    /// Person, it is the set of living persons.
    /// </summary>
    /// <param name="name">Name of the role</param>
    public RoleType(string name)
        : this(name, DefaultCollection(), (t, l) => true)
    { }

    /// <summary>
    /// Make an object representing a new kind of role that selects bindings
    /// using the specified binding procedure.
    /// </summary>
    /// <param name="name">Name of the role</param>
    /// <param name="binder">Procedure to find values for this role.</param>
    public RoleType(string name, Func<Action, T> binder)
    {
        Name = name;
        this.binder = binder;
    }

    /// <summary>
    /// Type based defaults for common types that will be used to make roles
    /// so the role writing process is easier
    /// </summary>
    /// <returns>The collection that will be used for this RoleType.</returns>
    private static List<T> DefaultCollection()
    {
        var t = typeof(T);
        if (t == typeof(Person))
            return PersonTown.Singleton.aliveResidents as List<T>;
        if (t == typeof(Institution))
            return InstitutionManager.institutionList as List<T>;
        if (t == typeof(ConstructionCompany))
            return InstitutionManager.constructionCompanyList as List<T>;
        if (t == typeof(Action))
            return ActionSimulator.action_history as List<T>;
        throw new InvalidOperationException($"No default collection defined for type {t.Name}");
    }

    /// <summary>
    /// Used by the FillRoleUntyped function to allow type specific role functions
    /// to be passed up to the RoleTypeBase.
    /// This standardizes the task of finding and filling in a role. Uses:
    ///  - Filter to check the entities (current and past)
    ///  - Name to return a Role<T> with the right object
    ///  - CAN use Collection as the source/target in a Linq query
    /// If binder is set, bypass the usual find and fill and use binder to set
    /// Role binding.
    /// </summary>
    /// <param name="action">Action the role will be bound to, stores alread bound
    /// roles so the filter function can access them</param>
    public Role<T> GetRole(Action action)
    {
        if (binder != null)
        {
            var value = binder(action);
            return value != null ? new Role<T>(Name, value) : null;
        }
        IEnumerable<T> candidates = from entity in collection
                                    where filter(entity, action)
                                    select entity;
        List<T> candidateList = candidates.ToList();
        if (candidateList.Any())
        {
            return new Role<T>(Name, candidateList.RandomElement());
        }
        return null;
    }

    /// <summary>
    /// Passes a new Role<T> back up to the RoleBase, and returns that role in
    /// the form of a RoleBase... Uses the custom GetRole to fill in and (if set)
    /// uses BuildFlag and Build to create a new object of the right type in the
    /// right collection.
    /// </summary>
    /// <returns>The role untyped.</returns>
    /// <param name="action">The action to be passed along to Build or GetRole (which then uses
    /// it in filtering)</param>
    public override RoleBase FillRoleUntyped(Action action)
    {
        if (BuildFlag)
        {
            Type typeParameterType = typeof(T);
            object newObject = Builder.Build(typeParameterType, action);
            if (newObject != null)
            {
                T newTypedObject = (T) newObject;
                collection.Add(newTypedObject);
                return new Role<T>(Name, newTypedObject);
            }
            return null;
        }
        return GetRole(action);
    }

    /// <summary>
    /// Fills the Role with the specified object if it passes the filter.
    /// Relies on the proper object type being passed to FillRoleWith...
    /// </summary>
    /// <returns>The filled role</returns>
    /// <param name="toFill">The object to try and fill the role with</param>
    /// <param name="action">The action being passed along to filter</param>
    public override RoleBase FillRoleWith(object toFill, Action action)
    {
        // WARNING: Not type safe
        if (filter((T)toFill, action)) { return new Role<T>(Name, (T)toFill); }
        return null;
    }
}