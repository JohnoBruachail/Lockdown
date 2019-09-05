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

public class CannonTower : MonoBehaviour
{

    public List<GameObject> enemiesInRange;
    private List<GameObject> toRemove;
    private float lastShotTime;
    private Tower tower;
    GameObject target;

    // Use this for initialization
    void Start()
    {
        enemiesInRange = new List<GameObject>();
        toRemove = new List<GameObject>();
        lastShotTime = Time.time;
        tower = gameObject.GetComponentInParent<Tower>();
    }

    // Update is called once per frame
    void Update(){
        
        if(tower.active){
            target = null;
            if(enemiesInRange.Count > 0){

                foreach (GameObject enemy in enemiesInRange){
                    
                    if(enemy.GetComponent<Enemy>().alive == false){
                        toRemove.Add(enemy);
                    }else{
                        target = enemy;
                    }
                    
                }
                //cleanup of dead enemys from the list.
                foreach (GameObject enemy in toRemove){
                    enemiesInRange.Remove(enemy);
                }

            }

            //if we have a target
            if(target != null){

                if (Time.time - lastShotTime > tower.towerTiers[tower.currentTier].fireRate){
                    Shoot(target.GetComponent<Collider2D>());
                    lastShotTime = Time.time;
                }
                
                /*
                    JOHN THIS MAY BE NEEDED IN THE EVENT THE TURRET CAN SWIVEL INORDER TO AIM AT THE TARGET ENEMY.

                    Vector3 direction = gameObject.transform.position - target.transform.position;
                    gameObject.transform.rotation = Quaternion.AngleAxis(
                    Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI,
                    new Vector3(0, 0, 1));
                */
            }
        }
        
    }

//if the enemy is destroyed remove it from the list (JOHN THERE MAY BE A BETTER WAY TO DO THIS)

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag.Equals("Enemy")){
            enemiesInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if (other.gameObject.tag.Equals("Enemy")){
            enemiesInRange.Remove(other.gameObject);
        }
    }

    private void Shoot(Collider2D target){
        GameObject bulletPrefab = tower.towerTiers[tower.currentTier].bullet;

        Vector3 startPosition = gameObject.transform.position;
        Vector3 targetPosition = target.transform.position;
        startPosition.z = bulletPrefab.transform.position.z;
        targetPosition.z = bulletPrefab.transform.position.z;

        GameObject newBullet = (GameObject) Instantiate(bulletPrefab);
        newBullet.transform.position = startPosition;
        BulletBehavior bulletComp = newBullet.GetComponent<BulletBehavior>();
        bulletComp.target = target.gameObject;
        bulletComp.startPosition = startPosition;
        bulletComp.targetPosition = targetPosition;

        //Animator animator = tower.CurrentTier.sprite.GetComponent<Animator>();
        //animator.SetTrigger("fireShot");
        //AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        //audioSource.PlayOneShot(audioSource.clip);
    }

}
