using System;
namespace TCPGameServer
{
    public struct Vector3
	{
		public float x;
		public float y;
		public float z;

		public Vector3(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}

    public struct Quanternion
	{
		public float x;
		public float y;
		public float z;
		public float w; // world space : Global or Local

        public Quanternion(float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}
	}
}
