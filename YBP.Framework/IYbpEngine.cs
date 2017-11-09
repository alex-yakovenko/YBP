using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace YBP.Framework
{
    public interface IYbpEngine
    {

        Task<TResult> StartAsync<TProcess, TResult>(Func<YbpContext<TProcess>, Task<TResult>> action, IYbpActionBase instance, string prmJson = null)
                    where TProcess : YbpProcessBase, new();

        Task<TResult> ExecAsync<TProcess, TResult>(string id, Func<YbpContext<TProcess>, Task<TResult>> action, IYbpActionBase instance, string prmJson = null)
                    where TProcess : YbpProcessBase, new();

    }

    public class YbpEngine: IYbpEngine
    {
        private readonly IServiceProvider _services;
        private readonly IYbpContextStorage _ctxStorage;
        private readonly YbpUserContext _userContext;

        public YbpEngine(
            IServiceProvider services,
            IYbpContextStorage ctxStorage,
            YbpUserContext userContext
            )
        {
            _services = services;
            _ctxStorage = ctxStorage;
            _userContext = userContext;
        }

        public async Task<TResult> ExecAsync<TProcess, TResult>(string id, Func<YbpContext<TProcess>, Task<TResult>> action, IYbpActionBase instance, string prmJson = null)
            where TProcess : YbpProcessBase, new()
        {
            var ctx = _ctxStorage.ById<TProcess>(id);

            if (instance.CannotToBeExecuted(ctx.Flags))
                return default(TResult);

            var result = await action(ctx);

            _ctxStorage.Save(ctx, _userContext);

            return result;
        }


        public async Task<TResult> StartAsync<TProcess, TResult>(Func<YbpContext<TProcess>, Task<TResult>> action, IYbpActionBase instance, string prmJson = null)
            where TProcess : YbpProcessBase, new()
        {
            var ctx = _ctxStorage.New<TProcess>();

            var isAuthorized = instance.CanExecute(_userContext);

            _ctxStorage.LogActionStart(ctx, instance.GetType().Name, prmJson, (int)_userContext["UserId"], isAuthorized);

            TResult result = default(TResult);
            try
            {
                result = await action(ctx);
            }
            catch (Exception e)
            {
                _ctxStorage.LogActionFailure(ctx, e);

                throw;
            }

            ctx.Flags.MarkAlreadyExecuted(instance.GetType());

            _ctxStorage.Save(ctx, _userContext);

            _ctxStorage.LogActionSucceed(ctx, JsonConvert.SerializeObject(result));

            return result;
        }

    }
}
