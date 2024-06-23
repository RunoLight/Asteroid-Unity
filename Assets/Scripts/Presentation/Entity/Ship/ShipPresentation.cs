using System.Collections;
using Asteroid.Presentation.Abstractions;
using Asteroid.Presentation.Entity.Ship.Abstractions;
using Asteroid.Presentation.Marker;
using UnityEngine;

namespace Asteroid.Presentation.Entity.Ship
{
    public class ShipPresentation : BaseEntityPresentation<IEnemy>, IShipPresentation
    {
        [SerializeField] private ParticleSystem shipParticleSystem;

        private Vector2 value;
        private Quaternion value1;

        public void EnableParticles(bool isActive)
        {
            if (isActive)
            {
                shipParticleSystem.Play();
            }
            else
            {
                shipParticleSystem.Stop();
                shipParticleSystem.Clear();
            }
        }

        public void DisableParticlesForOneFrame()
        {
            StartCoroutine(DisableParticleSystemForFrame());
        }

        private IEnumerator DisableParticleSystemForFrame()
        {
            shipParticleSystem.Stop();
            // Do not .Clear(), so still we have trail
            yield return null;
            shipParticleSystem.Play();
        }

        Vector2 IObjectPositionAdapter.Value
        {
            get => Position;
            set => Position = value;
        }

        Quaternion IObjectRotationAdapter.Value
        {
            get => Rotation;
            set => Rotation = value;
        }
    }
}