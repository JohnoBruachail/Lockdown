using UnityEngine;
using System.Collections;

public class BulletBehavior : MonoBehaviour
{

    public float speed = 50;
    public int damage;
    public GameObject target;
    public Vector3 startPosition;
    public Vector3 targetPosition;

    private float distance;
    private float startTime;

    // Use this for initialization
    void Start()
    {
        startTime = Time.time;
        distance = Vector2.Distance(startPosition, targetPosition);
    }

    // Update is called once per frame
    void Update()
    {
        float timeInterval = Time.time - startTime;
        gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, timeInterval * speed / distance);

/* 
        if (gameObject.transform.position.Equals(targetPosition))
        {
            if (target != null)
            {
                Destroy(gameObject);
            }
            Destroy(gameObject);
        }
*/
    }

}
