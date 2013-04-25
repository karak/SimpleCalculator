using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using IOperation = SimpleCalculator.IOperation;

namespace ExtendedOperations
{
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
