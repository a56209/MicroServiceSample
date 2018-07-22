using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.Api.Models.Enum
{
    public enum ApplyStatus
    {
        /// <summary>
        /// 待处理
        /// </summary>
        Waiting = 0,
        /// <summary>
        /// 通过
        /// </summary>
        Passed =1,
        /// <summary>
        /// 拒绝
        /// </summary>
        Reject =2
    } 
}
