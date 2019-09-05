using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//PRO TIP
/* 
For objects that are instantiated quite often (for example, bullets in an FPS game), 
make a pre-initialized pool of them and just pick one already initialized when you 
need it and activate it. Then, instead of destroying it when it is not required anymore, 
deactivate it and return it to the pool.
*/

public class CannonMonster : MonoBehaviour
{
    public EnemyZapper parent;
    public int fireRate;
    public GameObject bulletPrefab;
    public List<GameObject> targetsInRange;
    private List<GameObject> toRemove;
    public GameObject player;
    private GameObject currentTarget;
    private float lastShotTime;

    //two lists, one of targets for buildings, the other for character targets.
    


    // Use this for initialization
    void Start()
    {
        targetsInRange = new List<GameObject>();
        toRemove = new List<GameObject>();
        lastShotTime = Time.time;
    }

    // Update is called once per frame
    void Update(){

        currentTarget = null;

/* 
        players and buildings will be added to the list of targets on collision
        after this point any buildings which arn't active will be removed from the list.
*/



        if(targetsInRange.Count > 0){

            foreach (GameObject target in targetsInRange){
                
                if(target.GetComponent<Building>().active == false){
                    toRemove.Add(target);
                }else{
                    currentTarget = target;
                }
            }
            //cleanup of inactive buildings from the list.
            foreach (GameObject target in toRemove){
                targetsInRange.Remove(target);
            }
        }

        //prioritise player?
        if(player){
            currentTarget = player;
        }

        //if we have a target
        if(currentTarget != null){

            if (Time.time - lastShotTime > fireRate){
                Shoot(currentTarget.GetComponent<Collider2D>());
                lastShotTime = Time.time;
            }
        }
    }

//if the enemy is destroyed remove it from the list (JOHN THERE MAY BE A BETTER WAY TO DO THIS)

    void OnTriggerEnter2D(Collider2D other){

        if(other.gameObject.tag.Equals("Player")){
            player = other.gameObject;
            parent.ChangeState(new RangedAttackState());
        }
        else if(other.gameObject.name.Equals("BuildingTower")){

            if(other.gameObject.GetComponent<Building>().active == true){
                targetsInRange.Add(other.gameObject);
                parent.ChangeState(new RangedAttackState());
            }
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag.Equals("Player")){
            player = null;

            if(targetsInRange.Count == 0){
                parent.ChangeState(new IdleState());
            }
        }
        if(other.gameObject.name.Equals("BuildingTower")){
            targetsInRange.Remove(other.gameObject);

            if(targetsInRange.Count == 0){
                parent.ChangeState(new IdleState());
            }
        }
    }

    private void Shoot(Collider2D currentTarget){

        Vector3 startPosition   = gameObject.transform.position;
        Vector3 targetPosition  = currentTarget.transform.position;
        startPosition.z         = bulletPrefab.transform.position.z;
        targetPosition.z        = bulletPrefab.transform.position.z;

        GameObject newBullet            = (GameObject) Instantiate(bulletPrefab);
        newBullet.transform.position    = startPosition;
        BulletBehavior bulletComp       = newBullet.GetComponent<BulletBehavior>();
        bulletComp.target               = currentTarget.gameObject;
        bulletComp.startPosition        = startPosition;
        bulletComp.targetPosition       = targetPosition;

        //Animator animator = tower.CurrentTier.sprite.GetComponent<Animator>();
        //animator.SetTrigger("fireShot");
        //AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        //audioSource.PlayOneShot(audioSource.clip);
    }

}
