using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ITS.Worker.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ITS.Worker
{
    public class Worker : BackgroundService
    {
        private readonly WorkerConfiguration _config;

        public Worker(IConfiguration configuration)
        {
            _config = new WorkerConfiguration();
            configuration.GetSection("WorkerConfiguration").Bind(_config);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Log.Information("===== Rotina de backup iniciada =====");

                string sourceFile = Path.Combine(_config.SourcePath, _config.GetSourceFileName());

                if (File.Exists(sourceFile))
                {
                    Log.Information("Localizado o arquivo: {file}", sourceFile);
                    if (Directory.Exists(_config.TargetPath))
                    {
                        Log.Information("Localizado a pasta de destino: {targetPath}", _config.TargetPath);

                        string destFile = Path.Combine(_config.TargetPath, _config.GetTargetFileName());
                        try
                        {
                            File.Copy(sourceFile, destFile, true);
                            Log.Information("Backup efetuado com sucesso!");
                        }
                        catch (Exception e)
                        {
                            Log.Error(e, "**** Ocorreu um erro ao copiar o arquivo para a pasta de destino ****");
                        }
                    }
                    else
                        Log.Warning("Não foi localizado a pasta de destino: {targetPath}", _config.TargetPath);
                }
                else
                    Log.Warning("Nao foi localizado o arquivo: {file}", sourceFile);

                Log.Information("===== Rotina de backup finalizada =====");

                await Task.Delay(_config.Interval, stoppingToken);
            }
        }
    }
}
