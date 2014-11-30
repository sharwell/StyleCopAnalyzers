namespace StyleCop.Analyzers.Templates.Wizard
{
    using System.Collections.Generic;

    public class PageInfo
    {
        public PageInfo()
        {
            this.Cause = new List<Line>();
            this.RuleDescription = new List<Line>();
        }

        public string TypeName { get; set; }
        public string CheckId { get; set; }
        public string Category { get; set; }
        public List<Line> Cause { get; set; }
        public List<Line> RuleDescription { get; set; }
        public string Link { get; set; }
    }
}
