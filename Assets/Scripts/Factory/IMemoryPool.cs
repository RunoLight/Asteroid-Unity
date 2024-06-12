using System;

namespace Factory
{
    public interface IMemoryPool
    {
        int CountTotal { get; }
        int CountActive { get; }
        int CountInactive { get; }

        Type ItemType { get; }

        /// <summary>
        /// Changes pool size by creating new elements or destroying existing elements
        /// This bypasses the configured expansion method (OneAtATime or Doubling)
        /// </summary>
        void Resize(int desiredPoolSize);

        void Clear();

        /// <summary>
        /// Expands the pool by the additional size.
        /// This bypasses the configured expansion method (OneAtATime or Doubling)
        /// </summary>
        /// <param name="amountToAdd">The additional number of items to allocate in the pool</param>
        void ExpandBy(int amountToAdd);

        /// <summary>
        /// Shrinks the MemoryPool by removing a given number of elements
        /// This bypasses the configured expansion method (OneAtATime or Doubling)
        /// </summary>
        /// <param name="amountToRemove">The amount of items to remove from the pool</param>
        void ShrinkBy(int amountToRemove);

        void Despawn(object obj);
    }

    public interface IMemoryPool<TValue> : IMemoryPool
    {
        TValue Spawn();
        void Despawn(TValue item);
    }
}