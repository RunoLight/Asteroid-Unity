using System;
using Asteroid.Presentation.Abstractions;
using Asteroid.Presentation.Entity.Ship.Abstractions;
using UnityEngine;

namespace Asteroid.GameLogic.Ship
{
    public class ShipGunController
    {
        public event Action<Vector2, Quaternion> BulletShotRequested;

        private readonly IShipGun shipGunPresentation;
        private IObjectRotationAdapter rotationAdapter;

        private bool isEnabled;
        private float shootCooldownRemaining = OriginalShootCooldown;

        private const float OriginalShootCooldown = 0.15f;

        public ShipGunController(IShipGun shipGunPresentation)
        {
            this.shipGunPresentation = shipGunPresentation;
        }

        public void Setup(IObjectRotationAdapter newRotationAdapter)
        {
            rotationAdapter = newRotationAdapter;
        }

        public void Enable()
        {
            isEnabled = true;
        }

        public void Disable()
        {
            isEnabled = false;
        }

        public void Reset()
        {
            shootCooldownRemaining = OriginalShootCooldown;
        }

        public void Tick(float deltaTime)
        {
            if (!isEnabled)
                return;

            shootCooldownRemaining -= deltaTime;

            if (shootCooldownRemaining > 0f)
                return;

            shootCooldownRemaining += OriginalShootCooldown;
            Shoot(rotationAdapter.Rotation);
        }

        private void Shoot(Quaternion lookDirection)
        {
            BulletShotRequested?.Invoke(shipGunPresentation.GetNewBulletSourcePoint(), lookDirection);
        }
    }
}