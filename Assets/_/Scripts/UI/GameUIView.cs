using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameUIView : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI movesCountText;
    public TextMeshProUGUI gameOverText; 

    private void Start()
    {
        // Hide game over text initially
        gameOverText.gameObject.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        highScoreText.text = "Best Score: " + score.ToString();
    }

    public void UpdateMovesCount(int count)
    {
        movesCountText.text = "Moves: " + count.ToString();
    }

    public void ShowGameOver()
    {
        gameOverText.gameObject.SetActive(true);
        AnimateText(gameOverText);
    }
    public void AnimateText(TextMeshProUGUI textMesh)
    {
        // Make sure the text is invisible initially
        textMesh.alpha = 0;
        textMesh.transform.localScale = Vector3.zero;

        // Create a sequence of animations
        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(textMesh.DOFade(1, 0.7f)) 
            .Join(textMesh.transform.DOScale(Vector3.one, 0.5f)) 
            .AppendInterval(1f) 
            .Append(textMesh.DOFade(0, 3f)); 
    }

}