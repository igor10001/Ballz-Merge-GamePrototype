public class ScoreModel
{
    public int MovesCount { get; set; }
    public int HighScore { get; set; }


    public ScoreModel()
    {
        MovesCount = 0;
        HighScore = 0;
    }
    public void ResetData()
    {
        MovesCount = 0;
        
    }
}