using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAuctionHouse
{
    static class Auction_House
    {
        static public List<Product> Products = new List<Product>();
        static public List<Client> Clients = new List<Client>();

        static public List
            <(int selection, string product_name, string description, string client_name)>
            Search_Results = new List<(int, string, string, string)>();

        static public List<(string product_name, string description, string client_name,
            int bid, bool delivery_method)>
            Bidding_History =new List<(string, string, string, int, bool)>();
    }
}
