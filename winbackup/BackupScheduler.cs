using System;
using System.Timers;

namespace winbackup
{
    public class BackupScheduler : IBackupScheduler
    {
        private System.Timers.Timer _timer;

        public event Action OnTick;
        public bool EstaActivo => _timer?.Enabled ?? false;

        public void Iniciar(int intervaloSegundos)
        {
            Detener();

            _timer = new System.Timers.Timer(intervaloSegundos * 1000)
            {
                AutoReset = true
            };

            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }

        public void Detener()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Elapsed -= Timer_Elapsed;
                _timer.Dispose();
                _timer = null;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            OnTick?.Invoke();
        }
    }
}
