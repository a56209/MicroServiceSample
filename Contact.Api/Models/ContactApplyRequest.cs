using Contact.Api.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.Api.Models
{
    public class ContactApplyRequest
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 申请人ID
        /// </summary>
        public int ApplierId { get; set; }
        public ApplyStatus Approvaled { get; set; }
        public DateTime HandleTime { get; set; }
        public DateTime ApplyTime { get; set; }
    }
}
