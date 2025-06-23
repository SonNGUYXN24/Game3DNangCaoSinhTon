public class InvertNode : BTNodes
{
    private BTNodes child;

    public InvertNode(BTNodes child)
    {
        this.child = child;
    }

    public override State Evaluate()
    {
        State result = child.Evaluate();
        if (result == State.Success) return State.Failure;
        if (result == State.Failure) return State.Success;
        return State.Running;
    }
}
