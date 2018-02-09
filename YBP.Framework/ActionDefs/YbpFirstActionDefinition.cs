namespace YBP.Framework
{
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

}