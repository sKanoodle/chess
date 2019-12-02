using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Chess
{
    public interface IEngine
    {
        string GetMove(string feNotation);
    }

    class StockfishEngine : IEngine
    {
        private readonly Process EngineProcess;

        public StockfishEngine(string exePath)
        {
            var startInfo = new ProcessStartInfo(exePath)
            {
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
            };
            EngineProcess = Process.Start(startInfo);
        }

        ~StockfishEngine()
        {
            EngineProcess.Dispose();
        }

        public string GetMove(string feNotation)
        {
            EngineProcess.StandardInput.WriteLine($"position fen {feNotation}");
            EngineProcess.StandardInput.WriteLine("go");
            string output;
            while (!(output = EngineProcess.StandardOutput.ReadLine()).StartsWith("bestmove")) ;
            return output.Split(' ')[1];
        }
    }
}
