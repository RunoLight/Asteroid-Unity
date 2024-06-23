using UnityEngine;

namespace Asteroid.Presentation.Abstractions
{
    public interface IObjectRotationAdapter
    {
        public Quaternion Value { get; set; }
    }
}