using System;
using System.Text;
using TCPGameServer;
using Xunit;

namespace TCPServer.Test
{
    public class ByteBufferTest
    {
		string messageInputString = "Hello TCP server";
		int inputInteger = 4000;

		ByteBuffer byteBuffer;

        public ByteBufferTest()
		{
			byteBuffer =  new ByteBuffer();
		}
       

		/// <summary>
        /// Should write byte array to buffer object
        /// </summary>
        [Fact]
        public void ShouldWriteBytes()
        {
			byte[] messageBytes = Encoding.ASCII.GetBytes(messageInputString);
			byteBuffer.WriteBytes(messageBytes);
			int bufferLength = byteBuffer.Length();
			int messageByteLength = messageBytes.Length;
			Assert.Equal(bufferLength, messageByteLength);
        }

        /// <summary>
        /// Shoulds the read byte array from buffer object 
        /// </summary>
		[Fact]
        public void ShouldReadBytes()
        {
			byte[] messageBytes = Encoding.ASCII.GetBytes(messageInputString);
            byteBuffer.WriteBytes(messageBytes);

			byte[] byteRead = byteBuffer.ReadBytes(messageBytes.Length);

			Assert.Equal(messageBytes, byteRead);
        }


		/// <summary>
        /// Should write integer to bytebuffer object
        /// </summary>
        [Fact]
        public void ShouldWriteInteger()
        {
			byteBuffer.WriteInteger(inputInteger);
			byte[] integerByteLength = new byte[sizeof(int)];
            int bufferLength = byteBuffer.Length();
			Assert.Equal(bufferLength, integerByteLength.Length);
        }

        /// <summary>
        /// Shoulds the read integer from the bytebuffer object.
        /// </summary>
		[Fact]
        public void ShouldReadInteger()
        {
			byteBuffer.WriteInteger(inputInteger);
            int value = byteBuffer.ReadInteger();

			Assert.Equal(value, inputInteger);
        }
    }
}
