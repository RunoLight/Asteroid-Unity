namespace Factory
{
    public class MemoryPool<TValue> : MemoryPoolBase<TValue>, IMemoryPool<TValue>, IFactory<TValue>
    {
        public MemoryPool(IFactory<TValue> factory, MemoryPoolSettings memoryPoolSettings) :
            base(factory, memoryPoolSettings)
        {
        }

        public TValue Spawn()
        {
            var item = GetInternal();
            Reinitialize(item);
            return item;
        }

        protected virtual void Reinitialize(TValue item)
        {
            // Optional
        }

        TValue IFactory<TValue>.Create()
        {
            return Spawn();
        }
    }
}