// ProjectileLauncher.cs

using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class ProjectileLauncher : MonoBehaviour
{
    public static ProjectileLauncher Instance { get; private set; }

    private Vector3 m_StartPosition;
    private Vector3 m_EndPosition;
    private Vector3 m_WorldPosition;
    public event EventHandler OnBallSpawn;
    private Vector3 m_Direction;

    private LineRenderer m_LineRenderer;

    private Vector3 m_DefaultStartPosition;

    public SpriteRenderer m_BallSprite;

    public bool m_CanPlay = true;

    [Header("Linerenderer Colors")]
    public Color m_CorrectLineColor;
    public Color m_WrongLineColor;

    [Header("Ball")]
    public Ball m_BallPrefab;
    public Ball m_CurrentBall;

    private void Awake()
    {
        // Ensure only one instance exists in the scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        m_CanPlay = true;
        m_LineRenderer = GetComponent<LineRenderer>();

        m_DefaultStartPosition = transform.position;
    }

    private void Start()
    {
        SpawnNewBall();
    }

    private void Update()
    {
        if (!m_CanPlay || (m_CurrentBall != null && m_CurrentBall.GetComponent<Ball>().CurrentState is BallMovingState))
            return;
        

        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.touchCount > 0) // Check if there's at least one touch
        {
            Touch touch = Input.GetTouch(0); // Get the first touch

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    m_StartPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                    break;
                case TouchPhase.Moved:
                    Vector3 tempEndPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                    ContinueDrag(tempEndPosition);
                    break;
                case TouchPhase.Ended:
                    Vector3 endPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                    EndDrag(endPosition);
                    break;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                m_StartPosition = Camera.main.ScreenToWorldPoint(mousePosition + Vector3.back * 10);
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                Vector3 tempEndPosition = Camera.main.ScreenToWorldPoint(mousePosition + Vector3.back * 10);
                ContinueDrag(tempEndPosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                Vector3 endPosition = Camera.main.ScreenToWorldPoint(mousePosition + Vector3.back * 10);
                EndDrag(endPosition);
            }
        }
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
        if (m_StartPosition == endPosition)
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

            m_CanPlay = false;
            StartCoroutine(StartShootingBall());
        }
    }

    private void SpawnNewBall()
    {
        if (m_CurrentBall != null)
        {
            Destroy(m_CurrentBall.gameObject);
        }

        m_CurrentBall = Instantiate(m_BallPrefab, transform.position, Quaternion.identity);
        m_CurrentBall.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        OnBallSpawn?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator StartShootingBall()
    {
        m_CurrentBall.transform.position = transform.position;
        m_CurrentBall.gameObject.SetActive(true);
        m_CurrentBall.GetReadyAndAddForce(m_Direction);

        yield return new WaitForSeconds(0.05f);

        m_CanPlay = true;
    }

    public void ResetLauncher()
    {
        m_CanPlay = true;
        SpawnNewBall();
    }
}
