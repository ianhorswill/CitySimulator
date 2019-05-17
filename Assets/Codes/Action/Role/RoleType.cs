using System;
using System.Collections.Generic;
using System.Linq;

// We are using a generic type here to allow for any type in the Collection
//  -> the collection is be used in Linq queries to get individual objects.
// The T that you use must have a default constructor for build to work.
public abstract class RoleType<T> : RoleTypeBase where T : new()
{
    public abstract List<T> Collection { get; }

    public abstract Func<T, List<RoleBase>, bool> Filter { get; }

    // This standardizes the task of finding and filling in a role. Uses:
    //  - Collection as the source/target in a Linq query
    //  - Filter to check the entites (current and past)
    //  - Name to return a Role<T> with the right object
    public Role<T> GetRole(List<RoleBase> filled_roles)
    {   

        IEnumerable<T> candidates = from entity in Collection
                                    where Filter(entity, filled_roles)
                                    select entity;
        List<T> candidate_List = candidates.ToList();
        if (!candidate_List.Any())
        {
            return null;
        }
        else
        {
            // TODO: replace 0 with framework random number
            return new Role<T>(this.Name, candidate_List[0]);
        }
    }

    // Passes a new Role<T> back up to the RoleBase, and returns that role in the
    // form of a RoleBase... Uses the custom GetRole to fill in and (if set) uses:
    //  - BuildFlag to create a new object of the right type in the right collection.
    public override RoleBase GetRoleUntyped(List<RoleBase> filled_roles)
    {
        if (this.BuildFlag)
        {
            // new T() requires a type constraint... Otherwise build objects get
            // casted as object...
            T temp = new T();
            Collection.Add(temp);
            return new Role<T>(this.Name, temp);
        }
        else
        {
            return GetRole(filled_roles);
        }
    }
}