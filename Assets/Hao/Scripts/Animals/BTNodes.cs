using UnityEngine;


public abstract class BTNodes
{
    public enum State { Running, Success, Failure }
    public abstract State Evaluate();
    public virtual void Reset() { }
}
