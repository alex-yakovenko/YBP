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

    public class YbpFirstActionDefinition<TAction> : YbpActionDefinitionBase<TAction>
        where TAction : IYbpActionBase
    {
        public YbpFirstActionDefinition()
        {
            NeedsToBeExecuted = (f) => true;
            MayBeExecuted = (f) => NeedsToBeExecuted(f);
            MayNotBeExecuted = (f) => false;
        }
    }


    public class YbpActionDefinition<TAction>: YbpActionDefinitionBase<TAction>
        where TAction : IYbpActionBase
    {
        public YbpActionDefinition()
        {
            NeedsToBeExecuted = (f) => true;
            MayNotBeExecuted = (f) => false;
        }
    }


    public class YbpAutomaticActionDefinition<TAction> : YbpActionDefinitionBase<TAction>
        where TAction : IYbpActionBase
    {
        public YbpAutomaticActionDefinition()
        {
            RunOnlyOnce = true;
            CanBeExecutedAutomatically = true;
            MayNotBeExecuted = (f) => RunOnlyOnce && f.AlreadyExecuted<TAction>();
        }
    }

}