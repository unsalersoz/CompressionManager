namespace CompressionManager
{
	public interface ICompressionManager
	{
		byte[] Compress(byte[] data);
		byte[] Decompress(byte[] data);
	}
}
