using System;
using System.Collections.Generic;

public class RoleEmployee : RoleType<Person>
{
    public new string Name = "RoleEmployee";

    public new Func<Person, List<RoleBase>, bool> Filter => EmployeeFilter;

    public bool EmployeeFilter(Person p, List<RoleBase> role_list)
    {
        // TODO: empoyee filter
        return true;
    }
}