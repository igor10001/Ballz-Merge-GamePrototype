using TMPro;
using UnityEngine;

public class GameUIView : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI movesCountText;

    public void UpdateScore(int score)
    {
        highScoreText.text = "Best Score: " + score.ToString();
    }

    public void UpdateMovesCount(int count)
    {
        movesCountText.text = "Move: " + count.ToString();
    }
}