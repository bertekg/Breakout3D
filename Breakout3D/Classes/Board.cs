using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout3D.Classes;

class Board : DrawableGameComponent
{
    GraphicsDevice graphics;

    private Camera camera;
    private GameObject wallLeft, wallRight;
    private SpriteBatch spriteBatch;
    private Vector3 scale;
    private int boardHeight;
    private int boardSize;

    public Board(Model wall, Camera camera, Game game, GraphicsDevice graphics,
        SpriteBatch spriteBatch, int boardSize) : base(game)
    {
        this.spriteBatch = spriteBatch;
        this.graphics = graphics;
        this.boardSize = boardSize;
        boardHeight = 2 * boardSize;
        this.camera = camera;
        float z = 94.54f;
        scale = new Vector3(0.007f, boardSize / z, 0.05f);
        wallLeft = new GameObject();
        wallRight = new GameObject();

        wallLeft.Model = wall;
        wallRight.Model = wall;
        wallLeft.Scale = scale;
        wallRight.Scale = scale;
        wallLeft.Position = new Vector3(-2, boardSize, 0.0f);
        wallRight.Position = new Vector3(boardSize + 2, boardSize, 0.0f);
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
