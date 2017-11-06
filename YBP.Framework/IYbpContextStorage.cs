namespace YBP.Framework
{
    public interface IYbpContextStorage
    {
        YbpContext<TProcess> ById<TProcess>(string id)
            where TProcess : YbpProcessBase, new();

        YbpContext<TProcess> New<TProcess>()
            where TProcess : YbpProcessBase, new();

        void Save<TProcess>(YbpContext<TProcess> ctx) where TProcess : YbpProcessBase, new();
    }
}
