using System;
using System.Collections.Generic;

public enum AttributeTypes
{
    None,
    Initiator,
    Recipient,
    Build
}

public abstract class RoleTypeBase
{
    public abstract string Name { get; }
    public abstract AttributeTypes Attributes { get; }

    public abstract Role GetRole(object opt_initializer = null);
}