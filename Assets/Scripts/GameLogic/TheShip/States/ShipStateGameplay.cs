using UnityEngine;

namespace Asteroid.GameLogic.TheShip.States
{
    public sealed class ShipStateGameplay : ShipState
    {
        private readonly Ship ship;
        private readonly ShipBulletGuns defaultGun;
        private readonly ShipLaserGun laserGun;

        private Vector2 velocity = Vector2.zero;
        private float lookDirection;

        private bool inputForward;
        private float inputRotation; /* Negative is Left, Positive is Right */
        private bool inputUseLaser;

        public ShipStateGameplay(Ship ship, ShipBulletGuns defaultGun, ShipLaserGun laserGun)
        {
            this.ship = ship;
            this.defaultGun = defaultGun;
            this.laserGun = laserGun;
        }

        public override void Dispose()
        {
            base.Dispose();

            laserGun.OnDisabled -= defaultGun.Enable;
        }

        public override void Start()
        {
            ship.Position = Vector2.zero;
            defaultGun.Enable();

            ship.SetEngineParticles(true);

            laserGun.OnDisabled += defaultGun.Enable;
        }

        public override void Tick(float deltaTime)
        {
            laserGun.Tick(deltaTime);
            defaultGun.Tick(deltaTime);

            if (!inputUseLaser || !laserGun.IsReady || laserGun.IsEnabled)
                return;

            laserGun.EnableIfReadyAndShoot();
            defaultGun.Disable();
            defaultGun.Reset();
        }

        public override void FixedTick(float fixedDeltaTime)
        {
            const float speedIncreaseForward = 0.12f;
            const float speedIncreaseSteering = 0.06f;
            const float steeringAnglePerTick = 150f;
            const float momentumSavingPerTick = 0.98f;

            velocity *= momentumSavingPerTick;

            float radians = lookDirection * Mathf.Deg2Rad;
            var v = new Vector2(Mathf.Sin(radians), Mathf.Cos(radians));

            if (inputRotation != 0f)
            {
                velocity += v * speedIncreaseSteering;

                var rotationSide = inputRotation < 0 ? -1 : 1;

                lookDirection += rotationSide * steeringAnglePerTick * fixedDeltaTime;
            }
            else if (inputForward)
            {
                velocity += v * speedIncreaseForward;
            }

            ship.Value = Quaternion.Euler(0, 0, -lookDirection);
            ship.Velocity = velocity;
        }

        public override void OnCollision()
        {
            ship.ChangeState(Ship.StateGameOver);
        }

        #region Input Subscribers

        public void OnInputForward(bool isActive)
        {
            inputForward = isActive;
        }

        public void OnInputRotate(float direction)
        {
            inputRotation = direction;
        }

        public void OnInputUseLaser(bool isActive)
        {
            inputUseLaser = isActive;
        }

        #endregion
    }
}