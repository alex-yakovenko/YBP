using System;

namespace YBP.Framework
{
    public abstract class YbpActionDefinitionBase<TAction>: IYbpActionDefinition
        where TAction: IYbpActionBase
    {
        public bool CanBeExecutedAutomatically { get; set; }

        public bool RunOnlyOnce { get; set; }

        public Func<YbpFlagsDictionary, bool> NeedsToBeExecuted { get; set; }

        public Func<YbpFlagsDictionary, bool> MayBeExecuted { get; set; }

        public Func<YbpFlagsDictionary, bool> MayNotBeExecuted { get; set; }

        public Type ActionType => typeof(TAction);
    }

}