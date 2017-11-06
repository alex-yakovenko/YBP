namespace YBP.Framework
{
    public class YbpContextStorage: IYbpContextStorage
    {
        public YbpContext<TProcess> ById<TProcess>(string id)
            where TProcess: YbpProcessBase, new()
        {
            return new YbpContext<TProcess>
            {
                Id = id
            };
        }

        public YbpContext<TProcess> New<TProcess>()
            where TProcess : YbpProcessBase, new()
        {
            return new YbpContext<TProcess>();
        }

        public void Save<TProcess>(YbpContext<TProcess> ctx) where TProcess : YbpProcessBase, new()
        {
            ;
        }
    }
}
