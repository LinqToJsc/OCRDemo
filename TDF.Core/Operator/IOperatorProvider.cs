namespace TDF.Core.Operator
{
    /// <summary>
    /// Mvc操作人员控制器，用于后台身份认证
    /// </summary>
    public interface IOperatorProvider
    {
        /// <summary>
        /// 获得当前登录的管理员身份信息
        /// </summary>
        /// <returns></returns>
        T GetCurrent<T>() where T : OperatorModel;

        /// <summary>
        /// 获得当前登录的管理员身份信息
        /// </summary>
        /// <returns></returns>
        OperatorModel GetCurrent();

        /// <summary>
        /// 添加登录用户
        /// </summary>
        /// <param name="operatorModel"></param>
        void AddCurrent(OperatorModel operatorModel);

        /// <summary>
        /// 添加登录用户
        /// </summary>
        /// <param name="operatorModel"></param>
        void AddCurrent<T>(T operatorModel) where T : OperatorModel, new();

        /// <summary>
        /// 移除当前登录用户
        /// </summary>
        void RemoveCurrent();
    }
}
