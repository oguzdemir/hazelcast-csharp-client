using Hazelcast.IO.Serialization;

namespace Hazelcast.Serialization.Hook
{
    public sealed class CountDownLatchDataSerializerHook : DataSerializerHook
    {
        public static readonly int FId = FactoryIdHelper.GetFactoryId(FactoryIdHelper.CdlPortableFactory, -14);

        public int GetFactoryId()
        {
            return FId;
        }

        public IDataSerializableFactory CreateFactory()
        {
            return null;
        }
    }
}