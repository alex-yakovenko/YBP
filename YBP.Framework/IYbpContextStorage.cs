using System;

namespace YBP.Framework
{
    public interface IYbpContextStorage
    {
        YbpContext<TProcess> ById<TProcess>(string id)
            where TProcess : YbpProcessBase, new();

        YbpContext<TProcess> New<TProcess>()
            where TProcess : YbpProcessBase, new();

        void Save<TProcess>(YbpContext<TProcess> ctx, YbpUserContext userContext) where TProcess : YbpProcessBase, new();
        void LogActionStart<TProcess>(YbpContext<TProcess> ctx, string name, string prmJson, int userId, bool isAuthorized) where TProcess : YbpProcessBase, new();
        void LogActionFailure<TProcess>(YbpContext<TProcess> ctx, Exception e) where TProcess : YbpProcessBase, new();
        void LogActionSucceed<TProcess>(YbpContext<TProcess> ctx, string v) where TProcess : YbpProcessBase, new();
    }
}
