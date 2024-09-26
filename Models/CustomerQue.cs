using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SuperMarket_Queue_blazor.Components;

namespace CustomerQue
{
    public class CustomQueue
    {
        public List<Customer> CheckoutLine;
        public List<Customer> ExpressCheckoutLine;

        public Action UpdateDOM;

        public CustomQueue()
        {
            CheckoutLine = [];
            ExpressCheckoutLine = [];
        }

        public void Enqueue(string customerName, int customerItems, CustomQueue que)
        {
            if (customerItems > 5)
            {
                Customer newCustomer = new Customer(customerName, customerItems, this.CheckoutLine, que);
                CheckoutLine.Add(newCustomer);
            }
            else if (customerItems <= 5)
            {
                Customer newCustomer = new Customer(customerName, customerItems, this.ExpressCheckoutLine, que);
                ExpressCheckoutLine.Add(newCustomer);
            }
        }

        public bool IsEmpty(List<Customer> customers)
        {
            if (customers.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Dequeue(List<Customer> checkoutLine)
        {
            if (IsEmpty(checkoutLine))
            {
                throw new InvalidOperationException("Que be empty");
            }
            else
            {
                checkoutLine.RemoveAt(0);
                UpdateDOM?.Invoke();
            }
        }
    }

    public class Customer
    {
        public string Name;
        public int ItemCount;
        private readonly int TimePerItem = 1000;
        public List<Customer> CheckoutLine;
        public CustomQueue Que;
        public Timer timer;

        public Customer(string name, int itemCount, List<Customer> checkOutLine, CustomQueue que)
        {
            this.Name = name;
            this.ItemCount = itemCount;
            this.CheckoutLine = checkOutLine;
            this.timer = new Timer(CheckedOut, this, TimePerItem * ItemCount, Timeout.Infinite);
            this.Que = que;
        }

        private void CheckedOut(object? state)
        {
            Console.WriteLine($"TimePerItem: {TimePerItem}");
            Console.WriteLine($"ItemCount: {ItemCount}");
            Console.WriteLine($"CheckoutTime: {ItemCount * TimePerItem}");
            Que.Dequeue(CheckoutLine);
            this.timer.Dispose();
        }

    }
}
