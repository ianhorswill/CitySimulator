using System;
using System.Collections.Generic;
using System.Linq;

// We are using a generic type here to allow for any type in the list
//  -> the list will be used in Linq queries to get individual objects.
public abstract class RoleType<T> : RoleTypeBase
{
    public abstract List<T> Collection { get; }

    public abstract Boolean Filter(T self, object opt_initializer = null);

    public IEnumerable<T> GetCandidates(object opt_initializer = null)
    {
        return from entity in Collection
               where Filter(entity, opt_initializer)
               select entity;
    }

    public override Role GetRole(object opt_initializer = null)
    {   
        // TODO: replace with framework random
        Random rnd = new Random();
        List<T> candidates = GetCandidates(opt_initializer).ToList();
        if (!candidates.Any()) { return null; }
        else { return new Role(this.Name, candidates[rnd.Next(candidates.Count)]); }
    }
}