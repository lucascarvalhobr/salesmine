using SalesMine.Core.Utils;

namespace SalesMine.Core.DomainObjects
{
    public class Cpf
    {
        public const int CpfMaxLength = 11;

        public string Number { get; private set; }

        protected Cpf() { }

        public Cpf(string number)
        {
            if (!Validate(number)) throw new DomainException("Invalid CPF");

            Number = number;
        }

        public static bool Validate(string cpf)
        {
            cpf = cpf.OnlyNumbers(cpf);

            string value = cpf.Replace(".", "");

            value = value.Replace("-", "");

            if (value.Length != 11)
                return false;

            bool equal = true;

            for (int i = 1; i < 11 && equal; i++)
                if (value[i] != value[0])
                    equal = false;

            if (equal || value == "12345678909")
                return false;

            int[] numbers = new int[11];

            for (int i = 0; i < 11; i++)
                numbers[i] = int.Parse(value[i].ToString());

            int plus = 0;

            for (int i = 0; i < 9; i++)
                plus += (10 - i) * numbers[i];

            int result = plus % 11;

            if (result == 1 || result == 0)
            {
                if (numbers[9] != 0)
                    return false;
            }

            else if (numbers[9] != 11 - result)
                return false;

            plus = 0;

            for (int i = 0; i < 10; i++)
                plus += (11 - i) * numbers[i];

            result = plus % 11;

            if (result == 1 || result == 0)
            {
                if (numbers[10] != 0)
                    return false;
            }
            else
                if (numbers[10] != 11 - result)
                return false;

            return true;
        }
    }
}
