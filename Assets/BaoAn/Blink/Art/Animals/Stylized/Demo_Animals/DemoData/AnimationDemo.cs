using UnityEngine;

public abstract class BTNode
{
    public enum State { Running, Success, Failure }
    public abstract State Evaluate();
}
