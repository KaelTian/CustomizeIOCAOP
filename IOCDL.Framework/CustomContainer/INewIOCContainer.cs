namespace IOCDL.Framework
{
    public interface INewIOCContainer
    {
        void Register<TFrom, TTo>(string extension = null, object[] args = null, LifetimeType lifetimeType = LifetimeType.Transient) where TTo : TFrom;
        TFrom Resolve<TFrom>(string extension = null);
        TFrom ResolveLifetimeType<TFrom>(string extension = null);
        INewIOCContainer CloneContainer();
    }

    public enum LifetimeType
    {
        //瞬时
        Transient,
        //单例
        Singleton,
        //容器单例(多用于一个Http request过程中)
        Scoped,
        //线程单例
        PerThread,
        //外部可释放单例
    }
}
