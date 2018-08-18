using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.AggregatesModel
{
    public class ProjectProperty:ValueObject
    {
        public int ProjectId { get; set; }
        public string Key { set; get; }
        public string Value { get; set; }
        public string Text { get; set; }

        public ProjectProperty(int projectId,string key, string value, string text)
        {
            ProjectId = projectId;
            Key = key;
            Value = value;
            Text = text;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Key;
            yield return Text;
            yield return Value;
        }
    }
}
