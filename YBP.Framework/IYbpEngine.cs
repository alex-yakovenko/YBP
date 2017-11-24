using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using YBP.Framework.Regisry;

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

            if (instance.MayNotToBeExecuted(ctx.Flags))
                return result;

            await ProcessAction<TProcess, TResult>(result, ctx, action, instance, prmJson);

            return result;
        }

        private async Task ProcessAction<TProcess, TResult>(ActionExecResult<TResult> result, YbpContext<TProcess> ctx, Func<YbpContext<TProcess>, Task<TResult>> action, IYbpActionBase instance, string prmJson)
            where TProcess : YbpProcessBase, new()
        {
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

        }

        public async Task<bool> ProcessNextAction<TProcess>(int ctxId)
            where TProcess : YbpProcessBase, new()
        {
            var actions = YbpConfiguration
                .Actions[typeof(TProcess)]
                .Where(x => x.CanBeExecutedAutomatically && typeof(YbpAction<TProcess>).IsAssignableFrom(x.GetType()))
                .Select(x => x as YbpAction<TProcess>)
                .ToArray();

            if (!actions.Any())
                return false;

            var ctx = _ctxStorage.ById<TProcess>(ctxId);

            var action = actions
                .FirstOrDefault(x => x.NeedsToBeExecuted(ctx.Flags) 
                    && !x.MayNotToBeExecuted(ctx.Flags));

            if (action == null)
                return false;

            var t = action.GetType();

            action = _services.GetService(t) as YbpAction<TProcess>;

            await ExecAsync<TProcess, string>(ctx.Id, async c => await action.Run(c), action);

            return true;
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

            await ProcessAction<TProcess, TResult>(result, ctx, action, instance, prmJson);

            if (!string.IsNullOrWhiteSpace(ctx.Id))
                _ctxStorage.UpdateInstanceId(ctx.StoredId, ctx.Id);

            return result;
        }

    }
}
