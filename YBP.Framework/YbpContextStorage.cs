using System;
using System.Linq;
using YBP.Framework.Storage.EF;

namespace YBP.Framework
{
    public class YbpContextStorage: IYbpContextStorage
    {
        private readonly YbpDbContext _db;

        public YbpContextStorage(YbpDbContext db)
        {
            _db = db;
        }

        public YbpContext<TProcess> ById<TProcess>(string id)
            where TProcess: YbpProcessBase, new()
        {
            var pfx = new TProcess().Prefix;

            var data = _db.YbpProcesses.FirstOrDefault(x => x.Pfx == pfx && x.InstanceId == id);

            if (data == null)
                return null;

            var result = new YbpContext<TProcess>
            {
                Id = id,
                StoredId = data.Id,
                Flags = new YbpFlagsDictionary(_db
                    .YbpFlags
                    .Where(x => x.ProcessId == data.Id)
                    .ToDictionary(x => x.Key, x => x.IsSet))
            };

            return result;
        }

        public YbpContext<TProcess> ById<TProcess>(int id)
            where TProcess : YbpProcessBase, new()
        {
            var pfx = new TProcess().Prefix;

            var data = _db.YbpProcesses.FirstOrDefault(x => x.Pfx == pfx && x.Id == id);

            if (data == null)
                return null;

            var result = new YbpContext<TProcess>
            {
                Id = data.InstanceId,
                StoredId = data.Id,
                Flags = new YbpFlagsDictionary(_db
                    .YbpFlags
                    .Where(x => x.ProcessId == data.Id)
                    .ToDictionary(x => x.Key, x => x.IsSet))
            };

            return result;
        }

        public YbpContext<TProcess> New<TProcess>()
            where TProcess : YbpProcessBase, new()
        {
            var data = new YbpProcess
            {
                Pfx = new TProcess().Prefix,
                InstanceId = Guid.NewGuid().ToString()
            };

            _db.YbpProcesses.Add(data);
            _db.SaveChanges();

            var result = new YbpContext<TProcess>
            {
                StoredId = data.Id,
                Flags = new YbpFlagsDictionary()
            };

            return result;
        }

        public void Save<TProcess>(YbpContext<TProcess> ctx, YbpUserContext userContext) 
            where TProcess : YbpProcessBase, new()
        {
            var dflags = _db
                .YbpFlags
                .Where(x => x.ProcessId == ctx.StoredId)
                .ToArray();

            foreach (var f in ctx.Flags)
            {
                var df = dflags.FirstOrDefault(x => x.Key == f.Key);
                if (df == null)
                {
                    df = new YbpFlag
                    {
                        ProcessId = ctx.StoredId,
                        Key = f.Key,
                        IsSet = f.Value,
                        UpdatedUTC = DateTime.UtcNow,
                        UserId = (int)userContext["UserId"]
                    };

                    _db.YbpFlags.Add(df);

                    var dfh = new YbpFlagHistory
                    {
                        Flag = df,
                        IsSet = df.IsSet,
                        UpdatedUTC = df.UpdatedUTC,
                        UserId = df.UserId
                    };

                    _db.YbpFlagHistory.Add(dfh);
                }
                else if (df.IsSet != f.Value)
                {
                    df.IsSet = f.Value;
                    df.UpdatedUTC = DateTime.UtcNow;
                    df.UserId = (int)userContext["UserId"];

                    var dfh = new YbpFlagHistory
                    {
                        Flag = df,
                        IsSet = df.IsSet,
                        UpdatedUTC = df.UpdatedUTC,
                        UserId = df.UserId
                    };

                    _db.YbpFlagHistory.Add(dfh);

                }

            }

            _db.SaveChanges();
        }

        public void LogActionStart<TProcess>(YbpContext<TProcess> ctx, string name, string prmJson, int userId, bool isAuthorized) where TProcess : YbpProcessBase, new()
        {
            var data = new YbpActionHistory
            {
                IsAuthorized = isAuthorized,
                Name = name,
                Params = prmJson,
                StartedUTC = DateTime.UtcNow,
                ProcessId = ctx.StoredId,
                UserId = userId
            };

            _db.YbpActionHistory.Add(data);
            _db.SaveChanges();

            ctx.StoredActionId = data.Id;
        }


        public void LogActionFailure<TProcess>(YbpContext<TProcess> ctx, Exception e) where TProcess : YbpProcessBase, new()
        {
            var data = _db.YbpActionHistory.Find(ctx.StoredActionId);
            data.Results = $"{e.GetType().Name}: {e.Message}";
            data.FinishedUTC = DateTime.UtcNow;
            _db.SaveChanges();
        }

        public void LogActionSucceed<TProcess>(YbpContext<TProcess> ctx, string v) where TProcess : YbpProcessBase, new()
        {
            var data = _db.YbpActionHistory.Find(ctx.StoredActionId);
            data.Results = v;
            data.Succeed = true;
            data.FinishedUTC = DateTime.UtcNow;
            _db.SaveChanges();
        }

    }
}
