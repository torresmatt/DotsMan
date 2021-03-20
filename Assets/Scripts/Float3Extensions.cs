using Unity.Mathematics;

public static class Float3Extensions
{
    public static float3 Up => new float3(0, 1, 0);
    public static float3 Down => new float3(0, -1, 0);
    public static float3 Left => new float3(-1, 0, 0);
    public static float3 Right => new float3(1, 0, 0);
    public static float3 Forward => new float3(0, 0, 1);
    public static float3 Back => new float3(0, 0, -1);
}
