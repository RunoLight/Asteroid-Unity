using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Factory
{
    public enum PoolExpandMethods
    {
        OneAtATime,
        Double,
        Disabled
    }

    public class PoolExceededFixedSizeException : Exception
    {
        public PoolExceededFixedSizeException(string errorMessage) : base(errorMessage)
        {
        }
    }

    [Serializable]
    public class MemoryPoolSettings
    {
        public int initialSize;
        public int maxSize;
        public PoolExpandMethods expandMethod;

        public MemoryPoolSettings()
        {
            initialSize = 0;
            maxSize = int.MaxValue;
            expandMethod = PoolExpandMethods.OneAtATime;
        }

        public MemoryPoolSettings(int initialSize, int maxSize, PoolExpandMethods expandMethod)
        {
            this.initialSize = initialSize;
            this.maxSize = maxSize;
            this.expandMethod = expandMethod;
        }

        public static readonly MemoryPoolSettings _default = new();
    }

    public abstract class MemoryPoolBase<TContract> : IMemoryPool, IDisposable
    {
        Stack<TContract> inactiveItems;
        IFactory<TContract> factory;
        MemoryPoolSettings settings;

        protected MemoryPoolBase(IFactory<TContract> factory, MemoryPoolSettings settings)
        {
            this.factory = factory;
            this.settings = settings;

            Construct();
        }

        public void Construct()
        {
            inactiveItems = new Stack<TContract>(settings.initialSize);

            for (int i = 0; i < settings.initialSize; i++)
            {
                inactiveItems.Push(AllocNew());
            }
        }

        public IEnumerable<TContract> InactiveItems => inactiveItems;

        public int CountTotal => CountInactive + CountActive;

        public int CountInactive => inactiveItems.Count;

        public int CountActive { get; private set; }

        public Type ItemType => typeof(TContract);

        public void Dispose()
        {
        }

        void IMemoryPool.Despawn(object item)
        {
            Despawn((TContract)item);
        }

        public void Despawn(TContract item)
        {
            Assert.IsTrue(
                !inactiveItems.Contains(item), $"Tried to return an item to pool {GetType()} twice"
            );

            CountActive--;
            inactiveItems.Push(item);

            OnDespawned(item);

            if (inactiveItems.Count > settings.maxSize)
            {
                Resize(settings.maxSize);
            }
        }

        private TContract AllocNew()
        {
            try
            {
                var item = factory.Create();

                Assert.IsTrue(
                    item != null,
                    $"Factory {factory.GetType()} returned null value when creating via {GetType()}!"
                );
                OnCreated(item);

                return item;
            }
            catch (Exception e)
            {
                throw new Exception(
                    $"Error during construction of type {typeof(TContract)} via provided IFactory, {e.Message}"
                );
            }
        }

        public void Clear()
        {
            Resize(0);
        }

        public void ShrinkBy(int amountToRemove)
        {
            Resize(inactiveItems.Count - amountToRemove);
        }

        public void ExpandBy(int amountToAdd)
        {
            Resize(inactiveItems.Count + amountToAdd);
        }

        protected TContract GetInternal()
        {
            if (inactiveItems.Count == 0)
            {
                ExpandPool();
                Assert.IsTrue(inactiveItems.Count != 0);
            }

            var item = inactiveItems.Pop();
            CountActive++;
            OnSpawned(item);
            return item;
        }

        public void Resize(int desiredPoolSize)
        {
            if (inactiveItems.Count == desiredPoolSize)
            {
                return;
            }

            if (settings.expandMethod == PoolExpandMethods.Disabled)
            {
                throw new PoolExceededFixedSizeException(
                    $"Pool factory {GetType()} attempted resize but pool set to fixed size of {inactiveItems.Count}!"
                );
            }

            Assert.IsTrue(desiredPoolSize >= 0, "Attempted to resize the pool to a negative amount");

            while (inactiveItems.Count > desiredPoolSize)
            {
                OnDestroyed(inactiveItems.Pop());
            }

            while (desiredPoolSize > inactiveItems.Count)
            {
                inactiveItems.Push(AllocNew());
            }

            Assert.IsTrue(inactiveItems.Count == desiredPoolSize);
        }

        private void ExpandPool()
        {
            switch (settings.expandMethod)
            {
                case PoolExpandMethods.Disabled:
                {
                    throw new PoolExceededFixedSizeException(
                        $"Pool factory {GetType()} exceeded its fixed size of '{inactiveItems.Count}'!"
                    );
                }
                case PoolExpandMethods.OneAtATime:
                {
                    ExpandBy(1);
                    break;
                }
                case PoolExpandMethods.Double:
                {
                    ExpandBy(CountTotal == 0 ? 1 : CountTotal);

                    break;
                }
                default:
                {
                    throw new NotSupportedException();
                }
            }
        }

        protected virtual void OnDespawned(TContract item)
        {
            // Optional
        }

        protected virtual void OnSpawned(TContract item)
        {
            // Optional
        }

        protected virtual void OnCreated(TContract item)
        {
            // Optional
        }

        protected virtual void OnDestroyed(TContract item)
        {
            // Optional
        }
    }
}