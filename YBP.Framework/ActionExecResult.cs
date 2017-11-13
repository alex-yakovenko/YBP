namespace YBP.Framework
{
    public class ActionExecResult<TResult>
    {
        public int InstanceId { get; set; }
        public int ActionId { get; internal set; }
        public TResult Result { get; set; }
    }
}
