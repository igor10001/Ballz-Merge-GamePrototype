using System.Collections;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public static ProjectileLauncher Instance { get; private set; }

    private Vector3 m_StartPosition;
    private Vector3 m_EndPosition;
    private Vector3 m_WorldPosition;

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
    private Ball m_CurrentBall;

    private void Awake()
    {
        // Ensure only one instance exists hin the scene
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
        if (!m_CanPlay)
            return;

        if (Time.timeScale != 0)
        {
            Vector3 mousePosition = Input.mousePosition;
            if (mousePosition.x < 0 || mousePosition.x > Screen.width ||
                mousePosition.y < 0 || mousePosition.y > Screen.height)
            {
                return;
            }
            m_WorldPosition = Camera.main.ScreenToWorldPoint(mousePosition + Vector3.back * 10);
        };

        if (Input.GetMouseButtonDown(0))
            StartDrag(m_WorldPosition);
        else if (Input.GetMouseButton(0))
            ContinueDrag(m_WorldPosition);
        else if (Input.GetMouseButtonUp(0))
            EndDrag();
    }

    private void StartDrag(Vector3 worldPosition)
    {
        m_StartPosition = worldPosition;
    }
    
    private void ContinueDrag(Vector3 worldPosition)
    {
        Vector3 tempEndposition = worldPosition;
        Vector3 tempDirection = tempEndposition - m_StartPosition;
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

    private void EndDrag()
    {
        if (m_StartPosition == m_EndPosition)
            return;

        m_Direction = m_EndPosition - m_StartPosition;
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
       // m_CurrentBall.gameObject.SetActive(false);
        m_CurrentBall.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }

    private IEnumerator StartShootingBall()
    {
        //m_BallSprite.enabled = false;
        
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
