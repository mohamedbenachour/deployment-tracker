using System;
using System.Threading.Tasks;

namespace deployment_tracker.Actions {
    public interface IPostAction<T> {
        Task Perform(T actionResult);
    }
}