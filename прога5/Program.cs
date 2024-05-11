using System;
using System.Threading.Tasks;

public class ParallelUtils<T, TR>
{
    private Func<T, T, TR> operation;
    private T operand1;
    private T operand2;

    public ParallelUtils(Func<T, T, TR> operation, T operand1, T operand2)
    {
        this.operation = operation;
        this.operand1 = operand1;
        this.operand2 = operand2;
    }

    public TR Result { get; private set; }

    public void SetOperands(T operand1, T operand2)
    {
        this.operand1 = operand1;
        this.operand2 = operand2;
    }

    public void StartEvaluation()
    {
        Task.Run(() =>
        {
            Result = operation(operand1, operand2);
        });
    }

    public void Evaluate()
    {
        StartEvaluation();
        Task.WaitAll();
    }
}

class Operations
{
    public static int Add(int x, int y) => x + y;
    public static int Subtract(int x, int y) => x - y;
    public static int Multiply(int x, int y) => x * y;
    public static double Divide(double x, double y)
    {
        if (y == 0)
            throw new DivideByZeroException("Ділення на нуль неможливе.");
        return x / y;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Приклад використання операцій додавання, віднімання, множення та ділення
        ParallelUtils<int, int> addUtils = new ParallelUtils<int, int>(Operations.Add, 5, 3);
        ParallelUtils<int, int> subtractUtils = new ParallelUtils<int, int>(Operations.Subtract, 10, 4);
        ParallelUtils<int, int> multiplyUtils = new ParallelUtils<int, int>(Operations.Multiply, 8, 6);
        ParallelUtils<double, double> divideUtils = new ParallelUtils<double, double>(Operations.Divide, 20.0, 4.0);

        // Початок оцінки та вивід результатів
        addUtils.StartEvaluation();
        subtractUtils.StartEvaluation();
        multiplyUtils.StartEvaluation();
        divideUtils.StartEvaluation();

        addUtils.Evaluate();
        subtractUtils.Evaluate();
        multiplyUtils.Evaluate();
        divideUtils.Evaluate();

        Console.WriteLine("Результат додавання: " + addUtils.Result);
        Console.WriteLine("Результат віднімання: " + subtractUtils.Result);
        Console.WriteLine("Результат множення: " + multiplyUtils.Result);
        Console.WriteLine("Результат ділення: " + divideUtils.Result);
    }
}
