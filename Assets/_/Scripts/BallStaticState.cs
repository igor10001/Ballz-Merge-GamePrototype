public class BallStaticState : IBallState
{
    public void HandleBallState(Ball ball)
    {
        ball.DisablePhysics();
    }
}