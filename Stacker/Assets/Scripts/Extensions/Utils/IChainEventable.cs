using System;

namespace Stacker.Extensions.Utils
{

    interface IChainEventable
    {

        void TriggerChainEvent(Action nextCallback);

    }

}
