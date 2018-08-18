using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.AggregatesModel
{
    public class ProjectVisibleRule:Entity
    {
        public int ProjectId { get; set; }
        public bool Visible { get; set; }
        /// <summary>
        /// 该标签下用户是否可见
        /// </summary>
        public string Tags { get; set; }
    }
}
