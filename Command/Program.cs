using System;

namespace Conceptual
{
    // Интерфейс Команды объявляет метод для выполнения команд.
    public interface IWaiter
    {
        void ToSay();
    }

    // Некоторые команды способны выполнять простые операции самостоятельно.
    class WaiterGettingOrder : IWaiter
    {
        private string _payload = string.Empty;

        public WaiterGettingOrder(string payload)
        {
            _payload = payload;
        }

        public void ToSay()
        {
            Console.WriteLine($"Waiter: {this._payload}");
        }
    }


    class Taxi : IWaiter
    {
        private string _payload = string.Empty;

        public Taxi(string payload)
        {
            _payload = payload;
        }

        public void ToSay()
        {
            Console.WriteLine($"Taxi: {this._payload}");
        }
    }

    // Но есть и команды, которые делегируют более сложные операции другим
    // объектам, называемым «получателями».
    class WaiterHendToCooker : IWaiter
    {
        private Cooker _cook;

        // Данные о контексте, необходимые для запуска методов получателя.
        private string _a;

        private string _b;

        // Сложные команды могут принимать один или несколько объектов-
        // получателей вместе с любыми данными о контексте через конструктор.
        public WaiterHendToCooker(Cooker cooker, string a, string b)
        {
            _cook = cooker;
            _a = a;
            _b = b;
        }

        // Команды могут делегировать выполнение любым методам получателя.
        public void ToSay()
        {
            Console.WriteLine("Waiter hended the order to cooker");
            this._cook.CookAMeal(this._a);
            this._cook.CookSomethingElse(this._b);
        }
    }

    // Классы Получателей содержат некую важную бизнес-логику. Они умеют
    // выполнять все виды операций, связанных с выполнением запроса. Фактически,
    // любой класс может выступать Получателем.

    // receiver
    class Cooker
    {
        public void CookAMeal(string dish)
        {
            Console.WriteLine($"Cooker: Working on ({dish}.)");
        }

        public void CookSomethingElse(string b)
        {
            Console.WriteLine($"Cooker: Also working on ({b}.)");
        }
    }

    // Отправитель связан с одной или несколькими командами. Он отправляет
    // запрос команде.
    class Invoker
    {
        private IWaiter _onStart; // официант 1

        private IWaiter _onProcess; // официант 1

        private IWaiter _onFinish; // официант 2 

        // Инициализация команд
        public void SetOnStart(IWaiter command)
        {
            this._onStart = command;
        }

        public void SetOnProcess(IWaiter command)
        {
            this._onProcess = command;
        }

        public void SetOnFinish(IWaiter command)
        {
            this._onFinish = command;
        }

        // Отправитель не зависит от классов конкретных команд и получателей.
        // Отправитель передаёт запрос получателю косвенно, выполняя команду.
        public void DoSomethingImportant()
        {
            Console.WriteLine("Client: Hello");
            if (this._onStart is IWaiter)
            {
                this._onStart.ToSay();
            }

            Console.WriteLine("Cleint: I am waiting");
            if (_onProcess is IWaiter)
            {
                _onProcess.ToSay();
            }

            Console.WriteLine("Client: I need a taxi");
            if (_onFinish is IWaiter)
            {
                _onFinish.ToSay();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Клиентский код может параметризовать отправителя любыми
            // командами.
            Invoker invoker = new Invoker();
            invoker.SetOnStart(new WaiterGettingOrder("Say Hi!"));
            Cooker receiver = new Cooker();

            invoker.SetOnProcess(new WaiterHendToCooker(receiver, "Make fish", "Make meat"));

            invoker.SetOnFinish(new Taxi("Taxi is driving to client"));

            invoker.DoSomethingImportant();
            
            Console.ReadLine();
        }
    }
}
