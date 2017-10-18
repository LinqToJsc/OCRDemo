using System.Text;
using System.Web;
using System.Web.Mvc;
using TDF.Core.Json;
using TDF.Core.Models;

namespace TDF.Web.Models
{
    public class JsonResult : ActionResult, IResult
    {
        public string ContentType { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }

        public void Succeed()
        {
            Success = true;
        }

        public void Fail()
        {
            Success = false;
        }

        public void Succeed(string message)
        {
            Success = true;
            Message = message;
        }

        public void Fail(string message)
        {
            Success = false;
            Message = message;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            WriteToResponse(response);
        }

        protected virtual IResult ToCustomResult()
        {
            var result = new Result
            {
                Success = Success,
                Message = Message
            };
            return result;
        }

        public void WriteToResponse(HttpResponseBase response)
        {
            if (string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = "application/json";
            }
            else
            {
                response.ContentType = this.ContentType;
            }
            response.ContentEncoding = Encoding.UTF8;
            response.Write(ToCustomResult().ToJson());
        }
    }

    public class JsonResult<T> : JsonResult, IResult<T>
    {
        public T Value { get; set; }

        protected override IResult ToCustomResult()
        {
            var result = new Result<T>
            {
                Success = Success,
                Message = Message,
                Value = Value
            };
            return result;
        }
    }
}
