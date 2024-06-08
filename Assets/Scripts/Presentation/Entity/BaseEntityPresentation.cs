using System;
using Asteroid.Presentation.Entity.Abstractions;
using UnityEngine;
using UnityEngine.Assertions;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Asteroid.Presentation.Entity
{
    /// <summary></summary>
    /// <typeparam name="TMarkerForLethalCollision">
    /// This Presentation will be destroyed when collided with Presentation that implements this marker interface
    /// </typeparam>
    public class BaseEntityPresentation<TMarkerForLethalCollision> : MonoBehaviour, IEntityPresentation
    {
        public event Action OnLethalCollided;

        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        public float Scale
        {
            get
            {
                var scale = transform.localScale;
                // We assume scale is uniform
                Assert.IsTrue(scale[0] == scale[1] && scale[1] == scale[2]);
                return scale[0];
            }
            set => transform.localScale = new Vector3(value, value, value);
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void RotateAroundSelf(float angle)
        {
            transform.Rotate(Vector3.forward, angle);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out TMarkerForLethalCollision _))
            {
                OnLethalCollided?.Invoke();
            }
        }
    }
}