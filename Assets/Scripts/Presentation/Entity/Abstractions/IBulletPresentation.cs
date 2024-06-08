using System;
using UnityEngine;

namespace Asteroid.Presentation.Entity.Abstractions
{
    public interface IBulletPresentation : IEntityPresentation
    {
        public event Action OnLethalCollided;
        public Quaternion Rotation { get; set; }
        public void RotateAroundSelf(float degree);
        public void SetActive(bool isActive);
    }
}