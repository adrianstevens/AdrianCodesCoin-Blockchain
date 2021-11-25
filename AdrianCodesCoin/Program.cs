// See https://aka.ms/new-console-template for more information
using AdrianCodesCoin;

Console.WriteLine("Hello AC Coin!");

var acCoin = new ACBlockChain();

acCoin.AddBlock(new ACBlock(1, DateTime.Now, 5));
acCoin.AddBlock(new ACBlock(2, DateTime.Now, 10));
acCoin.AddBlock(new ACBlock(3, DateTime.Now, -6));

//acCoin.Chain[1].Data = new byte[1];

foreach(var coin in acCoin.Chain)
{
    Console.WriteLine(coin.ToString());
}

Console.WriteLine($"Blockchain is valid: {acCoin.IsChainValid()}");
