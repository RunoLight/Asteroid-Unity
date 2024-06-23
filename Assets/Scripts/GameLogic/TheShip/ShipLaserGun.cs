using System;
using Asteroid.Presentation.Entity.Ship.Abstractions;

namespace Asteroid.GameLogic.TheShip
{
    public class ShipLaserGun
    {
        public event Action OnDisabled;
        public event Action<int, float> OnCooldownChanged;

        public bool IsEnabled { get; private set; }
        public bool IsReady => charges > 0;

        private readonly IShipLaserGunPresentation shipLaserGunPresentationPresentation;

        private int charges = 0;

        private float remainUsageTime = 1f;
        private float remainCooldownTime = 1f;

        private const int MaxCharges = 3;
        private const float TotalUsageTimePerCharge = 1f;
        private const float TotalCooldownTime = 4f;

        public ShipLaserGun(IShipLaserGunPresentation shipLaserGunPresentationPresentation)
        {
            this.shipLaserGunPresentationPresentation = shipLaserGunPresentationPresentation;
        }

        public void EnableIfReadyAndShoot()
        {
            if (!IsReady)
                return;

            charges--;
            IsEnabled = true;
            remainUsageTime = TotalUsageTimePerCharge;
            remainCooldownTime = TotalCooldownTime;
            shipLaserGunPresentationPresentation.SetLaserBeamActive(true);
            OnCooldownChanged?.Invoke(charges, remainCooldownTime / TotalCooldownTime);
        }

        public void Tick(float deltaTime)
        {
            if (charges != MaxCharges && !IsEnabled)
            {
                remainCooldownTime -= deltaTime;
                if (remainCooldownTime <= 0f)
                {
                    charges++;
                    remainCooldownTime = charges == MaxCharges ? 0f : TotalCooldownTime;
                }

                OnCooldownChanged?.Invoke(charges, remainCooldownTime / TotalCooldownTime);
            }

            if (!IsEnabled)
                return;

            remainUsageTime -= deltaTime;
            if (remainUsageTime > 0f)
                return;

            IsEnabled = false;
            shipLaserGunPresentationPresentation.SetLaserBeamActive(false);

            OnDisabled?.Invoke();
        }
    }
}