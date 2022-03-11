using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAuctionHouse
{
    public abstract class Shipping
    {
        protected int sell_price;
        protected int sales_tax;
        public Shipping(int Sell_Price)
        {
            this.sell_price = Sell_Price;
            this.sales_tax = (int)((double)15 / sell_price * 100);
        }
    public virtual int Charges()
        {
            //15%
            return sales_tax;
        }

    }
     class Home_Delivery : Shipping
    {
        protected int delivery_fee = 20;
        protected int home_delivery = 5;
        public Home_Delivery(int Sell_Price) : base(Sell_Price)
        {

        }

        public override int Charges()
        {
            //15% + $20 delivery fee + $5 tax
            return this.sales_tax + this.delivery_fee + this.home_delivery;
        }
    }
    class Click_and_Collect : Shipping
    {
        protected int delivery_fee = 10;
        public Click_and_Collect (int Sell_Price) : base(Sell_Price)
        {

        }
        public override int Charges()
        {
            //15% + $10 delivery fee
            return this.sales_tax + this.delivery_fee;
        }
    }
}
