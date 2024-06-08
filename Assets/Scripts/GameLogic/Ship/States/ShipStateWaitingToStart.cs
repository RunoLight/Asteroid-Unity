using System;
using UnityEngine;

namespace Asteroid.GameLogic.Ship.States
{
    public class ShipStateWaitingToStart : ShipState
    {
        private readonly Settings settings;
        private readonly ShipController ship;

        private float theta;

        public ShipStateWaitingToStart(ShipController ship, Settings settings)
        {
            this.settings = settings;
            this.ship = ship;
        }

        public override void FixedTick(float fixedDeltaTime)
        {
        }

        public override void Start()
        {
            ship.Position = settings.startOffset;
        }

        public override void Tick(float deltaTime)
        {
            ship.Velocity = Vector2.zero;
            ship.Position = settings.startOffset + Vector3.up * (settings.amplitude * Mathf.Sin(theta));
            theta += deltaTime * settings.frequency;
        }

        [Serializable]
        public class Settings
        {
            public Vector3 startOffset;
            public float amplitude;
            public float frequency;
        }
    }
}