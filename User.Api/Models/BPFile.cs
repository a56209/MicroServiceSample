using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Api.Models
{
    public class BPFile
    {
        /// <summary>
        /// BPId
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 上传的源文件地址
        /// </summary>
        public string OriginFilePath { get; set; }
        /// <summary>
        /// 格式化后的文件地址
        /// </summary>
        public string FromatFilePath { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
