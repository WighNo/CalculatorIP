using CalculatorIP.Model.Data.IP;
using CalculatorIP.Model.Data.Mask;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using static CalculatorIP.Session;

namespace CalculatorIP.Model.Task.Types
{
    class SubnetsCount : TaskBase
    {
        private Random _random = new Random();

        private IPAdressType _ipAdressType;

        private IPAdress _ipAdress;
        private Netmask _netmask;

        private int _subnetCount;

        protected override string[] _content => new string[]
        {
            "Какое максимальное кол-во хостов при разбиении сети",
            "на",
            "подсетей?"
        };

        public override GeneratedTask GetTask()
        {
            _ipAdressType = ipAdressTypes[_random.Next(0, ipAdressTypes.Length)];

            _ipAdress = IPRandomizer.GenerateRandomIP(_ipAdressType);
            _netmask = ListNetmask.GetRandomMask(_ipAdressType);

            _subnetCount = _random.Next(1, _netmask.GetSubnetsCount() + 1);

            string task = TaskContentBuilder();
            string description = GetSolutionDescription();
            string result = GenerateSolution();

            GeneratedTask generatedTask = new GeneratedTask(task, result, description);

            return generatedTask;
        }

        private string TaskContentBuilder()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(_content[0] + "\n");
            stringBuilder.Append(_ipAdress.GetIP('.'));
            stringBuilder.Append(' ');
            stringBuilder.Append(_content[1]);
            stringBuilder.Append(' ');
            stringBuilder.Append(_subnetCount);
            stringBuilder.Append(' ');
            stringBuilder.Append(_content[2]);

            return stringBuilder.ToString();
        }

        protected override string GenerateSolution()
        {
            return _netmask.GetAdressesCount().ToString();
        }

        protected override string GetSolutionDescription()
        {
            StringBuilder stringBuilder = new StringBuilder();

            int subnetsLog = (int)Math.Ceiling(Math.Log(_subnetCount, 2));

            string defaultMask = ListNetmask.GetDefaultMask(_ipAdressType).GetAdress().GetBinaryData('.');

            int firstOctet = GetFirstOctet(_netmask.GetAdress());

            stringBuilder.Append($"Эта подсеть класса {_ipAdressType}, следовательно стандартная маска подсети: \n");
            stringBuilder.Append(defaultMask);
            stringBuilder.Append("\n\n");
            stringBuilder.Append($"Ближайшая степень двойки, влючающая число {_subnetCount} = {subnetsLog}\n");
            stringBuilder.Append($"{_subnetCount} =  2^{subnetsLog}, а значит: ");
            stringBuilder.Append($"{subnetsLog} - кол-во битов занятых под адрес подсети\n");
            stringBuilder.Append($"{8 * firstOctet} - кол-во свободных битов ");
            stringBuilder.Append($"{8 * firstOctet} - {subnetsLog} = {8 * firstOctet - subnetsLog} - кол-во битов доступных для адреса хоста\n\n");

            stringBuilder.Append("Заполнив отведённые биты для определения подсети, маска подсети примет иметь вид:\n");

            stringBuilder.Append($"{MaskWithAllottedBits(defaultMask, 8 * firstOctet - subnetsLog)}\n");
            stringBuilder.Append($"2^{8 * firstOctet - subnetsLog } - 2 = {_netmask.GetAdressesCount()}");
            stringBuilder.Append(" - количество доступных адресов хостов\n\n");

            stringBuilder.Append("Из общего кол-ва адресов вычитаем два адреса, так как они отведены для работы сети.");

            return stringBuilder.ToString();
        }


    }
}
