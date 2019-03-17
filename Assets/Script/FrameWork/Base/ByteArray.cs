using System;
using System.Text;

namespace Framework
{
	public class ByteArray
	{
		private readonly int MAX_BUFF_SIZE;

		protected byte[] mDataBuff;
		protected int mLength = 0;
		protected int mPosition = 0;

		public ByteArray(int bufferSize = 2046)
		{
			this.MAX_BUFF_SIZE = bufferSize;
			mDataBuff = new byte[MAX_BUFF_SIZE];
		}

		public ByteArray(byte[] buffer)
		{
			mDataBuff = buffer;
			mLength = buffer.Length;
		}

		public void InitBytesArray(byte[] buff, int len)
		{
			if (len > MAX_BUFF_SIZE)
			{
				throw new Exception("InitBytesArray 初始化失败，超出字节流最大限制");
			}
			Buffer.BlockCopy(buff,0, mDataBuff,0, len);
			mLength = len;
			mPosition = 0;
		}

		public bool CreateFromSocketBuff(byte[] buff, int nSize)
		{
			if (buff == null)
			{
				return false;
			}
			this.mLength = BitConverter.ToUInt16(buff, 0);
			if ((this.mLength > MAX_BUFF_SIZE) || (this.mLength <= 0))
			{
				return false;
			}
			Buffer.BlockCopy(buff, 0, this.mDataBuff, 0, this.mLength);
			mPosition = 0;
			return true;
		}

		public bool CopyFromByteArray(ref byte[] aBuff, ref int nSize)
		{
			if (aBuff == null)
			{
				return false;
			}
            Buffer.BlockCopy(this.mDataBuff, 0, aBuff, 0, this.mLength);
			nSize = mLength;
			return true;
		}

		public void WriteBool(bool value)
		{
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, mDataBuff, mPosition, 2);
			mPosition += 2;
			mLength = mPosition;
			CheckBuffSize();
		}

		public void WriteByte(byte value)
		{
			mDataBuff[mPosition] = value;
			mPosition++;
			mLength = mPosition;
			CheckBuffSize();
		}

		public void WriteBytes(ByteArray bytes, bool writeSize)
		{
			WriteBytes(bytes.mDataBuff, bytes.mLength, writeSize);
		}

		public void WriteBytes(byte[] bytes, int length, bool writeSize)
		{
			if (writeSize)
			{
                Buffer.BlockCopy(BitConverter.GetBytes(length), 0, mDataBuff, mPosition, 2);
				mPosition += 2;
			}
            Buffer.BlockCopy(bytes, 0, mDataBuff, mPosition, length);
			mPosition += length;
			mLength = mPosition;
			CheckBuffSize();
		}

