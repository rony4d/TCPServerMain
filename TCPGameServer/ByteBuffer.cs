using System;
using System.Collections.Generic;
using System.Text;

namespace TCPGameServer
{
	public class ByteBuffer
	{
		private List<byte> _buffers;
		private byte[] _readBuffer;
		private int _readPosition;
		private bool _bufferUpdated = false;

		#region Functions
		public ByteBuffer()
		{
			_buffers = new List<byte>();
			_readPosition = 0;
		}

		public int GetReadPosition()
		{
			return _readPosition;
		}

		public byte[] ToArray()
		{
			return _buffers.ToArray();
		}
		public int Count()
		{
			return _buffers.Count;
		}

		public int Length()
		{
			return Count() - _readPosition;
		}

		public void Clear()
		{
			_buffers.Clear();
			_readPosition = 0;
		}
		#endregion

		#region Write Data
		public void WriteBytes(byte[] input)
		{
			_buffers.AddRange(input);
			_bufferUpdated = true;
		}

		public void WriteShort(short input)
		{
			_buffers.AddRange(BitConverter.GetBytes(input));
			_bufferUpdated = true;
		}
		public void WriteInteger(int input)
		{
			_buffers.AddRange(BitConverter.GetBytes(input));
			_bufferUpdated = true;
		}
		public void WriteLong(long input)
		{
			_buffers.AddRange(BitConverter.GetBytes(input));
			_bufferUpdated = true;
		}
		public void WriteFloat(float input)
		{
			_buffers.AddRange(BitConverter.GetBytes(input));
			_bufferUpdated = true;
		}

		public void WriteString(string input)
		{
			_buffers.AddRange(BitConverter.GetBytes(input.Length));
			_buffers.AddRange(Encoding.ASCII.GetBytes(input));
			_bufferUpdated = true;
		}

		public void WriteVector3(Vector3 input)
		{
			byte[] vectorArray = new byte[sizeof(float) * 3];

			Buffer.BlockCopy(BitConverter.GetBytes(input.x), 0, vectorArray, 0 * sizeof(float), sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(input.y), 0, vectorArray, 1 * sizeof(float), sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(input.z), 0, vectorArray, 2 * sizeof(float), sizeof(float));

			_buffers.AddRange(vectorArray);
			_bufferUpdated = true;
		}

		public void WriteQuaternion(Quanternion input)
		{
			byte[] vectorArray = new byte[sizeof(float) * 4];

			Buffer.BlockCopy(BitConverter.GetBytes(input.x), 0, vectorArray, 0 * sizeof(float), sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(input.y), 0, vectorArray, 1 * sizeof(float), sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(input.z), 0, vectorArray, 2 * sizeof(float), sizeof(float));
			Buffer.BlockCopy(BitConverter.GetBytes(input.w), 0, vectorArray, 3 * sizeof(float), sizeof(float));

			_buffers.AddRange(vectorArray);
			_bufferUpdated = true;
		}
		#endregion

		#region Read Data
        
		public int ReadInteger(bool Peek = true)
		{
			if (_buffers.Count > _readPosition)
			{
                if (_bufferUpdated)
				{
					_readBuffer = _buffers.ToArray();
					_bufferUpdated = false;
				}

				int value = BitConverter.ToInt32(_readBuffer, _readPosition);
                if (Peek & (_buffers.Count > _readPosition))
				{
					_readPosition += 4;
				}
                
				return value;
			}
			else{
				throw new Exception(" [Int32] You read our incorrect values or Bytebuffer is empty");
			}
		}
		#endregion
	}
}
