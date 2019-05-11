using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// A component of the simulator.  Basically an object that can be ticked.
/// </summary>
public abstract class SimulatorComponent
{
    public abstract void Tick();
}