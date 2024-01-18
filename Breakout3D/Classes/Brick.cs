using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Breakout3D.Classes;

class Brick : DrawableGameComponent
{
    private int posX;
    private int posY;
    private int width;
    public bool Active { get; set; } = true;
    private GameObject brick;
    private Camera camera;
    private SpriteBatch spriteBatch;
    private BoundingSphere brickSphere;
    static Random rand = new();
    GraphicsDevice graphics;
    private Vector3 color;
    public Brick(Model model, Camera camera, Game game,
        GraphicsDevice graphics, SpriteBatch spriteBatch,
        int PosX, int PosY, int width, Vector3 color) : base(game)
    {
        this.spriteBatch = spriteBatch;
        this.graphics = graphics;
        this.width = width;
        this.camera = camera;
        this.color = color;
        posX = PosX;
        posY = PosY;
        brick = new GameObject();
        brick.Model = model;
        brick.Scale = new Vector3(0.004f * width, 0.01f, 0.015f);
        brickSphere = brick.Model.Meshes[0].BoundingSphere;
        brickSphere.Center = new Vector3(posX, posY, 0);
        brickSphere.Radius = width / 2;
        brick.Position = new Vector3(posX, posY, 0.0f);
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
