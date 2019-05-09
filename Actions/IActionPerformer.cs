using System;
using System.Threading.Tasks;

namespace deployment_tracker.Actions {
    public interface IActionPerformer<T> {
        bool Succeeded { get; }

        String Error { get; }

        T Result { get; }

        Task Perform();
    }
}