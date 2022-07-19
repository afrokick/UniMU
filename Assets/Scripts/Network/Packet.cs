using System;
using System.Text;
using MUnique.OpenMU.Network;

public class Packet
{
    public byte[] Data { get; }

    private string dataAsString;

    /// <summary>
    /// Initializes a new instance of the <see cref="Packet"/> class.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <param name="toServer">If set to <c>true</c>, the packet was sent to server; Otherwise it was sent to the client.</param>
    public Packet(byte[] data, bool toServer = true)
    {
        this.Data = data;
        this.Timestamp = DateTime.Now;
        this.ToServer = toServer;
        this.Direction = this.ToServer ? "C->S" : "S->C";

        ReadOffset = data.GetPacketHeaderSize() + 1;//+ code
    }

    /// <summary>
    /// Gets the timestamp.
    /// </summary>
    public DateTime Timestamp { get; }

    /// <summary>
    /// Gets the type of the packet, which is the first byte of the packet byte array.
    /// </summary>
    public byte Type => this.Data[0];

    /// <summary>
    /// Gets the code of the packet, which specifies the kind of the message.
    /// </summary>
    public byte Code => this.Data.GetPacketType();

    /// <summary>
    /// Gets the sub code of the packet, which further specifies the kind of the message, if specified.
    /// </summary>
    public byte SubCode => this.Data.GetPacketSubType();

    /// <summary>
    /// Gets the direction as string.
    /// </summary>
    public string Direction { get; }

    /// <summary>
    /// Gets a value indicating whether the packet was sent to the server; Otherwise it was sent to the client.
    /// </summary>
    public bool ToServer { get; }

    /// <summary>
    /// Gets the size of the packet byte array.
    /// </summary>
    public int Size => this.Data.Length;

    /// <summary>
    /// Gets the packet data as binary string.
    /// </summary>
    public string DataAsString => this.dataAsString ?? (this.dataAsString = BitConverter.ToString(this.Data).Replace('-', ' '));

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{this.Direction}: {this.DataAsString}";
    }


    public int ReadOffset { get; set; }

    public byte ReadByte()
    {
        var val = Data[ReadOffset];
        ReadOffset++;

        return val;
    }

    public ushort ReadShort(bool smallEndian = true)
    {
        var val = smallEndian ? Data.MakeWordSmallEndian(ReadOffset) : Data.MakeWordBigEndian(ReadOffset);
        ReadOffset += 2;

        return val;
    }

    public uint ReadInt(bool smallEndian = true)
    {
        var val = smallEndian ? Data.MakeDwordSmallEndian(ReadOffset) : Data.MakeDwordBigEndian(ReadOffset);
        ReadOffset += 4;

        return val;
    }

    public long ReadLong()
    {
        var val = (long)Data.MakeQword(ReadOffset);
        ReadOffset += 8;

        return val;
    }

    public string ReadString(int size)
    {
        var val = Data.ExtractString(ReadOffset, size, Encoding.UTF8);
        ReadOffset += size;

        return val;
    }

    public bool ReadBool()
    {
        var val = Data[ReadOffset];
        ReadOffset++;

        return val == 1;
    }
}
