namespace StyleCop.Analyzers.Generator
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class RuleNotFoundException : Exception
    {
        public RuleNotFoundException()
            : base("Rule not found or status code different than 200 received!")
        { }
        public RuleNotFoundException(string message) : base(message) { }
        public RuleNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected RuleNotFoundException(
          SerializationInfo info,
          StreamingContext context) : base(info, context)
        { }
    }
}
