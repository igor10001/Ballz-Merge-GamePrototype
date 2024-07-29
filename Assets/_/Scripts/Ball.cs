using UnityEngine;
using System;
using DG.Tweening;
using Zenject;

public class Ball : MonoBehaviour
{
    public static Vector3 s_FirstCollisionPoint { private set; get; }

    private Rigidbody2D m_Rigidbody2D;
    private CircleCollider2D m_Collider2D;
    private SpriteRenderer m_SpriteRenderer;

    public int m_WallCollisionDuration = 0;
    [SerializeField] private float m_MoveSpeed;
    public float MoveSpeed
    {
        get => m_MoveSpeed;
        set => m_MoveSpeed = value;
    }

    public float m_MinimumYPosition = -4.7f;
    private const string OnBallReturnedMethod = "OnBallReturned";
    private IBallState _currentState;

    public IBallState CurrentState => _currentState;
    private ProjectileLauncher _projectileLauncher;
    private IEventAggregator _eventAggregator;

    public event EventHandler OnBallStaticState;

    [Inject]
    public void Construct(ProjectileLauncher projectileLauncher, IEventAggregator eventAggregator)
    {
        _projectileLauncher = projectileLauncher;
        _eventAggregator = eventAggregator;
    }

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Rigidbody2D.bodyType = RigidbodyType2D.Static;

        m_Collider2D = GetComponent<CircleCollider2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        _currentState = new BallStaticState();
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
            }

            ChangeState(new BallStaticState());
            MoveTo(s_FirstCollisionPoint, 0.1f, OnBallReturnedMethod);
        }
    }

    public void ChangeState(IBallState newState)
    {
        _currentState = newState;
    }

    private void OnBallReturned()
    {
        if (s_FirstCollisionPoint != Vector3.zero)
        {
            var launcher = _projectileLauncher;
            launcher.transform.position = s_FirstCollisionPoint;
        }

        s_FirstCollisionPoint = Vector3.zero;
        ChangeState(new BallStaticState());

        // Ensure the event is only invoked once
        OnBallStaticState?.Invoke(this, EventArgs.Empty);
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
        ChangeState(new BallMovingState());
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

    public void MoveTo(Vector3 position, float duration = 0.1f, string onCompleteMethod = OnBallReturnedMethod)
    {
        if (gameObject == null || transform == null)
        {
            Debug.LogWarning("The ball object or its transform is null. Returning from MoveTo.");
            return;
        }

        transform.DOLocalMove(position, duration).OnComplete(() => Invoke(onCompleteMethod, 0.1f));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GridObj gridObj = collision.gameObject.GetComponent<GridObj>();
        if (gridObj != null)
        {
            Vector2 hitDirection = collision.contacts[0].normal; // Direction of the hit
            gridObj.OnBallHit(hitDirection);
        }
    }
}
