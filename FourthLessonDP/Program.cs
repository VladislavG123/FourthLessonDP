using System;

namespace Conceptual
{
    // Абстракция устанавливает интерфейс для «управляющей» части двух иерархий
    // классов. Она содержит ссылку на объект из иерархии Реализации и
    // делегирует ему всю настоящую работу.
    class Figure
    {
        protected IColor _color;
        protected IMaterial _material;

        public Figure(IColor color, IMaterial material)
        {
            _color = color;
            _material = material;
        }

        public virtual string Operation()
        {
            return "Color:\n" + _color.OperationImplementation() + "\n"
                 + "Material:\n" + _material.OperationImplementation();
        }
    }



    // Можно расширить Абстракцию без изменения классов Реализации.
    class ExtendedAbstraction : Figure
    {
        public ExtendedAbstraction(IColor color, IMaterial material) : base(color, material)
        {
        }

        public override string Operation()
        {
            return "ExtendedAbstraction: Extended operation with:\n" + _color.OperationImplementation();
        }
    }



    // Реализация устанавливает интерфейс для всех классов реализации. Он не
    // должен соответствовать интерфейсу Абстракции. На практике оба интерфейса
    // могут быть совершенно разными. Как правило, интерфейс Реализации
    // предоставляет только примитивные операции, в то время как Абстракция
    // определяет операции более высокого уровня, основанные на этих примитивах.
    public interface IColor
    {
        string OperationImplementation();
    }


    public interface IMaterial
    {
        string OperationImplementation();
    }

    // Каждая Конкретная Реализация соответствует определённой платформе и
    // реализует интерфейс Реализации с использованием API этой платформы.
    class Green : IColor
    {
        public string OperationImplementation()
        {
            return "Green.\n";
        }
    }



    class Wood : IMaterial
    {
        public string OperationImplementation()
        {
            return "Wood.\n";
        }
    }






    class Client
    {
        // За исключением этапа инициализации, когда объект Абстракции
        // связывается с определённым объектом Реализации, клиентский код должен
        // зависеть только от класса Абстракции. Таким образом, клиентский код
        // может поддерживать любую комбинацию абстракции и реализации.
        public void ClientCode(Figure abstraction)
        {
            Console.Write(abstraction.Operation());
        }
    }



    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();

            Figure abstraction;
            // Клиентский код должен работать с любой предварительно
            // сконфигурированной комбинацией абстракции и реализации.
            abstraction = new Figure(new Green(), new Wood());
            client.ClientCode(abstraction);

            Console.WriteLine();

            abstraction = new ExtendedAbstraction(new Green(), new Wood());
            client.ClientCode(abstraction);

            Console.ReadLine();
        }
    }
}