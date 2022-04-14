using System;
using System.Collections.Generic;
using CalculatorIP.Model.Data.Mask;

namespace CalculatorIP.Model.Data.IP
{
    public static class IPRandomizer
    {
        private static Dictionary<IPAdressType, FirstOctet> _valuesOfTheFirstOctets;

        private static Random _random = new Random();

        private static void InitOctets()
        {
            _valuesOfTheFirstOctets = new Dictionary<IPAdressType, FirstOctet>();

            _valuesOfTheFirstOctets.Add(IPAdressType.A, new FirstOctet(1, 127));
            _valuesOfTheFirstOctets.Add(IPAdressType.B, new FirstOctet(128, 192));
            _valuesOfTheFirstOctets.Add(IPAdressType.C, new FirstOctet(192, 224));
        }

        private static void CheckingStandardValues()
        {
            if (_valuesOfTheFirstOctets is null == true)
                InitOctets();
        }

        public static IPAdress GenerateRandomIP(IPAdressType ipType)
        {
            CheckingStandardValues();

            byte[] adress = new byte[4];

            adress[0] = _valuesOfTheFirstOctets[ipType].GetRandomValue();

            for (int i = 1; i <= ipType.GetHashCode(); i++)
            {
                adress[i] = (byte)_random.Next(1, 256);
            }

            IPAdress ipAdress = new IPAdress(ipType, adress);

            return ipAdress;
        }



        private class FirstOctet
        {
            private byte _minimum;
            private byte _maximum;

            private Random _random = new Random();

            public FirstOctet(byte minimum, byte maximum)
            {
                _minimum = minimum;
                _maximum = maximum;
            }

            public byte GetRandomValue() => (byte)_random.Next(_minimum, _maximum);
        }
    }

    public enum IPAdressType
    {
        A,
        B,
        C
    }
}
