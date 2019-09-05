using UnityEngine;

public class RangedAttackState : IState
{
    private Enemy parent;
	//private LayerMask collidable = LayerMask.NameToLayer("Collidable");
	private int collidable = 1 << 8;


    public void Enter(Enemy parent){
        Debug.Log("Entered ranged attack state");
        this.parent = parent;
    }

    public void Exit(){
		
    }

    public void Update(){

        //lock target and shoot at player
    }
}