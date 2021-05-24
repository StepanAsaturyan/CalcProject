using System;
using System.IO;
using System.Text;

namespace CalcProject
{
    public class CalcFile
    {
        private readonly string[] _fileData;
        private readonly string[] _resultData;

        public CalcFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new NullReferenceException("File not found (filepath is wrong or empty)");
            }
            
            _fileData = File.ReadAllLines(filePath);
            _resultData = new string[_fileData.Length];
        }

        public void Start()
        {

            for (int i = 0; i < _fileData.Length; i++)
            {
                var stringBuilder = new StringBuilder();
                var calc = new Calc(_fileData[i]);

                try
                {
                    stringBuilder.Append(_resultData[i]).Append(calc.Calculate());
                }

                catch (DivideByZeroException)
                {
                    stringBuilder.Append(_resultData[i]).Append("dividing by zero is not allowed");
                }

                catch (ArgumentException)
                {
                    stringBuilder.Append(_resultData[i]).Append("invalid expression");
                }

                _resultData[i] = stringBuilder.ToString();
            }

            WriteResult();
        }

        private void WriteResult()
        {
            using var streamWriter = new StreamWriter("Results.txt");

            for (int i = 0; i < _fileData.Length; i++)
            {
                var sb = new StringBuilder();
                sb.Append(_fileData[i]).Append(" = ").Append(_resultData[i]);
                streamWriter.WriteLine(sb);
            }
        }
    }
}
