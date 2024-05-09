using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float speed;
    Rigidbody2D rb;
    [SerializeField] Vector2 boundaries;
    public Vector2 direction = Vector2.zero;
    [SerializeField] Transform canonPosition;
    GameManager gm;
    [SerializeField] GameObject explosionPrefab;

    float fireCounter = 0;
    [SerializeField] float fireFrequency = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gm = FindObjectOfType<GameManager>();
        gm.UpdateShields(numShields);
    }

    // Update is called once per frame
    void Update()
    {

        PlayerInputs();
        TestBoundaries();


     
    }

    int numShields = 3;
    public int NumShields
    {
        get
        {
            return numShields;
        }
        set
        {
            numShields = value;
            if (numShields > 3)
            {
                numShields = 3;
            }
            if (numShields < 0)
            {
                numShields = 0;
            }
            gm.UpdateShields(numShields);
            
        }
    }

    void TestBoundaries()
    {   // Droite
        if (transform.position.x > boundaries.x)
        {
            transform.position = new Vector2(boundaries.x, transform.position.y);
        }
        // Gauche
        if (transform.position.x < -boundaries.x)
        {
            transform.position = new Vector2(-boundaries.x, transform.position.y);
        }
        // Haut
        if (transform.position.y > boundaries.y)
        {
            transform.position = new Vector2(transform.position.x, boundaries.y);
        }
        if (transform.position.y < -boundaries.y)
        {
            transform.position = new Vector2(transform.position.x, -boundaries.y);
        }



    }

    private void FixedUpdate()
    {
        rb.velocity = direction.normalized * speed;
    }

    private void PlayerInputs()
    {
        direction = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction += Vector2.right;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction += Vector2.up;
        }

        Debug.DrawRay(transform.position, direction.normalized);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
            fireCounter = 0;
        }

        if (Input.GetKey(KeyCode.Space))
        {

            fireCounter += Time.deltaTime;
            if (fireCounter >= fireFrequency){
                
                fireCounter -= fireFrequency;
                Shoot();


            }

            
        }
    }

    void Shoot()
    {
        GameObject p = Instantiate(projectilePrefab);
        p.transform.position = canonPosition.position;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger");
        if (collision.CompareTag("ProjectileEnemy"))
        {

            collision.GetComponent<Projectiles>().Impact();
            Hit();
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag( "Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().Explode();

            Hit();
        }
    }

    void Hit()
    {
        if (NumShields > 0)
        {
            NumShields--;



        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        GameObject e = Instantiate(explosionPrefab);
        e.transform.position = transform.position;

        // Destruction du joueur
        Destroy(gameObject);
    }
}
