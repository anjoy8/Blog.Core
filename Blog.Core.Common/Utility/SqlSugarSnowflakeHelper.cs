namespace Blog.Core.Common.Utility;

/// <summary>
/// SqlSugar 雪花算法 工具类
/// </summary>
public class SqlSugarSnowflakeHelper
{
    private const long Twepoch = 1288834974657L;
    private const int TimestampLeftShift = 22;
    private const int DatacenterIdShift = 17;
    private const int WorkerIdShift = 12;
    private const long SequenceMask = 0xFFF;  // 4095
    private const long WorkerMask = 0x1F;     // 31
    private const long DatacenterMask = 0x1F; // 31

    public static DateTime GetDateTime(long id)
    {
        long timestamp = (id >> TimestampLeftShift) + Twepoch;
        return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).ToLocalTime().DateTime;
    }

    public static (DateTime time, long datacenterId, long workerId, long sequence) Decode(long id)
    {
        long timestamp    = (id >> TimestampLeftShift) + Twepoch;
        long datacenterId = (id >> DatacenterIdShift) & DatacenterMask;
        long workerId     = (id >> WorkerIdShift) & WorkerMask;
        long sequence     = id & SequenceMask;
        var  time         = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).ToLocalTime().DateTime;
        return (time, datacenterId, workerId, sequence);
    }
}