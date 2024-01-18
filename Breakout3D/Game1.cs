using Breakout3D.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Breakout3D
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Camera camera;
        private Model model;
        private Board board;
        private int boardSize = 52;
        private int windowHeight = 800;
        private int windowWidth = 600;
        private List<Brick> listBricks;

        int paddleSize = 4;
        Paddle paddle;

        private int ballSize = 10;
        Ball ball;

        SpriteFont font, title;

        private bool start = false;
        private bool pause = false;
        private bool gameover = false;
        private int score = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = windowHeight;
            _graphics.PreferredBackBufferWidth = windowWidth;
            _graphics.ApplyChanges();

            camera = new Camera(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            model = Content.Load<Model>("model");
            ((BasicEffect)model.Meshes[0].Effects[0]).EnableDefaultLighting();
            board = new Board(model, camera, this, GraphicsDevice,
                _spriteBatch, boardSize);
            Components.Add(board);

            paddle = new Paddle(model, camera, this, GraphicsDevice,
                _spriteBatch, paddleSize, boardSize);
            Components.Add(paddle);

            listBricks = new List<Brick>();
            CreateBricks();

            ball = new Ball(model, camera, this, GraphicsDevice, _spriteBatch,
                ballSize, boardSize);
            Components.Add(ball);

            font = Content.Load<SpriteFont>("text");
            title = Content.Load<SpriteFont>("title");
        }
        public void CreateBricks()
        {
            int boardHeigth = boardSize * 2;
            int brickWidth = 4;
            int brickRows = 6;
            int brickColumns = boardSize / brickWidth;
            Vector3[] colors = new Vector3[4] {
            new Vector3(1f,0f,0f), //Czerwony
            new Vector3(1f,0.5f,0f), //Pomarańczowy
            new Vector3(0f,1f,0f), //Zielony
            new Vector3(0f,0f,1f)//Niebieski
        };
            //Kolumny 
            for (int i = 2; i <= boardSize; i += brickWidth)
            {
                //Wiersze 
                for (int j = 1; j <= brickRows; j++)
                {
                    listBricks.Add(new Brick(model, camera, this, GraphicsDevice,
                         _spriteBatch, i, boardHeigth - (j * 3), brickWidth, colors[(j - 1) % 4]));
                }
            }
            foreach (var brick in listBricks)
            {
                Components.Add(brick);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            //ruch paletki 
            if (keyboardState.IsKeyDown(Keys.A) ||
                keyboardState.IsKeyDown(Keys.Left))
            {
                paddle.MoveLeft();
            }
            if (keyboardState.IsKeyDown(Keys.D) ||
                keyboardState.IsKeyDown(Keys.Right))
            {
                paddle.MoveRight();
            }
            if (keyboardState.IsKeyDown(Keys.P))
            {
                ball.Run = false;
                pause = true;
                Draw(gameTime);
            }
            if (keyboardState.IsKeyDown(Keys.Space) && ball.Run == false)
            {
                if (gameover || score == listBricks.Count)
                {
                    foreach (var item in listBricks)
                    {
                        item.Active = true;
                    }
                    score = 0;
                    ball.Lives = 3;
                    gameover = false;
                }
                start = true;
                pause = false;
                ball.Run = true;
            }
            paddle.CheckPaddleBallColisionAndUpdateDir(ball);
            foreach (var item in listBricks)
            {
                if (item.CheckBallColision(ball))
                {
                    item.Active = false;
                    score++;
                }
            }
            if (ball.Lives == 0)
            {
                gameover = true;
                Draw(gameTime);
            }
            camera.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(51, 51, 51));

            if (!start)
            {
                _spriteBatch.Begin();
                _spriteBatch.DrawString(font, "PRESS SPACE TO START",
                new Vector2(215, 460), Color.White);
                _spriteBatch.DrawString(title, "BREAKOUT 3D",
                new Vector2(240, 260), Color.Orange);
                _spriteBatch.End();
            }
            if (pause && start)
            {
                _spriteBatch.Begin();
                _spriteBatch.DrawString(font, "PRESS SPACE TO RESUME",
                new Vector2(205, 460), Color.White);
                _spriteBatch.End();
            }
            if (gameover)
            {
                _spriteBatch.Begin();
                _spriteBatch.DrawString(title, "GAME OVER",
                new Vector2(250, 260), Color.Orange);
                _spriteBatch.DrawString(font, "PRESS SPACE TO RESTART",
                new Vector2(205, 460), Color.White);
                _spriteBatch.End();
            }
            if (score == listBricks.Count)
            {
                _spriteBatch.Begin();
                _spriteBatch.DrawString(title, "CONGRATULATIONS! YOU WIN!",
                new Vector2(160, 260), Color.Orange);
                _spriteBatch.DrawString(font, "PRESS SPACE TO RESTART",
                new Vector2(205, 460), Color.White);
                _spriteBatch.End();
                ball.Run = false;
                ball.ResetBallPosition();
                ball.ResetBallDireciton();
            }
            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "Score: " + score,
            new Vector2(10, 10), Color.White);
            _spriteBatch.DrawString(font, "Lives: " + ball.Lives,
            new Vector2(windowWidth - 80, 10), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
