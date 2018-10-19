using System;
namespace TCPGameServer
{
	public class Types
	{
		public static PlayerRec[] PlayerRecs = new PlayerRec[Constants.MAX_PLAYERS];
		public static TempPlayerRec[] TempPlayerRecs = new TempPlayerRec[Constants.MAX_PLAYERS];

	}
	public struct PlayerRec
	{
		public Vector3 Position { get; set; }
		public Vector3 Collider { get; set; }
		public Quaternion Rotation { get; set; }
		public int Health { get; set; }
	}

    public struct TempPlayerRec
	{
		public bool isPlaying;
	}
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

    public struct Quaternion
	{
		public float x;
		public float y;
		public float z;
		public float w; 

        public Quaternion(float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}
	}
}
