using System;
using System.Threading.Tasks;

namespace YBP.Framework
{
    public interface IYbpActionBase
    {
        bool CanBeExecutedAutomatically { get; }
        Func<YbpFlagsDictionary, bool> NeedsToBeExecuted { get; }

        Func<YbpFlagsDictionary, bool> MayToBeExecuted { get; }

        Func<YbpFlagsDictionary, bool> CannotToBeExecuted { get;  }

        bool CanExecute(YbpUserContext user);
    }

    public abstract class YbpActionBase<TProcess, TParam, TResult>: IYbpActionBase
        where TProcess : YbpProcessBase, new()
    {
        internal readonly IYbpEngine _engine;

        public abstract Func<YbpFlagsDictionary, bool> NeedsToBeExecuted { get; }
        public virtual Func<YbpFlagsDictionary, bool> MayToBeExecuted => NeedsToBeExecuted;
        public abstract Func<YbpFlagsDictionary, bool> CannotToBeExecuted { get; }
        public virtual bool CanBeExecutedAutomatically => false;


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
    }

    public abstract class YbpFirstAction<TProcess, TParam, TResult>
        : YbpActionBase<TProcess, TParam, TResult>
        where TProcess : YbpProcessBase, new()
    {

        public override Func<YbpFlagsDictionary, bool> NeedsToBeExecuted => flags => true;

        public override Func<YbpFlagsDictionary, bool> MayToBeExecuted => flags => true;

        public override Func<YbpFlagsDictionary, bool> CannotToBeExecuted => flags => false;


        public YbpFirstAction(IYbpEngine engine) : base(engine)
        {
        }

        public async Task<TResult> StartAsync(TParam prm)
        {
            return await _engine
                .StartAsync<TProcess, TResult>(async c => await RunAsync(c, prm), this);
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
            return await _engine
                .ExecAsync<TProcess, TResult>(id, async c => await RunAsync(c, prm), this);
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
        public override bool CanBeExecutedAutomatically => true;

        public YbpAction(IYbpEngine engine) : base(engine)
        {
        }
    }

}
