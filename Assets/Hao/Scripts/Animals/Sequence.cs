using System.Collections.Generic;

public class Sequence : BTNodes
{
    private List<BTNodes> children;
    private int currentNode = 0;

    public Sequence(List<BTNodes> children)
    {
        this.children = children;
    }

    public override State Evaluate()
    {
        while (currentNode < children.Count)
        {
            State childState = children[currentNode].Evaluate();

            switch (childState)
            {
                case State.Running:
                    return State.Running;

                case State.Failure:
                    currentNode = 0; // Reset về node đầu nếu thất bại
                    return State.Failure;

                case State.Success:
                    currentNode++;
                    break;
            }
        }

        // Tất cả node đều thành công
        currentNode = 0;
        return State.Success;
    }

    public override void Reset()
    {
        currentNode = 0;
        foreach (BTNodes node in children)
        {
            node.Reset();
        }
    }
}
