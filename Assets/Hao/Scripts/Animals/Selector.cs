using System.Collections.Generic;

public class Selector : BTNodes
{
    private List<BTNodes> children;

    public Selector(List<BTNodes> children)
    {
        this.children = children;
    }

    public override State Evaluate()
    {
        foreach (var child in children)
        {
            var result = child.Evaluate();
            if (result == State.Success || result == State.Running)
                return result;
        }

        return State.Failure;
    }

    public override void Reset()
    {
        foreach (var child in children)
            child.Reset();
    }
}
