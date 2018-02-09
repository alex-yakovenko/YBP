using System;
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
}
