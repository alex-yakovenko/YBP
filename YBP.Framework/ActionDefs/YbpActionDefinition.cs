namespace YBP.Framework
{

    public class YbpActionDefinition<TAction>: YbpActionDefinitionBase<TAction>
        where TAction : IYbpActionBase
    {
        public YbpActionDefinition()
        {
            NeedsToBeExecuted = (f) => true;
            MayNotBeExecuted = (f) => false;
        }
    }

}