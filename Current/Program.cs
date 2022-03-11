using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OnlineAuctionHouse
{
    public class Program
    {
        public static string Active_User { get; set; }
        static void Main(string[] args)
        {
            MainMenu();
        }
        static void MainMenu()
        {
            const int REGISTER = 0, LOGIN = 1, EXIT = 2;
            Client User = new Client();
            Product product = new Product();
            bool running = true;
            while (running)
            {

                if (!User.LoggedIn)
                {
                    int option = UserInterface.GetOption("",
                    ") Register as new Client", ") Login as Current User", ") Exit"
                      );
                    switch (option)
                    {
                        case REGISTER:
                            string client_name = UserInterface.GetInput("> Full Name");
                            string client_email = UserInterface.GetInput("> Email");
                            string client_password = UserInterface.GetPassword("> Password");
                            string client_address = UserInterface.GetInput("> Address");
                            bool registered = User.Register_Client(client_name, client_email,
                                client_password, client_address);
                            if (registered)
                            {
                                Console.WriteLine
                                    ("> {0} successfully registered!\n", client_name);
                            }
                            else if (!registered)
                            {
                                break;
                            }
                            break;
                        case LOGIN:
                            string login_email = UserInterface.GetInput("> Email");
                            string login_password = UserInterface.GetPassword("> Password");
                            Active_User = login_email;
                            User.Login(login_email, login_password);
                            break;
                        case EXIT:
                            running = false;
                            break;
                        default:
                            break;
                    }
                }
                else if (User.LoggedIn)
                {

                    const int ITEM_REGISTER = 0, LIST_ITEM = 1, SEARCH_ITEM = 2,
                    BID_ITEM = 3, MY_ITEM_BIDS = 4, CONFIRM_HIGHEST_BID = 5, LOGOUT = 6;
                    int options = UserInterface.GetOption("",
                    ") Register item for sale", ") List my items", ") Search for items", ") Place a bid on an item",
                    ") List bids received for my items", ") Sell one of my items to the highest bidder",
                    ") Logout");
                    switch (options)
                    {
                        case ITEM_REGISTER:
                            string type = UserInterface.GetInput("> Type");
                            string description = UserInterface.GetInput("> Description");
                            int initial_cost = UserInterface.GetInt("> Initial Cost");
                            
                            product.Register_Product(type, description, initial_cost, Active_User);
                            break;
                        case LIST_ITEM:
                            product.List_Users_Items(Active_User, save_search: false);
                            break;
                        case SEARCH_ITEM:
                            string search_query = UserInterface.GetInput("> Type");
                            product.Search_Items(search_query, save_search: false);
                            break;
                        case BID_ITEM:
                            search_query = UserInterface.GetInput("> Type");
                            int found_items = product.Search_Items(search_query, save_search: true);

                            if (found_items >= 1)
                            {
                                int selection = UserInterface.GetInt
                                    ("> Please enter the item you'd like to bid on");
                                int bid = UserInterface.GetInt("> Enter Bid ($)");
                                bool delivery = UserInterface.GetBool("> Home Delivery?");
                                User.Place_Bid(selection, bid, delivery, Active_User);

                            }
                            Auction_House.Search_Results.Clear();
                            break;
                        case MY_ITEM_BIDS:
                            product.List_Users_Items(Active_User, save_search: true);
                            product.Show_Item_Bids();
                            Auction_House.Search_Results.Clear();
                            break;
                        case CONFIRM_HIGHEST_BID:
                            product.List_Users_Items(Active_User, save_search: true);
                            bool empty_list = !Auction_House.Products.Any();
                            if (empty_list)
                            {
                                Console.WriteLine("> There are no items registered.");
                            }
                            else
                            {
                                User.Sell_Product();
                            }
                            Auction_House.Search_Results.Clear();
                            
                            break;
                        case LOGOUT:
                            User.Logout(Active_User);
                            break;
                        default: break;
                    }
                }


            }
        }
    }
}
