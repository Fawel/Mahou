using UnityEngine;
using System.Collections;

public class EnableMovement : StateMachineBehaviour {
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int laterIndex){
		animator.SetBool ("Controllable", true);
	}
}
