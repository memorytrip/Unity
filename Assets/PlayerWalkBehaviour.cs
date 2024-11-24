using UnityEngine;

public class PlayerWalkBehaviour : StateMachineBehaviour
{   
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EventManager.Instance.OnStartedWalking();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EventManager.Instance.OnStoppedWalking();
    }
}
