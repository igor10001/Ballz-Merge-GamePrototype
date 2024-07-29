using System.Collections;
using UnityEngine;
using Zenject;
using System;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] private InputHandler input;

    private Vector3 m_StartPosition;
    private Vector3 m_EndPosition;
    private Vector3 m_Direction;
    public event EventHandler OnBallSpawn;

    private LineRenderer m_LineRenderer;
    private Vector3 m_DefaultStartPosition;

    [Header("Linerenderer Colors")]
    public Color m_CorrectLineColor;
    public Color m_WrongLineColor;

    [Header("Ball")]
    public Ball m_BallPrefab;
    private Ball m_CurrentBall;
    private GameState _currentState;

    private DiContainer _container;
    private IEventAggregator _eventAggregator;

    [Inject]
    public void Construct(DiContainer container, IEventAggregator eventAggregator)
    {
        _container = container;
        _eventAggregator = eventAggregator;
    }

    private void Awake()
    {
        

        m_LineRenderer = GetComponent<LineRenderer>();
        m_DefaultStartPosition = transform.position;

        input.OnDragStart += HandleDragStart;
        input.OnDrag += HandleDrag;
        input.OnDragEnd += HandleDragEnd;
    }
    public void SetGameState(GameState newState)
    {
        _currentState = newState;
        if (_currentState == GameState.GameOver)
        {
            StopAllCoroutines();
            if (m_CurrentBall != null)
            {
                m_CurrentBall.gameObject.SetActive(false);
            }
           // m_LineRenderer.SetPosition(1, Vector3.zero);
        }
    }

    private void Start()
    {
        SpawnNewBall();
    }

    private void HandleDragStart(Vector3 startPosition)
    {
        if (_currentState == GameState.GameOver) return;
        m_StartPosition = startPosition;
    }

    private void HandleDrag(Vector3 worldPosition)
    {
        if (_currentState == GameState.GameOver) return;
        ContinueDrag(worldPosition);
    }

    private void HandleDragEnd(Vector3 endPosition)
    {
        if (_currentState == GameState.GameOver) return;
        EndDrag(endPosition);
    }

    private void ContinueDrag(Vector3 worldPosition)
    {
        Vector3 tempDirection = worldPosition - m_StartPosition;
        tempDirection.Normalize();

        if (Mathf.Abs(Mathf.Atan2(tempDirection.x, tempDirection.y)) < 1.35f)
        {
            m_LineRenderer.startColor = m_CorrectLineColor;
            m_LineRenderer.endColor = m_CorrectLineColor;
        }
        else
        {
            m_LineRenderer.startColor = m_WrongLineColor;
            m_LineRenderer.endColor = m_WrongLineColor;
        }

        m_EndPosition = worldPosition;
        m_LineRenderer.SetPosition(1, m_EndPosition - m_StartPosition);
    }

    private void EndDrag(Vector3 endPosition)
    {
        if (m_StartPosition == endPosition || _currentState == GameState.GameOver)
            return;

        m_Direction = endPosition - m_StartPosition;
        m_Direction.Normalize();

        m_LineRenderer.SetPosition(1, Vector3.zero);

        if (Mathf.Abs(Mathf.Atan2(m_Direction.x, m_Direction.y)) < 1.35f)
        {
            if (m_CurrentBall == null)
            {
                SpawnNewBall();
            }

            StartCoroutine(StartShootingBall());
        }
    }

    private void SpawnNewBall()
    {
        if (m_CurrentBall == null)
            m_CurrentBall = _container.InstantiatePrefabForComponent<Ball>(m_BallPrefab.gameObject, transform.position, Quaternion.identity, null);

        if (m_CurrentBall != null)
        {
            m_CurrentBall.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            _eventAggregator.Publish(new BallSpawnedEvent { Ball = m_CurrentBall });
        }
        else
        {
            Debug.LogError("Failed to get Ball component from the instantiated GameObject.");
        }
    }

    private IEnumerator StartShootingBall()
    {
        m_CurrentBall.transform.position = transform.position;
        m_CurrentBall.gameObject.SetActive(true);
        m_CurrentBall.GetReadyAndAddForce(m_Direction);

        yield return new WaitForSeconds(0.05f);
    }
}
