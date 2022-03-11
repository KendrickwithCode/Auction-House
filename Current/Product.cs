using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAuctionHouse
{
    public class Product
    {
        public string listed_by { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public int initial_cost { get; set; }
        public int current_bid { get; set; } = 0;

        public Product(string Type, string Description, int Initial_Cost, string Listed_By)
        {
            this.type = Type;
            this.description = Description;
            this.initial_cost = Initial_Cost;
            this.listed_by = Listed_By;
            this.current_bid = current_bid;
        }

        public Product()
        {

        }


        public void Register_Product
            (string type, string description, int initial_cost, string listed_by)
        {
            Product product = new Product(type, description, initial_cost, listed_by);
            Auction_House.Products.Add(product);
        }

        public void List_Users_Items
            (string login_email, bool save_search)
        {
            int item_count = 1;
            foreach (Product item in Auction_House.Products)
            {
                if (item.listed_by == login_email)
                {
                    Console.WriteLine("> {0}) {1}, {2}", item_count, item.type,
                        item.description);
                    if (save_search)
                    {
                        Auction_House.Search_Results.Add((item_count, item.type, item.description, login_email));
                    }

                    item_count++;

                }
            }

        }
        public void Show_Item_Bids()
        {
            //Search Results is cleared each query, check if any results were found first.
            if (Auction_House.Search_Results.Any())
            {
               int selection = UserInterface.GetInt
                  ("> Please select one of your items");
                foreach (var result in Auction_House.Search_Results)
                {
                    if (result.selection == selection)
                    {
                        foreach (var prev_bid in Auction_House.Bidding_History)
                        {
                            if (prev_bid.product_name == result.product_name &&
                                result.description == prev_bid.description)
                            {
                                Console.WriteLine("> {0} bid ${1}",
                                    prev_bid.client_name, prev_bid.bid);
                            }
                        }
                        if (!Auction_House.Bidding_History.Any())
                        {
                            Console.WriteLine("> No bids found for this item yet.");
                        }
                        //Reset selection after all bids have been displayed.
                        selection = 0;
                    }
                }
            }

        }
        public int Search_Items(string search_query, bool save_search)
        {
            int item_count = 1;
            int count = 0;
            foreach (Product item in Auction_House.Products)
            {
                //Negate items sold by the current active user.
                if (item.type.Contains(search_query)
                    && item.listed_by == Program.Active_User)
                {
                    continue;
                }
                else if (item.type.Contains(search_query))
                {
                    if (count == 0)
                    {
                        Console.WriteLine("> Type found:");
                    }
                    Console.WriteLine("{0}) {1}, {2}",
                        item_count, item.type, item.description);
                    if (save_search)
                    {
                        Auction_House.Search_Results.Add
                            ((item_count, item.type, item.description, Program.Active_User));
                    }
                    item_count++;
                    count++;
                }
            }
            if (count == 0)
            {
                Console.WriteLine("> No items found");
                return count;
            }
            return item_count;
        }

    }
}
