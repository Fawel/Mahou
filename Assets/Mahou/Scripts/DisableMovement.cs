using UnityEngine;
using System.Collections;

public class DisableMovement : StateMachineBehaviour {
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int laterIndex){
		animator.SetBool ("Controllable", false);
	}
}
