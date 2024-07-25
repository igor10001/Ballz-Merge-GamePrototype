using UnityEngine;

public class Ball : MonoBehaviour
{
    public static Vector3 s_FirstCollisionPoint { private set; get; }

    private Rigidbody2D m_Rigidbody2D;
    private CircleCollider2D m_Collider2D;
    private SpriteRenderer m_SpriteRenderer;

    public int m_WallCollisionDuration = 0;
    [SerializeField] private float m_MoveSpeed = 20;
    public float m_MinimumYPosition = -4.7f;
    public AudioSource audio;

    private IBallState _currentState;
    private bool _hasMovedObjectsDown = true; // Flag to track if MoveAllObjectsDown has been called

    public IBallState CurrentState => _currentState;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Rigidbody2D.bodyType = RigidbodyType2D.Static;

        m_Collider2D = GetComponent<CircleCollider2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        _currentState = new BallStaticState(); // Default state
    }

    private void Update()
    {
        _currentState.HandleBallState(this);

        if (m_Rigidbody2D.bodyType != RigidbodyType2D.Dynamic)
            return;

        m_Rigidbody2D.velocity = m_Rigidbody2D.velocity.normalized * m_MoveSpeed;

        if (transform.localPosition.y < m_MinimumYPosition)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, m_MinimumYPosition, 0);

            if (s_FirstCollisionPoint == Vector3.zero)
            {
                s_FirstCollisionPoint = transform.position;
                var launcher = ProjectileLauncher.Instance;
                launcher.m_BallSprite.transform.position = s_FirstCollisionPoint;
                launcher.m_BallSprite.enabled = true;
            }

            // Call MoveAllObjectsDown only once per cycle
            if (!_hasMovedObjectsDown)
            {
                //Testing.Instance.Grid.MoveAllObjectsDown();
                _hasMovedObjectsDown = true; // Set the flag to indicate the action has been performed
            }

            ChangeState(new BallStaticState()); // Change state to static
            MoveTo(s_FirstCollisionPoint, iTween.EaseType.linear, (Vector2.Distance(transform.position, s_FirstCollisionPoint) / 5.0f), "OnBallReturned");
        }
    }

    public void ChangeState(IBallState newState)
    {
        _currentState = newState;

        // Reset the flag when the state changes
        if (newState is BallStaticState)
        {
            _hasMovedObjectsDown = false; // Allow MoveAllObjectsDown to be called again
        }
    }

    private void OnBallReturned()
    {
        if (s_FirstCollisionPoint != Vector3.zero)
        {
            var launcher = ProjectileLauncher.Instance;
            launcher.transform.position = s_FirstCollisionPoint;
        }

        var projectileLauncher = ProjectileLauncher.Instance;
        projectileLauncher.m_BallSprite.enabled = true;

        s_FirstCollisionPoint = Vector3.zero;
        projectileLauncher.m_CanPlay = true;
        ChangeState(new BallStaticState()); // Ensure the ball is in static state
    }

    public static void ResetFirstCollisionPoint()
    {
        s_FirstCollisionPoint = Vector3.zero;
    }

    public void GetReadyAndAddForce(Vector2 direction)
    {
        m_SpriteRenderer.enabled = true;
        m_Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        m_Collider2D.enabled = true;
        m_Rigidbody2D.AddForce(direction);
        ChangeState(new BallMovingState()); // Change state to moving
    }

    public void EnablePhysics()
    {
        m_Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        m_Collider2D.enabled = true;
    }

    public void DisablePhysics()
    {
        m_Collider2D.enabled = false;
        m_Rigidbody2D.bodyType = RigidbodyType2D.Static;
    }

    public void MoveTo(Vector3 position, iTween.EaseType easeType = iTween.EaseType.linear, float time = 0.1f, string onCompleteMethod = "OnBallReturned")
    {
        iTween.Stop(gameObject);

        if (m_SpriteRenderer.enabled)
        {
            iTween.MoveTo(gameObject, iTween.Hash("position", position, "easetype", easeType, "time", time, "oncomplete", onCompleteMethod));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GridObj gridObj = collision.gameObject.GetComponent<GridObj>();
        if (gridObj != null)
        {
            Vector2 hitDirection = collision.contacts[0].normal; // Direction of the hit
            gridObj.OnBallHit(hitDirection);
            audio.Play();
        }
    }
}
