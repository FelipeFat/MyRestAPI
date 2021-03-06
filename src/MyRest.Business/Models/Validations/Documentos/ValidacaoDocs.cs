using System.Collections.Generic;
using System.Linq;

namespace MyRest.Business.Models.Validations.Documentos
{
    public class CpfValidacao
    {
        public const int CPFSize = 11;

        public static bool Validate(string cpf)
        {
            var cpfNumbers = Utils.OnlyNumbers(cpf);

            if (!ValidSize(cpfNumbers)) return false;
            return !HasRepeatedDigits(cpfNumbers) && HasValidDigits(cpfNumbers);
        }

        private static bool ValidSize(string valor)
        {
            return valor.Length == CPFSize;
        }

        private static bool HasRepeatedDigits(string value)
        {
            string[] invalidNumbers =
            {
                "00000000000",
                "11111111111",
                "22222222222",
                "33333333333",
                "44444444444",
                "55555555555",
                "66666666666",
                "77777777777",
                "88888888888",
                "99999999999"
            };
            return invalidNumbers.Contains(value);
        }

        private static bool HasValidDigits(string value)
        {
            var number = value.Substring(0, CPFSize - 2);
            var verifyingDigit = new VerifyingDigit(number)
                .WithMultipliersOf(2, 11)
                .Replacing("0", 10, 11);
            var firstDigit = verifyingDigit.CalculateDigit();
            verifyingDigit.AddDigit(firstDigit);
            var secondDigit = verifyingDigit.CalculateDigit();

            return string.Concat(firstDigit, secondDigit) == value.Substring(CPFSize - 2, 2);
        }
    }

    public class CnpjValidation
    {
        public const int CnpjSize = 14;

        public static bool Validate(string cpnj)
        {
            var cnpjNumbers = Utils.OnlyNumbers(cpnj);

            if (!HasValidSize(cnpjNumbers)) return false;
            return !HasRepeatedDigits(cnpjNumbers) && HasValidDigits(cnpjNumbers);
        }

        private static bool HasValidSize(string valor)
        {
            return valor.Length == CnpjSize;
        }

        private static bool HasRepeatedDigits(string valor)
        {
            string[] invalidNumbers =
            {
                "00000000000000",
                "11111111111111",
                "22222222222222",
                "33333333333333",
                "44444444444444",
                "55555555555555",
                "66666666666666",
                "77777777777777",
                "88888888888888",
                "99999999999999"
            };
            return invalidNumbers.Contains(valor);
        }

        private static bool HasValidDigits(string valor)
        {
            var number = valor.Substring(0, CnpjSize - 2);

            var verifyingDigit = new VerifyingDigit(number)
                .WithMultipliersOf(2, 9)
                .Replacing("0", 10, 11);
            var firstDigit = verifyingDigit.CalculateDigit();
            verifyingDigit.AddDigit(firstDigit);
            var secondDigit = verifyingDigit.CalculateDigit();

            return string.Concat(firstDigit, secondDigit) == valor.Substring(CnpjSize - 2, 2);
        }
    }

    public class VerifyingDigit
    {
        private string number;
        private const int module = 11;
        private readonly List<int> _multipliers = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9 };
        private readonly IDictionary<int, string> _replacements = new Dictionary<int, string>();
        private bool _supplementOfModule = true;

        public VerifyingDigit(string number)
        {
            this.number = number;
        }

        public VerifyingDigit WithMultipliersOf(int firstMultiplier, int lastMultiplier)
        {
            _multipliers.Clear();
            for (var i = firstMultiplier; i <= lastMultiplier; i++)
                _multipliers.Add(i);

            return this;
        }

        public VerifyingDigit Replacing(string substitute, params int[] digits)
        {
            foreach (var i in digits)
            {
                _replacements[i] = substitute;
            }
            return this;
        }

        public void AddDigit(string digit)
        {
            number = string.Concat(number, digit);
        }

        public string CalculateDigit()
        {
            return !(number.Length > 0) ? "" : GetDigitSum();
        }

        private string GetDigitSum()
        {
            var sum = 0;
            for (int i = number.Length - 1, m = 0; i >= 0; i--)
            {
                var product = (int)char.GetNumericValue(number[i]) * _multipliers[m];
                sum += product;

                if (++m >= _multipliers.Count) m = 0;
            }

            var mod = (sum % module);
            var resultado = _supplementOfModule ? module - mod : mod;

            return _replacements.ContainsKey(resultado) ? _replacements[resultado] : resultado.ToString();
        }
    }

    public class Utils
    {
        public static string OnlyNumbers(string valor)
        {
            var onlyNumber = "";
            foreach (var s in valor)
            {
                if (char.IsDigit(s))
                {
                    onlyNumber += s;
                }
            }
            return onlyNumber.Trim();
        }
    }
}