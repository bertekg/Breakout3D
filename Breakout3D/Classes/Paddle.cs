using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Breakout3D.Classes;

class Paddle : DrawableGameComponent
{
    public double PosX;
    public double PosY;

    private int boardSize;
    private int size;
    private int speed = 1;
    private GameObject paddleObject;
    private Camera camera;
    private SpriteBatch spriteBatch;
    private BoundingSphere boundingSphere;
    GraphicsDevice graphics;

    public Paddle(Model model, Camera camera, Game game,
    GraphicsDevice graphics, SpriteBatch spriteBatch,
    int size, int boardSize) : base(game)
    {
        this.spriteBatch = spriteBatch;
        this.graphics = graphics;
        this.camera = camera;
        this.size = size;
        this.boardSize = boardSize;
        paddleObject = new GameObject();
        paddleObject.Model = model;
        paddleObject.Scale = new Vector3(0.03f, 0.01f, 0.01f);
        boundingSphere = paddleObject.Model.Meshes[0].BoundingSphere;
        boundingSphere.Center = paddleObject.Position;
        boundingSphere.Radius = size / 2;
        ResetPaddlePosition();
    }
    public void ResetPaddlePosition()
    {
        PosX = (double)boardSize / 2;
        PosY = 2.0f;
    }
    public override void Update(GameTime gameTime)
    {
        paddleObject.Position = new Vector3((float)PosX, (float)PosY, 0.0f);
        boundingSphere.Center = paddleObject.Position;
        base.Update(gameTime);
    }
    public override void Draw(GameTime gameTime)
    {
        paddleObject.Position = new Vector3((float)PosX, (float)PosY, 0.0f);
        boundingSphere.Center = paddleObject.Position;
        paddleObject.DrawModel(camera, new Vector3(1f, 1f, 1f));
        base.Draw(gameTime);
    }
    public void MoveLeft()
    {
        PosX -= speed;
        if (PosX - size / 2 <= 0)
        {
            PosX = (size / 2);
        }
    }
    public void MoveRight()
    {
        PosX += speed;
        if (PosX + (size / 2) >= boardSize)
        {
            PosX = boardSize - (size / 2);
        }
    }
    public void CheckPaddleBallColisionAndUpdateDir(Ball ball)
    {
        if (boundingSphere.Intersects(ball.BallSphere) && ball.DirY < 0)
        {
            double intersectX = ball.PosX;
            double maxBounceAngle = Math.PI / 6; //60 stopni od 0 do 60 
            double relativeIntersectX = PosX - intersectX;
            double normalizedRelativeIntersectionX = (relativeIntersectX /
            (boardSize / 2));
            double bounceAngle = normalizedRelativeIntersectionX * maxBounceAngle;
            ball.DirX = (ball.DirX >= 0 ? 1 : -1) * Math.Abs(Math.Sin(bounceAngle));
            ball.DirY = -ball.DirY;
            double norm = Math.Sqrt(ball.DirX * ball.DirX + ball.DirY * ball.DirY);
            ball.DirX /= norm;
            ball.DirY /= norm;
        }
    }
}
