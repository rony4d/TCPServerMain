using System;
using System.Collections.Generic;
using System.Text;

namespace TCPGameServer
{
	public class ByteBuffer: IDisposable
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

		public void WriteQuaternion(Quaternion input)
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
                if (Peek && (_buffers.Count > _readPosition))
				{
					_readPosition += 4;
				}
                
				return value;
			}
			else{
				throw new Exception(" [Int32] You read our incorrect values or Bytebuffer is empty");
			}
		}

		public byte[] ReadBytes(int Length , bool Peek = true)
        {
            if (_buffers.Count > _readPosition)
            {
                if (_bufferUpdated)
                {
                    _readBuffer = _buffers.ToArray();
                    _bufferUpdated = false;
                }

				byte[] value = _buffers.GetRange(_readPosition, Length).ToArray();
                if (Peek && (_buffers.Count > _readPosition))
                {
                    _readPosition += Length;
                }

                return value;
            }
            else
            {
                throw new Exception(" [Byte] You read our incorrect values or Bytebuffer is empty");
            }
        }

		public string ReadString(bool Peek = true)
        {
			int length = ReadInteger(true);
			if (_bufferUpdated)
            {
                _readBuffer = _buffers.ToArray();
                _bufferUpdated = false;
            }

			string value = Encoding.ASCII.GetString(_readBuffer, _readPosition, length);
            if (Peek && (_buffers.Count > _readPosition))
            {
                _readPosition += length;
            }

            return value;
           
        }


        public short ReadShort(bool Peek = true)
        {
            if (_buffers.Count > _readPosition)
            {
                if (_bufferUpdated)
                {
                    _readBuffer = _buffers.ToArray();
                    _bufferUpdated = false;
                }

                short value = BitConverter.ToInt16(_readBuffer, _readPosition);
                if (Peek && (_buffers.Count > _readPosition))
                {
                    _readPosition += 2;
                }

                return value;
            }
            else
            {
                throw new Exception(" [SHORT] You read our incorrect values or Bytebuffer is empty");
            }
        }


		public long ReadLong(bool Peek = true)
		{
			if (_buffers.Count > _readPosition)
			{
				if (_bufferUpdated)
				{
					_readBuffer = _buffers.ToArray();
					_bufferUpdated = false;
				}

				long value = BitConverter.ToInt64(_readBuffer, _readPosition);
				if (Peek && (_buffers.Count > _readPosition))
				{
					_readPosition += 8;
				}

				return value;
			}
			else
			{
				throw new Exception(" [LONG] You read our incorrect values or Bytebuffer is empty");
			}

		}

		public float ReadFloat(bool Peek = true)
        {
            if (_buffers.Count > _readPosition)
            {
                if (_bufferUpdated)
                {
                    _readBuffer = _buffers.ToArray();
                    _bufferUpdated = false;
                }

                float value = BitConverter.ToSingle(_readBuffer, _readPosition);
                if (Peek && (_buffers.Count > _readPosition))
                {
                    _readPosition +=  4;
                }

                return value;
            }
            else
            {
                throw new Exception(" [FLOAT] You read our incorrect values or Bytebuffer is empty");
            }

        }

		public Vector3 ReadVector3(bool Peek = true)
		{
			if (_bufferUpdated)
			{
				_readBuffer = _buffers.ToArray();
				_bufferUpdated = false;
			}

			byte[] value = _buffers.GetRange(_readPosition, sizeof(float) * 3).ToArray();
			Vector3 vector3;
			vector3.x = BitConverter.ToSingle(value, 0 * sizeof(float));
			vector3.y = BitConverter.ToSingle(value, 1 * sizeof(float));
			vector3.z = BitConverter.ToSingle(value, 2 * sizeof(float));

            if (Peek)
			{
				_readPosition += (sizeof(float) * 3);
			}

			return vector3;
		}

		public Quaternion ReadQuaternion(bool Peek = true)
        {
            if (_bufferUpdated)
            {
                _readBuffer = _buffers.ToArray();
                _bufferUpdated = false;
            }

            byte[] value = _buffers.GetRange(_readPosition, sizeof(float) * 4).ToArray();
			Quaternion quaternion;
			quaternion.x = BitConverter.ToSingle(value, 0 * sizeof(float));
			quaternion.y = BitConverter.ToSingle(value, 1 * sizeof(float));
			quaternion.z = BitConverter.ToSingle(value, 2 * sizeof(float));
			quaternion.w = BitConverter.ToSingle(value, 3 * sizeof(float));


            if (Peek)
            {
                _readPosition += (sizeof(float) * 4);
            }

			return quaternion;
        }


		#endregion

		#region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
					// TODO: dispose managed state (managed objects).
					_buffers.Clear();
					_readPosition = 0;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ByteBuffer() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
             GC.SuppressFinalize(this);
        }
        #endregion
	}
}
