using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Breakout3D.Classes;

class Ball : DrawableGameComponent
{
    private readonly double speed;
    private readonly int boardSize;
    private readonly int boardHeight;
    private readonly Camera camera;
    private readonly Random rand;
    private readonly GameObject ball;
    public readonly GraphicsDevice graphics;

    public SpriteBatch SpriteBatch;
    public int BallSize;
    public BoundingSphere BallSphere;
    public bool Run { get; set; } = false;
    public int Lives { get; set; } = 3;
    public double DirX { get; set; }
    public double DirY { get; set; }
    public double PosX { get; set; }
    public double PosY { get; set; }

    public Ball(Model model, Camera camera, Game game, GraphicsDevice graphics,
        SpriteBatch spriteBatch, int ballSize, int boardSize) : base(game)
    {
        SpriteBatch = spriteBatch;
        BallSize = ballSize;
        this.graphics = graphics;
        this.camera = camera;
        this.boardSize = boardSize;

        boardHeight = 2 * boardSize;
        speed = 1f;
        ball = new GameObject
        {
            Model = model,
            Scale = new Vector3(0.01f, 0.01f, 0.01f)
        };
        BallSphere = ball.Model.Meshes[0].BoundingSphere;
        rand = new Random();

        ResetBallPosition();
        ResetBallDireciton();
    }

    public void ResetBallPosition()
    {
        PosX = boardSize / 2;
        PosY = boardSize;
    }

    public void ResetBallDireciton()
    {
        do
        {
            DirX = rand.Next(-1, 1);
        } 
        while (DirX == 0);

        do
        {
            DirY = rand.Next(-1, 1);
        } 
        while (DirY == 0);

        double norm = Math.Sqrt(DirX * DirX + DirY * DirY);
        DirX /= norm;
        DirY /= norm;
    }

    public void CheckWallCollision()
    {
        PosX += DirX * speed;
        PosY += DirY * speed;

        if (PosX <= 0)
        {
            PosX = 0;
            DirX = -DirX;
        }
        if (PosX >= boardSize)
        {
            PosX = boardSize;
            DirX = -DirX;
        }
        if (PosY >= boardHeight)
        {
            PosY = boardHeight;
            DirY = -DirY;
        }
        if (PosY <= 1)
        {
            Run = false;
            Lives--;
            ResetBallPosition();
            ResetBallDireciton();
        }
    }

    public override void Update(GameTime gameTime)
    {
        ball.Position = new Vector3((float)PosX, (float)PosY, 0.0f);
        BallSphere.Center = ball.Position;
        BallSphere.Radius = 1;
        if (Run)
        {
            CheckWallCollision();
        }
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        ball.DrawModel(camera, new Vector3(1f, 0, 0));
        base.Draw(gameTime);
    }
}
