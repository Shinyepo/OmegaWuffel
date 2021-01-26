using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog;
using OWuffel.Extensions;
using OWuffel.Services.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Services
{
    public class ShardsMother
    {
        private class ShardsCoordinatorQueue
        {
            private readonly object _locker = new object();
            private readonly HashSet<int> _set = new HashSet<int>();
            private readonly Queue<int> _queue = new Queue<int>();
            public int Count => _queue.Count;

            public void Enqueue(int i)
            {
                lock (_locker)
                {
                    if (_set.Add(i))
                        _queue.Enqueue(i);
                }
            }

            public bool TryPeek(out int id)
            {
                lock (_locker)
                {
                    return _queue.TryPeek(out id);
                }
            }

            public bool TryDequeue(out int id)
            {
                lock (_locker)
                {
                    if (_queue.TryDequeue(out id))
                    {
                        _set.Remove(id);
                        return true;
                    }
                }
                return false;
            }
        }
        private ShardComMessage _defaultShardState;
        private readonly Process[] _shardProcesses;

        private ShardsCoordinatorQueue _shardStartQueue =
            new ShardsCoordinatorQueue();

        private ConcurrentHashSet<int> _shardRestartWaitingList =
            new ConcurrentHashSet<int>();

        private readonly int _curProcessId;
        private Logger _log;

        private readonly MainConfig Config;

        

        public ShardsMother()
        {
            Config = new MainConfig();
            LogInit.SetupLogger(-1);
            _log = LogManager.GetCurrentClassLogger();

            _log.Info("Starting OmegaWuffelBot");

            _defaultShardState = new ShardComMessage()
            {
                ConnectionState = Discord.ConnectionState.Disconnected,
                Guilds = 0,
                Time = DateTime.UtcNow
            };

            _shardProcesses = new Process[Config.TotalShards];

            var shardIdsEnum = Enumerable.Range(1, Config.TotalShards - 1)
                .Shuffle()
                .Prepend(0);


            var shardIds = shardIdsEnum
               .ToArray();
            for (var i = 0; i < shardIds.Length; i++)
            {
                var id = shardIds[i];
                //add it to the list of shards which should be started

#if DEBUG
                if (id > 0)
                    _shardStartQueue.Enqueue(id);
                else
                    _shardProcesses[id] = Process.GetCurrentProcess();
#else
                _shardStartQueue.Enqueue(id);
#endif

                //set the shard's initial state in redis cache
                var msg = _defaultShardState.Clone();
                msg.ShardId = id;
                //this is to avoid the shard coordinator thinking that
                //the shard is unresponsive while starting up
                var delay = 45;

                msg.Time = DateTime.UtcNow + TimeSpan.FromSeconds(delay * (id + 1));
                
            }

            _curProcessId = Process.GetCurrentProcess().Id;
        }
        public async Task RunAsync()
        {
            //this task will complete when the initial start of the shards 
            //is complete, but will keep running in order to restart shards
            //which are disconnected for too long
            TaskCompletionSource<bool> tsc = new TaskCompletionSource<bool>();
            var _ = Task.Run(async () =>
            {
                do
                {
                    //start a shard which is scheduled for start every 6 seconds 
                    while (_shardStartQueue.TryPeek(out var id))
                    {
                        // if the shard is on the waiting list again
                        // remove it since it's starting up now

                        _shardRestartWaitingList.TryRemove(id);
                        //if the task is already completed,
                        //it means the initial shard starting is done,
                        //and this is an auto-restart
                        if (tsc.Task.IsCompleted)
                        {
                            _log.Warn("Auto-restarting shard {0}, {1} more in queue.", id, _shardStartQueue.Count);
                        }
                        else
                        {
                            _log.Warn("Starting shard {0}, {1} more in queue.", id, _shardStartQueue.Count - 1);
                        }
                        var rem = _shardProcesses[id];
                        if (rem != null)
                        {
                            try
                            {
                                rem.KillTree();
                                rem.Dispose();
                            }
                            catch { }
                        }
                        _shardProcesses[id] = StartShard(id);
                        _shardStartQueue.TryDequeue(out var __);
                        await Task.Delay(10000).ConfigureAwait(false);
                    }
                    tsc.TrySetResult(true);
                    await Task.Delay(6000).ConfigureAwait(false);
                }
                while (true);
                // ^ keep checking for shards which need to be restarted
            });

            //restart unresponsive shards
            

            await tsc.Task.ConfigureAwait(false);
            return;
        }
        private Process StartShard(int shardId)
        {
            return Process.Start(new ProcessStartInfo()
            {                
                FileName = "Z:\\Omega-Wuffel\\OWuffel\\bin\\Debug\\net5.0\\OWuffel.exe",
                Arguments = string.Format("{0} {1}", shardId, _curProcessId)
            });
        }
    }
}
