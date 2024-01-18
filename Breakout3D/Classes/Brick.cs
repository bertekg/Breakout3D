using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Breakout3D.Classes;

class Brick : DrawableGameComponent
{
    private readonly GameObject brick;
    private readonly Camera camera;
    private BoundingSphere brickSphere;
    private Vector3 color;

    public GraphicsDevice Graphics;
    public SpriteBatch SpriteBatch;
    public int PosX;
    public int PosY;
    public int Width;

    static readonly Random rand = new();

    public bool Active { get; set; } = true;

    public Brick(Model model, Camera camera, Game game, GraphicsDevice graphics,
        SpriteBatch spriteBatch, int posX, int posY, int width, Vector3 color) : base(game)
    {
        Graphics = graphics;
        Width = width;
        PosX = posX;
        PosY = posY;
        SpriteBatch = spriteBatch;
        this.camera = camera;
        this.color = color;

        brick = new GameObject
        {
            Model = model,
            Scale = new Vector3(0.004f * width, 0.01f, 0.015f),
            Position = new Vector3(posX, posY, 0.0f)
        };
        brickSphere = brick.Model.Meshes[0].BoundingSphere;
        brickSphere.Center = new Vector3(posX, posY, 0);
        brickSphere.Radius = width / 2;
    }

    public override void Draw(GameTime gametime)
    {
        if (Active)
        {
            brick.DrawModel(camera, color);
        }
        base.Draw(gametime);
    }

    public bool CheckBallColision(Ball ball)
    {
        if (Active && brickSphere.Intersects(ball.BallSphere))
        {
            double randAngle = Math.Sin(rand.NextDouble() * Math.PI / 6 - Math.PI / 12);
            ball.DirX = -ball.DirX + randAngle;
            ball.DirY = -ball.DirY;
            double norm = Math.Sqrt(ball.DirX * ball.DirX + ball.DirY * ball.DirY);
            ball.DirX /= norm;
            ball.DirY /= norm;
            return true;
        }
        return false;
    }
}
