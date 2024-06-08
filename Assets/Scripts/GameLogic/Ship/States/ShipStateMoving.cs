using UnityEngine;

namespace Asteroid.GameLogic.Ship.States
{
    public sealed class ShipStateMoving : ShipState
    {
        private readonly ShipController ship;
        private readonly ShipGunController defaultGun;
        private readonly ShipLaserGunController laserGun;

        private Vector2 velocity = Vector2.zero;
        private float lookDirection;

        private bool inputForward;
        private float inputRotation; /* Negative is Left, Positive is Right */
        private bool inputUseLaser;

        public ShipStateMoving(ShipController ship, ShipGunController defaultGun, ShipLaserGunController laserGun)
        {
            this.ship = ship;
            this.defaultGun = defaultGun;
            this.laserGun = laserGun;

            ship.Position = Vector2.zero;

            laserGun.OnDisabled += defaultGun.Enable;

            defaultGun.Enable();
        }

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

            ship.Rotation = Quaternion.Euler(0, 0, -lookDirection);
            ship.Velocity = velocity;
        }

        public override void Start()
        {
            ship.SetEngineParticles(true);
        }

        public override void Dispose()
        {
            base.Dispose();

            laserGun.OnDisabled -= defaultGun.Enable;
        }

        public override void OnCollision()
        {
            ship.ChangeState(ShipStates.Dead);
        }
    }
}