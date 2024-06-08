using UnityEngine;

namespace Asteroid.Presentation.Entity.Abstractions
{
    public interface IEntityPresentation
    {
        public Vector2 Position { get; set; }
        public float Scale { get; set; }
    }
}