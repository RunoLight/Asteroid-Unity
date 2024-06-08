using UnityEngine;

namespace Asteroid.Presentation.Abstractions
{
    public interface IObjectRotationAdapter
    {
        public Quaternion Rotation { get; set; }
    }
}