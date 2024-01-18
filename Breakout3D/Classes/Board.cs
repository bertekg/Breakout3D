using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout3D.Classes;

class Board : DrawableGameComponent
{
    private readonly GraphicsDevice graphics;
    private readonly Camera camera;
    private readonly GameObject wallLeft;
    private readonly GameObject wallRight;
    private Vector3 scale;

    public SpriteBatch SpriteBatch;
    public int BoardHeight;
    public int BoardSize;

    public GraphicsDevice Graphics => graphics;

    public Board(Model wall, Camera camera, Game game, GraphicsDevice graphics,
        SpriteBatch spriteBatch, int boardSize) : base(game)
    {
        SpriteBatch = spriteBatch;
        BoardSize = boardSize;
        BoardHeight = 2 * boardSize;
        this.graphics = graphics;
        this.camera = camera;
        float z = 94.54f;
        scale = new Vector3(0.007f, boardSize / z, 0.05f);
        wallLeft = new GameObject
        {
            Model = wall,
            Scale = scale,
            Position = new Vector3(-2, boardSize, 0.0f)
        };
        wallRight = new GameObject
        {
            Model = wall,
            Scale = scale,
            Position = new Vector3(boardSize + 2, boardSize, 0.0f)
        };
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        wallLeft.DrawModel(camera, new Vector3(5f, 10f, 0));
        wallRight.DrawModel(camera, new Vector3(5f, 10f, 0));
        base.Draw(gameTime);
    }
}
