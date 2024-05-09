using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] Vector3 rotSpeed;
    [SerializeField] int speed;
    public Vector2 direction = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotSpeed * Time.deltaTime);
    }

    void OnExplode()
    {
        Debug.Log("Meteor détruit");
        GetComponent<Enemy>().ShootToDirection(1,1);
        GetComponent<Enemy>().ShootToDirection(-1,-1);
        GetComponent<Enemy>().ShootToDirection(1,-1);
        GetComponent<Enemy>().ShootToDirection(-1,1);
    }
}
