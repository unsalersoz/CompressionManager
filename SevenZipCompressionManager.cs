using System;
using System.IO;
using SevenZip;

namespace CompressionManager
{
	public class SevenZipCompressionManager : ICompressionManager
	{
		public SevenZipCompressionManager()
		{
			const string SEVEN_ZIP_DLL_PATH = @"PATH_TO_7Z(or 7z64).DLL";
			SevenZipBase.SetLibraryPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SEVEN_ZIP_DLL_PATH));				
		}

		public byte[] Compress(byte[] data)
		{
			byte[] result;

			SevenZipCompressor.LzmaDictionarySize = GetDictionarySize(data.LongLength);
			SevenZipCompressor compressor =
				new SevenZipCompressor
					{
						ArchiveFormat = OutArchiveFormat.SevenZip,
						CompressionLevel = CompressionLevel.Ultra,
						CompressionMethod = CompressionMethod.Lzma2,
						CompressionMode = CompressionMode.Create,
						FastCompression = false
					};

			using (MemoryStream uncompressedStream = new MemoryStream(data))
			using (MemoryStream compressedStream = new MemoryStream())
			{
				compressor.CompressStream(uncompressedStream, compressedStream);
				result = compressedStream.ToArray();
			}

			return result;
		}

		public byte[] Decompress(byte[] data)
		{
			byte[] result;

			using (MemoryStream compressedStream = new MemoryStream(data))
			using (MemoryStream uncompressedStream = new MemoryStream())
			using (SevenZipExtractor extractor = new SevenZipExtractor(compressedStream))
			{
				extractor.ExtractFile(0, uncompressedStream);
				result = uncompressedStream.ToArray();
			}

			return result;
		}

		private static int GetDictionarySize(long length)
		{
			int result = 1;
			double lengthInMb = length / 1024d / 1024d;

			if (Math.Abs(lengthInMb - 1d) > double.Epsilon)
			{
				while (result <= (int)lengthInMb)
				{
					result <<= 1;
				}
			}

			return result;
		}
	}
}