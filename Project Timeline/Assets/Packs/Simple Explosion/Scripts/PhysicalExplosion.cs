using UnityEngine;
using System.Collections;
 
public class PhysicalExplosion : MonoBehaviour 
{
    public float timer = 0.5f;
    public ObjectTimeLine objectTimeLine;

    public float Radius;// explosion radius
    public float Force;// explosion force

    private void Start()
    {
        objectTimeLine = gameObject.GetComponent<ObjectTimeLine>();
    }

    void Update () 
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Radius);// create explosion

        if (objectTimeLine.timeManagerScript.currentTime >= timer && objectTimeLine.timeManagerScript.currentTime <= timer + 0.5f)
        {
            
            for (int i = 0; i < hitColliders.Length; i++)
            {
                
                if (hitColliders[i].GetComponent<Rigidbody>())
                {
                    hitColliders[i].GetComponent<Rigidbody>().AddExplosionForce(Force, transform.position, Radius, 0.0F); // push game object
                }


            }
            //Destroy(gameObject, 0.2f);// destroy explosion
        }
        
        
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,Radius);
    }
}