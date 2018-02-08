namespace Autonomous.Public
{
    public class GameObjectState
    {
        public float VY { get; }
        public float VX { get; }
        public RectangleF BoundingBox { get; }
        public GameObjectType GameObjectType { get; }
        public string Id { get; }

        // TODO Damage, score
    }
}
