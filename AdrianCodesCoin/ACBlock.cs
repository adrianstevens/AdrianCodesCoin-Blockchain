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

        public long Nonce { get; set; } = 0;

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
            Hash = CalculateHash(Index, TimeStamp, data, previousHash, Nonce);
        }

        public void UpdateHash()
        {
            Hash = CalculateHash(Index, TimeStamp, Data, PreviousHash, Nonce);
        }

        public byte[] CalculateCurrentHash()
        {
            return CalculateHash(Index, TimeStamp, Data, PreviousHash, Nonce);
        }
               
        byte[] CalculateHash(int index, DateTime timeStamp, byte[] data, byte[] previousHash, long nonce)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                try
                {
                    var dataToHash = new byte[data.Length + previousHash.Length + sizeof(int) + 2 * sizeof(long)];

                    //data
                    int destinationIndex = 0;
                    Array.Copy(data, 0, dataToHash, 0, data.Length);
                    //previous hash
                    destinationIndex = data.Length;
                    Array.Copy(previousHash, 0, dataToHash, destinationIndex, previousHash.Length);
                    //index
                    destinationIndex += previousHash.Length;
                    Array.Copy(BitConverter.GetBytes(index), 0, dataToHash, destinationIndex, sizeof(int));
                    //timeStamp (as long)
                    destinationIndex += sizeof(int);
                    Array.Copy(BitConverter.GetBytes(timeStamp.Ticks), 0, dataToHash, destinationIndex, sizeof(long));
                    //nonce
                    destinationIndex += sizeof(long);
                    Array.Copy(BitConverter.GetBytes(nonce), 0, dataToHash, destinationIndex, sizeof(long));

                    return sha256.ComputeHash(dataToHash);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new byte[0];
                }
            }
        }

        public void MineBlock(int difficulty)
        {
            Console.WriteLine("Mining block ...");

            //should probably cap difficulty
            bool isWorking = true;

            while (isWorking)
            {
                isWorking = false;
                for (int i = 0; i < difficulty; i++)
                {
                    if (Hash[i] != 0)
                    {
                        isWorking = true;
                        Nonce++;
                        UpdateHash();
                        break;
                    }
                }
            }

            Console.WriteLine($"Mined coin with a nonce of {Nonce}");
        }


        public override string ToString()
        {
            return $"Index: {Index}\r\n TimeStamp: {TimeStamp.ToString()}\r\n Hash: {BitConverter.ToString(Hash)}\r\n Prev: {BitConverter.ToString(PreviousHash)}";
        }
    }
}
