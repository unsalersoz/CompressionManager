using System.IO;
using System.IO.Compression;

namespace CompressionManager
{
	public class DeflateCompressionManager : ICompressionManager
	{
		public byte[] Compress(byte[] data)
		{
			byte[] compressArray;

			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress))
				{
					deflateStream.Write(data, 0, data.Length);
				}

				compressArray = memoryStream.ToArray();
			}

			return compressArray;
		}

		public byte[] Decompress(byte[] data)
		{
			const int BUFFER_SIZE = 4096;
			byte[] decompressedArray = new byte[BUFFER_SIZE];

			using (MemoryStream outputStream = new MemoryStream())
			using (MemoryStream compressedStream = new MemoryStream(data))
			using (DeflateStream deflateStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
			{
				while (true)
				{
					int readBytes = deflateStream.Read(decompressedArray, 0, decompressedArray.Length);

					if (readBytes <= 0)
					{
						break;
					}

					outputStream.Write(decompressedArray, 0, readBytes);
				}

				decompressedArray = outputStream.ToArray();
			}

			return decompressedArray;
		}
	}
}