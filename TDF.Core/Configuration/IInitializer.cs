namespace TDF.Core.Configuration
{
    /// <summary>
    /// 配置初始化接口
    /// </summary>
    public interface IInitializer
    {
        int Order { get; }

        /// <summary>
        /// 初始化的组件名称
        /// </summary>
        string ComponentName { get; }

        void Initialize();
    }
}
