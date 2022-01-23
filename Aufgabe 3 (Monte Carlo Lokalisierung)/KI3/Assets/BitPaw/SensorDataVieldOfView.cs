namespace BitPaw
{
    // Raycast Result, what does the sensor see in a 2D view
    public class SensorDataVieldOfView
    {
        public int DataSize { get; set; } = 0;
        public float[] DistanceToWall { get; set; } = null;        
        public float Rating { get; set; }

        public void Reserve(int size)
        {
            if (DataSize == size)
            {
                // We have the size, dont allocate.
                return;
            }

            DataSize = size;
            DistanceToWall = new float[size];
        }        
    }
}