		public void WriteDouble(double value)
		{
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, mDataBuff, mPosition, 8);
			mPosition += 8;
			mLength = mPosition;
			CheckBuffSize();
		}

		public void WriteFloat(float value)
		{
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, mDataBuff, mPosition, 4);
			mPosition += 4;
			mLength = mPosition;
			CheckBuffSize();
		}

		public void WriteInt(int value)
		{
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, mDataBuff, mPosition, 4);
			mPosition += 4;
			mLength = mPosition;
			CheckBuffSize();
		}

		public void WriteUInt(uint value)
		{
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, mDataBuff, mPosition, 4);
			mPosition += 4;
			mLength = mPosition;
			CheckBuffSize();
		}

		public void WriteShort(short value)
		{
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, mDataBuff, mPosition, 2);
			mPosition += 2;
			mLength = mPosition;
			CheckBuffSize();
		}

		public void WriteUShort(ushort value)
		{
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, mDataBuff, mPosition, 2);
			mPosition += 2;
			mLength = mPosition;
			CheckBuffSize();
		}

		public void WriteString(string value)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(value);
			short strLength = (short)bytes.Length;
            Buffer.BlockCopy(BitConverter.GetBytes(strLength), 0, mDataBuff, mPosition, 2);
			mPosition += 2;
            Buffer.BlockCopy(bytes, 0, mDataBuff, mPosition, strLength);
			mPosition += strLength;
			mLength = mPosition;
			CheckBuffSize();
		}

		public bool ReadBoolean()
		{
			if ((mPosition + 2) > mLength)
			{
				throw new Exception("ReadBoolean读取数据失败，读取数据超出字节流范围");
			}
			bool value = BitConverter.ToBoolean(mDataBuff, mPosition); ;
			mPosition += 2;
			return value;
		}

		public byte ReadByte()
		{
			if ((mPosition + 1) > mLength)
			{
				throw new Exception("ReadByte读取数据失败，读取数据超出字节流范围");
			}
			byte value = (byte)mDataBuff[mPosition];
			mPosition++;
			return value;
		}

		public ByteArray ReadBytes()
		{
			if ((mPosition + 2) > mLength)
			{
				throw new Exception("ReadBytes[0]读取数据失败，读取数据超出字节流范围");
			}
			int length = BitConverter.ToInt32(mDataBuff, mPosition);
			mPosition += 2;
			if ((length < 0) || (length > MAX_BUFF_SIZE))
			{
				throw new Exception("ReadBytes[1]读取数据失败，读取数据超出字节流范围");
			}
			ByteArray values = new ByteArray();
            Buffer.BlockCopy(mDataBuff, mPosition, values.mDataBuff, 0, length);
			values.mPosition = 0;
			values.mLength = length;
			mPosition += length;
			return values;
		}

		public byte[] ReadBytes(int byteNum)
		{
			if ((mPosition + byteNum) > mLength)
			{
				throw new Exception("ReadBytes[0]读取数据失败，读取数据超出字节流范围");
			}
			byte[] values = new byte[byteNum];
            Buffer.BlockCopy(mDataBuff, mPosition, values, 0, byteNum);
			mPosition += byteNum;
			return values;
		}

		public double ReadDouble()
		{
			if ((mPosition + 8) > mLength)
			{
				throw new Exception("ReadDouble读取数据失败，读取数据超出字节流范围");
			}
			double value = BitConverter.ToDouble(mDataBuff, mPosition);
			mPosition += 8;
			return value;
		}

		public float ReadFloat()
		{
			if ((mPosition + 4) > mLength)
			{
				throw new Exception("ReadFloat读取数据失败，读取数据超出字节流范围");
			}
			float value = BitConverter.ToSingle(mDataBuff, mPosition);
			mPosition += 4;
			return value;
		}

		public int ReadInt()
		{
			if ((mPosition + 4) > mLength)
			{
				throw new Exception("ReadInt读取数据失败，读取数据超出字节流范围");
			}
			int value = BitConverter.ToInt32(mDataBuff, mPosition);
			mPosition += 4;
			return value;
		}

		public uint ReadUInt()
		{
			if ((mPosition + 4) > mLength)
			{
				throw new Exception("ReadInt读取数据失败，读取数据超出字节流范围");
			}
			uint value = BitConverter.ToUInt32(mDataBuff, mPosition);
			mPosition += 4;
			return value;
		}

		public short ReadShort()
		{
			if ((mPosition + 2) > mLength)
			{
				throw new Exception("ReadShort读取数据失败，读取数据超出字节流范围");
			}
			short value = BitConverter.ToInt16(mDataBuff, mPosition);
			mPosition += 2;
			return value;
		}

		public ushort ReadUShort()
		{
			if ((mPosition + 2) > mLength)
			{
				throw new Exception("ReadShort读取数据失败，读取数据超出字节流范围");
			}
			ushort value = BitConverter.ToUInt16(mDataBuff, mPosition);
			mPosition += 2;
			return value;
		}

		public string ReadString()
		{
			if ((mPosition + 2) > mLength)
			{
				throw new Exception("ReadString[0]读取数据失败，读取数据超出字节流范围");
			}
			short count = BitConverter.ToInt16(mDataBuff, mPosition);
			mPosition += 2;
			string value = Encoding.UTF8.GetString(mDataBuff, mPosition, count);
			mPosition += count;
			return value;
		}

		private void CheckBuffSize()
		{
			if (mPosition > MAX_BUFF_SIZE)
			{
				throw new Exception("InitBytesArray初始化失败，超出字节流最大限制");
			}
		}

		public byte[] Buff
		{
			get
			{
				return mDataBuff;
			}
		}

		public int Length
		{
			get
			{
				return mLength;
			}
		}

		public int Postion
		{
			get
			{
				return mPosition;
			}
			set
			{
				mPosition = value;
			}
		}
	}
}
