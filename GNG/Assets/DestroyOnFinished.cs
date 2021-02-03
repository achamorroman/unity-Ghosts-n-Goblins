using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnFinished : StateMachineBehaviour
{
    /// <summary>
    /// This StateMachineBehavior destroys the parent GameObject when an animation ends. This behavior should be added to the animation in the animation controller
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Schedule destruction of gameobject after N seconds, where N == the length of the animation (in seconds)
        Destroy(animator.gameObject, stateInfo.length);
    }

}
