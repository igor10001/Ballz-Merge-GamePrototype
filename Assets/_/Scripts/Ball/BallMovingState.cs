public class BallMovingState : IBallState
{
    public void HandleBallState(Ball ball)
    {
        ball.EnablePhysics();
    }
}