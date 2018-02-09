namespace YBP.Framework
{
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