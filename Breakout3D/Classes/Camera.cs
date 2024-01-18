using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Breakout3D.Classes;

public class Camera
{
    const int MAX_TRANSITION_TIME = 400;

    private Vector3 camTarget;
    private int transitionTime = MAX_TRANSITION_TIME;

    protected GraphicsDevice GraphicsDevice { get; set; }

    public Matrix ViewMatrix { get; set; }
    public Matrix ProjectionMatrix { get; set; }
    public Matrix WorldMatrix;
    public Vector3 CamPosition;
    public Vector3 prevCamTarget, prevCamPosition;
    public Vector3 newCamTarget, newCamPosition;

    public Camera(GraphicsDevice graphicsDevice)
    {
        GraphicsDevice = graphicsDevice;
        camTarget = new Vector3(25f, 50f, -50f);
        CamPosition = new Vector3(25f, 50f, 150f);

        ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(45f),
            GraphicsDevice.Viewport.AspectRatio,
            1f, 1000f);

        ViewMatrix = Matrix.CreateLookAt(CamPosition, camTarget,
            new Vector3(0f, 1f, 0f));

        WorldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);
        prevCamTarget = camTarget;
        prevCamPosition = CamPosition;
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Z) && transitionTime >= MAX_TRANSITION_TIME)
        {
            newCamTarget = new Vector3(25f, 50f, -50f);
            newCamPosition = new Vector3(25f, 50f, 150f);
            transitionTime = 0;
        }
        if (keyboardState.IsKeyDown(Keys.X) && transitionTime >= MAX_TRANSITION_TIME)
        {
            newCamTarget = new Vector3(25f, 70f, -50f);
            newCamPosition = new Vector3(25f, -40f, 160f);
            transitionTime = 0;
        }
        if (keyboardState.IsKeyDown(Keys.C) && transitionTime >= MAX_TRANSITION_TIME)
        {
            newCamTarget = new Vector3(25f, 190f, -50f);
            newCamPosition = new Vector3(25f, -100f, 60f);
            transitionTime = 0;
        }
        if (transitionTime < MAX_TRANSITION_TIME)
        {
            transitionTime += gameTime.ElapsedGameTime.Milliseconds;
            if (transitionTime >= MAX_TRANSITION_TIME)
            {
                transitionTime = MAX_TRANSITION_TIME;
                prevCamPosition = newCamPosition;
                prevCamTarget = newCamTarget;
            }
            float step = transitionTime / (float)MAX_TRANSITION_TIME;
            CamPosition = Lerp(prevCamPosition, newCamPosition, step);
            camTarget = Lerp(prevCamTarget, newCamTarget, step);
        }
        ViewMatrix = Matrix.CreateLookAt(CamPosition, camTarget, Vector3.Up);
    }

    static Vector3 Lerp(Vector3 firstVector, Vector3 secondVector, float by)
    {
        float retX = Lerp(firstVector.X, secondVector.X, by);
        float retY = Lerp(firstVector.Y, secondVector.Y, by);
        float retZ = Lerp(firstVector.Z, secondVector.Z, by);
        return new Vector3(retX, retY, retZ);
    }

    static float Lerp(float firstFloat, float secondFloat, float by)
    {
        return firstFloat * (1 - by) + secondFloat * by;
    }
}
