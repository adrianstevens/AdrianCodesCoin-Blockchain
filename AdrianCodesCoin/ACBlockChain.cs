using System.Text;

namespace AdrianCodesCoin
{
    internal class ACBlockChain
    {
        public List<ACBlock> Chain { get; private set; }

        public int Difficulty { get; set; } = 2; //increase to give your system a workout

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

                newBlock.MineBlock(Difficulty);

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
