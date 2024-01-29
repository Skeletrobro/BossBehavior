using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBehavior : MonoBehaviour
{   //Game objects that will be made in attacks
    public GameObject spawnedShockwave;
    public GameObject spawnedDagger;

    [Header("Boss")]
    //The actual variable that the boss needs
    [SerializeField] private bool attacking = false;
    [SerializeField] private float speed = 3;
    private bool facingRight = false;
    private bool start = false;
    private Vector2 targetPos = new Vector2(6f, -2.6f);
    private Animator animator = null;
    [SerializeField] private Image bHealthBar;
    [SerializeField] private float bHealth = 500;

    //Variables SwordRain needs
    [Header("SwordRain")]
    public GameObject Sword;
    [SerializeField] private float swordLife = 5f;
    [SerializeField] private float swordCooldown = 6;
    private GameObject spawnedSword;

    //Variables Beams needs
    [Header("Beams")]
    public GameObject Beam;
    [SerializeField] private float beamLife = .5f;
    [SerializeField] private float beamSeparation = 2.5f;  
    [SerializeField] private float beamCooldown = 6;
    private GameObject spawnedBeam;

    //Variables Orb needs
    [Header("Orb")]
    public GameObject Orb;
    [SerializeField] private float orbLife = 10f;
    [SerializeField] private float orbSpeed = 1f;
    [SerializeField] public Transform target;
    [SerializeField] private float homingness = 1;
    [SerializeField] private float orbCooldown = 6;
    private GameObject spawnedOrb;

    //Multi-Explosions attack (Think Pure Vessel)
    [Header("Explosions")]
    public GameObject Explosion;
    [SerializeField] private float explosionSignal = 1f;
    [SerializeField] private float explosionLife = 1f;
    [SerializeField] private float explosionCooldown = 6;
    private GameObject spawnedExplosion;

    //Variables Teleport Needs
    [Header("Teleport")]
    [SerializeField] private Transform[] platforms;
    private int i;
    [SerializeField] private int startingPoint;

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Attack")
        {
            TakeDamage(1);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Vector2 initialTeleportLocation = platforms[startingPoint].position;
            transform.position = new Vector2(initialTeleportLocation.x, initialTeleportLocation.y + 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
        if (bHealth < 0) {
            StartCoroutine(Death());
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            start = !start;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(Teleport());
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(Beams());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(SwordRain());
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(Orbs());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Explosions());
        }

        if (!attacking && start)
        {
            int attackNumber = Random.Range(0, 7);
            if (attackNumber == 1)
            {
                StartCoroutine(Beams());
            }
            if (attackNumber == 2)
            {
                StartCoroutine(SwordRain());
            }
            if (attackNumber == 3)
            {
                StartCoroutine(Orbs());
            }
            if (attackNumber == 4)
            {
                StartCoroutine(Explosions());
            }
            if (attackNumber > 4)
            {
                StartCoroutine(Teleport());
            }
            
            // if (bHealth <= 0) {
            //     Tracker.score += 1000;
            //     Tracker.enemiesKilled++;
            //     Tracker.gameOver = true;
            //     Destroy(gameObject);
            // }
        }
            
    // if (Vector2.Distance(transform.position, targetPos) < 0.02f)
    // {
    //     ChangeTargetLocation();
    // }
    } 

    // //Movement
    // private void ChangeTargetLocation()
    // {
    //     targetPos = new Vector2(Random.Range(-7f, 7f), -2.88f + Random.Range(0f, 3f));  
    // }
    //flip
    void Flip()
    {
        if ((facingRight && target.position.x < transform.position.x) || (!facingRight && target.position.x > transform.position.x))
        {
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    //Teleoport
    private IEnumerator Teleport()
    {
        if (!attacking)
        {
            attacking = true;
            animator.SetBool("Teleporting", true);
            i = Random.Range(0, platforms.Length);
            yield return new WaitForSeconds(1.1f);
            Vector2 teleportLocation = platforms[i].position;
            transform.position = new Vector2(teleportLocation.x, teleportLocation.y + 2.5f);
            yield return new WaitForSeconds(.6f);
            animator.SetBool("Teleporting", false);
            attacking = false;
        }
    }

    private IEnumerator Death() {
        attacking = true;
        animator.SetBool("Dying", true);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
    //Sword Rain Full Screen Attack
    private IEnumerator SwordRain()
    {
        if (!attacking)
        {   attacking = true;
            animator.SetBool("Attack1", true);
            yield return new WaitForSeconds(1.5f);
            animator.SetBool("Attack1", false);
            SpawnSword(new Vector2(target.position.x, 12));
            yield return new WaitForSeconds(6);
            attacking = false;
        }
    }

    private void SpawnSword(Vector2 location)
    {
        spawnedSword = Instantiate(Sword, location, Quaternion.identity);
        spawnedSword.GetComponent<Sword>().swordLife = swordLife;
    }


    //Beams Full Screen Attack (2, 3, 2)
    private IEnumerator Beams()
    {
       if (!attacking)
       {
        attacking = true;
        animator.SetBool("Attack1", true);
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("Attack1", false);
        float center = Random.Range(-4, 4) + target.position.x;
        SpawnBeam(new Vector2(center + beamSeparation, 0));
        SpawnBeam(new Vector2(center - beamSeparation, 0));
        yield return new WaitForSeconds(beamLife);
        SpawnBeam(new Vector2(center, 0));
        SpawnBeam(new Vector2(center + (2 + beamSeparation), 0));
        SpawnBeam(new Vector2(center - (2 + beamSeparation), 0));
        yield return new WaitForSeconds(beamLife);
        SpawnBeam(new Vector2(center + beamSeparation, 0));
        SpawnBeam(new Vector2(center - beamSeparation, 0));
        attacking = false;
        yield return new WaitForSeconds(beamCooldown - 2*beamLife);
       }
    }
   
    private void SpawnBeam(Vector2 location)
    {
        spawnedBeam = Instantiate(Beam, location, Quaternion.identity);
        spawnedBeam.GetComponent<Beam>().beamLife = beamLife;
    }

    //Orb Seeking Attack
    private IEnumerator Orbs()
    {
        if (!attacking)
        {
            attacking = true;
            animator.SetBool("Attack2", true);
            yield return new WaitForSeconds(2);
            animator.SetBool("Attack2", false);
            SpawnOrb(transform.position);
            yield return new WaitForSeconds(orbCooldown);
            attacking = false;
        }
    }

    private void SpawnOrb(Vector2 location)
    {
        spawnedOrb = Instantiate(Orb, location, Quaternion.identity);
        spawnedOrb.GetComponent<Orb>().target = target;
        spawnedOrb.GetComponent<Orb>().homingness = homingness;
        spawnedOrb.GetComponent<Orb>().speed = orbSpeed;
        spawnedOrb.GetComponent<Orb>().orbLife = orbLife;
    }


    private IEnumerator Explosions()
    {
        if (!attacking) 
        {
            attacking = true;
            StartCoroutine(startExplosions());
            yield return new WaitForSeconds(.3f);
            float center = Random.Range(-2, 2) + target.position.x;
            float yCenter = target.position.y;
            Vector2 pos1 = new Vector2(center + Random.Range(-7f, -2f), yCenter + Random.Range(-1.5f, 1.5f));
            Vector2 pos2 = new Vector2(center + Random.Range(-2f, 2f), yCenter + Random.Range(-1.5f, 1.5f));
            Vector2 pos3 = new Vector2(center + Random.Range(2f, 7f), yCenter + Random.Range(-1.5f, 1.5f));
            SpawnExplosion(pos1);
            SpawnExplosion(pos2);
            SpawnExplosion(pos3);
            yield return new WaitForSeconds(explosionCooldown);
            attacking = false;
        }
    }
    private IEnumerator startExplosions()
    {
        animator.SetBool("Attack3", true);
        yield return new WaitForSeconds(2);
        animator.SetBool("Attack3", false);
    }

    private void SpawnExplosion(Vector2 location)
    {
        spawnedExplosion = Instantiate(Explosion, location, Quaternion.identity);
        spawnedExplosion.GetComponent<Explosion>().explosionLife = explosionLife;
        spawnedExplosion.GetComponent<Explosion>().explosionSignal = explosionSignal;
    }

    void TakeDamage(float damage)
    {
        bHealth -= damage;
        bHealthBar.fillAmount = bHealth / 500f;
    }




}
