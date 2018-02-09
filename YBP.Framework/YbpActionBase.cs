﻿using System;
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

        const int MaxActionRuns = 200;

        protected async Task ProcessActions(int instanceId)
        {
            int i = 0;
            bool mayRun = true;

            while (mayRun && i < MaxActionRuns)
            {
                mayRun = await _engine.ProcessNextAction<TProcess>(instanceId);
                i++;
            }
        }
    }

    public abstract class YbpFirstAction<TProcess, TParam, TResult>
        : YbpActionBase<TProcess, TParam, TResult>
        where TProcess : YbpProcessBase, new()
    {

        public YbpFirstAction(IYbpEngine engine) : base(engine)
        {
        }

        public async Task<TResult> StartAsync(TParam prm)
        {
            var result = await _engine
                .StartAsync<TProcess, TResult>(async c => await RunAsync(c, prm), this);

            await ProcessActions(result.InstanceId);

            return result.Result;
        }

    }

    public abstract class YbpAction<TProcess, TParam, TResult>
        : YbpActionBase<TProcess, TParam, TResult>
        where TProcess : YbpProcessBase, new()
    {

        public YbpAction(IYbpEngine engine) : base(engine)
        {
        }

        public async Task<TResult> ExecAsync(string id, TParam prm)
        {
            var result = await _engine
                .ExecAsync<TProcess, TResult>(id, async c => await RunAsync(c, prm), this);

            await ProcessActions(result.InstanceId);

            return result.Result;
        }

    }


    public abstract class YbpFirstAction<TProcess, TParam> : YbpFirstAction<TProcess, TParam, string>
        where TProcess : YbpProcessBase, new()
    {
        public YbpFirstAction(IYbpEngine engine) : base(engine)
        {
        }
    }

    public abstract class YbpFirstAction<TProcess> : YbpFirstAction<TProcess, string, string>
        where TProcess : YbpProcessBase, new()
    {
        public YbpFirstAction(IYbpEngine engine) : base(engine)
        {
        }
    }


    public abstract class YbpAction<TProcess, TParam> : YbpAction<TProcess, TParam, string>
        where TProcess : YbpProcessBase, new()
    {
        public YbpAction(IYbpEngine engine) : base(engine)
        {
        }
    }

    public abstract class YbpAction<TProcess> : YbpAction<TProcess, string, string>
        where TProcess : YbpProcessBase, new()
    {

        public YbpAction(IYbpEngine engine) : base(engine)
        {
        }

        internal async Task<string> Run(YbpContext<TProcess> ctx)
        {
            await RunAsync(ctx, null);
            return null;
        }
    }

}
