using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using TDF.Core.Exceptions;

namespace TDF.Web.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        #region MVC

        public static List<string> GetErrorMessages(this System.Web.Mvc.ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                return new List<string>();
            }
            return modelState.Where(x => x.Value.Errors.Count > 0)
                .Select(
                    x =>
                        string.Format("字段[{0}]=>错误:{1}", x.Key,
                            string.Join(";", x.Value.Errors.Select(t => t.ErrorMessage)))).ToList();
        }

        public static Dictionary<string, string> GetErrors(this System.Web.Mvc.ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                return new Dictionary<string, string>();
            }
            return modelState.Where(x => x.Value.Errors.Count > 0).ToDictionary(x => x.Key, v => v.Value.Errors.FirstOrDefault().ErrorMessage);

        }

        public static string GetFirstError(this System.Web.Mvc.ModelStateDictionary modelState)
        {
            var firstOrDefault = modelState.FirstOrDefault(x => x.Value.Errors.Count > 0).Value.Errors.FirstOrDefault();
            if (firstOrDefault != null)
                return firstOrDefault.ErrorMessage;
            return string.Empty;
        }

        public static void Validate(this System.Web.Mvc.ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                return;
            }
            throw new KnownException(GetFirstError(modelState));

        }

        #endregion

        #region WebAPI
        
        public static List<string> GetErrorMessages(this System.Web.Http.ModelBinding.ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                return new List<string>();
            }
            return modelState.Where(x => x.Value.Errors.Count > 0)
                .Select(
                    x =>
                        string.Format("字段[{0}]=>错误:{1}", x.Key,
                            string.Join(";", x.Value.Errors.Select(t => t.ErrorMessage)))).ToList();
        }

        public static Dictionary<string, string> GetErrors(this System.Web.Http.ModelBinding.ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                return new Dictionary<string, string>();
            }
            return modelState.Where(x => x.Value.Errors.Count > 0).ToDictionary(x => x.Key, v => v.Value.Errors.FirstOrDefault().ErrorMessage);

        }

        public static string GetFirstError(this System.Web.Http.ModelBinding.ModelStateDictionary modelState)
        {
            var firstOrDefault = modelState.FirstOrDefault(x => x.Value.Errors.Count > 0).Value.Errors.FirstOrDefault();
            if (firstOrDefault != null)
                return firstOrDefault.ErrorMessage;
            return string.Empty;
        }

        public static void Validate(this System.Web.Http.ModelBinding.ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                return;
            }
            throw new KnownException(GetFirstError(modelState));

        }

        #endregion
    }
}
