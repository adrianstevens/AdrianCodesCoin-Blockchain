using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdrianCodesCoin
{
    internal class ACBlockChain
    {
        public List<ACBlock> Chain { get; private set; }

        public ACBlockChain()
        {
            Chain = new List<ACBlock>();
            Chain.Add(CreateGenesisBlock());
        }

        ACBlock CreateGenesisBlock()
        {
            return new ACBlock(0,
                new DateTime(1977, 1, 1),
                Encoding.ASCII.GetBytes("ACGenesis"), new byte[0]);
        }

        ACBlock GetLatestBlock()
        {
            return Chain.Last();
        }

        public bool AddBlock(ACBlock newBlock)
        {
            try
            {
                newBlock.PreviousHash = GetLatestBlock().Hash;

                var hash1 = newBlock.CalculateCurrentHash();
                var hash2 = newBlock.CalculateCurrentHash();

                newBlock.UpdateHash();

                Chain.Add(newBlock);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsChainValid()
        {
            //skip genesis 
            for (int i = 1; i < Chain.Count; i++)
            {
                //validate index sequence
                if(Chain[i -1].Index + 1 != Chain[i].Index)
                {
                    Console.WriteLine($"Block {i}'s index is not sequential");
                    return false;
                }

                //validate current hash
                if (Chain[i].Hash.SequenceEqual(Chain[i].CalculateCurrentHash()) == false)
                {
                    Console.WriteLine($"Block {i}'s hash is invalid");
                    return false;
                }

                //validate previous hash
                if (Chain[i].PreviousHash.SequenceEqual(Chain[i - 1].CalculateCurrentHash()) == false)
                {
                    Console.WriteLine($"Block {i}'s previous hash is invalid");
                    return false;
                }
            }
            return true;
        }
    }
}
