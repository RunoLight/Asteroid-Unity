using UnityEngine;

namespace Factory
{
    public sealed class MonoMemoryPool<TValue> : MemoryPool<TValue> where TValue : MonoBehaviour
    {
        public MonoMemoryPool(IFactory<TValue> factory, MemoryPoolSettings memoryPoolSettings)
            : base(factory, memoryPoolSettings)
        {
        }

        protected override void Reinitialize(TValue item)
        {
            base.Reinitialize(item);
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(TValue item)
        {
            base.OnDespawned(item);
            item.gameObject.SetActive(false);
        }
    }
}