using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdrianCodesCoin
{
    internal class ACBlock
    {
        public int Index { get; set; } = 0;
        public DateTime TimeStamp { get; set; }
        public byte[] Data { get; set; }
        public byte[] Hash { get; set; }
        public byte[] PreviousHash { get; set; }

        public ACBlock(int index, DateTime timeStamp, int data) :
            this(index, timeStamp, BitConverter.GetBytes(data), new byte[0])
        {
        }

        public ACBlock(int index, DateTime timeStamp, byte[] data):
            this(index, timeStamp, data, new byte[0])
        {
        }

        public ACBlock(int index, DateTime timeStamp, byte[] data, byte[] previousHash)
        {
            Index = index;
            TimeStamp = timeStamp;
            Data = data;
            PreviousHash = previousHash;
            Hash = CalculateHash(Index, TimeStamp, data, previousHash);
        }

        public void UpdateHash()
        {
            Hash = CalculateHash(Index, TimeStamp, Data, PreviousHash);
        }

        public byte[] CalculateCurrentHash()
        {
            return CalculateHash(Index, TimeStamp, Data, PreviousHash);
        }
               
        byte[] CalculateHash(int index, DateTime timeStamp, byte[] data, byte[] previousHash)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                try
                {
                    var dataToHash = new byte[data.Length + previousHash.Length + sizeof(int) + sizeof(long)];
                   
                    Array.Copy(data, 0, dataToHash, 0, data.Length);
                    Array.Copy(previousHash, 0, dataToHash, data.Length, previousHash.Length);
                    Array.Copy(BitConverter.GetBytes(index), 0, dataToHash, data.Length + previousHash.Length, sizeof(int));
                    Array.Copy(BitConverter.GetBytes(timeStamp.Ticks), 0, dataToHash, data.Length + previousHash.Length + sizeof(int), sizeof(long));

                    Array.Copy(previousHash, 0, dataToHash, data.Length + sizeof(int), previousHash.Length);

                    return sha256.ComputeHash(dataToHash);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new byte[0];
                }
            }
        }

        public override string ToString()
        {
            return $"Index: {Index}\r\n TimeStamp: {TimeStamp.ToString()}\r\n Hash: {BitConverter.ToString(Hash)}\r\n Prev: {BitConverter.ToString(PreviousHash)}";
        }
    }
}
