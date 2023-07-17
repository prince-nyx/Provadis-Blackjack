using System.Timers;

namespace BlackJack.Gaming
{
    public class GameTimer
    {
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;
        private Timer timer;

        public GameTimer(ElapsedEventHandler ExecuteFunction)
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            timer = new Timer(ExecuteFunction, cancellationTokenSource, 5000, Timeout.Infinite);
        }


    }
}
