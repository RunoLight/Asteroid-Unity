using System;
using Asteroid.Presentation.Entity.Ship.Abstractions;

namespace Asteroid.GameLogic.Ship
{
    public class ShipLaserGunController
    {
        public event Action OnDisabled;
        public event Action<int, float> OnCooldownChanged;

        public bool IsEnabled { get; private set; } 
        public bool IsReady => charges > 0;

        private readonly IShipLaserGun shipLaserGunPresentation;

        private int charges = 0;
        private const int MaxCharges = 3;
        private float remainTime = 1f;
        private float remainCooldownTime = 1f;
        private const float TotalTime = 1f;
        private const float TotalCooldownTime = 4f;

        public ShipLaserGunController(IShipLaserGun shipLaserGunPresentation)
        {
            this.shipLaserGunPresentation = shipLaserGunPresentation;
        }

        public void EnableIfReadyAndShoot()
        {
            if (!IsReady)
            {
                return;
            }

            charges--;
            IsEnabled = true;
            remainTime = TotalTime;
            remainCooldownTime = TotalCooldownTime;
            shipLaserGunPresentation.SetLaserBeamActive(true);
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
            
            if (IsEnabled)
            {
                remainTime -= deltaTime;
                if (remainTime > 0f)
                    return;

                IsEnabled = false;
                shipLaserGunPresentation.SetLaserBeamActive(false);

                OnDisabled?.Invoke();
            }
        }
    }
}