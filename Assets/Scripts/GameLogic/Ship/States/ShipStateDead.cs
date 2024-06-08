using System;
using Asteroid.GameLogic.Factories;
using Asteroid.Presentation.Entity.Ship.Abstractions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroid.GameLogic.Ship.States
{
    public class ShipStateDead : ShipState
    {
        private readonly Settings settings;
        private readonly ShipController ship;
        private readonly IFactory<IShipExplosion> explosionFactory;
        private readonly IFactory<IBrokenShip> brokenShipFactory;

        private IBrokenShip brokenShip;
        private IShipExplosion explosion;

        public ShipStateDead(
            Settings settings, ShipController ship,
            IFactory<IShipExplosion> explosionFactory,
            IFactory<IBrokenShip> brokenShipFactory
        )
        {
            this.settings = settings;
            this.ship = ship;
            this.explosionFactory = explosionFactory;
            this.brokenShipFactory = brokenShipFactory;
        }

        public override void FixedTick(float fixedDeltaTime)
        {
        }

        public override void Start()
        {
            ship.Enabled = false;
            ship.SetEngineParticles(false);

            ship.Die();

            explosion = explosionFactory.Create();
            explosion.Setup(ship.Position);

            brokenShip = brokenShipFactory.Create();

            var randomTheta = Random.Range(0, Mathf.PI * 2.0f);
            var randomDir = new Vector3(Mathf.Cos(randomTheta), Mathf.Sin(randomTheta), 0);

            brokenShip.Setup(ship.Position, ship.Rotation, randomDir * settings.explosionForce);
        }

        public override void Dispose()
        {
            ship.Enabled = true;

            if (!explosion.IsDestroyedViaLifetimeExpiration)
                explosion.Destroy();
            brokenShip.Destroy();
        }

        public override void Tick(float deltaTime)
        {
        }

        [Serializable]
        public class Settings
        {
            public float explosionForce;
        }
    }
}