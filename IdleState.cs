using UnityEngine;

public class IdleState : IState
{
    private Enemy parent;
	//private LayerMask collidable = LayerMask.NameToLayer("Collidable");
	private int collidable = 1 << 8;


    public void Enter(Enemy parent){
        this.parent = parent;
    }


    public void Exit(){


    }

    public void Update(){
		
        parent.transform.Translate(Vector2.right * parent.speed * Time.deltaTime);

		//checks ahead of the character to see if the platform ends
		if(parent.movingRight){
			parent.wallCollisionInfo = Physics2D.Raycast(parent.wallDetector.position, Vector2.right, 0.2f, collidable);
			Debug.DrawRay(parent.wallDetector.position, Vector2.right * 0.2f, Color.green);
		}else{
			parent.wallCollisionInfo = Physics2D.Raycast(parent.wallDetector.position, Vector2.left, 0.2f, collidable);
			Debug.DrawRay(parent.wallDetector.position, Vector2.left * 0.2f, Color.red);
		}
		parent.groundCollisionInfo = Physics2D.Raycast(parent.groundDetector.position, Vector2.down, 1f, collidable);
		Debug.DrawRay(parent.groundDetector.position, Vector2.down * 1, Color.yellow);

		Debug.Log("floor collider is: " + parent.groundCollisionInfo.collider);
		Debug.Log("wall collider is: " + parent.wallCollisionInfo.collider);

		if(parent.groundCollisionInfo.collider == false || parent.wallCollisionInfo.collider == true){
			parent.movingRight = !parent.movingRight;//toggle direction
			
			if(parent.movingRight){
				parent.transform.eulerAngles = new Vector3(0, 0, 0);
			}else{
				parent.transform.eulerAngles = new Vector3(0, -180, 0);
			}
		}
    }
}