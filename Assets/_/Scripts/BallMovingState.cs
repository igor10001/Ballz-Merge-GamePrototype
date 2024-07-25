public class BallMovingState : IBallState
{
    public void HandleBallState(Ball ball)
    {
        // Handle logic when the ball is moving
        ball.EnablePhysics();
    }
}