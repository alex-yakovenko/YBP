using System;
using System.Threading.Tasks;

namespace YBP.Framework
{
    public interface IYbpEngine
    {

        Task<TResult> StartAsync<TProcess, TResult>(Func<YbpContext<TProcess>, Task<TResult>> action, IYbpActionBase instance)
                    where TProcess : YbpProcessBase, new();


        Task<TResult> ExecAsync<TProcess, TResult>(string id, Func<YbpContext<TProcess>, Task<TResult>> action, IYbpActionBase instance)
                    where TProcess : YbpProcessBase, new();

    }

    public class YbpEngine: IYbpEngine
    {
        private readonly IServiceProvider _services;
        private readonly IYbpContextStorage _ctxStorage;

        public YbpEngine(
            IServiceProvider services,
            IYbpContextStorage ctxStorage
            )
        {
            _services = services;
            _ctxStorage = ctxStorage;
        }

        public TResult Exec<TProcess, TResult>(string id, Func<YbpContext<TProcess>, TResult> action, IYbpActionBase instance)
            where TProcess : YbpProcessBase, new()
        {
            var ctx = _ctxStorage.ById<TProcess>(id);
            var result = action(ctx);
            _ctxStorage.Save(ctx);
            return result;
        }

        public async Task<TResult> ExecAsync<TProcess, TResult>(string id, Func<YbpContext<TProcess>, Task<TResult>> action, IYbpActionBase instance)
            where TProcess : YbpProcessBase, new()
        {
            var ctx = _ctxStorage.ById<TProcess>(id);
            var result = await action(ctx);
            _ctxStorage.Save(ctx);
            return result;
        }


        public TResult Start<TProcess, TResult>(Func<YbpContext<TProcess>, TResult> action, IYbpActionBase instance)
            where TProcess : YbpProcessBase, new()
        {
            var ctx = _ctxStorage.New<TProcess>();
            var result = action(ctx);

            if (!string.IsNullOrWhiteSpace(ctx.Id))
                _ctxStorage.Save(ctx);

            return result;
        }

        public async Task<TResult> StartAsync<TProcess, TResult>(Func<YbpContext<TProcess>, Task<TResult>> action, IYbpActionBase instance)
            where TProcess : YbpProcessBase, new()
        {
            var ctx = _ctxStorage.New<TProcess>();

            if (instance.CannotToBeExecuted(ctx.Flags))
                return default(TResult);

            var result = await action(ctx);

            ctx.Flags.MarkAlreadyExecuted(instance.GetType());

            if (!string.IsNullOrWhiteSpace(ctx.Id))
                _ctxStorage.Save(ctx);

            return result;
        }

    }
}
