using CalculatorIP;
using CalculatorIP.Model.Data.IP;
using CalculatorIP.Model.Data.Mask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static CalculatorIP.Session;

namespace CalculatorIP.Model.Task.Types
{
    public class SubnetOctetCalculator : TaskBase
    {
        private Random _random = new Random();

        private IPAdressType _ipAdressType;

        private IPAdress _ipAdress;
        private Netmask _netmask;

        private string _result;

        private int _targetSubnet = 0;
        private int _targetOctet = 0;
        private int _numberOfBrokenSubnets = 0;

        protected override string[] _content => new string[]
        {
            "Сеть",
            "разбита на",
            "подсети(ей)",
            "Каким будет последний октет",
            "выданного IP-адреса в",
            "подсети?"
        };


        public override GeneratedTask GetTask()
        {
            _ipAdressType = ipAdressTypes[_random.Next(0, ipAdressTypes.Length)];

            _ipAdress = IPRandomizer.GenerateRandomIP(_ipAdressType);
            _netmask = ListNetmask.GetRandomMask(_ipAdressType);

            string task = TaskContentGenerator();
            string description = GetSolutionDescription();
            string result = GenerateSolution();

            GeneratedTask generatedTask = new GeneratedTask(task, result, description);

            return generatedTask;
        }

        private string TaskContentGenerator()
        {
            int[] values = RestrictionForTypes();

            _numberOfBrokenSubnets = _random.Next(1, values[0] + 1);
            _targetSubnet = values[1];
            _targetOctet = values[2];

            return $"{_content[0]} {_ipAdress.GetIP('.')} {_content[1]} {_numberOfBrokenSubnets} {_content[2]}.\n{_content[3]} {_targetOctet} ";
        }

        private int[] RestrictionForTypes()
        {
            int[] values = new int[3];

            switch(_ipAdressType)
            {
                case IPAdressType.A:
                    values[0] = (int)Math.Pow(2, _random.Next(3, 5));
                    values[1] = _random.Next((int)Math.Pow(2, 2), values[0]);
                    values[2] = _random.Next(1, 4);
                    break;

                case IPAdressType.B:
                    values[0] = (int)Math.Pow(2, _random.Next(3, 5));
                    values[1] = _random.Next((int)Math.Pow(2, 2), values[0]);
                    values[2] = _random.Next(1, 4);
                    break;

                case IPAdressType.C:
                    values[0] = _netmask.GetSubnetsCount();
                    values[1] = _random.Next(1, _netmask.GetSubnetsCount() + 1);
                    values[2] = _random.Next(1, _netmask.GetAdressesCount() + 1);
                    break;
            }

            return values;
        }

        protected override string GenerateSolution()
        {
            return _result;
        }

        protected override string GetSolutionDescription()
        {
            StringBuilder stringBuilder = new StringBuilder();

            int byteCount = (int)Math.Ceiling(Math.Log(_numberOfBrokenSubnets, 2)   );
            int zeroBytesInMask = 0;
            int hostCount = 0;

            stringBuilder.Append($"Сеть класса {_ipAdressType}, следовательно стандартная маска подсети:\n");
            stringBuilder.Append(ListNetmask.GetDefaultMask(_ipAdressType).GetAdress().GetBinaryData('.') + "\n");
            stringBuilder.Append($"{_numberOfBrokenSubnets} = 2^{byteCount} n = {byteCount}\n");
            stringBuilder.Append($"Маска: {CalculateMask(byteCount, out zeroBytesInMask)}\n");

            hostCount = (int)Math.Pow(2, zeroBytesInMask) - 2;

            stringBuilder.Append($"M = 2^m-2 | m = {zeroBytesInMask}\n");
            stringBuilder.Append($"M = 2^{zeroBytesInMask} - 2 = {Math.Pow(2, zeroBytesInMask)} - 2 = {hostCount}\n\n");

            if (_ipAdressType == IPAdressType.C)
                stringBuilder.Append(CSolution(byteCount));
            else
                stringBuilder.Append(ABSolution(byteCount));

            return stringBuilder.ToString();
        }
        
