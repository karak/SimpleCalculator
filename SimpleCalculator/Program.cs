using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace SimpleCalculator
{
    public interface ICalculator
    {
        string Calculate(string input);
    }

    /**
     * <summary>MEF sample program</summary>
     * 
     * <see cref="http://msdn.microsoft.com/ja-jp/library/vstudio/dd460648.aspx"/>
     */
    class Program
    {
        private CompositionContainer _container;

        [Import(typeof(ICalculator))]
        public ICalculator _calculator;

        private Program()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));

            _container = new CompositionContainer(catalog);

            try
            {
                this._container.ComposeParts(this);
            }
            catch (CompositionException ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        static void Main(string[] args)
        {
            var p = new Program();
            string input;
            Console.WriteLine("Enter your expression:");
            while (true)
            {
                input = Console.ReadLine();
                string solution = p._calculator.Calculate(input);
                Console.WriteLine(solution);
            }
        }
    }

    public interface IOperation
    {
        int Operate(int left, int right);
    }

    public interface IOperationData
    {
        char Symbol { get; }
    }

    [Export(typeof(ICalculator))]
    class MySimpleCalculator : ICalculator
    {
        [ImportMany]
        IEnumerable<Lazy<IOperation, IOperationData>> _operations;

        public string Calculate(string input)
        {
            int left, right;
            char @operator;

            int whereOp = FindFirstNonDigitChar(input);
            if (whereOp < 0) return "Could not parse command!";

            try
            {
                left = int.Parse(input.Substring(0, whereOp));
                right = int.Parse(input.Substring(whereOp + 1));
                @operator = input[whereOp];
            }
            catch
            {
                return "Could not parse command!";
            }

            foreach (var op in _operations)
            {
                if (op.Metadata.Symbol == @operator)
                {
                    return op.Value.Operate(left, right).ToString();
                }
            }
            return String.Format("Operation \"{0}\" not found!", @operator);
        }

        private static int FindFirstNonDigitChar(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (!Char.IsDigit(s[i]))
                {
                    return i;
                }
            }
            return -1;
        }
    }

    [Export(typeof(IOperation))]
    [ExportMetadata("Symbol", '+')]
    class Addition : IOperation
    {
        public int Operate(int left, int right)
        {
            return left + right;
        }
    }

    [Export(typeof(IOperation))]
    [ExportMetadata("Symbol", '-')]
    class Subtraction : IOperation
    {
        public int Operate(int left, int right)
        {
            return left - right;
        }
    }

    [Export(typeof(IOperation))]
    [ExportMetadata("Symbol", '*')]
    class Multiplication : IOperation
    {
        public int Operate(int left, int right)
        {
            return left * right;
        }
    }

    [Export(typeof(IOperation))]
    [ExportMetadata("Symbol", '/')]
    class Division : IOperation
    {
        public int Operate(int left, int right)
        {
            return left / right;
        }
    }

    [Export(typeof(IOperation))]
    [ExportMetadata("Symbol", '%')]
    class Modulo : IOperation
    {
        public int Operate(int left, int right)
        {
            return left % right;
        }
    }
}
