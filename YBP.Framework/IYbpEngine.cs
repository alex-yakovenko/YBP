using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace YBP.Framework
{
    public interface IYbpEngine
    {

        Task<ActionExecResult<TResult>> StartAsync<TProcess, TResult>(Func<YbpContext<TProcess>, Task<TResult>> action, IYbpActionBase instance, string prmJson = null)
                    where TProcess : YbpProcessBase, new();

        Task<ActionExecResult<TResult>> ExecAsync<TProcess, TResult>(string id, Func<YbpContext<TProcess>, Task<TResult>> action, IYbpActionBase instance, string prmJson = null)
                    where TProcess : YbpProcessBase, new();

        Task<bool> ProcessNextAction<TProcess>(int ctxId)
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

        public async Task<ActionExecResult<TResult>> ExecAsync<TProcess, TResult>(string id, Func<YbpContext<TProcess>, Task<TResult>> action, IYbpActionBase instance, string prmJson = null)
            where TProcess : YbpProcessBase, new()
        {
            var ctx = _ctxStorage.ById<TProcess>(id);

            var result = new ActionExecResult<TResult>
            {
                InstanceId = ctx.StoredId,
                ActionId = ctx.StoredActionId,
            };

            if (instance.CannotToBeExecuted(ctx.Flags))
                return result;

            result.Result = await action(ctx);

            _ctxStorage.Save(ctx, _userContext);

            return result;
        }

        public async Task<bool> ProcessNextAction<TProcess>(int ctxId)
            where TProcess : YbpProcessBase, new()
        {
            var ctx = _ctxStorage.ById<TProcess>(ctxId);
            return false;
        }

        public async Task<ActionExecResult<TResult>> StartAsync<TProcess, TResult>(Func<YbpContext<TProcess>, Task<TResult>> action, IYbpActionBase instance, string prmJson = null)
            where TProcess : YbpProcessBase, new()
        {
            var ctx = _ctxStorage.New<TProcess>();

            var result = new ActionExecResult<TResult>
            {
                InstanceId = ctx.StoredId,
                ActionId = ctx.StoredActionId
            };

            var isAuthorized = instance.CanExecute(_userContext);

            _ctxStorage.LogActionStart(ctx, instance.GetType().Name, prmJson, (int)_userContext["UserId"], isAuthorized);

            try
            {
                result.Result = await action(ctx);
            }
            catch (Exception e)
            {
                _ctxStorage.LogActionFailure(ctx, e);

                throw;
            }

            ctx.Flags.MarkAlreadyExecuted(instance.GetType());

            _ctxStorage.Save(ctx, _userContext);

            _ctxStorage.LogActionSucceed(ctx, JsonConvert.SerializeObject(result.Result));

            return result;
        }

    }
}
