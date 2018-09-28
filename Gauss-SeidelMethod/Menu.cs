using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gauss_SeidelMethod.App
{
    internal interface IMenuItem : IEnumerable<IMenuItem>
    {
        string Text { get; }
        IReadOnlyList<IMenuItem> Children { get; }

        void InvokeHandler();
    }

    internal class Menu
    {
        readonly IMenuItem _root;

        public Menu(IMenuItem root)
        {
            _root = root;
        }

        public void RunMenu()
        {
            this.RunMenuImpl(_root);
        }

        private void RunMenuImpl(IMenuItem currMenu)
        {
            var selectedMenu = this.SelectMenu(currMenu);

            while (selectedMenu != null)
            {
                if (selectedMenu.Children.Count > 0)
                    this.RunMenuImpl(selectedMenu);
                else
                    selectedMenu.InvokeHandler();

                selectedMenu = this.SelectMenu(currMenu);
            }
        }

        private IMenuItem SelectMenu(IMenuItem currMenu)
        {
            Console.WriteLine(currMenu.Text + ":");

            currMenu.ForEeach((item, n) => Console.WriteLine($"{n + 1}. {item.Text}"));
            Console.WriteLine($"{currMenu.Children.Count + 1}. " + "Exit");

            int index;
            Console.Write("Item's number" + ": ");
            while (!int.TryParse(Console.ReadLine(), out index) || index < 1 || index > currMenu.Children.Count + 1)
            {
                Console.WriteLine("Invalid Number");
                Console.Write("Item's number" + ": ");
            }

            return index > currMenu.Children.Count ? null : currMenu.Children[index - 1];
        }

    }
}
