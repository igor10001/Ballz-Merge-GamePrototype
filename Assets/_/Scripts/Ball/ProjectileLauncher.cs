using System.Collections;
using UnityEngine;
using Zenject;

public class ProjectileLauncher : MonoBehaviour
{

    private Vector3 m_StartPosition;
    private Vector3 m_EndPosition;
    private Vector3 m_Direction;
    [SerializeField] private LinePrediction _linePrediction;
    private Vector3 m_DefaultStartPosition;

    [Header("Ball")]
    public Ball m_BallPrefab;
    private Ball m_CurrentBall;
    public Ball CurrentBall => m_CurrentBall;

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
        m_DefaultStartPosition = transform.position;
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
            _linePrediction.EndDrag();
        }
    }

    private void Start()
    {
        SpawnNewBall();
    }

    public void HandleDragStart(Vector3 startPosition)
    {
        if (_currentState == GameState.GameOver) return;
        if (m_CurrentBall != null && m_CurrentBall.CurrentState is BallMovingState)
        {
            return;
        }

        m_StartPosition = startPosition;
    }

    public void HandleDrag(Vector3 worldPosition)
    {
        if (_currentState == GameState.GameOver) return;
        if (m_CurrentBall != null && m_CurrentBall.CurrentState is BallMovingState)
        {
            return;
        }

        ContinueDrag(worldPosition);
    }

    public void HandleDragEnd(Vector3 endPosition)
    {
        if (_currentState == GameState.GameOver) return;
        if (m_CurrentBall != null && m_CurrentBall.CurrentState is BallMovingState)
        {
            return;
        }

        EndDrag(endPosition);
    }

    private void ContinueDrag(Vector3 worldPosition)
    {
        Vector3 tempDirection = worldPosition - m_StartPosition;
        tempDirection.Normalize();

        if (Mathf.Abs(Mathf.Atan2(tempDirection.x, tempDirection.y)) < 1.35f)
        {
            _linePrediction.SetCorectColor();
        }
        else
        {
            _linePrediction.SetWrongColor();
        }

        m_EndPosition = worldPosition;
        _linePrediction.ContinueDrag(m_EndPosition, m_StartPosition);
    }

    private void EndDrag(Vector3 endPosition)
    {
        if (m_StartPosition == endPosition || _currentState == GameState.GameOver)
            return;

        m_Direction = endPosition - m_StartPosition;
        m_Direction.Normalize();

        _linePrediction.EndDrag();

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
