using System;
using System.Windows.Forms;

namespace winbackup
{
    public class LogService : ILogService
    {
        private readonly ListBox _listBox;
        private const int MaxLineas = 500;

        public LogService(ListBox listBox)
        {
            _listBox = listBox ?? throw new ArgumentNullException(nameof(listBox));
        }

        public void Info(string mensaje)
        {
            Escribir("INFO", mensaje);
        }

        public void Exito(string mensaje)
        {
            Escribir("OK", mensaje);
        }

        public void Advertencia(string mensaje)
        {
            Escribir("WARN", mensaje);
        }

        public void Error(string mensaje)
        {
            Escribir("ERROR", mensaje);
        }

        private void Escribir(string nivel, string mensaje)
        {
            if (_listBox.IsDisposed) return;

            var linea = $"[{DateTime.Now:HH:mm:ss}] {nivel}: {mensaje}";

            if (_listBox.InvokeRequired)
            {
                _listBox.Invoke(() =>
                {
                    _listBox.Items.Insert(0, linea);
                    if (_listBox.Items.Count > MaxLineas)
                        _listBox.Items.RemoveAt(_listBox.Items.Count - 1);
                });
            }
            else
            {
                _listBox.Items.Insert(0, linea);
                if (_listBox.Items.Count > MaxLineas)
                    _listBox.Items.RemoveAt(_listBox.Items.Count - 1);
            }
        }
    }
}
