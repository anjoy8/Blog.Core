using Yitter.IdGenerator;

namespace Blog.Core.Common.Utility;

/// <summary>
/// Yitter 雪花算法 工具类
/// </summary>
public class YitterSnowflakeHelper
{
    /// <summary>
    /// 从ID中解析时间
    /// </summary>
    public static DateTime GetDateTime(IdGeneratorOptions options, long id)
    {
        int  shift    = options.SeqBitLength + options.WorkerIdBitLength + options.DataCenterIdBitLength;
        long timeDiff = id >> shift;

        DateTime utcTime = options.TimestampType == 1
            ? options.BaseTime.AddSeconds(timeDiff)
            : options.BaseTime.AddMilliseconds(timeDiff);

        return DateTime.SpecifyKind(utcTime, DateTimeKind.Utc).ToLocalTime();
    }

    /// <summary>
    /// 从ID中解析时间、WorkerId、序列号
    /// </summary>
    public static (DateTime time, long workerId, long sequence, long datacenterId) Decode(IdGeneratorOptions options,
        long id)
    {
        int seqBits        = options.SeqBitLength;
        int workerBits     = options.WorkerIdBitLength;
        int datacenterBits = options.DataCenterIdBitLength;

        long sequence     = id & ((1L << seqBits) - 1);
        long workerId     = (id >> seqBits) & ((1L << workerBits) - 1);
        long datacenterId = datacenterBits == 0 ? 0 : (id >> (seqBits + workerBits)) & ((1L << datacenterBits) - 1);
        long timeDiff     = id >> (seqBits + workerBits + datacenterBits);

        DateTime utcTime = options.TimestampType == 1
            ? options.BaseTime.AddSeconds(timeDiff)
            : options.BaseTime.AddMilliseconds(timeDiff);
        
        DateTime localTime = DateTime.SpecifyKind(utcTime, DateTimeKind.Utc).ToLocalTime();

        return (localTime, workerId, sequence, datacenterId);
    }
}