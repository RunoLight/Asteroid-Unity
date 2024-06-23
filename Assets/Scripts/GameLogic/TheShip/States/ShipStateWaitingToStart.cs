using System;
using UnityEngine;

namespace Asteroid.GameLogic.TheShip.States
{
    public class ShipStateWaitingToStart : ShipState
    {
        private readonly Settings settings;
        private readonly Ship ship;

        private float theta;

        public ShipStateWaitingToStart(Ship ship, Settings settings)
        {
            this.settings = settings;
            this.ship = ship;
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

        public override void FixedTick(float fixedDeltaTime)
        {
        }

        [Serializable]
        public class Settings
        {
            public static Settings Default = new()
            {
                amplitude = 0.5f,
                frequency = 1f,
                startOffset = Vector3.zero
            };

            public Vector3 startOffset;
            public float amplitude;
            public float frequency;
        }
    }
}