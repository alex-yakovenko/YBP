using System.Threading.Tasks;

namespace YBP.Framework
{
    public abstract class YbpAction<TProcess, TParam> : YbpAction<TProcess, TParam, string>
        where TProcess : YbpProcessBase, new()
    {
        public YbpAction(IYbpEngine engine) : base(engine)
        {
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