        private string CalculateMask(int byteCount, out int zeroByte)
        {
            int octetCount = Math.Abs(2 - _ipAdressType.GetHashCode());
            byteCount -= 8 * octetCount;

            zeroByte = 0;

            StringBuilder stringBuilder = new StringBuilder();

            string[] mask = ListNetmask.GetDefaultMask(_ipAdressType).GetAdress().GetBinaryData('.').Split('.');

            for (int i = 0; i < mask.Length - 1; i++)
            {
                mask[i] = "11111111.";
                stringBuilder.Append(mask[i]);
            }
            
            for (int i = 0; i < 8; i++)
            {
                if (byteCount > 0)
                {
                    stringBuilder.Append("1");
                    byteCount--;
                }
                else
                {
                    stringBuilder.Append("0");
                    zeroByte++;
                }
            }

            return stringBuilder.ToString();
        }

        private string ABSolution(int byteCount)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int lastIndex = 0;

            string[] binaryIPArray = _ipAdress.GetIP().GetBinaryData('.').Split('.');
            char[] binaryIP = (binaryIPArray[0] + binaryIPArray[1] + binaryIPArray[2] + binaryIPArray[3]).ToCharArray();

            int targetSubnet = _targetSubnet;

            while (targetSubnet > 0)
            {

                lastIndex = (int)Math.Abs(Math.Floor(Math.Log(targetSubnet, 2)));


                binaryIP[binaryIP.Length - lastIndex - 1] = '1';

                targetSubnet -= (int)Math.Pow(2, Math.Floor(Math.Log(targetSubnet, 2)));
            }

            string[] binaryOctets = CharArrayToBinaryStringArray(binaryIP);

            int[] intOctets = new int[4];

            for (int i = 0; i < binaryOctets.Length; i++)
            {
                intOctets[i] = binaryOctets[i].ToIntInBinaryString();
            }

            stringBuilder.Append($"Диапазон адресов для подсети ");
            stringBuilder.Append($"{intOctets[0]}.{intOctets[1]}.{intOctets[2]}.{intOctets[3]}\n");
            stringBuilder.Append($"{intOctets[0]}.{intOctets[1]}.{intOctets[2]}.{intOctets[3] + 1} - ");
            stringBuilder.Append($"{intOctets[0]}.{intOctets[1]}.{intOctets[2]}.{intOctets[3] + byteCount}\n\n\n");

            _result = $"{intOctets[0]}.{intOctets[1]}.{intOctets[2]}.{intOctets[3] + _targetOctet}";

            stringBuilder.Append($"Ответ: {_result}");

            return stringBuilder.ToString();
        }

        private string CSolution(int byteCount)
        {
            StringBuilder stringBuilder = new StringBuilder();

            int lastElementInLastOctet = (256 / _numberOfBrokenSubnets * (_targetSubnet - 1)) + byteCount;

            int[] octets = new int[4];
            string[] stringOctets = _ipAdress.GetIP('.').Split('.');

            for(int i = 0; i < octets.Length; i++)
                octets[i] = int.Parse(stringOctets[i]);
            


            stringBuilder.Append($"Диапазон адресов для подсети {octets[0]}.{octets[1]}.{octets[2]}.{octets[3]}\n");
            stringBuilder.Append($"{octets[0]}.{octets[1]}.{octets[2]}.{octets[3] + 1} - ");
            stringBuilder.Append($"{octets[0]}.{octets[1]}.{octets[2]}.{lastElementInLastOctet}\n\n\n");

            _result = $"{octets[0]}.{octets[1]}.{octets[2]}.{octets[3] + _targetOctet}";

            stringBuilder.Append($"Ответ: {_result}");

            return stringBuilder.ToString();

        }

        private string[] CharArrayToBinaryStringArray(char[] array)
        {
            string[] binaryOctets = new string[4];

            List<char> chars = new List<char>();
            chars = array.ToList();

            for (int i = 0; i < binaryOctets.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    binaryOctets[i] += chars[0];
                    chars.RemoveAt(0);
                }
            }

            return binaryOctets;
        }
    }
}
