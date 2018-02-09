using System;
using System.Threading.Tasks;

namespace YBP.Framework
{
    public interface IYbpActionBase
    {

        bool CanExecute(YbpUserContext user);
    }

    public abstract class YbpActionBase<TProcess, TParam, TResult>: IYbpActionBase
        where TProcess : YbpProcessBase, new()
    {
        internal readonly IYbpEngine _engine;

        public YbpActionBase(IYbpEngine engine)
        {
            _engine = engine;
        }

        private bool _IsSyncOverriden = true;
        private bool _IsAsyncOverriden = true;

        protected abstract Task<TResult> RunAsync(YbpContext<TProcess> context, TParam prm);

        public virtual bool CanExecute(YbpUserContext user)
        {
            return true;
        }

        const int MaxSecuentalActionRuns = 200;

        protected async Task ProcessActions(int instanceId)
        {
            int i = 0;
            bool mayRun = true;

            while (mayRun && i < MaxSecuentalActionRuns)
            {
                mayRun = await _engine.ProcessNextAction<TProcess>(instanceId);
                i++;
            }
        }
    }

}
