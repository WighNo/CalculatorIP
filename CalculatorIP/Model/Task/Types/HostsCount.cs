using CalculatorIP.Model.Data.IP;
using CalculatorIP.Model.Data.Mask;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using static CalculatorIP.Session;

namespace CalculatorIP.Model.Task.Types
{
    class HostsCount : TaskBase
    {
        private Random _random = new Random();

        private IPAdressType _ipAdressType;

        private IPAdress _ipAdress;
        private Netmask _netmask;

        private int _hostsCount;

        protected override string[] _content => new string[] 
        {
            "На какое максимальное количество подсетей должна быть разбита сеть",
            ", чтобы в каждой могло быть до",
            "хостов?"
        };

        public override GeneratedTask GetTask()
        {
            _ipAdressType = ipAdressTypes[_random.Next(0, ipAdressTypes.Length)];

            _ipAdress = IPRandomizer.GenerateRandomIP(_ipAdressType);
            _netmask = ListNetmask.GetRandomMask(_ipAdressType);

            _hostsCount = _random.Next(1, _netmask.GetAdressesCount() + 1);

            string task = TaskContentBuilder();
            string description = GetSolutionDescription();
            string result = GenerateSolution();

            GeneratedTask generatedTask = new GeneratedTask(task, result, description);

            return generatedTask;
        }

        private string TaskContentBuilder()
        {
            return $"{_content[0]}\n{_ipAdress.GetIP('.')} {_content[1]} {_hostsCount} {_content[2]}";
        }

        protected override string GenerateSolution()
        {
            return _netmask.GetSubnetsCount().ToString();
        }

        protected override string GetSolutionDescription()
        {
            StringBuilder stringBuilder = new StringBuilder();

            int maximumHostsCount = _hostsCount + 2;
            int logarithmMaximumNumberOfHosts = (int)Math.Ceiling(Math.Log(maximumHostsCount, 2));

            int numberFirstOctet = GetFirstOctet(_netmask.GetAdress());
            int bitsUsedToDefineTheNetwork = 8 * numberFirstOctet - logarithmMaximumNumberOfHosts;

            string defaultMask = ListNetmask.GetDefaultMask(_ipAdressType).GetAdress().GetBinaryData('.');


            stringBuilder.Append($"Класс подсети {_ipAdressType},");
            stringBuilder.Append(' ');
            stringBuilder.Append("следовательно стандарная маска подсети будет:\n");
            stringBuilder.Append(defaultMask + "\n\n");

            stringBuilder.Append($"{maximumHostsCount} = 2^{logarithmMaximumNumberOfHosts}; следовательно: ");
            stringBuilder.Append($"{logarithmMaximumNumberOfHosts} - количесвто битов занятых для адреса хоста\n");
            stringBuilder.Append($"{8 * numberFirstOctet} - кол-во свободных битов\n");
            stringBuilder.Append($"{8 * numberFirstOctet} - {logarithmMaximumNumberOfHosts} = {bitsUsedToDefineTheNetwork}");
            stringBuilder.Append(" - кол-во битов занятых для определения сети\n\n");

            stringBuilder.Append("Заполнив биты отведённые для определения подсети, маска подсети примет иметь вид:\n");           
            
            stringBuilder.Append($"{MaskWithAllottedBits(defaultMask, bitsUsedToDefineTheNetwork, _ipAdressType)}\n");
            stringBuilder.Append($"2^{  (int)Math.Log(_netmask.GetSubnetsCount(), 2)} = {_netmask.GetSubnetsCount()}");
            stringBuilder.Append(" - кол-во подсетей на которые разбита сеть\n\n\n");

            stringBuilder.Append("Ответ: " + _netmask.GetSubnetsCount());

            return stringBuilder.ToString();
        }
    }
}
