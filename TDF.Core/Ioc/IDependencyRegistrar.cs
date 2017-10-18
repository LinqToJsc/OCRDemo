using Autofac;
using TDF.Core.Reflection;

namespace TDF.Core.Ioc
{
    /// <summary>
    /// 依赖注册接口，实现此接口可进行自动注册
    /// </summary>
    public interface IDependencyRegistrar
    {
        /// <summary>
        /// 注册服务和接口
        /// </summary>
        void Register(ContainerBuilder builder, ITypeFinder typeFinder);

        int Order { get; }
    }
}
