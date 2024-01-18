using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Breakout3D.Classes;

public class GameObject
{
    public Model Model { get; set; }
    public Vector3 Position { get; set; } = Vector3.Zero;
    public Vector3 Scale { get; set; } = new Vector3(1.0f, 1.0f, 1.0f);

    public void DrawModel(Camera camera, Vector3 color)
    {
        Matrix[] transforms = new Matrix[Model.Bones.Count];
        Model.CopyAbsoluteBoneTransformsTo(transforms);
        foreach (ModelMesh mesh in Model.Meshes)
        {
            foreach (BasicEffect e in mesh.Effects.Cast<BasicEffect>())
            {
                e.EnableDefaultLighting();
                e.AmbientLightColor = color;
                e.World = transforms[mesh.ParentBone.Index]
                * Matrix.CreateScale(Scale) *
                Matrix.CreateTranslation(Position);
                e.View = camera.ViewMatrix;
                e.Projection = camera.ProjectionMatrix;
            }
            mesh.Draw();
        }
    }
}
