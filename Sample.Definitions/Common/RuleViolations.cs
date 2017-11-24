using System.Collections.Generic;

namespace Sample.Definitions.Common
{
    public class RuleViolations : Dictionary<string, List<string>>
    {
        public void AddError(string key, string error)
        {
            if (!this.ContainsKey(key))
                this[key] = new List<string> { error };
            else
                this[key].Add(error);
        }
    }
}
