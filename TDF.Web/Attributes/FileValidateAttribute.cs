using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TDF.Core.Models;

namespace TDF.Web.Attributes
{

    /// <summary>
    /// 文件验证数据验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property)]
    public class FileValidateAttribute : ValidationAttribute
    {
        /// <summary>
        /// 可接收文件最大的大小单位kb，默认为1MB
        /// </summary>
        public long MaxContentLength { get; set; }

        public bool IsRequired { get; set; }

        /// <summary>
        /// 超过最大大小后的提示
        /// </summary>
        public string ContentLengthError { get; set; }

        /// <summary>
        /// 文件为空的提示
        /// </summary>
        public string EmptyError { get; set; }

        /// <summary>
        /// 文件格式有误
        /// </summary>
        public string FormatError { get; set; }

        private readonly List<string> _suffixs;

        /// <summary>
        /// 可接收的后缀
        /// </summary>
        /// <param name="suffixs">可接收的后缀,可以接收多种格式，如： jpg,png,zip</param>
        public FileValidateAttribute(string suffixs)
        {
            _suffixs = suffixs.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            _suffixs.ForEach(x =>
            {
                x = x.ToLower();
            });
            Initialize();
        }

        public override bool IsValid(object value)
        {
            if (value == null && IsRequired)
            {
                ErrorMessage = EmptyError;
                return false;
            }
            var filePosted = value as PostedFile;
            var filePosteds = value as List<PostedFile>;
            var isValid = false;
            if (filePosted != null)
            {
                return Validate(filePosted);
            }
            if (filePosteds != null && filePosteds.Any())
            {
                isValid = true;
                foreach (var item in filePosteds)
                {
                    isValid = Validate(item);
                    if (!isValid)
                    {
                        break;
                    }
                }
            }
            else if (IsRequired)
            {
                ErrorMessage = EmptyError;
            }
            return isValid;
        }

        private bool Validate(PostedFile postedFile)
        {
            if (postedFile == null && IsRequired)
            {
                ErrorMessage = EmptyError;
                return false;
            }
            else if (postedFile != null && !_suffixs.Contains(postedFile.Suffixs))
            {
                ErrorMessage = string.Format(FormatError, postedFile.Suffixs);
                return false;
            }
            else if (postedFile != null && postedFile.ContentLength > MaxContentLength * 1024)
            {
                ErrorMessage = string.Format(ContentLengthError,
                    (MaxContentLength / 1024f).ToString("##.###"));
                return false;
            }

            return true;
        }

        private void Initialize()
        {
            MaxContentLength = 1024;
            ContentLengthError = "文件大小必须小于{0}MB";
            EmptyError = "文件不能为空";
            FormatError = "上传格式有误,不支持格式{0}";
        }
    }
}
