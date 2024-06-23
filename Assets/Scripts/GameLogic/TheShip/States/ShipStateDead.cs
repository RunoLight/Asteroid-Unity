using System;
using Asteroid.Presentation.Entity.Ship.Abstractions;
using Factory;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroid.GameLogic.TheShip.States
{
    public class ShipStateDead : ShipState
    {
        private readonly Settings settings;
        private readonly Ship ship;
        private readonly IFactory<IShipExplosion> explosionFactory;
        private readonly IFactory<IBrokenShip> brokenShipFactory;

        private IBrokenShip brokenShip;
        private IShipExplosion explosion;

        public ShipStateDead(
            Ship ship, Settings settings,
            IFactory<IShipExplosion> explosionFactory,
            IFactory<IBrokenShip> brokenShipFactory
        )
        {
            this.settings = settings;
            this.ship = ship;
            this.explosionFactory = explosionFactory;
            this.brokenShipFactory = brokenShipFactory;
        }

        public override void Dispose()
        {
            ship.Enabled = true;

            if (!explosion.IsDestroyedViaLifetimeExpiration)
                explosion.Destroy();
            brokenShip.Destroy();
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

            brokenShip.Setup(ship.Position, ship.Value, randomDir * settings.explosionForce);
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void FixedTick(float fixedDeltaTime)
        {
        }

        [Serializable]
        public class Settings
        {
            public static Settings Default = new() { explosionForce = 1f };

            public float explosionForce;
        }
    }
}