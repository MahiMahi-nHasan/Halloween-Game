using System.Collections;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
public class FloorClerk : NPC
{
    [System.Serializable]
    public enum DebugLevel
    {
        NONE,
        MESSAGES,
        VERBOSE,
        EVERYTHING
    }
    public DebugLevel debugLevel = DebugLevel.NONE;

    public enum State
    {
        PATROL,
        CHASE,
        COMMAND,
        ATTACK
    }
    public State state;
    private State checkState;

    // Class-level declarations

    [SerializeField] private AudioClip deathSound;
    [SerializeField] private Animator animator;

    [Header("Movement and Pathfinding")]

    private Vector3 initPos;
    [SerializeField] private float patrolRadius;
    private Vector3 lastKnownPosition;

    [SerializeField] private float speed = 5f;

    [SerializeField] private float attackDistance = 5f;
    [SerializeField] private float attackTime = 2f;
    private float attackCooldown = 0f;

    // Pathfinding - AstarPathfindingProject
    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    [SerializeField] private float nextWaypointDistance = 3;
    [SerializeField] private float pathfindingInterval;
    [SerializeField] private float continuousGenerationInterval;
    private float pathfindingCooldown;

    [SerializeField] private bool drawGizmos = false;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        seeker.pathCallback += OnPathComplete;
    }

    private void OnDisable()
    {
        seeker.pathCallback -= OnPathComplete;
    }

    /*
    Initialize all of the constraints with the reference to this gameObject so the constraint can
    access the object properties
    */
    private void Start()
    {
        initPos = transform.position;
        SelectState();
        StartCoroutine(StartPath());
        checkState = state;
    }

    /*
    Go down the list of behaviours and make sure the correct one is selected before pathfinding 
    with the correct targeting behaviour
    */
    private void Update()
    {
        timer += Time.deltaTime;
        pathfindingCooldown += Time.deltaTime;

        if (attackCooldown > 0f)
            attackCooldown -= Time.deltaTime;
        /*
        Check if NPC can see target
        Returns true if the raycast hits no obstacles (returns false), false if the raycast 
        hits obstacles (returns true)
         > ie seeingTarget = !Raycast
        */
        float dist;
        eyePos = transform.position + Vector3.up * height;
        targetAtEyeHeight = target.position + Vector3.up * height;
        seeingTarget = seeingDistance > (dist = Vector3.Distance(eyePos, targetAtEyeHeight)) ? !Physics.Raycast(
            eyePos,
            (targetAtEyeHeight - eyePos).normalized,
            dist,
            obstacleLayer
        ) : false;
        Debug.DrawRay(
        eyePos,
        2 * (targetAtEyeHeight - eyePos).normalized,
        seeingTarget ? Color.green : Color.red
        );

        // Update the last position in which the NPC saw the player
        if (seeingTarget)
            lastKnownPosition = target.position;

        // If state has changed, generate new path
        if (checkState != (state = SelectState())) {
            Debug.Log("State changed!");
            StartCoroutine(StartPath());
            checkState = state;
        }
        
        // If chasing and enough time has passed, update the path to reflect the new player position
        if (state == State.CHASE && pathfindingCooldown > continuousGenerationInterval)
        {
            pathfindingCooldown = 0;
            StartCoroutine(StartPath());
        }

        // Follow path, using whether path exists as a status variable
        if (path != null)
        {
            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            Vector3 movement = speed * Time.deltaTime * dir;
            animator.SetFloat("speed", movement.magnitude);
            if (debugLevel >= DebugLevel.VERBOSE)
                Log(string.Format(
                    "Waypoint {0} = {1}\nMovement vector = {2}",
                    currentWaypoint,
                    path.vectorPath[currentWaypoint],
                    movement
                ));
            transform.position += movement;

            // Complete all waypoints which are close enough
            while (true)
            {
                float distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
                if (distanceToWaypoint < nextWaypointDistance)
                    if (currentWaypoint + 1 < path.vectorPath.Count)
                        currentWaypoint++;
                    else
                    {
                        OnEndpointReached();
                        break;
                    }
                else break;
            }
        }
    }

    private IEnumerator StartPath(bool reset = true, bool delay = true)
    {
        if (reset)
            path = null;    // Reset path like a status variable

        Vector3 targetPosition = SelectTarget();

        if (state != State.CHASE && delay)
            yield return new WaitForSeconds(pathfindingInterval);

        seeker.StartPath(transform.position, targetPosition);

        if (state == State.CHASE)
        {
            yield return new WaitForSeconds(continuousGenerationInterval);
            StartCoroutine(StartPath());
        }

        yield break;
    }

    private void OnEndpointReached()
    {
        if (debugLevel >= DebugLevel.VERBOSE) Log("Finished path - generating new path");
        if (state != State.CHASE)
        {
            sawCandyStolen = false;
            followingCommand = false;
        }
        StartCoroutine(StartPath());
    }

    private State SelectState()
    {
        // If target is chasing and within attack range
        if (seeingTarget && sawCandyStolen && Vector3.Distance(target.position, transform.position) < attackDistance)
            return State.ATTACK;

        // Select CHASE if NPC can see the target and saw candy stolen
        // Set followingCommand to true in case the NPC loses sight of the target so it will
        // continue to the last known position
        if (seeingTarget && sawCandyStolen)
        {
            followingCommand = true;
            return State.CHASE;
        }

        // Select COMMAND if the NPC is supposed to be following a command
        if (followingCommand)
        {
            return State.COMMAND;
        }

        // Default
        return State.PATROL;
    }

    private Vector3 SelectTarget() {
        switch(state) {
            case State.CHASE:
                return target.position;
            case State.COMMAND:
                return lastKnownPosition;
            case State.PATROL:
                Vector2 rand = patrolRadius * Random.insideUnitCircle;
                return initPos + new Vector3(rand.x, initPos.y, rand.y);
            case State.ATTACK:
                Attack();
                return transform.position;
            default:
                return initPos;
        }
    }

    private void OnPathComplete(Path p)
    {
        if (p.error == true) return;

        path = p;
        currentWaypoint = 0;        // Reset to avoid IndexOutOfBounds errors
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || !drawGizmos) return;
    }

    private void Attack()
    {
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0)
        {
            target.GetComponent<PlayerController>().DamagePlayer();
            attackCooldown = attackTime;
        }
    }

    private IEnumerator OnSleep()
    {
        // Disable movement
        speed = 0;

        // Play death sound
        GameManager.active.universalSoundEffect.PlayOneShot(deathSound);

        // Despawn NPC
        Despawn();

        yield break;
    }

    public void Sleep()
    {
        if (debugLevel > DebugLevel.NONE) Log("Sleeping");
        StartCoroutine(OnSleep());
    }

    private void Despawn()
    {
        if (parent != null)
            parent.Despawn(id);
        else
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
            Sleep();
    }

    public void ReceiveCommand()
    {
        followingCommand = true;
        sawCandyStolen = true;
        lastKnownPosition = target.position;

        SelectState();
        StartCoroutine(StartPath(delay: false));
        currentWaypoint = 0;
    }

    public void Log(string message)
    {
        Debug.Log(gameObject.name + " - " + message);
    }
}