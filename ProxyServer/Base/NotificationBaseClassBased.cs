namespace Loadbalancer.Base
{
    public class NotificationBase<T> : NotificationBase where T : class, new()
    {
        protected T This;

        public static implicit operator T(NotificationBase<T> thing) { return thing.This; }

        public NotificationBase(T thing = null)
        {
            //if thing == null create new
            This = thing ?? new T();
        }
    }
}
