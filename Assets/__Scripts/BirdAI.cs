using UnityEngine;
using System.Collections;

public class BirdAI : MonoBehaviour {

    Vector3[] m_Waypoints;              //positions that the birds can be in
    private Vector3 curWaypoint;        //current position
    private Vector3 nextWaypoint;       //next position

    public GameObject specialWaypoint;  //use for a special waypoint
    private Vector3 specialLoc;

    public float speed = 10f;           //how fast the bird flies
    public float moveChance = 1;        //chance for the bird to move from its current position to a different one
    public int hitPoints = 5;           
    public int attackDmg = 5;

    private float moveTime;             //used for lerping
    private float journeyLength;        //also used for lerping

    private GameObject m_Char;
    private bool isDead = false;
    public float dropRng = 1.0f;        //chance to get a droppable item
    public float attackRange = 10.0f;    //How far the bird can be to do damage to the player

    private bool canAttack = true;
    public float attackSpeed = 2.0f;

    private DayNightManager daynight;
    private AudioManager mAudio;

    bool isSafe = false;

    public enum BirdState
    {
        Idle,
        Moving,
        Attack
    }
    private BirdState m_State;

	// Use this for initialization
	void Start () {
        GameObject[] wpObjs = GameObject.FindGameObjectsWithTag("waypoint");
        m_Waypoints = new Vector3[wpObjs.Length];
        for(int i=0; i<wpObjs.Length; i++){
            m_Waypoints[i] = wpObjs[i].GetComponent<Transform>().position;
        }
        if(specialWaypoint != null){
            specialLoc = specialWaypoint.GetComponent<Transform>().position;
        }
        m_Char = GameObject.FindGameObjectWithTag("Char");

        //spawn the bird wherever you want and it will fly to an arbirary waypoint
        m_State = BirdState.Moving; 

        //move the bird to an arbitrary waypoint
        curWaypoint = transform.position;
        nextWaypoint = m_Waypoints[(Random.Range(0, m_Waypoints.Length))];
        moveTime = Time.time;
        journeyLength = Vector3.Distance(curWaypoint, nextWaypoint);

        daynight = GameObject.FindGameObjectWithTag("Manager").GetComponent<DayNightManager>();
        mAudio = GameObject.FindGameObjectWithTag ( "Manager" ).GetComponent<AudioManager> ();
	}
	
	// Update is called once per frame
	void Update () {
        int rng = Random.Range(1, 100);
        if (m_State == BirdState.Idle)
        {
            if (rng <= moveChance)
            {
                m_State = BirdState.Moving;
                nextWaypoint = m_Waypoints[(Random.Range(0, m_Waypoints.Length))];
                moveTime = Time.time;
                journeyLength = Vector3.Distance(curWaypoint, nextWaypoint);
            }
        }
        else if (m_State == BirdState.Moving)
        {
            updateFacing();
            float distCovered = (Time.time - moveTime) * speed;
            float fracJourney = distCovered/journeyLength;
            transform.position = Vector3.Lerp(curWaypoint, nextWaypoint, fracJourney);
            if(transform.position == nextWaypoint){ //we have reached the next position
                curWaypoint = nextWaypoint;
                m_State = BirdState.Idle;
            }
        }
        else if(m_State == BirdState.Attack){
            Vector3 charOffset = m_Char.transform.position;
            charOffset.y += m_Char.GetComponent<SpriteRenderer>().bounds.size.y;
            charOffset.x += m_Char.GetComponent<SpriteRenderer>().bounds.size.x;
            if (m_Char.transform.position.x > transform.position.x)
            {
                Vector3 newScale = transform.localScale;
                newScale.x = -Mathf.Abs(newScale.x);
                transform.localScale = newScale;
            }
            else
            {
                Vector3 newScale = transform.localScale;
                newScale.x = Mathf.Abs(newScale.x);
                transform.localScale = newScale;
            }
            transform.position = Vector3.MoveTowards(transform.position, charOffset, Time.deltaTime*speed);
            if ( canAttack )
            {
                StartCoroutine ( AttackPlayer () );
            }
        }
        if(isDead){
            //play death sound
            Destroy(gameObject);
        }
        if (daynight.GetTimeOfDay() > (50 * 0.35) && daynight.GetTimeOfDay() < (50 * 0.75f)) //this is night time
        {
            if (!isSafe)
            {
                m_State = BirdState.Attack;
            }
        }
	}

    private void updateFacing()
    {
        if (curWaypoint.x <= nextWaypoint.x)
        {
            Vector3 newScale = transform.localScale;
            newScale.x = -Mathf.Abs(newScale.x);
            transform.localScale = newScale;
        }
        else
        {
            Vector3 newScale = transform.localScale;
            newScale.x = Mathf.Abs(newScale.x);
            transform.localScale = newScale;
        }
    }

    public void MoveToSpecial()
    {
        m_State = BirdState.Moving;
        nextWaypoint = specialLoc;
        moveTime = Time.time;
        journeyLength = Vector3.Distance(curWaypoint, nextWaypoint);
    }

    public void Attack()
    {
        m_State = BirdState.Attack;
        canAttack = true;
    }

    public void StopAttacking()
    {
        m_State = BirdState.Moving;
        curWaypoint = transform.position;
        nextWaypoint = m_Waypoints[(Random.Range(0, m_Waypoints.Length))];
        moveTime = Time.time;
        journeyLength = Vector3.Distance(curWaypoint, nextWaypoint);
        isSafe = true;
    }
    public void exitedSafeArea()
    {
        isSafe = false;
    }
    public void enterSafeArea()
    {
        isSafe = true;
    }

    IEnumerator AttackPlayer()
    {
        if (canAttack && Mathf.Abs(m_Char.transform.position.x - transform.position.x) <= attackRange)
        {
            GetComponent<Animator>().SetTrigger("Attack");
            m_Char.GetComponent<Character>().TakeDamage(attackDmg);
        }
        canAttack = false;
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "arrow")
        {
            Destroy(other.gameObject);
            //if you hit the bird, it aggros to you
            if (m_State != BirdState.Attack)
            {
                m_State = BirdState.Attack;
            }
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        hitPoints--;        
        if (hitPoints <= 0)// && !isDead)
        {
            //obtain dropable item!
            if (GetComponent<Interactable>().gatherableItem != null)
            {
                if (Random.value <= dropRng)
                {
                    GetComponent<Interactable>().ReceiveItem();
                    GetComponent<Interactable>().gatherableItem = null;
                }
            }
            //GetComponent<Animator>().SetTrigger("Death");
            isDead = true;
            mAudio.PlayOnce ( "enemyDeath" );
        }
        else
        {
            mAudio.PlayOnce ( "enemyDamaged" );
        }
    }
}
