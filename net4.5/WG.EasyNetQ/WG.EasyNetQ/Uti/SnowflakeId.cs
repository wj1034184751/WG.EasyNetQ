﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.Uti
{
    public class SnowflakeId
    {
        public const long Twepoch = 1288834974657L;

        private const int WorkerIdBits = 5;
        private const int DatacenterIdBits = 5;
        private const int SequenceBits = 12;
        private const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);
        private const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);

        private const int WorkerIdShift = SequenceBits;
        private const int DatacenterIdShift = SequenceBits + WorkerIdBits;
        public const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;
        private const long SequenceMask = -1L ^ (-1L << SequenceBits);

        private static SnowflakeId _snowflakeId;

        private readonly object _lock = new object();
        private static readonly object SLock = new object();
        private long _lastTimestamp = -1L;

        public SnowflakeId(long workerId, long datacenterId, long sequence = 0L)
        {
            WorkerId = workerId;
            DatacenterId = datacenterId;
            Sequence = sequence;

            // sanity check for workerId
            if (workerId > MaxWorkerId || workerId < 0)
                throw new ArgumentException(string.Format("worker Id can't be greater than {0} or less than 0", MaxWorkerId));

            if (datacenterId > MaxDatacenterId || datacenterId < 0)
                throw new ArgumentException(string.Format("datacenter Id can't be greater than {0} or less than 0", MaxDatacenterId));
        }

        public long WorkerId { get; protected set; }
        public long DatacenterId { get; protected set; }

        public long Sequence { get; internal set; }

        public static SnowflakeId Default()
        {
            lock (SLock)
            {
                if (_snowflakeId != null)
                {
                    return _snowflakeId;
                }

                var random = new Random();

                if (!int.TryParse(Environment.GetEnvironmentVariable("CAP_WORKERID", EnvironmentVariableTarget.Machine), out var workerId))
                {
                    workerId = random.Next((int)MaxWorkerId);
                }

                if (!int.TryParse(Environment.GetEnvironmentVariable("CAP_DATACENTERID", EnvironmentVariableTarget.Machine), out var datacenterId))
                {
                    datacenterId = random.Next((int)MaxDatacenterId);
                }

                return _snowflakeId = new SnowflakeId(workerId, datacenterId);
            }
        }

        public virtual long NextId()
        {
            lock (_lock)
            {
                var timestamp = TimeGen();

                if (timestamp < _lastTimestamp)
                    throw new Exception(
                        string.Format("InvalidSystemClock: Clock moved backwards, Refusing to generate id for {0} milliseconds", _lastTimestamp - timestamp));

                if (_lastTimestamp == timestamp)
                {
                    Sequence = (Sequence + 1) & SequenceMask;
                    if (Sequence == 0) timestamp = TilNextMillis(_lastTimestamp);
                }
                else
                {
                    Sequence = 0;
                }

                _lastTimestamp = timestamp;
                var id = ((timestamp - Twepoch) << TimestampLeftShift) |
                         (DatacenterId << DatacenterIdShift) |
                         (WorkerId << WorkerIdShift) | Sequence;

                return id;
            }
        }

        protected virtual long TilNextMillis(long lastTimestamp)
        {
            var timestamp = TimeGen();
            while (timestamp <= lastTimestamp) timestamp = TimeGen();
            return timestamp;
        }

        protected virtual long TimeGen()
        {
            //heng
            // return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return DateTimeOffset.UtcNow.UtcTicks;
        }
    }
}
