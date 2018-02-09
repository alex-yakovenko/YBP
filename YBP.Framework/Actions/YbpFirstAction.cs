using System.Threading.Tasks;

namespace YBP.Framework
{

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

}
