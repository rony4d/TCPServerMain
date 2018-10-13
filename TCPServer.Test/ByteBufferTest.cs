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
		short inputShort = 20;
		long inputLong = 250000000000000000;
		float inputFloat = 245.34564f;
		Quaternion inputQuaternion = new Quaternion(4f, 4f, 4f, 4f);
		Vector3 inputVector3 = new Vector3(4f, 4f, 4f);

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

        /// <summary>
        /// Shoulds the write short to the bytebuffer object
        /// </summary>
        [Fact]
		public void ShouldWriteShort()
		{
			byteBuffer.WriteShort(inputShort);
            byte[] shortByteLength = new byte[sizeof(short)];
            int bufferLength = byteBuffer.Length();
			Assert.Equal(bufferLength, shortByteLength.Length);
		}

        /// <summary>
        /// Should read short from the byte buffer object
        /// </summary>
        [Fact]
        public void ShouldReadShort()
		{
			byteBuffer.WriteShort(inputShort);
            short value = byteBuffer.ReadShort();

            Assert.Equal(value, inputShort);
		}

		/// <summary>
        /// Shoulds the write long to the bytebuffer object
        /// </summary>
        [Fact]
        public void ShouldWriteLong()
        {
			byteBuffer.WriteLong(inputLong);
            byte[] longByteLength = new byte[sizeof(long)];
            int bufferLength = byteBuffer.Length();
			Assert.Equal(bufferLength, longByteLength.Length);
        }

        /// <summary>
        /// Should read long from the byte buffer object
        /// </summary>
        [Fact]
        public void ShouldReadLong()
        {
            byteBuffer.WriteLong(inputLong);
            long value = byteBuffer.ReadLong();

            Assert.Equal(value, inputLong);
        }

		/// <summary>
        /// Shoulds the write float to the bytebuffer object
        /// </summary>
        [Fact]
        public void ShouldWriteFloat()
        {
            byteBuffer.WriteFloat(inputFloat);
            byte[] floatByteLength = new byte[sizeof(float)];
            int bufferLength = byteBuffer.Length();
            Assert.Equal(bufferLength, floatByteLength.Length);
        }

        /// <summary>
        /// Should read float from the byte buffer object
        /// </summary>
        [Fact]
        public void ShouldReadFloat()
        {
            byteBuffer.WriteFloat(inputFloat);
            float value = byteBuffer.ReadFloat();

            Assert.Equal(value, inputFloat);
        }

		/// <summary>
        /// Shoulds the write string to the bytebuffer object
        /// </summary>
        [Fact]
        public void ShouldWriteString()
        {
            byteBuffer.WriteString(messageInputString);
            int bufferLength = byteBuffer.Length();
        }

        /// <summary>
        /// Should read string from the byte buffer object
        /// </summary>
        [Fact]
        public void ShouldReadString()
        {
			byteBuffer.WriteString(messageInputString);
			string value = byteBuffer.ReadString();

            Assert.Equal(value, messageInputString);
        }

        /// <summary>
        /// Shoulds the write quaternion to bytebuffer object
        /// </summary>
        [Fact]
        public void ShouldWriteQuaternion()
		{
			byteBuffer.WriteQuaternion(inputQuaternion);
            byte[] quaternionByteLength = new byte[sizeof(float) * 4];
            int bufferLength = byteBuffer.Length();
			Assert.Equal(bufferLength, quaternionByteLength.Length);
		}

		/// <summary>
        /// Shoulds the read quaternion from bytebuffer object
        /// </summary>
        [Fact]
        public void ShouldReadQuaternion()
        {
			byteBuffer.WriteQuaternion(inputQuaternion);

			Quaternion value = byteBuffer.ReadQuaternion();
            
            Assert.Equal(value.x, inputQuaternion.x);
        }

		/// <summary>
        /// Shoulds the write Vector3 to bytebuffer object
        /// </summary>
        [Fact]
        public void ShouldWriteVector3()
        {
			byteBuffer.WriteVector3(inputVector3);
            byte[] vector3ByteLength = new byte[sizeof(float) * 3];
            int bufferLength = byteBuffer.Length();
			Assert.Equal(bufferLength, vector3ByteLength.Length);
        }

		/// <summary>
        /// Shoulds the read Vector3 from bytebuffer object
        /// </summary>
        [Fact]
        public void ShouldReadVector3()
        {
			byteBuffer.WriteVector3(inputVector3);

            Vector3 value = byteBuffer.ReadVector3();

            Assert.Equal(value.x, inputVector3.x);
        }
    }
}
