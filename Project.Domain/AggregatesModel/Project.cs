using System;
using System.Collections.Generic;
using System.Text;
using Project.Domain.SeedWork;
using System.Linq;
using Project.Domain.Events;

namespace Project.Domain.AggregatesModel
{
    public class Project : Entity,IAggregateRoot
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 项目logo
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 原BP文件地址
        /// </summary>
        public string OriginBPFile { get; set; }
        /// <summary>
        /// 转换后的BP文件地址
        /// </summary>
        public string FormatBPFile { get; set; }
        /// <summary>
        /// 是否显示敏感信息
        /// </summary>
        public bool ShowSecurityInfo { get; set; }
        /// <summary>
        /// 公司所在省Id
        /// </summary>
        public int ProvinceId { get; set; }
        /// <summary>
        /// 公司所在省名称
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 公司所在城市Id
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 公司所在城市名称
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区域Id
        /// </summary>
        public int AreaId { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 公司成立时间
        /// </summary>
        public DateTime RegisterTime { get; set; }
        /// <summary>
        /// 项目基本信息
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 出让股票比例
        /// </summary>
        public string FinPercentage { get; set; }
        /// <summary>
        /// 融资阶段 
        /// </summary>
        public string FinStage { get; set; }
        /// <summary>
        ///融资金额 单位（万）
        /// </summary>
        public int FinMoney { get; set; }
        /// <summary>
        /// 收入 单位（万）
        /// </summary>
        public int Incom { get; set; }
        /// <summary>
        /// 利润 单位（万）
        /// </summary>
        public int Revenue { get; set; }
        /// <summary>
        /// 估值 单位（万）
        /// </summary>
        public int valuation { get; set; }
        /// <summary>
        /// 佣金分配方式
        /// </summary>
        public int BrokerageOpints { get; set; }
        /// <summary>
        /// 是否委托平台
        /// </summary>
        public bool OnPlatform { get; set; }
        /// <summary>
        /// 可见范围
        /// </summary>
        public ProjectVisibleRule VisibleRule { get; set; }
        /// <summary>
        /// 根引用项目Id（源头）
        /// </summary>
        public int SourceId { get; set; }
        /// <summary>
        /// 上级引用项目Id
        /// </summary>
        public int ReferenceId { get; set; }
        public string Tags { get; set; }
        /// <summary>
        /// 项目属性：行业领域、融资币种
        /// </summary>
        public List<ProjectProperty> Properties { get; set; }
        /// <summary>
        /// 贡献者列表
        /// </summary>
        public List<ProjectContributor> Contributors { get; set; }
        /// <summary>
        /// 察看者
        /// </summary>
        public List<ProjectViewer> Viewers { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }

        public Project()
        {
            this.Viewers = new List<ProjectViewer>();
            this.Contributors = new List<ProjectContributor>();

            AddDomainEvent(new ProjectCreatedEvent { Project = this });
        }

        public void AddViewer(int userId,string userName,string avatar)
        {
            var viewer = new ProjectViewer
            {
                UserId = userId,
                UserName = userName,
                Avatar = avatar,
                CreateTime = DateTime.Now
            };

            if(! Viewers.Any(v => v.UserId == userId))
            {
                Viewers.Add(viewer);

                AddDomainEvent(new ProjectViewedEvent { Viewer = viewer });
            }                       
        }

        public void AddContributor(ProjectContributor contributor)
        {
            if (!Contributors.Any(v => v.UserId == contributor.UserId))
            {
                Contributors.Add(contributor);

                AddDomainEvent(new ProjectJoinedEvent { Contributor = contributor});
            }            
        }
    }
}
