using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAuctionHouse
{ 
    class Client
    {
        public string Name { get { return name; } }
        private string name;
        public string Password { get { return password; } }
        private string password;
        public string Email { get { return email; } }
        private string email;
        public string Address { get { return address; } }
        private string address;

        public bool LoggedIn = false;


        public Client(string full_name, string email, string password, string address)
        {
            this.name = full_name;
            this.password = password;
            this.email = email;
            this.address = address;
        }

        public Client(string full_name, string email, string password)
        {
            this.name = full_name;
            this.password = password;
            this.email = email;
        }

        public Client()
        {

        }

        public bool Register_Client(string name, string email, string password, string address)
        {
            foreach (Client detail in Auction_House.Clients)
            {
                if (detail.Email.Equals(email))
                {
                    Console.WriteLine("> Email already registered.");
                    return false;
                }
            }
            Client User = new Client(name, email, password, address);
            Auction_House.Clients.Add(User);
            return true;
        }

        public void Login(string login_email, string login_password)
        {
            int count = 0;
            //if there are no login details in the list.
            if (!Auction_House.Clients.Any())
            {
                Console.WriteLine("> No Users Found");
            }
            foreach (Client detail in Auction_House.Clients)
            {
                if (detail.Email.Equals(login_email))
                {
                    if (detail.Password.Equals(login_password))
                    {
                        count = 1;
                        LoggedIn = true;
                        Console.WriteLine
                            ("Welcome to the Online Auction House, {0}!", detail.Name);
                        break;
                    }
                    else
                    {
                        count = 1;
                        Console.WriteLine("> Invalid Password");
                        break;
                    }
                }
            }
            if (count == 0)
            {
                Console.WriteLine("> Invalid Credentials");
            }

        }
        public void Logout(string name)
        {
            LoggedIn = false;
            Console.WriteLine("> {0} logged out.", name);
        }
        public bool Place_Bid
            (int selection, int bid, bool home_delivery, string client_name)
        {
            foreach (var result in Auction_House.Search_Results)
            {
                if (result.selection == selection)
                {
                    foreach (Product item in Auction_House.Products)
                    {
                        if (result.product_name == item.type &&
                            result.description == item.description)
                        {
                            if (bid < item.initial_cost)
                            {
                                Console.WriteLine("> Bid is too low, " +
                                    "please enter a value higher than ${0}", item.initial_cost);
                                return false;
                            }
                            item.current_bid = bid;
                            Console.WriteLine("> {0} bid ${1}", client_name, item.current_bid);
                            Auction_House.Bidding_History.Add
                                ((item.type, item.description, 
                                client_name, item.current_bid, home_delivery));
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void Sell_Product()
        {
            string highest_bidder = "";
            int highest_bid = 0;
            bool delivery_method = false;
            int count = 2;
            List<Product> Product_Removal =
                new List<Product>();

            int selection = UserInterface.GetInt
                ("> Please select one of your items to sell");
            foreach (var result in Auction_House.Search_Results)
            {
                if (result.selection == selection)
                {
                    //This is updated on first loop
                    int previous_bid = 0;
                    foreach (Product item in Auction_House.Products)
                    {
                        if (result.product_name == item.type && 
                            result.description == item.description)
                        {
                            foreach (var current_bid in Auction_House.Bidding_History)
                            {
                                var bids_to_remove = current_bid;
                                if (current_bid.product_name == result.product_name &&
                                    current_bid.client_name != result.client_name &&
                                    current_bid.description == result.description)
                                {
                                    //Update previous bid every 2nd loop after the first
                                    //compare each bid and get the highest bidder
                                    if (count % 2 == 0)
                                    {
                                        previous_bid = current_bid.bid;
                                    }
                                    if (current_bid.bid >= previous_bid)
                                    {
                                        highest_bidder = "> Sold " + current_bid.product_name + " to " +
                                            current_bid.client_name + " for $" + current_bid.bid;
                                        Product_Removal.Add(item);
                                        highest_bid = current_bid.bid;
                                        delivery_method = current_bid.delivery_method;
                                    }
                                    count++;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            foreach (Product product in Product_Removal)
            {
                Auction_House.Products.Remove(product);
            }
            if (delivery_method == true)
            {
                Shipping delivery = new Home_Delivery(highest_bid);
                Console.WriteLine(highest_bidder);
                int charges = delivery.Charges();
                Console.WriteLine("Charges: ${0}", charges);
            }
            else if (delivery_method == false && count > 2)
            {
                Shipping delivery = new Click_and_Collect(highest_bid);
                Console.WriteLine(highest_bidder);
                int charges = delivery.Charges();
                Console.WriteLine("Charges: ${0}", charges);
            }
        }

    }
}
