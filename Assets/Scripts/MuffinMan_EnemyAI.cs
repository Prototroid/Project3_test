using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MuffinMan_EnemyAI : MovingObject
{
    private Transform player;               // Reference to the player's position.
    private Transform enemy;
    public float speed = 0.1f;
    private Rigidbody2D myEnemy;

    //PlayerHealth playerHealth;      // Reference to the player's health.
    //EnemyHealth enemyHealth;        // Reference to this enemy's health.
    NavMeshAgent nav;               // Reference to the nav mesh agent.

    private Animator animator;
    Vector3 current;
    Vector3 target;
    Vector3 dest;

    private Ray2D inRange;
    //RaycastHit2D hitPlayer;

    private bool skipMove;

    // Use this for initialization
    protected override void Start ()
    {
        //GameManager.instance.AddEnemyToList(this);

        animator = GetComponent<Animator>();
        //Debug.Log(animator);

        myEnemy = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;

        //nav = GetComponent<NavMeshAgent>();
        //dest = nav.destination;

        base.Start();
    }

    void FixedUpdate ()
    {
        current = enemy.position;
        target = player.position;
        MoveEnemy();

    }

    void Awake()
    {
        // Set up the references.
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        //enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        //playerHealth = player.GetComponent<PlayerHealth>();
        //enemyHealth = GetComponent<EnemyHealth>();
        //nav = GetComponent<NavMeshAgent>();
    }


    protected override void AttemptMove<T>(float xDir, float yDir)
    {
        Debug.Log(Vector3.Distance(current, target));

        if (Vector3.Distance(current, target) > 1.5f)
        {
            if(Mathf.Abs(xDir) > Mathf.Abs(yDir))
            {
                float xOffset = target.x < current.x ? 1.5f : -1.5f;
                base.AttemptMove<T>(xDir + xOffset, yDir);
                myEnemy.AddForce(new Vector2(speed,speed));
                //base.AttemptMove<T>(Mathf.Abs(target.x - 1), Mathf.Abs(target.y - 1));
            }
            else
            {
                float yOffset = target.y < current.y ? 1.0f : -1.0f;
                base.AttemptMove<T>(xDir, yDir + yOffset);
                myEnemy.AddForce(new Vector2(speed, speed));
            }

            skipMove = false;
            return;
            
        }
        //else if (Vector3.Distance(current, target) == 2.5f)
        //{
        //    base.AttemptMove<T>(Mathf.Abs(target.x - 1), Mathf.Abs(target.y - 1));
        //    skipMove = false;
        //    return;
        //}

        //Call the AttemptMove function from MovingObject.
        //base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }

    public void MoveEnemy()
    {
        float xPos = 0.0f;
        float yPos = 0.0f;

        float deltaX = target.x - current.x;
        float deltaY = target.y - current.y;

        if (current.x > (target.x + 1) && (Math.Abs(deltaX) > Math.Abs(deltaY)))
        {
            //animator.SetTrigger("MuffinManLeft");
            animator.Play("MuffinManLeft");
        }
        else if (current.x < (target.x - 1) && (Math.Abs(deltaX) > Math.Abs(deltaY)))
        {
            //animator.SetTrigger("MuffinManRight");
            animator.Play("MuffinManRight");
        }
        else if (current.y < target.y && (Math.Abs(deltaX) < Math.Abs(deltaY)))
        {
            //animator.SetTrigger("MuffinManUp");
            animator.Play("MuffinManUp");
        }
        else if (current.y > target.y && (Math.Abs(deltaX) < Math.Abs(deltaY)))
        {
            //animator.SetTrigger("MuffinManDown");
            animator.Play("MuffinManDown");
        }
        else
        {
            animator.SetTrigger("MuffinManIdle");
            //animator.Play("Player1Idle");
        }

        /*if (Mathf.Abs(deltaX) < float.Epsilon)
        {
            yPos = target.y > current.y ? deltaY : -deltaY;
        }
        else
        {
            xPos = target.x > current.x ? deltaX : -deltaX;
        }*/

        //if (Mathf.Abs(deltaX) < float.Epsilon)
        //{
        //yPos = target.y > current.y ? deltaY : -deltaY;
        //xPos = target.x > current.x ? deltaX : -deltaX;
        yPos = deltaY;
            xPos = deltaX;
        //}

        AttemptMove<Player1Controller>(xPos, yPos);
    }

    protected override void OnCantMove<T>(T component)
    {
        //Declare hitPlayer and set it to equal the encountered component.
        Player1Controller hitPlayer = component as Player1Controller;

        //Call the LoseFood function of hitPlayer passing it playerDamage, the amount of foodpoints to be subtracted.
        //hitPlayer.LoseFood(playerDamage);

        //Set the attack trigger of animator to trigger Enemy attack animation.
        animator.SetTrigger("MuffinManAttack_Left");
        //if (hitPlayer.collider.tag == "Player") { 
        //animator.Play("MuffinManAttack_Left");
        /// Attack Left ////
        /*if ((current.x >= target.x))
        {
            animator.Play("MuffinManAttack_Left");
        }
        //// Attack Right ////
        else if ((current.x <= target.x))
        {
            animator.SetTrigger("MuffinManAttack_Right");
            //Debug.Log(isAttackingRight);
        }
            /// Attack Up ////
        else if ((current.y < 0) & (current.y == target.y))
        {
            animator.Play("MuffinManAttack_Up");
            //Debug.Log(isAttackingUp);
        }
            /// Attack Down ////
        else if ((current.y > 0) & (current.y == target.y))
        {
            animator.Play("MuffinManAttack_Down");
            //Debug.Log(isAttackingDown);
        }

       // }*/
    }

    /*
    void FixedUpdate ()
    {
        current = enemy.position;
        //Debug.Log(current);

        target = player.position;
        //Debug.Log(target);

        float deltaX = current.x - target.x;
        Debug.Log("deltaX : " + deltaX.ToString("0.0000"));

        float deltaY = current.y - target.y;
        Debug.Log("deltaY : " + deltaY.ToString("0.0000"));

        Vector2 move = new Vector2(deltaX, deltaY);

        //Vector2 velocity = new Vector2((current.x - target.x) * speed, (current.y - target.y) * speed);
        //myEnemy.AddForce(-velocity); // - myEnemy.velocity);

        

        inRange.origin = current;
        inRange.direction = target;

        //Vector3 fwdDir = transform.TransformDirection(Vector3.forward);

        // If the enemy and the player have health left...
        //if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        //{
        // ... set the destination of the nav mesh agent to the player.

        /*if (Vector3.Distance(current, target) > 0.5f)
        {
            //nav.SetDestination(target);
            //dest = target;
            //nav.destination = dest;
            //transform.Translate(new Vector3(-(current.x - target.x) * speed, (current.y - target.y) * speed, 0));
            Vector2 loc = new Vector2(deltaX, deltaY);
            if (move == loc)
            {
                //myEnemy.AddForce(-myEnemy.velocity);
            }
            else
            {
                //myEnemy.AddForce(move * speed);
                //transform.Translate(move);
            }

            transform.Translate(move * speed - myEnemy.velocity);
        }
        else if (Vector3.Distance(current, target) == 0.5f)
        {
            myEnemy.AddForce(-(move * speed - myEnemy.velocity));
        }
        else
        {
            myEnemy.AddForce(-myEnemy.velocity);
        }*/

        /// Attack Left ////
        //if ((current.x > 0) & (current.x == target.x))
        //if (Physics2D.Raycast(inRange.origin, inRange.direction, Vector3.Distance(current, target)))
        //{

        /*RaycastHit2D hitPlayer = Physics2D.Raycast(inRange.origin, inRange.direction);//, Vector3.Distance(current, target));
        if (hitPlayer.collider != null)
        {
            //dest = target;
            //nav.destination = dest;
            transform.Translate(new Vector2(target.x, target.y) * speed - myEnemy.velocity);
        }
            

            //Debug.DrawLine(inRange.origin, hitPlayer.point, Color.red);
            //if (hitPlayer.collider.tag == "Player")
            //{
                //animator.Play("MuffinManAttack_Left");
            //}
            
            //Debug.Log(isAttackingLeft);
        //}
        /// Attack Right ////
        if ((current.x < 0) & (current.x == target.x))
        {
            animator.Play("MuffinManAttack_Right");
            //Debug.Log(isAttackingRight);
        }
        /// Attack Up ////
        else if ((current.y < 0) & (current.y == target.y))
        {
            animator.Play("MuffinManAttack_Up");
            //Debug.Log(isAttackingUp);
        }
        /// Attack Down ////
        else if ((current.y > 0) & (current.y == target.y))
        {
            animator.Play("MuffinManAttack_Down");
            //Debug.Log(isAttackingDown);
        }
        else if (current.x > target.x)// && (Math.Abs(move.x) > Math.Abs(move.y)))
        {
            //animator.SetTrigger("MuffinManLeft");
            animator.Play("MuffinManLeft");
        }
        else if (current.x < target.x)// && (move.x > Math.Abs(move.y)))
        {
            //animator.SetTrigger("MuffinManRight");
            animator.Play("MuffinManRight");
        }
        else if (current.y < target.y)// && (move.y > Math.Abs(move.x)))
        {
            //animator.SetTrigger("MuffinManUp");
            animator.Play("MuffinManUp");
        }
        else if (current.y > target.y)// && -move.y > Math.Abs(move.x)))
        {
            //animator.SetTrigger("MuffinManDown");
            animator.Play("MuffinManDown");
        }
        else
        {
            animator.SetTrigger("MuffinManIdle");
            //animator.Play("Player1Idle");
        }

        //}
        // Otherwise...
        //else
        //{
        // ... disable the nav mesh agent.
        //nav.enabled = false;
        //}
    }*/
}
